using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;

namespace Dragon.Controller.TaskDeviceManager.Model.HttpResponse
{
    public class HttpRequestConfig
    {
        public ControlMode ControlMode { get; set; }
        public string HttpUrl { get; set; } = string.Empty;
        public string HttpTitle { get; set; } = string.Empty;
        public string RegexResult { get; set; } = string.Empty;
        public string CompareResult { get; set; } = string.Empty;
        public CompareType CompareType_Result { get; set; } = CompareType.None;

        public string RegexValueNextRequest { get; set; } = string.Empty;
        public string CompareNextRequest { get; set; } = string.Empty;
        public CompareType CompareType_NextRequest { get; set; } = CompareType.None;

        public string ApiKey { get; set; } = string.Empty;
        public UseRegexMode UseRegexMode { get; set; } = UseRegexMode.IsUseOne;
        public bool IsSaveValue { get; set; } = false;
        public TypeOption TypeOption { get; set; } = TypeOption.Typing;
        public int LoopTimes { get; set; } = 1;
        public int LoopDelay { get; set; } = 100;
    }

    public enum UseRegexMode { IsUseOne, IsUseTwo }
    public enum CompareType { None, Greater, Smaller, Equal, Any }
}