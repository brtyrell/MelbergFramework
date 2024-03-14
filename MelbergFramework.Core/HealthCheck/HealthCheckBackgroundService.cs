using System.Net;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MelbergFramework.Core.HealthCheck;

public class HealthCheckBackgroundService : BackgroundService
{
    private readonly ILogger<HealthCheckBackgroundService> _logger;
    private readonly IHealthCheckChecker _checker;
    public HealthCheckBackgroundService(
        IHealthCheckChecker checker,
        ILogger<HealthCheckBackgroundService> logger)
    {
        _checker = checker;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken token)
    {
        if(!HttpListener.IsSupported)
        {
            _logger.LogError("Oops");
        }
        _logger.LogInformation("Beginning Health Checker");
        try
        {
            using(var listener = new HttpListener())
            {
                await ListenerLoop(listener, token);
            };
        }
        catch (System.Exception ex)
        {
            throw ex;
        }
    }

    private async Task ListenerLoop(
            HttpListener listener,
            CancellationToken token)
    {
        listener.Prefixes.Add("http://+:8180/health/");
        listener.Start();
        while(!token.IsCancellationRequested)
        {
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest req = context.Request;
            
            string data = string.Empty;
            using HttpListenerResponse resp = context.Response;
            if(req.HttpMethod != WebRequestMethods.Http.Get ||
               req?.Url?.PathAndQuery != "/health")
            {
                resp.StatusCode = 404;
                data = "Oops";
            }
            else
            {
                (data, resp.StatusCode ) = await CheckHealth(_checker);
            }

            resp.Headers.Set("Content-Type", "text/plain");

            byte[] buffer = Encoding.UTF8.GetBytes(data);
            resp.ContentLength64 = buffer.Length;

            using Stream ros = resp.OutputStream;
            ros.Write(buffer, 0, buffer.Length);
        }
        listener.Stop();
    }

    private async Task<(string,int)> CheckHealth(IHealthCheckChecker checker)
    {
        string data = string.Empty;
        var status = await checker.IsOk();

        if(status)
        {
            data = "Health Check Succeeded";
            _logger.LogInformation(data);
            return (data,200);
        }
        else
        {
            data = "Health Check Failed";
            _logger.LogError(data);
            return (data,500);
        }
    }
}
