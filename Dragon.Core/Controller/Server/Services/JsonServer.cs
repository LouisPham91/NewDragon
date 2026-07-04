using Dragon.Controller.TaskDeviceManager.Model.Vision;
using Dragon.Controller.Firebase.Model;
using Dragon.Controller.GlobalControl.Extensions;
using Dragon.Controller.Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Dragon.Controller.Server.Services
{

    // Các class tại server
    [JsonSourceGenerationOptions(
     PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
     WriteIndented = false,
     Converters = new[] {
        typeof(UtcDateTimeConverter),
        typeof(JsonStringEnumConverter<VisionAction>),
     })]
    [JsonSerializable(typeof(string))]
    [JsonSerializable(typeof(Guid))]
    [JsonSerializable(typeof(User))]
    [JsonSerializable(typeof(Computer))]
    [JsonSerializable(typeof(List<ComputerOnline>))]
    [JsonSerializable(typeof(Token))]
    [JsonSerializable(typeof(EncryptedPayload))]
    [JsonSerializable(typeof(EncryptedPayload2value))]
    [JsonSerializable(typeof(RefreshRequest))]
    [JsonSerializable(typeof(EncryptedBinaryPayload))]
    [JsonSerializable(typeof(EncryptedBinaryPayloadV2))]
    
    // Các class trong FireBase
    [JsonSerializable(typeof(GoogleTokenResponse))]
    [JsonSerializable(typeof(GoogleAccount))]
    [JsonSerializable(typeof(FirebaseGoogleSignInPayload))]
    [JsonSerializable(typeof(FirebaseRefreshPayload))]
    [JsonSerializable(typeof(FirebaseUserLookupResponse))]
    [JsonSerializable(typeof(FirebaseUser))]
    [JsonSerializable(typeof(FirebaseRefreshResponse))]

    [JsonSerializable(typeof(ServerDloop))]
    [JsonSerializable(typeof(List<ServerDloop>))]
    [JsonSerializable(typeof(DloopShare))]
    [JsonSerializable(typeof(ShareRequest))]
    public partial class JsonServer : JsonSerializerContext
    {
    }



}
