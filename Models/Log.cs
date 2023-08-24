namespace PDLiSiteAPI.Models;

/// <summary>
/// The Log object when sending real-time logs
/// </summary>
public class Log
{
    /// <summary>
    /// The name of the service
    /// </summary>
    public string Service { get; set; }

    /// <summary>
    /// the log
    /// </summary>
    public string Message { get; set; }
}
