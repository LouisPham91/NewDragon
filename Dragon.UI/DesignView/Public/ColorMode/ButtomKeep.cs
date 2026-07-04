

using Dragon.ControlHelper;
using System.ComponentModel;

namespace Dragon.DesignView.Public.ColorMode
{
    public class ButtomKeep : Button, IThemeable
    {
        private Color _backColorNormal = Color.FromArgb(245, 245, 245);
        private Color _forceColorNormal = Color.Black;
        private Color _forceColorClick = Color.FromArgb(207, 66, 51);

        private bool _toggled = false;
        private string _svgString = string.Empty;
        private Image? _imageNormal;
        private Image? _imageClick;
        private int _lightenPercent = 20;

        // 🔥 Theme handler


        [Category("Appearance")]
        [DefaultValue(typeof(Color), "245, 245, 245")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_BackColorNormal
        {
            get => _backColorNormal;
            set
            {
                _backColorNormal = value;
                if (!_toggled)
                    BackColor = _backColorNormal;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ForceColorNormal
        {
            get => _forceColorNormal;
            set
            {
                _forceColorNormal = value;
                if (!_toggled)
                {
                    ForeColor = _forceColorNormal;
                    UpdateSvgImageForState();
                }
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "207, 66, 51")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Color DG_ForceColorClick
        {
            get => _forceColorClick;
            set
            {
                _forceColorClick = value;
                if (_toggled)
                {
                    ForeColor = _forceColorClick;
                    UpdateSvgImageForState();
                }
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DG_Toggled
        {
            get => _toggled;
            set
            {
                _toggled = value;
                DrawLineBaseOnClick();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [Description("SVG content trực tiếp")]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string DG_SVGString
        {
            get => _svgString;
            set
            {
                _svgString = value;
                UpdateSvgImage();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(20)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int DG_LightenPercent
        {
            get => _lightenPercent;
            set
            {
                _lightenPercent = Math.Max(0, Math.Min(255, value));
            }
        }

        private void UpdateSvgImage()
        {
            if (string.IsNullOrEmpty(_svgString)) return;

            int h = Font.Height - 2;
            _imageNormal = SvgRenderer.RenderSvgFromString(_svgString, h, h, _forceColorNormal);
            _imageClick = SvgRenderer.RenderSvgFromString(_svgString, h, h, _forceColorClick);

            if (_toggled)
                Image = _imageClick;
            else
                Image = _imageNormal;
        }

        private void UpdateSvgImageForState()
        {
            if (string.IsNullOrEmpty(_svgString)) return;

            int h = Font.Height - 2;
            if (_toggled)
                Image = SvgRenderer.RenderSvgFromString(_svgString, h, h, _forceColorClick);
            else
                Image = SvgRenderer.RenderSvgFromString(_svgString, h, h, _forceColorNormal);
        }

        public ButtomKeep()
        {
            ImageAlign = ContentAlignment.MiddleLeft;
            TabStop = false;
            FlatStyle = FlatStyle.Flat;
            DoubleBuffered = true;
            FlatAppearance.BorderSize = 0;
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer, true);

            // 🔥 Subscribe theme
            
            
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            // 🔥 Lấy màu từ ThemeHelper theo Light/Dark mode
            if (ThemeHelper.CurrentMode == ThemeMode.Light)
            {
                _backColorNormal = ThemeHelper.BackNormalFirst;
                _forceColorNormal = ThemeHelper.ForeNormalFirst;

                // Màu accent theo style
                _forceColorClick = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColoful4th,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful2nd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }
            else // Dark mode
            {
                _backColorNormal = ThemeHelper.BackNormalFirst;
                _forceColorNormal = ThemeHelper.ForeNormalFirst;

                // Màu accent theo style
                _forceColorClick = ThemeHelper.CurrentStyle switch
                {
                    ThemeStyle.ThemeRed => ThemeHelper.ForceColofulFist,
                    ThemeStyle.ThemeGreen => ThemeHelper.ForceColoful2nd,
                    ThemeStyle.ThemeGold => ThemeHelper.ForceColoful3rd,
                    _ => ThemeHelper.ForceColofulFist
                };
            }

            // Cập nhật SVG images với màu mới
            if (!string.IsNullOrEmpty(_svgString))
            {
                var height = Font.Height - 2;
                _imageNormal = SvgRenderer.RenderSvgFromString(_svgString, height, height, _forceColorNormal);
                _imageClick = SvgRenderer.RenderSvgFromString(_svgString, height, height, _forceColorClick);
            }

            // Áp dụng màu cho control
            BackColor = _backColorNormal;
            ForeColor = _toggled ? _forceColorClick : _forceColorNormal;

            if (!string.IsNullOrEmpty(_svgString))
                Image = _toggled ? _imageClick : _imageNormal;

            Invalidate();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            
            base.OnHandleDestroyed(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                
            base.Dispose(disposing);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            if (!string.IsNullOrEmpty(_svgString))
            {
                UpdateSvgImage();
                Invalidate();
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (IsDisposed) return;
            _toggled = !_toggled;
            DrawLineBaseOnClick();
            Invalidate();
            base.OnClick(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsDisposed) return;
            base.OnPaint(e);

            if (_toggled)
            {
                using var pen = new Pen(ForeColor, 1);
                e.Graphics.DrawLine(pen, 0, Height - 1, Width, Height - 1);
            }
        }

        public void DrawLineBaseOnClick()
        {
            if (IsDisposed) return;

            Control? parent = Parent;
            if (parent is null) return;

            foreach (Control control in parent.Controls)
            {
                if (control is not ButtomKeep ctrl) continue;

                // Cập nhật SVG cho tất cả các button cùng loại
                if (!string.IsNullOrEmpty(ctrl._svgString))
                {
                    var height = Font.Height - 2;
                    ctrl._imageNormal = SvgRenderer.RenderSvgFromString(ctrl._svgString, height, height, ctrl._forceColorNormal);
                    ctrl._imageClick = SvgRenderer.RenderSvgFromString(ctrl._svgString, height, height, ctrl._forceColorClick);
                }

                if (ctrl.Name != Name)
                {
                    ctrl._toggled = false;
                    ctrl.BackColor = ctrl._backColorNormal;
                    ctrl.ForeColor = ctrl._forceColorNormal;
                    ctrl.Image = ctrl._imageNormal;
                }
                else
                {
                    ctrl._toggled = true;
                    ctrl.BackColor = ctrl._backColorNormal;
                    ctrl.ForeColor = ctrl._forceColorClick;
                    ctrl.Image = ctrl._imageClick;
                }
                ctrl.Invalidate();
            }
        }
    }
}