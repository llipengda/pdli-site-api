using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Models;
using PDLiSiteAPI.Services;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace PDLiSiteAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("Api/[controller]/[action]")]
public class CodeServerController : ControllerBase
{
    private readonly ILogger<CodeServerController> _logger;
    private readonly ICodeServerService _CodeServerService;

    public CodeServerController(
        ILogger<CodeServerController> logger,
        ICodeServerService CodeServerService
    )
    {
        _logger = logger;
        _CodeServerService = CodeServerService;
    }

    [HttpPost]
    public ActionResult<StartResult> Start()
    {
        try
        {
            _CodeServerService.Start();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST /Api/CodeServer/Start ERROR");
            return BadRequest(
                new StartResult()
                {
                    Success = false,
                    Id = _CodeServerService.Id,
                    Err = ex.Message
                }
            );
        }
        var res = new StartResult() { Success = true, Id = _CodeServerService.Id };
        _logger.LogInformation("POST /Api/CodeServer/Start {res}", JsonSerializer.Serialize(res));
        return Ok(res);
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<GetLogResult> GetLog()
    {
        try
        {
            var res = new GetLogResult()
            {
                Success = true,
                Log = _CodeServerService.OutPut.ToString(),
                Err = _CodeServerService.IsRunning() ? null : "CodeServer is not Running"
            };
            _logger.LogInformation(
                "GET /Api/CodeServer/GetLog {res}",
                JsonSerializer.Serialize(res)
            );
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /Api/CodeServer/GetLog ERROR");
            return BadRequest(new GetLogResult { Success = false, Err = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<PostCmdResult> PostCmd([Required] string cmd)
    {
        try
        {
            _CodeServerService.Input(cmd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /Api/CodeServer/PostCmd?cmd={cmd} ERROR", cmd);
            return BadRequest(new PostCmdResult() { Success = false, Err = ex.Message });
        }
        var res = new PostCmdResult() { Success = true, PostedCmd = cmd };
        _logger.LogInformation(
            "POST /Api/CodeServer/PostCmd?cmd={cmd} {res}",
            cmd,
            JsonSerializer.Serialize(res)
        );
        return Ok(res);
    }

    [HttpPost]
    public ActionResult<StopResult> Stop()
    {
        try
        {
            _CodeServerService.Stop();
            var res = new StopResult() { Success = true };
            _logger.LogInformation(
                "POST /Api/CodeServer/Stop {res}",
                JsonSerializer.Serialize(res)
            );
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST /Api/CodeServer/Stop ERROR");
            return BadRequest(new StopResult() { Success = false, Err = ex.Message });
        }
    }
}
