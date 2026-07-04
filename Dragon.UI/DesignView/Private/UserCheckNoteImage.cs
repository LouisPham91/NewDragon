using Dragon.Controller.TaskDeviceManager.Model.Vision;

using Dragon.Controller.GlobalControl.Helper;
using Dragon.DesignView.FormUI;
using System.Text.RegularExpressions;
using Dragon.DesignView.Public;

namespace Dragon.DesignView.ControlUI.Private
{
    public partial class UserCheckNoteImage : UserControl
    {
        public ImageOrcText imageOrcText = new ImageOrcText();
        FormADDNoteImage? formADDNoteImage = null;
        (string cropPercentSmaller, Bitmap bmSmaller) StringAndBitmap;

        

        public UserCheckNoteImage(FormADDNoteImage formADDNoteImage, (string cropPercentSmaller, Bitmap bmSmaller) stringAndBitmap)
        {
            InitializeComponent();

            
            
            ApplyTheme();

            StringAndBitmap = stringAndBitmap;
            this.formADDNoteImage = formADDNoteImage;
            picboxImageCheck.Image = StringAndBitmap.bmSmaller;

            imageOrcText.VisionAction = VisionAction.DetectOnly; // <-- thêm
            imageOrcText.IsActive = true;


            txtPointCheck.Enabled = false;
            txtCropRegion.Enabled = false;
            txtTextToFind.Enabled = false;
            numbericAccuracy.Enabled = true;

            chkActiveCheck.Checked = true;
        }

        public void ApplyTheme()
        {
            if (IsDisposed) return;

            

            SetVisionActionFromParent(imageOrcText.VisionAction);
        }

        public void SetVisionActionFromParent(VisionAction action)
        {
            imageOrcText.VisionAction = action;

            txtPointCheck.Enabled = chkActiveCheck.Checked && action == VisionAction.ClickAtPoint;

            // reset về base
            var labels = new[] { lblDetectOnly, lblDetectAndClick, lblDetectAtPoint };
            foreach (var l in labels)
            {
                l.Font = new Font(Font, FontStyle.Regular);
                l.Text = l.Text.Replace("● ", "").Replace("✓ ", "");
            }

            // chọn label active
            Label? active = action switch
            {
                VisionAction.DetectOnly => lblDetectOnly,
                VisionAction.DetectAndClick => lblDetectAndClick,
                VisionAction.ClickAtPoint => lblDetectAtPoint,
                _ => null
            };

            // màu theo theme
            bool isDark = ThemeHelper.CurrentMode == ThemeMode.Dark;

            foreach (var l in labels)
            {
                l.ForeColor = isDark ? Color.FromArgb(110, 110, 110) : Color.DimGray;
            }

            if (active != null)
            {
                active.Font = new Font(Font, FontStyle.Bold);
                active.ForeColor = isDark ? Color.White : Color.Black;
                active.Text = "● " + active.Text;
            }
        }

        public void SetCheckImage(VisionAction visionAction = VisionAction.None)
        {
            if (visionAction != VisionAction.None)
            {
                imageOrcText.VisionAction = visionAction;
            }

            imageOrcText.Accuracy = (double)numbericAccuracy.Value / 100.0;
            imageOrcText.SecialClickPoint = txtPointCheck.Text.Trim();
            imageOrcText.CropRegion = txtCropRegion.Text.Trim();
            imageOrcText.TextToFind = txtTextToFind.Text.Trim();
            imageOrcText.IsActive = chkActiveCheck.Checked;


            var imgBytes = BitmapConverter.BitmapToByteArray(StringAndBitmap.bmSmaller);
            if (imgBytes != null) imageOrcText.ImageDataSrcpy = imgBytes;


            formADDNoteImage?.UpdateCheckNoteImage(imageOrcText);
        }

        private void chkActiveCheck_CheckedChanged(object sender, EventArgs e)
        {
            bool isActive = chkActiveCheck.Checked;

            txtCropRegion.Enabled = isActive;
            txtTextToFind.Enabled = isActive;
            numbericAccuracy.Enabled = isActive;
            txtPointCheck.Enabled = isActive && imageOrcText.VisionAction == VisionAction.ClickAtPoint;

            SetCheckImage();
        }

        private void labelDeleteCheck_Click(object sender, EventArgs e)
        {
            formADDNoteImage?.RemoveUserCheckNoteImage(this);
            Dispose();
        }

        // để ở cấp class / form
        private static readonly Regex PointCheckRegex = new Regex(
            @"^(100|(\d{1,2}(\.\d+)?)),(100|(\d{1,2}(\.\d+)?))$",
            RegexOptions.CultureInvariant);

        private static readonly Regex CropRegionRegex = new Regex(
            @"^(\d{1,3}(\.\d+)?),(\d{1,3}(\.\d+)?),(\d{1,3}(\.\d+)?),(\d{1,3}(\.\d+)?)$",
            RegexOptions.CultureInvariant);

        private void txtPointCheck_TextChanged(object sender, EventArgs e)
        {
            string input = txtPointCheck.Text.Trim();
            txtPointCheck.BackColor = PointCheckRegex.IsMatch(input)
                ? Color.DarkGreen : Color.DarkRed;
            SetCheckImage();
        }

        private void txtCropRegion_TextChanged(object sender, EventArgs e)
        {
            string input = txtCropRegion.Text.Trim();
            txtCropRegion.BackColor = string.IsNullOrEmpty(input) || CropRegionRegex.IsMatch(input)
                ? Color.FromArgb(40, 40, 40) : Color.DarkRed;
            SetCheckImage();
        }

        private void txtTextToFind_TextChanged(object sender, EventArgs e)
        {
            SetCheckImage();
        }

        private void numbericAccuracy_ValueChanged(object sender, EventArgs e)
        {
            SetCheckImage();
        }
    }
}
