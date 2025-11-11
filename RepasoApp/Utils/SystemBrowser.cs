using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityModel.OidcClient.Browser;

namespace RepasoApp.Utils;

public class SystemBrowser : IBrowser
{
    private readonly string _path;
    private readonly int _port;

    public SystemBrowser(int port, string path = null)
    {
        _port = port;
        _path = path;
    }

    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        using var listener = new HttpListener();
        listener.Prefixes.Add($"http://127.0.0.1:{_port}/");
        listener.Start();

        Process.Start(new ProcessStartInfo
        {
            FileName = options.StartUrl,
            UseShellExecute = true
        });

        var context = await listener.GetContextAsync();
        string response = "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Login completado. Puedes cerrar esta ventana.</body></html>";

        var buffer = System.Text.Encoding.UTF8.GetBytes(response);
        context.Response.ContentLength64 = buffer.Length;
        await context.Response.OutputStream.WriteAsync(buffer);
        context.Response.OutputStream.Close();

        return new BrowserResult
        {
            Response = context.Request.Url.ToString(),
            ResultType = BrowserResultType.Success
        };
    }
}