using Microsoft.AspNetCore.SignalR;

namespace PDLiSiteAPI.Hubs;

public class LogHub : Hub
{
    private readonly ILogger<LogHub> _logger;

    public LogHub(ILogger<LogHub> logger)
    {
        _logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        _logger.LogInformation("[SignalR] LogHub is Connected");
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("[SignalR] LogHub is Disonnected");
        return base.OnDisconnectedAsync(exception);
    }
}
