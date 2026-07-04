using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public static class BitmapFinderServices
    {
        // API chính - nhận ImageSharp trực tiếp, không Bitmap
        public static Point? FindTemplate(Image<Rgba32> source, Image<Rgba32> template, float threshold = 0.8f)
        {
            int sw = source.Width, sh = source.Height;
            int tw = template.Width, th = template.Height;
            if (tw > sw || th > sh) return null;

            var pool = ArrayPool<float>.Shared;
            int srcSize = sw * sh;
            int tplSize = tw * th;
            int intSize = (sw + 1) * (sh + 1);

            float[] srcGray = pool.Rent(srcSize);
            float[] tplGray = pool.Rent(tplSize);
            float[] sum = pool.Rent(intSize);
            float[] sumSq = pool.Rent(intSize);

            try
            {
                ToGrayOpenCV(source, srcGray);
                ToGrayOpenCV(template, tplGray);

                BuildIntegral(srcGray, sw, sh, sum, sumSq);

                // template stats
                float tplMean = 0f;
                for (int i = 0; i < tplSize; i++) tplMean += tplGray[i];
                tplMean /= tplSize;

                float tplNorm = 0f;
                for (int i = 0; i < tplSize; i++)
                {
                    float d = tplGray[i] - tplMean;
                    tplNorm += d * d;
                }
                tplNorm = MathF.Sqrt(tplNorm) + 1e-5f;

                float bestVal = -1f;
                int bestX = 0, bestY = 0;
                int area = tw * th;

                // KHÔNG Parallel - để farm 400 phone tự scale
                for (int y = 0; y <= sh - th; y++)
                {
                    for (int x = 0; x <= sw - tw; x++)
                    {
                        float patchSum = GetSum(sum, sw + 1, x, y, tw, th);
                        float patchMean = patchSum / area;
                        float patchSumSq = GetSum(sumSq, sw + 1, x, y, tw, th);
                        float patchVar = patchSumSq - patchSum * patchMean;
                        float patchNorm = MathF.Sqrt(MathF.Max(patchVar, 0f)) + 1e-5f;

                        float cross = CrossCorrelation(srcGray, sw, x, y, tplGray, tw, th, patchMean, tplMean);

                        float val = cross / (patchNorm * tplNorm);
                        if (val > bestVal)
                        {
                            bestVal = val;
                            bestX = x;
                            bestY = y;
                            if (bestVal > 0.999f) goto done; // early exit
                        }
                    }
                }
            done:
                return bestVal >= threshold ? new Point(bestX + tw / 2, bestY + th / 2) : null;
            }
            finally
            {
                pool.Return(srcGray); pool.Return(tplGray);
                pool.Return(sum); pool.Return(sumSq);
            }
        }

        // overload tiện
        public static Point? FindTemplate(string sourcePath, string templatePath, float threshold = 0.8f)
        {
            using var src = Image.Load<Rgba32>(sourcePath);
            using var tpl = Image.Load<Rgba32>(templatePath);
            return FindTemplate(src, tpl, threshold);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ToGrayOpenCV(Image<Rgba32> img, float[] dst)
        {
            img.ProcessPixelRows(acc =>
            {
                for (int y = 0; y < acc.Height; y++)
                {
                    var row = acc.GetRowSpan(y);
                    int off = y * acc.Width;
                    for (int x = 0; x < row.Length; x++)
                    {
                        ref var p = ref row[x];
                        dst[off + x] = 0.114f * p.B + 0.587f * p.G + 0.299f * p.R;
                    }
                }
            });
        }

        private static void BuildIntegral(float[] src, int w, int h, float[] sum, float[] sumSq)
        {
            int stride = w + 1;
            Array.Clear(sum, 0, (w + 1) * (h + 1));
            Array.Clear(sumSq, 0, (w + 1) * (h + 1));

            for (int y = 1; y <= h; y++)
            {
                float rowSum = 0f, rowSumSq = 0f;
                int srcOff = (y - 1) * w;
                int dstOff = y * stride;
                int prevOff = (y - 1) * stride;
                for (int x = 1; x <= w; x++)
                {
                    float v = src[srcOff + x - 1];
                    rowSum += v;
                    rowSumSq += v * v;
                    sum[dstOff + x] = sum[prevOff + x] + rowSum;
                    sumSq[dstOff + x] = sumSq[prevOff + x] + rowSumSq;
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float GetSum(float[] intImg, int stride, int x, int y, int w, int h)
        {
            int x2 = x + w, y2 = y + h;
            return intImg[y2 * stride + x2] - intImg[y * stride + x2] - intImg[y2 * stride + x] + intImg[y * stride + x];
        }

        private static unsafe float CrossCorrelation(float[] src, int sw, int sx, int sy,
            float[] tpl, int tw, int th, float patchMean, float tplMean)
        {
            int len = tw * th;
            float sum = 0f;

            fixed (float* pTpl = tpl)
            {
                for (int ty = 0; ty < th; ty++)
                {
                    int sIdx = (sy + ty) * sw + sx;
                    int tIdx = ty * tw;
                    fixed (float* pSrc = &src[sIdx])
                    {
                        sum += CrossRow(pSrc, pTpl + tIdx, tw, patchMean, tplMean);
                    }
                }
            }
            return sum;
        }

        private static unsafe float CrossRow(float* src, float* tpl, int len, float pm, float tm)
        {
            float sum = 0f;
            int i = 0;

            if (Avx2.IsSupported)
            {
                var vpm = Vector256.Create(pm);
                var vtm = Vector256.Create(tm);
                var vacc = Vector256<float>.Zero;
                for (; i <= len - 8; i += 8)
                {
                    var vs = Avx.LoadVector256(src + i);
                    var vt = Avx.LoadVector256(tpl + i);
                    var v = Avx.Multiply(Avx.Subtract(vs, vpm), Avx.Subtract(vt, vtm));
                    vacc = Avx.Add(vacc, v);
                }
                sum += vacc.GetElement(0) + vacc.GetElement(1) + vacc.GetElement(2) + vacc.GetElement(3)
                     + vacc.GetElement(4) + vacc.GetElement(5) + vacc.GetElement(6) + vacc.GetElement(7);
            }

            for (; i < len; i++)
                sum += (src[i] - pm) * (tpl[i] - tm);

            return sum;
        }
    }
}