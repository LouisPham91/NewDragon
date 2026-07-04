using DirectN;
using DirectN.Extensions.Com;
using DirectN.Extensions.Utilities;
using System.Reflection;
using WebView2;
using WebView2.Utilities;

namespace Dragon.ControlHelper.WebSite;

public class WebViewWindow : Window
{
    private readonly HostObject _hostObject = new();
    private ComObject<ICoreWebView2Controller>? _controller;

    // giữ System.Drawing.Icon sống để handle không bị GC
    private static readonly System.Drawing.Icon _sysIcon = LoadSysIcon();

    // bọc handle đó thành DirectN.Icon (không destroy)
    private static readonly DirectN.Extensions.Utilities.Icon _directIcon = DirectN.Extensions.Utilities.Icon.FromHandle(_sysIcon.Handle, destroyHandleOnDispose: false)!;

    private static System.Drawing.Icon LoadSysIcon()
    {
        var asm = typeof(WebViewWindow).Assembly;
        var s = asm.GetManifestResourceStream("Dragon.WebView2.ico")
                ?? throw new FileNotFoundException("Thiếu resource Dragon.WebView2.ico");
        return new System.Drawing.Icon(s);
    }

    // 1. Icon cho class window (đăng ký lúc RegisterClass)
    protected override DirectN.Extensions.Utilities.Icon? LoadCreationIcon() => _directIcon;

    public WebViewWindow(string? title = null) : base(title)
    {
        // this checks WebView2Loader.dll is present somewhere (file path or embedded resource)
        //WebView2Utilities.Initialize(Assembly.GetEntryAssembly());

        WebView2Utilities.Initialize(typeof(WebViewWindow).Assembly);

        // 2. Set lại cho chắc sau khi có HWND
        this.BigIconHandle = _directIcon.Handle;   // WM_SETICON ICON_BIG
        this.SmallIconHandle = _directIcon.Handle; // WM_SETICON ICON_SMALL

        // this checks WebView2 itself is installed
        var browserVersion = WebView2Utilities.GetAvailableCoreWebView2BrowserVersionString();
        if (browserVersion != null)
        {
            Text = $"{Text} - WebView2 V{browserVersion}";
        }
        else
        {
            Text = $"{Text} - WebView2 was not found";
        }

        WebView2.Functions.CreateCoreWebView2EnvironmentWithOptions(PWSTR.Null, PWSTR.Null, null!,
            new CoreWebView2CreateCoreWebView2EnvironmentCompletedHandler((result, env) =>
            {
                env.CreateCoreWebView2Controller(Handle, new CoreWebView2CreateCoreWebView2ControllerCompletedHandler((result, controller) =>
                {
                    _controller = new ComObject<ICoreWebView2Controller>(controller);
                    controller.put_Bounds(ClientRect).ThrowOnError();
                    controller.get_CoreWebView2(out var webView2).ThrowOnError();

                    // this is for a full support of .NET Task or Task<T> methods
                    // unfortunately, uses undocumented (private) interfaces
                    if (webView2 is ICoreWebView2PrivatePartial partial)
                    {
                        partial.AddHostObjectHelper(new WebViewHostObjectHelper()).ThrowOnError();
                        _hostObject.ContinueOnAsync = true;
                        _hostObject.OneStepInvoke = true;
                    }

                    //webView2.OpenDevToolsWindow();

                    _hostObject.ClockTick += (s, e) =>
                    {
                        Text = $"Dragon Google Login: {e}";
                    };

                    // get IUnknown from the host object and wrap it in a VARIANT
                    DirectN.Extensions.Com.ComObject.WithComInstance(_hostObject, unk =>
                    {
                        using var variant = new Variant(unk, VARENUM.VT_UNKNOWN);
                        var detached = variant.Detached;
                        webView2.AddHostObjectToScript(PWSTR.From("dotnet"), ref detached).ThrowOnError();
                    }, true);

                    var asm = Assembly.GetExecutingAssembly();
                    const string resourceName = "Dragon.Index.html";

                    using var stream = asm.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException($"Không thấy resource '{resourceName}'. Resource hiện có: {string.Join(", ", asm.GetManifestResourceNames())}");
                    using var reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                    var html = reader.ReadToEnd();

                    // load index.html from the current assembly
                    //var html = System.Text.Encoding.UTF8.GetString(Assembly.GetExecutingAssembly().LoadFromResource(GetType().Namespace + ".Index.html"));
                    webView2.NavigateToString(PWSTR.From(html));
                }));
            }));
    }

    protected override bool OnResized(WindowResizedType type, SIZE size)
    {
        _controller?.Object.put_Bounds(ClientRect).ThrowOnError();
        return base.OnResized(type, size);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _controller?.Dispose();
        }
        base.Dispose(disposing);
    }
}
