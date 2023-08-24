namespace PDLiSiteAPI.Models;

/// <summary>
/// The result when trying to start a service
/// </summary>
public class StartResult
{
    public bool Success { get; set; }

    /// <summary>
    /// The pid of the service
    /// </summary>
    public int? Id { get; set; }

    public string? Err { get; set; }
}

/// <summary>
/// The result when trying to stop(kill) a service
/// </summary>
public class StopResult
{
    public bool Success { get; set; }

    public string? Err { get; set; }
}

/// <summary>
/// The result when trying to get log of a service
/// </summary>
public class GetLogResult
{
    /// <summary>
    /// true if the service is running else false
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// current log of the service if the service is running else previous log
    /// </summary>
    public string? Log { get; set; }

    public string? Err { get; set; }
}

/// <summary>
/// The result when trying to post a command to a service
/// </summary>
public class PostCmdResult
{
    public bool Success { get; set; }

    /// <summary>
    /// The command you posted just now
    /// </summary>
    public string? PostedCmd { get; set; }

    public string? Err { get; set; }
}
