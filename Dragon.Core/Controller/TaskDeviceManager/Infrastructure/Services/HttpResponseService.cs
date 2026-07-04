using Dragon.Controller.TaskDeviceManager.Infrastructure;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Dragon.Controller.TaskDeviceManager.Infrastructure.Services
{
    public static class HttpResponseService
    {
        private static readonly HttpClient _shared = new(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15),
            MaxConnectionsPerServer = 200,
            AutomaticDecompression = System.Net.DecompressionMethods.All
        })
        {
            Timeout = TimeSpan.FromSeconds(20)
        };

        private static bool IsMatch(string actual, string expected, CompareType type)
        {
            if (double.TryParse(actual, out double numActual) && double.TryParse(expected, out double numExpected))
            {
                return type switch
                {
                    CompareType.Equal => numActual == numExpected,
                    CompareType.Greater => numActual > numExpected,
                    CompareType.Smaller => numActual < numExpected,
                    CompareType.Any => true,
                    _ => false
                };
            }
            else
            {
                return type switch
                {
                    CompareType.Equal => string.Equals(actual.Trim(), expected.Trim(), StringComparison.OrdinalIgnoreCase),
                    CompareType.Any => actual.Trim().ToLower() == expected.Trim().ToLower(),
                    _ => false
                };
            }
        }

        private static readonly ConcurrentDictionary<string, Regex> _rxCache = new();

        private static Regex GetRx(string pattern) =>
            _rxCache.GetOrAdd(pattern, p => new Regex(p, RegexOptions.CultureInvariant));

        private static string ReplaceIgnoreCase(string input, string oldValue, string newValue)
        {
            return input.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
        }

        public static async Task<(string phoneNumber, string requestId, bool IsRequestSucsess)>
            GetAndRegexAsync(HttpRequestConfig model, string? ValueNextRequest = null,
                             CancellationToken token = default, string deviceId = "")
        {
            try
            {
                // 1. thay <API> và <ValueNextRequest> bằng string, không dùng Regex
                string url1 = ReplaceIgnoreCase(model.HttpUrl, "<API>", model.ApiKey);
                if (!string.IsNullOrEmpty(ValueNextRequest))
                    url1 = ReplaceIgnoreCase(url1, "<ValueNextRequest>", ValueNextRequest);

                string response1 = await _shared.GetStringAsync(url1, token);

                if (string.IsNullOrEmpty(response1))
                {
                    Logger.Notify(deviceId, "Response is empty", Logger.Icon.Warning);
                    return (string.Empty, string.Empty, false);
                }

                Logger.Notify(deviceId, $"Response: {response1}", Logger.Icon.Information);

                // 2. match phone - dùng cache
                var matchPhone = GetRx(model.RegexResult).Match(response1);
                string phoneNumber = matchPhone.Success && matchPhone.Groups.Count > 1
                    ? matchPhone.Groups[1].Value : string.Empty;

                if (!string.IsNullOrEmpty(model.CompareResult) &&
                    model.CompareType_Result != CompareType.Any)
                {
                    if (!IsMatch(phoneNumber, model.CompareResult, model.CompareType_Result))
                    {
                        Logger.Notify(deviceId, $"Phone '{phoneNumber}' không khớp '{model.CompareResult}'", Logger.Icon.Warning);
                        return (string.Empty, string.Empty, false);
                    }
                }

                if (model.UseRegexMode == UseRegexMode.IsUseOne)
                {
                    Logger.Notify(deviceId, $"Phone '{phoneNumber}' lấy thành công (IsUseOne)", Logger.Icon.Information);
                    return (phoneNumber, string.Empty, true);
                }

                // 3. match requestId - dùng cache
                var matchRequestId = GetRx(model.RegexValueNextRequest).Match(response1);
                string requestId = matchRequestId.Success && matchRequestId.Groups.Count > 1
                    ? matchRequestId.Groups[1].Value : string.Empty;

                if (string.IsNullOrEmpty(requestId))
                {
                    Logger.Notify(deviceId, $"RequestId rỗng. Regex: '{model.RegexValueNextRequest}'", Logger.Icon.Warning);
                    return (string.Empty, string.Empty, false);
                }

                if (!string.IsNullOrEmpty(model.CompareNextRequest) &&
                    model.CompareType_NextRequest != CompareType.Any)
                {
                    if (!IsMatch(requestId, model.CompareNextRequest, model.CompareType_NextRequest))
                    {
                        Logger.Notify(deviceId, $"RequestId '{requestId}' không khớp '{model.CompareNextRequest}'", Logger.Icon.Warning);
                        return (string.Empty, string.Empty, false);
                    }
                }

                Logger.Notify(deviceId, $"Phone '{phoneNumber}' và RequestId '{requestId}' lấy thành công", Logger.Icon.Information);
                return (phoneNumber, requestId, true);
            }
            catch (Exception ex)
            {
                Logger.Notify(deviceId, $"Lỗi HTTP: {ex.Message}", Logger.Icon.Error);
                return ("", "", false);
            }
        }
    }
}