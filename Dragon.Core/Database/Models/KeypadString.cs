using System;

namespace Dragon.Controller.Database.Models
{
    public class KeypadString
    {
        public string Model { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string PassCoce { get; set; } = "123789";
        public string num0 { get; set; } = "50.00,79.75";
        public string num1 { get; set; } = "22.38,57.29";
        public string num2 { get; set; } = "50.71,57.29";
        public string num3 { get; set; } = "79.76,58.10";
        public string num4 { get; set; } = "20.95,64.81";
        public string num5 { get; set; } = "50.71,64.47";
        public string num6 { get; set; } = "78.81,65.16";
        public string num7 { get; set; } = "20.00,71.53";
        public string num8 { get; set; } = "50.95,71.88";
        public string num9 { get; set; } = "79.76,71.88";
        public string OK { get; set; } = "80.00,80.32";

        public string Get(int digit) => digit switch
        {
            0 => num0,
            1 => num1,
            2 => num2,
            3 => num3,
            4 => num4,
            5 => num5,
            6 => num6,
            7 => num7,
            8 => num8,
            9 => num9,
            _ => num0
        };

        private (float x, float y) Parse(string s)
        {
            var p = s.Split(',');
            return (float.Parse(p[0]), float.Parse(p[1]));
        }

        private (float x, float y) Parse(int digit) => Parse(Get(digit));

        // tọa độ cho số
        public (int x, int y) GetClick(int digit, int screenW, int screenH)
        {
            var (xp, yp) = Parse(digit);
            return ((int)(screenW * xp / 100f), (int)(screenH * yp / 100f));
        }

        // === SỬA LẠI HÀM OK ===
        public (int x, int y) GetOk(int screenW, int screenH)
        {
            var (xp, yp) = Parse(OK); // parse chuỗi OK, không dùng digit
            return ((int)(screenW * xp / 100f), (int)(screenH * yp / 100f));
        }
    }
}