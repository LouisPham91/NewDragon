

namespace Dragon.Controller.TaskDeviceManager.Model.Vision
{
    public class ImageOrcText
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public byte[] ImageDataSrcpy { get; set; } = Array.Empty<byte>();
        public double Accuracy { get; set; } = 0.8;
        public bool IsActive { get; set; } = true;
        public VisionAction VisionAction { get; set; }
        public string SecialClickPoint { get; set; } = ""; // click riêng cho case này
        public string CropRegion { get; set; } = "";    // crop riêng cho case này
        public string TextToFind { get; set; } = "";    // tách text OCR ra cho sạch
    }
}
