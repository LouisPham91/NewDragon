

namespace Dragon.DesignView.Public.NormalMode
{
    public class AotSafeDataGridView : DataGridView, IThemeable
    {
        // 🔥 1. Khai báo biến lưu event handler
        

        public AotSafeDataGridView()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            AutoGenerateColumns = false;
            EnableHeadersVisualStyles = false;
            RowHeadersVisible = false;
        
            // 🔥 3. Áp dụng theme ban đầu
            ApplyTheme();

            // Các thiết lập border
            GridColor = ThemeHelper.CurrentMode == ThemeMode.Light
                ? Color.FromArgb(200, 200, 200)
                : Color.Gray;

            CellBorderStyle = DataGridViewCellBorderStyle.Single;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            var h = AdvancedColumnHeadersBorderStyle;
            h.Top = DataGridViewAdvancedCellBorderStyle.Single;
            h.Left = DataGridViewAdvancedCellBorderStyle.Single;
            h.Right = DataGridViewAdvancedCellBorderStyle.Single;
            h.Bottom = DataGridViewAdvancedCellBorderStyle.Single;

            var c = AdvancedCellBorderStyle;
            c.Top = DataGridViewAdvancedCellBorderStyle.Single;
            c.Left = DataGridViewAdvancedCellBorderStyle.Single;
            c.Right = DataGridViewAdvancedCellBorderStyle.Single;
            c.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
        }

        // 🔥 4. Method ApplyTheme - chứa logic đổi màu theo theme
        public void ApplyTheme()
        {
            if (IsDisposed) return;

            BackgroundColor = ThemeHelper.BackNormalFirst;
           
            // Lấy màu từ ThemeHelper
            DefaultCellStyle.BackColor = ThemeHelper.BackNormalFirst;
            DefaultCellStyle.ForeColor = ThemeHelper.ForeNormalFirst;

            // Màu selection
            DefaultCellStyle.SelectionBackColor = ThemeHelper.CurrentMode == ThemeMode.Light
                ? Color.FromArgb(200, 220, 240)  // Light mode selection
                : Color.FromArgb(60, 70, 80);    // Dark mode selection

            DefaultCellStyle.SelectionForeColor = ThemeHelper.ForeNormalFirst;

            // Áp dụng cho ColumnHeaders
            ColumnHeadersDefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
            ColumnHeadersDefaultCellStyle.ForeColor = DefaultCellStyle.ForeColor;
            ColumnHeadersDefaultCellStyle.SelectionBackColor = DefaultCellStyle.SelectionBackColor;
            ColumnHeadersDefaultCellStyle.SelectionForeColor = DefaultCellStyle.SelectionForeColor;

            // Áp dụng cho RowHeaders
            RowHeadersDefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
            RowHeadersDefaultCellStyle.ForeColor = DefaultCellStyle.ForeColor;
            RowHeadersDefaultCellStyle.SelectionBackColor = DefaultCellStyle.SelectionBackColor;
            RowHeadersDefaultCellStyle.SelectionForeColor = DefaultCellStyle.SelectionForeColor;

            // Cập nhật GridColor theo theme
            GridColor = ThemeHelper.CurrentMode == ThemeMode.Light
                ? Color.FromArgb(180, 180, 180)
                : Color.FromArgb(80, 80, 80);

            // Cập nhật màu cho tất cả các cột đã tồn tại
            foreach (DataGridViewColumn col in Columns)
            {
                col.DefaultCellStyle.BackColor = DefaultCellStyle.BackColor;
                col.DefaultCellStyle.ForeColor = DefaultCellStyle.ForeColor;
                col.DefaultCellStyle.SelectionBackColor = DefaultCellStyle.SelectionBackColor;
                col.DefaultCellStyle.SelectionForeColor = DefaultCellStyle.SelectionForeColor;
            }

            // Refresh lại giao diện
            Refresh();
        }

        // 🔥 5. Unsubscribe khi handle bị hủy
        protected override void OnHandleDestroyed(EventArgs e)
        {
            
            base.OnHandleDestroyed(e);
        }

        // 🔥 6. Unsubscribe trong Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
            base.Dispose(disposing);
        }
    }


}