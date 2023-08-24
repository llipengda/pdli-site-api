using Microsoft.AspNetCore.SignalR;
using PDLiSiteAPI.Hubs;
using PDLiSiteAPI.Models;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace PDLiSiteAPI.Services;

public interface IMinecraftService
{
    int? Id { get; }

    StringBuilder OutPut { get; }

    void Input(string statement);

    bool IsRunning();

    void Start();

    void Stop();

    Task StartAsync();

    Task StopAsync();
}

public class MinecraftService : IMinecraftService
{
    private readonly IHubContext<LogHub> _hubContext;

    private readonly IConfiguration _configuration;

    private readonly ILogger<MinecraftService> _logger;

    private static Process? _process = null;

    public StringBuilder OutPut { get; private set; } = new StringBuilder();

    public int? Id { get; private set; } = null;

    public MinecraftService(
        IHubContext<LogHub> hubContext,
        IConfiguration configuration,
        ILogger<MinecraftService> logger
    )
    {
        _hubContext = hubContext;
        _configuration = configuration;
        _logger = logger;
    }

    public void Start()
    {
        if (IsRunning())
        {
            throw new InvalidOperationException("Minecraft is already running!");
        }
        OutPut.Clear();
        _process = new()
        {
            StartInfo = new ProcessStartInfo()
            {
                WorkingDirectory = _configuration["Minecraft:WorkingDirectory"],
                FileName = _configuration["Minecraft:FileName"],
                Arguments = _configuration["Minecraft:Arguments"],
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            }
        };
        _process.OutputDataReceived += (sender, args) =>
        {
            OutPut.Append($"[{DateTime.Today:yy-MM-dd}] ");
            OutPut.AppendLine(args.Data);
            _hubContext.Clients.All.SendAsync(
                "receiveLog",
                new Log()
                {
                    Service = "Minecraft",
                    Message = $"[{DateTime.Today:yy-MM-dd}] {args.Data}"
                }
            );
        };
        _process.Start();
        _process.BeginOutputReadLine();
        Id = _process.Id;
        _logger.LogInformation(
            "Minecraft started at [{time}] with id = {id}",
            DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
            _process.Id
        );
    }

    public bool IsRunning()
    {
        try
        {
            return _process is not null
                && Id is not null
                && Process.GetProcessById((int)Id) is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void Input(string statement)
    {
        if (!IsRunning())
        {
            throw new InvalidOperationException("Minecraft is not running!");
        }
        _process?.StandardInput.WriteLine(statement);
    }

    public void Stop()
    {
        if (!IsRunning())
        {
            throw new InvalidOperationException("Minecraft is not running!");
        }
        Process.Start("kill", $"-2 {Id}").WaitForExit();
        _process?.WaitForExit();
        _logger.LogWarning(
            "Minecraft exited at [{time}] with code {code}",
            DateTime.Now.ToString("yy-MM-dd HH:mm:ss"),
            _process?.ExitCode
        );
        _process = null;
    }

    public async Task StartAsync()
    {
       await Task.Run(() => Start());
    }

    public async Task StopAsync()
    {
       await Task.Run(() => Stop());
    }
}
