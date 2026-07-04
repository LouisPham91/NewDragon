using System.Text.Json.Serialization;
using Dragon.Controller.TaskDeviceManager.Core;
using Dragon.Controller.TaskDeviceManager.Model.App;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.Emoji;
using Dragon.Controller.TaskDeviceManager.Model.File;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Controller.TaskDeviceManager.Model.Vision;

namespace Dragon.Controller.TaskDeviceManager.Core
{
    [JsonSerializable(typeof(DLoop))]
    [JsonSerializable(typeof(List<DLoop>))]

    // App
    [JsonSerializable(typeof(AppArgs))]

    // Database Column
    [JsonSerializable(typeof(SetColumnDataArgs))]
    [JsonSerializable(typeof(GetColumnDataArgs))]

    // Emoji
    [JsonSerializable(typeof(EmojiArgs))]
    [JsonSerializable(typeof(Reaction))]

    //File
    [JsonSerializable(typeof(FileArgs))]

    // Http
    [JsonSerializable(typeof(HttpRequestConfig))]

    // Input
    [JsonSerializable(typeof(SendTextArgs))]
    [JsonSerializable(typeof(KeyPressArgs))]
    [JsonSerializable(typeof(ImeActionArgs))]

    // Mouse
    [JsonSerializable(typeof(ClickArg))]
    [JsonSerializable(typeof(LongPressArg))]
    [JsonSerializable(typeof(SwipeArg))]
    [JsonSerializable(typeof(DragArg))]

    // Vision
    [JsonSerializable(typeof(VisionScanArgs))]
    [JsonSerializable(typeof(ImageOrcText))]
    [JsonSerializable(typeof(ATXNode))]

    // Primitives
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(int))]
    [JsonSerializable(typeof(bool))]
    [JsonSerializable(typeof(float))]

    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        WriteIndented = false)]
    public partial class DLoopJsonContext : JsonSerializerContext { }
}