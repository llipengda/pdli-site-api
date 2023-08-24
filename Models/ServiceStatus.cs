namespace PDLiSiteAPI.Models;

/// <summary>
/// The result when trying to get the status of a service
/// </summary>
public class ServiceStatus
{
    public bool Success { get; set; }

    public string Name { get; set; }

    /// <summary>
    /// status of the service - "running" or "stopped"
    /// </summary>
    public string? Status { get; set; }

    public string? Err { get; set; }
}
