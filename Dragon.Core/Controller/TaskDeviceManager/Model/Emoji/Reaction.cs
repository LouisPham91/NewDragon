
using System.Globalization;
using System.Text.Json.Serialization;

namespace Dragon.Controller.TaskDeviceManager.Model.Emoji
{
    public class ReactionPoint
    {
        [JsonPropertyName("x")]
        public float X { get; set; }

        [JsonPropertyName("y")]
        public float Y { get; set; }

        public ReactionPoint() { }
        public ReactionPoint(float x, float y) { X = x; Y = y; }

        public static ReactionPoint Parse(string text)
        {
            var p = text.Split(',');
            return new ReactionPoint(
                float.Parse(p[0].Trim(), CultureInfo.InvariantCulture),
                float.Parse(p[1].Trim(), CultureInfo.InvariantCulture));
        }

        public override string ToString() => $"{X:F2},{Y:F2}";
    }

    public class ReactionSet
    {
        [JsonPropertyName("buttomLike")]
        public ReactionPoint ButtomLike { get; set; } = new ReactionPoint();

        [JsonPropertyName("like")]
        public ReactionPoint Like { get; set; } = new ReactionPoint();

        [JsonPropertyName("love")]
        public ReactionPoint Love { get; set; } = new ReactionPoint();

        [JsonPropertyName("care")]
        public ReactionPoint Care { get; set; } = new ReactionPoint();

        [JsonPropertyName("haha")]
        public ReactionPoint Haha { get; set; } = new ReactionPoint();

        [JsonPropertyName("wow")]
        public ReactionPoint Wow { get; set; } = new ReactionPoint();

        [JsonPropertyName("sad")]
        public ReactionPoint Sad { get; set; } = new ReactionPoint();

        [JsonPropertyName("angry")]
        public ReactionPoint Angry { get; set; } = new ReactionPoint();

        /// <summary>
        /// Ngưỡng Y tối đa để Reaction xuất hiện ở Bottom.
        /// Nếu ButtomLike.Y > MaxDy thì Reaction sẽ xuất hiện ở Top.
        /// </summary>
        [JsonPropertyName("maxDy")]
        public float MaxDY { get; set; }

        [JsonIgnore]
        private static readonly Random _rng = new Random();

        /// <summary>
        /// Lấy tọa độ reaction khi ButtomLike di chuyển đến vị trí mới
        /// </summary>
        internal ReactionPoint GetPoint(ReactionType type, float newButtomLikeX, float newButtomLikeY)
        {
            // Tính offset di chuyển của ButtomLike
            float deltaX = newButtomLikeX - ButtomLike.X;
            float deltaY = newButtomLikeY - ButtomLike.Y;

            // Lấy tọa độ gốc của reaction
            ReactionPoint originalPoint = GetOriginalPoint(type);

            // Trả về tọa độ mới = tọa độ gốc + offset
            return new ReactionPoint(
                originalPoint.X + deltaX,
                originalPoint.Y + deltaY
            );
        }

        /// <summary>
        /// Lấy tọa độ gốc (khi ButtomLike ở vị trí mặc định)
        /// </summary>
        private ReactionPoint GetOriginalPoint(ReactionType type)
        {
            switch (type)
            {
                case ReactionType.LikeOnly: return Like;
                case ReactionType.LoveOnly: return Love;
                case ReactionType.CareOnly: return Care;
                case ReactionType.HahaOnly: return Haha;
                case ReactionType.WowOnly: return Wow;
                case ReactionType.SadOnly: return Sad;
                case ReactionType.AngryOnly: return Angry;
                case ReactionType.Random:
                    var all = new[] { Like, Love, Care, Haha, Wow, Sad, Angry };
                    return all[_rng.Next(all.Length)];
                default: return Like;
            }
        }
    }

    public enum ReactionType
    {
        Random,
        LikeOnly,
        LoveOnly,
        CareOnly,
        HahaOnly,
        WowOnly,
        SadOnly,
        AngryOnly,
    }

    public class Reaction
    {
        [JsonPropertyName("reactionName")]
        public string ReactionName { get; set; } = string.Empty;

        [JsonPropertyName("deviceModel")]
        public string DeviceModel { get; set; } = string.Empty;

        [JsonPropertyName("netWorkName")]
        public string NetWorkName { get; set; } = string.Empty;

        [JsonPropertyName("reactionType")]
        public ReactionType ReactionType { get; set; } = ReactionType.LikeOnly;

        [JsonPropertyName("topSet")]
        public ReactionSet TopSet { get; set; } = new ReactionSet();

        [JsonPropertyName("bottomSet")]
        public ReactionSet BottomSet { get; set; } = new ReactionSet();

        /// <summary>
        /// Hàm chính - Tính tọa độ reaction từ vị trí ButtomLike mới
        /// Logic: Dùng MaxDy của TopSet để xác định dùng TopSet hay BottomSet
        /// </summary>
        public ReactionPoint GetReactionPoint(float buttomLikeX, float buttomLikeY)
        {
            // Dùng MaxDy của TopSet làm ngưỡng (vì đây là ngưỡng lật)
            // Nếu buttomLikeY > TopSet.MaxDy → Reaction xuất hiện ở Top
            // Nếu buttomLikeY <= TopSet.MaxDy → Reaction xuất hiện ở Bottom
            var activeSet = buttomLikeY > BottomSet.MaxDY ? TopSet : BottomSet;

            return activeSet.GetPoint(ReactionType, buttomLikeX, buttomLikeY);
        }

        /// <summary>
        /// Parse input dạng "12.15,13.54" và trả về ReactionPoint tương ứng.
        /// </summary>
        public ReactionPoint GetReactionPoint(string buttomLikePercent)
        {
            if (string.IsNullOrWhiteSpace(buttomLikePercent))
                throw new ArgumentException("Input is null or empty.", nameof(buttomLikePercent));

            var s = buttomLikePercent.Trim();
            var parts = s.Split(',');

            if (parts.Length != 2)
                throw new ArgumentException("Invalid format. Expected 'x,y'.", nameof(buttomLikePercent));

            if (!float.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var x) ||
                !float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
            {
                throw new ArgumentException("Invalid numeric values in input.", nameof(buttomLikePercent));
            }

            return GetReactionPoint(x, y);
        }

        //// Serialize / Deserialize
        //public JsonElement ToJsonElement()
        //{
        //    return JsonSerializer.SerializeToElement(this, DLoopJsonContext.Default.Reactions);
        //}

        //public static Reactions FromJsonElement(JsonElement element)
        //{
        //    return JsonSerializer.Deserialize(element.GetRawText(), DLoopJsonContext.Default.Reactions)!;
        //}
    }
}