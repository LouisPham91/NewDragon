using Dragon.Controller.TaskDeviceManager.Model.Mouse;

namespace Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn
{
    public class SetColumnDataArgs
    {
        public ControlMode ControlMode { get; set; }
        public ReadType ReadType { get; set; }
        public string SocialNetworkName { get; set; } = string.Empty;
        public bool IsNodeGetFullText { get; set; } = true;
        public bool IsNodeResourceName { get; set; } = false;
        private string _NodeResourceName = string.Empty;
        public string NodeResourceName
        {
            get => _NodeResourceName;
            set => _NodeResourceName = Clean(value);
        }

        public bool IsNodeClassName { get; set; } = false;
        private string _NodelassName = string.Empty;
        public string NodeClassName
        {
            get => _NodelassName;
            set => _NodelassName = Clean(value);
        }

        public bool IsNodePackageName { get; set; } = false;
        private string _NodePackageName = string.Empty;
        public string NodePackageName
        {
            get => _NodePackageName;
            set => _NodePackageName = Clean(value);
        }

        private string _NodeRegexPattern = string.Empty;
        public string NodeRegexParttern
        {
            get => _NodeRegexPattern;
            set => _NodeRegexPattern = Clean(value);
        }

        private string _ColumnName = string.Empty;
        public string ColumnName
        {
            get => _ColumnName;
            set => _ColumnName = Clean(value);
        }
        public Language Language { get; set; }
        public string ImageRectangleRegion { get; set; } = string.Empty;
        public string ImageRegexPattern { get; set; } = string.Empty;

        // Hàm tiện ích loại bỏ \r
        private static string Clean(string input)
        {
            return input?.Replace("\r", string.Empty) ?? string.Empty;
        }
    }
    public enum Language
    {
        English,
        TiengViet
    }
    public enum ReadType
    {
        None,
        NodeText,
        ImageToText
    }
}
