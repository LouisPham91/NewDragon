
namespace Dragon.Controller.GlobalControl.Extensions
{
    public static class AndroidVersionMapper
    {
        private static readonly Dictionary<int, string> ApiToVersion = new Dictionary<int, string>
        {
            { 1,  "1.0" },
            { 2,  "1.1" },
            { 3,  "1.5" }, // Cupcake
            { 4,  "1.6" }, // Donut
            { 5,  "2.0" }, { 6, "2.0.1" },
            { 7,  "2.1" },
            { 8,  "2.2" }, // Froyo
            { 9,  "2.3" }, {10, "2.3.3–2.3.7"},
            {11,  "3.0" }, {12, "3.1" }, {13, "3.2"},
            {14,  "4.0.1–4.0.2"}, {15, "4.0.3–4.0.4"},
            {16,  "4.1" }, {17, "4.2" }, {18, "4.3"},
            {19,  "4.4" }, {20, "4.4W"},
            {21,  "5.0" }, {22, "5.1"},
            {23,  "6.0" },
            {24,  "7.0" }, {25, "7.1"},
            {26,  "8.0" }, {27, "8.1"},
            {28,  "9"    },
            {29,  "10"   },
            {30,  "11"   },
            {31,  "12"   }, {32, "12L"},
            {33,  "13"   },
            {34,  "14"   },
            {35,  "15"   },
            {36,  "16"   }
        };

        public static string GetAndroidVersion(int apiLevel)
        {
            return ApiToVersion.TryGetValue(apiLevel, out var version)
                ? version
                : $"Unknown (API‑{apiLevel})";
        }
    }

}
