using AdvancedSharpAdbClient.Models;
using Dragon.Controller.TaskDeviceManager.Model.DatabaseColumn;
using Dragon.Controller.TaskDeviceManager.Model.HttpResponse;
using Dragon.Controller.TaskDeviceManager.Model.Input;
using Dragon.Controller.TaskDeviceManager.Model.Mouse;
using Dragon.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.DesignView.Public
{
    public static class EnumCache
    {
        public static readonly CompareType[] CompareTypes = [CompareType.None, CompareType.Greater, CompareType.Smaller, CompareType.Equal, CompareType.Any];
        public static readonly UseRegexMode[] UseRegexModes = [UseRegexMode.IsUseOne, UseRegexMode.IsUseTwo];
        public static readonly TypeOption[] TypeOptions = [TypeOption.Typing, TypeOption.CopyPaste];
        public static readonly Language[] Languages = [Language.English, Language.TiengViet];
        public static readonly Direction[] Directions = [Direction.Up, Direction.Down, Direction.Left, Direction.Right];
        public static readonly AccStatus[] AccStatuses = [AccStatus.None, AccStatus.Live, AccStatus.Died, AccStatus.Pending, AccStatus.CheckPoint, AccStatus.Locked, AccStatus.Disabled, AccStatus.Suspended, AccStatus.Restricted, AccStatus.Verified, AccStatus.Unknown];
        public static readonly Gender[] Genders = [Gender.Male, Gender.Female, Gender.Other];
        public static readonly Marital[] Maritals = [Marital.Single, Marital.Married, Marital.Divorced, Marital.Widowed, Marital.Other];
        public static readonly PhoneMode[] PhoneModes = [PhoneMode.USB, PhoneMode.WIFI, PhoneMode.UATX, PhoneMode.WATX, PhoneMode.UHDI, PhoneMode.WHDI, PhoneMode.ACC, PhoneMode.ALL];
        public static readonly DeviceState[] States = [DeviceState.Online, DeviceState.Connecting, DeviceState.Offline, DeviceState.Unknown, DeviceState.BootLoader, DeviceState.Recovery, DeviceState.Download, DeviceState.Authorizing, DeviceState.Unauthorized, DeviceState.Host, DeviceState.NoPermissions, DeviceState.Sideload];
        public static readonly Langeuage[] Langeuages = [Langeuage.None, Langeuage.Vietnamese, Langeuage.English, Langeuage.ATX_Unicode];
        public static readonly ResolverType[] ResolverTypes = [ResolverType.None, ResolverType.Activity, ResolverType.Receiver, ResolverType.Service, ResolverType.Provider];
        public static readonly TypeAction[] TypeActions = [TypeAction.None, TypeAction.FullMiMeType, TypeAction.BaseMIMEType, TypeAction.WildMimeType, TypeAction.NonDataAction, TypeAction.MimeTypedAction, TypeAction.Schemes];
    }
}
