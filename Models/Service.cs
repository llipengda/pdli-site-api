namespace PDLiSiteAPI.Models;

public class StartResult
{
    public bool Success { get; set; }
    public int? Id { get; set; }
    public string? Err { get; set; }
}

public class StopResult
{
    public bool Success { get; set; }
    public string? Err { get; set; }
}

public class GetLogResult
{
    public bool Success { get; set; }
    public string? Log { get; set; }
    public string? Err { get; set; }
}

public class PostCmdResult
{
    public bool Success { get; set; }
    public string? PostedCmd { get; set; }
    public string? Err { get; set; }
}
