using AdvancedSharpAdbClient;
using Dragon.Controller.GlobalControl.Helper;
using System.Diagnostics;
using System.Text.RegularExpressions;
namespace Dragon.Database.Models
{

    public enum ResolverType
    {
        None,
        Activity,
        Receiver,
        Service,
        Provider
    }
    public enum TypeAction
    {
        None,
        FullMiMeType,
        BaseMIMEType,
        WildMimeType,
        NonDataAction,
        MimeTypedAction,
        Schemes,
    }

    public class AdbCommandIntent 
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string VersionName { get; set; } = string.Empty;
        public int VersionCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
    }
    public class IntentInfo 
    {
        public int Id { get; set; }
        public string Model { get; set; } = string.Empty;
        public string PackageName { get; set; } = string.Empty;
        public string VersionName { get; set; } = string.Empty;
        public int VersionCode { get; set; }
        public int API { get; set; } // Phiên bản Android của thiết bị, để phân biệt dump Android 6 và 9
        public ResolverType ResolverType { get; set; }
        public TypeAction TypeAction { get; set; }
        // Nhiều dump chỉ có MIME hay Scheme, nhiều dump có Action + DataUri
        public string MimeActionType { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Types { get; set; } = string.Empty;
        public string Scheme { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string Authority { get; set; } = string.Empty;
        public string Categories { get; set; } = string.Empty;
        public bool AutoVerify { get; set; }
        public bool RequiresRoot { get; set; }

        private List<string> SplitToList(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new List<string>();

            return value
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .ToList();
        }
        List<string> AppendToAll(List<string> commands, List<string> values, string flag)
        {
            if (values == null || values.Count == 0)
                return commands;

            var result = new List<string>();
            foreach (var cmd in commands)
            {
                foreach (var val in values)
                {
                    result.Add($"{cmd} {flag} '{val}'");
                }
            }
            return result;
        }
        List<string> AppendToAll_NgoacDon(List<string> commands, List<string> values, string flag)
        {
            if (values == null || values.Count == 0)
                return commands;

            var result = new List<string>();
            foreach (var cmd in commands)
            {
                foreach (var val in values)
                {

                    result.Add($"{cmd} {flag} \'{val}\'");
                }
            }
            return result;
        }

        public List<AdbCommandIntent> BuildCommand()
        {
            if (!string.IsNullOrEmpty(Action) || !string.IsNullOrEmpty(Types) || !string.IsNullOrEmpty(Categories) || !string.IsNullOrEmpty(Scheme) || !string.IsNullOrEmpty(Path) || !string.IsNullOrEmpty(Authority))
            {
                //Action → Types → Scheme → Path → Authority → Categories

                var ListAdbCommand = new List<string>();

                var listAction = SplitToList(Action);
                var listScheme = SplitToList(Scheme);
                var listPath = SplitToList(Path);
                var listAuthority = SplitToList(Authority);
                var listTypes = SplitToList(Types);
                var listCategories = SplitToList(Categories);


                if (ResolverType == ResolverType.Activity)
                {
                    ListAdbCommand = AppendToAll(new List<string> { "am start" }, listAction, "-a");

                    ListAdbCommand = AppendToAll(ListAdbCommand, listTypes, "-t");

                    if (listScheme.Count > 0)
                    {
                        var listUri = new List<string>();
                        foreach (var s in listScheme)
                        {
                            // Nếu không có Authority/Path vẫn tạo URI với scheme://
                            if (listAuthority.Count == 0 && listPath.Count == 0)
                            {
                                listUri.Add($"{s}://");
                            }
                            else
                            {
                                foreach (var auth in listAuthority.DefaultIfEmpty(string.Empty))
                                {
                                    foreach (var p in listPath.DefaultIfEmpty(string.Empty))
                                    {
                                        var uri = BuildUri(s, auth, p);
                                        if (!string.IsNullOrEmpty(uri))
                                            listUri.Add(uri);
                                    }
                                }
                            }
                        }
                        ListAdbCommand = AppendToAll(ListAdbCommand, listUri, "-d");
                    }

                }
                else if (ResolverType == ResolverType.Provider)
                {
                    foreach (var act in listAction)
                    {
                        if (!string.IsNullOrWhiteSpace(act))
                            ListAdbCommand.Add($"content query --uri \"content://{act}\"");
                    }

                    if (!string.IsNullOrWhiteSpace(MimeActionType))
                    {
                        ListAdbCommand.Add($"content query --uri \"content://{MimeActionType}\"");
                    }

                }
                else if (ResolverType == ResolverType.Receiver)
                {

                    ListAdbCommand = AppendToAll(new List<string> { "am startservice" }, listAction, "-a");


                    if (!string.IsNullOrWhiteSpace(MimeActionType))
                    {
                        ListAdbCommand.Add($"am startservice -a \"{MimeActionType}\"");
                    }
                }
                else if (ResolverType == ResolverType.Service)
                {

                    ListAdbCommand = AppendToAll(new List<string> { "am startservice" }, listAction, "-a");


                    if (!string.IsNullOrWhiteSpace(MimeActionType))
                    {
                        ListAdbCommand.Add($"am startservice -a \"{MimeActionType}\"");
                    }
                }


                ListAdbCommand = AppendToAll(ListAdbCommand, listCategories, "-c");

                ListAdbCommand = AppendToAll_NgoacDon(ListAdbCommand, new List<string> { Component }, "-n");



                if (ResolverType == ResolverType.Activity)
                {
                    if (!string.IsNullOrWhiteSpace(Component))
                    {
                        ListAdbCommand.Add($"am start -n '{Component}'");
                    }

                    if (!string.IsNullOrWhiteSpace(MimeActionType))
                    {
                        ListAdbCommand.Add($"am start -a {MimeActionType}");
                    }

                }



                if (ListAdbCommand.Count == 0)
                {
                    return new List<AdbCommandIntent>();
                }
                else
                {
                    var listCommand = new List<AdbCommandIntent>();
                    foreach (var cmd in ListAdbCommand)
                    {
                        if (!string.IsNullOrEmpty(cmd))
                        {
                            listCommand.Add(new AdbCommandIntent
                            {
                                Model = Model,
                                PackageName = PackageName,
                                VersionName = VersionName,
                                VersionCode = VersionCode,
                                Name = string.Empty,
                                Command = cmd
                            });
                        }
                    }
                    return listCommand;
                }


            }
            else
            {
                var comand = BuildAdbCommand(ResolverType, TypeAction, MimeActionType, Component);
                if (!string.IsNullOrEmpty(comand))
                {
                    var newComand = new AdbCommandIntent
                    {
                        Model = Model,
                        PackageName = PackageName,
                        VersionName = VersionName,
                        VersionCode = VersionCode,
                        Name = string.Empty,
                        Command = comand
                    };
                    return new List<AdbCommandIntent> { newComand };

                }

            }

            return new List<AdbCommandIntent>();

        }



        string BuildUri(string scheme, string authority, string path)
        {
            if (string.IsNullOrEmpty(scheme))
                return string.Empty;

            var uri = scheme + "://";

            if (!string.IsNullOrEmpty(authority))
                uri += authority;

            if (!string.IsNullOrEmpty(path))
            {
                if (!path.StartsWith("/"))
                    uri += "/";
                uri += path.TrimStart('/');
            }

            return uri;
        }




        private string BuildAdbCommand(ResolverType resolver, TypeAction typeAction, string mimeOrAction, string component)
        {
            switch (resolver)
            {
                case ResolverType.Activity:
                    if (typeAction == TypeAction.NonDataAction)
                    {
                        return $"am start -a '{mimeOrAction}' -n '{component}'";
                    }
                    else if (typeAction == TypeAction.FullMiMeType || typeAction == TypeAction.WildMimeType || typeAction == TypeAction.BaseMIMEType)
                    {
                        return $"am start -t '{mimeOrAction}' -n '{component}'";
                    }
                    else if (typeAction == TypeAction.MimeTypedAction)
                    {
                        return $"am start -a '{mimeOrAction}' -n '{component}'";
                    }
                    else if (typeAction == TypeAction.Schemes)
                    {
                        return $"am start -d '{mimeOrAction}' -n '{component}'";
                    }
                    break;

                case ResolverType.Receiver:
                    if (typeAction == TypeAction.NonDataAction)
                        return $"am broadcast -a '{mimeOrAction}' -n '{component}'";
                    else
                        return $"shell am broadcast -n '{component}'";

                case ResolverType.Service:
                    if (typeAction == TypeAction.NonDataAction)
                        return $"am startservice -a '{mimeOrAction}' -n '{component}'";
                    else
                        return $"am startservice -n '{component}'";

                case ResolverType.Provider:
                    return $"content query --uri \"content://{MimeActionType}\" -n  '{component}'";
            }

            return $"# Không xác định được command cho '{component}'";
        }

        public override string ToString()
        {
            return Model + "-" + PackageName;
        }
    }

    public class SettingAppList
    {
        public static async Task<List<string>> getAllPackage(Phone phone)
        {
            var danhsachAppString = await CMD.ExecuteAdbAsync($"adb -s {phone.Serial} shell pm list packages");
            if (danhsachAppString != string.Empty && danhsachAppString.Length > 0)
            {
                var apps = danhsachAppString.Split("\r\n");
                List<string> packageNames = new List<string>();
                foreach (var app in apps)
                {
                    var value = app.Split(':');
                    if (value.Count() > 1)
                    {
                        var packageName = value[1].Trim();
                        if (!string.IsNullOrEmpty(packageName))
                        {
                            packageNames.Add(packageName);
                        }
                    }
                }
                return packageNames;
            }

            return new List<string>();
        }

        public static async Task<List<string>> getAllUserPackage(Phone phone)
        {
            var danhsachAppString = await CMD.ExecuteAdbAsync($"adb -s {phone.Serial} shell pm list packages -3");
            if (danhsachAppString != string.Empty && danhsachAppString.Length > 0)
            {
                var apps = danhsachAppString.Split("\r\n");
                List<string> packageNames = new List<string>();
                foreach (var app in apps)
                {
                    var value = app.Split(':');
                    if (value.Count() > 1)
                    {
                        var packageName = value[1].Trim();
                        if (!string.IsNullOrEmpty(packageName))
                        {
                            packageNames.Add(packageName);
                        }
                    }
                }
                return packageNames;
            }

            return new List<string>();
        }

        public static async Task<(string? DumpyString, List<IntentInfo>? IntentList)> getIntentSinglePackage(Phone phone, string packageName)
        {
            var sw = Stopwatch.StartNew();
            var dumpy = await CMD.ExecuteAdbAsync($"adb -s {phone.Serial} shell dumpsys package {packageName}");

            if (string.IsNullOrEmpty(dumpy))
            {
                sw.Stop();
                Debug.WriteLine($"Thời gian lấy intent: {sw.ElapsedMilliseconds} ms");
                return (null, null);
            }

            var vc = VersionCodeRegex.Match(dumpy);
            int versionCode = 0;
            if (vc.Success) int.TryParse(vc.Groups["code"].Value.Split('.')[0], out versionCode);

            var vn = VersionNameRegex.Match(dumpy);
            string versionName = vn.Success ? vn.Groups["name"].Value : "";

            var list = ParseActionComponent(dumpy, phone.Model, versionName, versionCode, packageName, phone.API) ?? new List<IntentInfo>();

            foreach (var item in list)
            {
                if (!string.IsNullOrEmpty(item.Action) || !string.IsNullOrEmpty(item.Types) || !string.IsNullOrEmpty(item.Categories)
                    || !string.IsNullOrEmpty(item.Scheme) || !string.IsNullOrEmpty(item.Path) || !string.IsNullOrEmpty(item.Authority))
                {
                    Debug.WriteLine($"ResolverType:{item.ResolverType}|TypeAction:{item.TypeAction}|MimeActionType:{item.MimeActionType}|Action:{item.Action}|Categories:{item.Categories}|Scheme:{item.Scheme}|Path:{item.Path}|Types:{item.Types}|Authority:{item.Authority}|Component:{item.Component}|AutoVerify:{item.AutoVerify}|RequiresRoot:{item.RequiresRoot}");
                }
            }

            sw.Stop();
            Debug.WriteLine($"Thời gian lấy intent: {sw.ElapsedMilliseconds} ms");
            return (dumpy, list);
        }
        private static readonly Regex PackageRegex = new Regex(@"package:(?<pkg>[\w.]+)", RegexOptions.CultureInvariant);
        private static readonly Regex VersionCodeRegex = new Regex(@"versionCode=(?<code>[\d.]+)", RegexOptions.CultureInvariant);
        private static readonly Regex VersionNameRegex = new Regex(@"versionName=(?<name>[\d.]+)", RegexOptions.CultureInvariant);

        public static async Task<(string? lastDump, List<IntentInfo> intentInfos)> GetALlUserApp(Phone phone)
        {
            var sw = Stopwatch.StartNew();
            var intentInfos = new List<IntentInfo>();
            string? lastDump = null;

            var listStr = await CMD.ExecuteAdbAsync($"adb -s {phone.Serial} shell pm list packages -3");
            if (string.IsNullOrWhiteSpace(listStr))
            {
                sw.Stop();
                Debug.WriteLine($"Thời gian lấy danh sách ứng dụng: {sw.ElapsedMilliseconds} ms");
                return (null, intentInfos);
            }

            var packages = listStr.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in packages)
            {
                var m = PackageRegex.Match(item);
                if (!m.Success) continue;

                string pkg = m.Groups["pkg"].Value;
                var dumpy = await CMD.ExecuteAdbAsync($"adb -s {phone.Serial} shell dumpsys package {pkg}");
                if (string.IsNullOrEmpty(dumpy)) continue;

                lastDump = dumpy; // giữ dump cuối cùng

                int versionCode = 0;
                var vc = VersionCodeRegex.Match(dumpy);
                if (vc.Success) int.TryParse(vc.Groups["code"].Value.Split('.')[0], out versionCode); // versionCode thường là số nguyên

                var vn = VersionNameRegex.Match(dumpy);
                string versionName = vn.Success ? vn.Groups["name"].Value : string.Empty;

                var list = ParseActionComponent(dumpy, phone.Model, versionName, versionCode, pkg, phone.API);
                if (list?.Count > 0) intentInfos.AddRange(list);
            }

            sw.Stop();
            Debug.WriteLine($"Thời gian lấy danh sách ứng dụng: {sw.ElapsedMilliseconds} ms");
            return (lastDump, intentInfos);
        }
        static string BetweenQuotes(string line)
        {
            var i1 = line.IndexOf('"');
            var i2 = line.LastIndexOf('"');
            if (i1 >= 0 && i2 > i1)
                return line.Substring(i1 + 1, i2 - i1 - 1);
            return line;
        }

        private static readonly Regex PathRegex = new Regex(@"([A-Za-z0-9_.\-\\/*]+)}", RegexOptions.CultureInvariant);
        public static string getPath(string line)
        {
            var m = PathRegex.Match(line);
            return m.Success ? m.Groups[1].Value : string.Empty;
        }

        public static bool Check(IntentInfo intentInfo)
        {
            if (intentInfo.MimeActionType != string.Empty && intentInfo.Component != string.Empty)
            {
                return true;
            }
            return false;
        }

        private static readonly Regex HexPathRegex = new Regex(@"^[0-9a-f]+\s+([a-zA-Z0-9_.]+/[a-zA-Z0-9_.$]+)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        // dùng
        public static List<IntentInfo> ParseActionComponent(string dump, string Model, string VersionName, int VersionCode, string packageName, int API)
        {
            var results = new List<IntentInfo>();
            using var reader = new StringReader(dump);

            ResolverType currentResolver = ResolverType.None;
            TypeAction currentTypeAction = TypeAction.None;

            IntentInfo intentInfo = new IntentInfo();
            string LastMimeActionType = string.Empty;
            string? rawLine;
            while ((rawLine = reader.ReadLine()) != null)
            {
                var line = rawLine.Trim();
                if (line.Length == 0) continue;

                if (line.StartsWith("Activity Resolver Table:")) { currentResolver = ResolverType.Activity; continue; }
                if (line.StartsWith("Receiver Resolver Table:")) { currentResolver = ResolverType.Receiver; continue; }
                if (line.StartsWith("Service Resolver Table:")) { currentResolver = ResolverType.Service; continue; }
                if (line.StartsWith("Provider Resolver Table:")) { currentResolver = ResolverType.Provider; continue; }
                if (line.StartsWith("Full MIME Types:")) { currentTypeAction = TypeAction.FullMiMeType; if (Check(intentInfo)) results.Add(intentInfo); intentInfo = new IntentInfo() { ResolverType = currentResolver, TypeAction = TypeAction.FullMiMeType }; continue; }
                if (line.StartsWith("Base MIME Types:")) { currentTypeAction = TypeAction.BaseMIMEType; if (Check(intentInfo)) results.Add(intentInfo); intentInfo = new IntentInfo() { ResolverType = currentResolver, TypeAction = TypeAction.BaseMIMEType }; continue; }
                if (line.StartsWith("Wild MIME Types:")) { currentTypeAction = TypeAction.WildMimeType; if (Check(intentInfo)) results.Add(intentInfo); intentInfo = new IntentInfo() { ResolverType = currentResolver, TypeAction = TypeAction.WildMimeType }; continue; }
                if (line.StartsWith("Non-Data Actions:")) { currentTypeAction = TypeAction.NonDataAction; if (Check(intentInfo)) results.Add(intentInfo); intentInfo = new IntentInfo() { ResolverType = currentResolver, TypeAction = TypeAction.NonDataAction }; continue; }
                if (line.StartsWith("MIME Typed Actions:")) { currentTypeAction = TypeAction.MimeTypedAction; if (Check(intentInfo)) results.Add(intentInfo); intentInfo = new IntentInfo() { ResolverType = currentResolver, TypeAction = TypeAction.MimeTypedAction }; continue; }
                if (line.StartsWith("Schemes:")) { currentTypeAction = TypeAction.Schemes; if (Check(intentInfo)) results.Add(intentInfo); intentInfo = new IntentInfo() { ResolverType = currentResolver, TypeAction = TypeAction.Schemes }; continue; }


                if (line.StartsWith("Action:", StringComparison.Ordinal))
                {
                    var action = BetweenQuotes(line);
                    if (!string.IsNullOrEmpty(intentInfo.Action) && intentInfo.Action.Length > 1)
                    {
                        intentInfo.Action += "," + action;
                    }
                    else
                    {
                        intentInfo.Action += action;
                    }
                    continue;
                }
                if (line.StartsWith("Category:", StringComparison.Ordinal))
                {
                    var category = BetweenQuotes(line);
                    if (!string.IsNullOrEmpty(intentInfo.Categories) && intentInfo.Categories.Length > 1)
                    {
                        intentInfo.Categories += "," + category;
                    }
                    else
                    {
                        intentInfo.Categories += category;
                    }
                    continue;
                }
                if (line.StartsWith("Scheme:", StringComparison.Ordinal))
                {
                    var scheme = BetweenQuotes(line);
                    if (!string.IsNullOrEmpty(intentInfo.Scheme) && intentInfo.Scheme.Length > 1)
                    {
                        intentInfo.Scheme += "," + scheme;
                    }
                    else
                    {
                        intentInfo.Scheme += scheme;
                    }

                    continue;
                }
                if (line.StartsWith("Authority:", StringComparison.Ordinal))
                {
                    var Authority = BetweenQuotes(line);
                    if (!string.IsNullOrEmpty(intentInfo.Authority) && intentInfo.Authority.Length > 1)
                    {
                        intentInfo.Authority += "," + Authority;
                    }
                    else
                    {
                        intentInfo.Authority += Authority;
                    }
                    continue;
                }
                if (line.StartsWith("Path:", StringComparison.Ordinal))
                {
                    var stringpath = BetweenQuotes(line);
                    if (string.IsNullOrEmpty(stringpath)) continue;
                    var path = getPath(line);
                    if (!string.IsNullOrEmpty(intentInfo.Path) && intentInfo.Path.Length > 1)
                    {
                        intentInfo.Path += "," + path;
                    }
                    else
                    {
                        intentInfo.Path += path;
                    }
                    continue;
                }
                if (line.StartsWith("Type:", StringComparison.Ordinal))
                {
                    var types = BetweenQuotes(line);
                    if (!string.IsNullOrEmpty(intentInfo.Types) && intentInfo.Types.Length > 1)
                    {
                        intentInfo.Types += "," + types;
                    }
                    else
                    {
                        intentInfo.Types += types;
                    }
                    continue;
                }
                if (line.StartsWith("AutoVerify=", StringComparison.Ordinal))
                {
                    intentInfo.AutoVerify = line.Substring("AutoVerify=".Length).Equals("true", StringComparison.OrdinalIgnoreCase);
                    continue;
                }

                var m = HexPathRegex.Match(line);
                if (m.Success)
                {
                    if (string.IsNullOrEmpty(intentInfo.Component))
                    {
                        intentInfo.Component = m.Groups[1].Value;
                    }
                    else if (!string.IsNullOrEmpty(intentInfo.Component) && !string.IsNullOrEmpty(intentInfo.MimeActionType))
                    {
                        var newIntent = new IntentInfo()
                        {
                            ResolverType = currentResolver,
                            TypeAction = currentTypeAction,
                            MimeActionType = intentInfo.MimeActionType,
                            Action = intentInfo.Action,
                            Scheme = intentInfo.Scheme,
                            Authority = intentInfo.Authority,
                            Path = intentInfo.Path,
                            Component = intentInfo.Component,
                            PackageName = packageName,
                            Model = Model,
                            VersionName = VersionName,
                            VersionCode = VersionCode,
                            RequiresRoot = false,
                            API = API,
                            AutoVerify = intentInfo.AutoVerify,
                            Categories = intentInfo.Categories,
                            Types = intentInfo.Types,
                        };

                        results.Add(newIntent);

                        intentInfo = new IntentInfo();
                        intentInfo.PackageName = packageName;
                        intentInfo.Model = Model;
                        intentInfo.VersionName = VersionName;
                        intentInfo.VersionCode = VersionCode;
                        intentInfo.Component = m.Groups[1].Value;
                    }
                    else if (!string.IsNullOrEmpty(intentInfo.Component) && string.IsNullOrEmpty(intentInfo.MimeActionType))
                    {
                        var newIntent = new IntentInfo()
                        {
                            ResolverType = currentResolver,
                            TypeAction = currentTypeAction,
                            MimeActionType = intentInfo.MimeActionType,
                            Action = intentInfo.Action,
                            Scheme = intentInfo.Scheme,
                            Authority = intentInfo.Authority,
                            Path = intentInfo.Path,
                            Component = intentInfo.Component,
                            PackageName = packageName,
                            Model = Model,
                            VersionName = VersionName,
                            VersionCode = VersionCode,
                            RequiresRoot = false,
                            API = API,
                            AutoVerify = intentInfo.AutoVerify,
                            Categories = intentInfo.Categories,
                            Types = intentInfo.Types,
                        };
                        if (API <= 23)
                            newIntent.MimeActionType = LastMimeActionType;

                        results.Add(newIntent);

                        intentInfo = new IntentInfo();
                        intentInfo.PackageName = packageName;
                        intentInfo.Model = Model;
                        intentInfo.VersionName = VersionName;
                        intentInfo.VersionCode = VersionCode;
                        intentInfo.Component = m.Groups[1].Value;
                    }

                    continue;
                }

                if (line.Contains(":"))
                {
                    intentInfo.MimeActionType = line.TrimEnd(':');
                    LastMimeActionType = line.TrimEnd(':');
                }


                if (line.StartsWith("Permissions:") || line.StartsWith("Packages:") || line.StartsWith("Key Set Manager:") || line.StartsWith("Registered ContentProviders:"))
                {                   
                    break;
                }
            }
            CheckAndEditIntent(results, Model, packageName, VersionName, VersionCode, API);
            return results;
        }
        public static void CheckAndEditIntent(List<IntentInfo> results, string model, string packageName, string versionName, int versionCode, int Api)
        {
            if (results != null && results.Count() > 0)
            {
                var danhsachcomdnNoneValue = results.Where(c => string.IsNullOrEmpty(c.Model) || string.IsNullOrEmpty(c.PackageName)
                || string.IsNullOrEmpty(c.VersionName) || c.VersionCode == 0).ToList();
                if (danhsachcomdnNoneValue != null && danhsachcomdnNoneValue.Count() > 0)
                {
                    foreach (var item in danhsachcomdnNoneValue)
                    {
                        item.Model = model;
                        item.PackageName = packageName;
                        item.VersionName = versionName;
                        item.VersionCode = versionCode;
                        item.API = Api;
                    }
                }

            }

        }
    }




}