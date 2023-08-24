using PDLiSiteAPI.Models;
using System.Diagnostics;

namespace PDLiSiteAPI.Services;

public interface IServiceStatusService
{
    ServiceStatus GetServiceStatus(string service);

    Task<ServiceStatus> GetServiceStatusAsync(string service);
}

public class ServiceStatusService : IServiceStatusService
{
    public ServiceStatus GetServiceStatus(string service)
    {
        var sh = new Process();
        sh.StartInfo.FileName = "ps";
        sh.StartInfo.Arguments = "aux";
        sh.StartInfo.UseShellExecute = false;
        sh.StartInfo.RedirectStandardOutput = true;
        sh.Start();
        sh.WaitForExit();
        bool isRunning = sh.StandardOutput.ReadToEnd().Contains(service);
        sh.Close();
        return new ServiceStatus()
        {
            Success = true,
            Name = service,
            Status = isRunning ? "running" : "stopped"
        };
    }

    public async Task<ServiceStatus> GetServiceStatusAsync(string service)
    {
        return await Task.Run(() => GetServiceStatus(service));
    }
}
