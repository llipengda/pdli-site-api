using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;
using System.ComponentModel.DataAnnotations;

namespace PDLiSiteAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("Api/[controller]/[action]")]
public class CodeServerController : ControllerBase
{
    private readonly ICodeServerService _CodeServerService;

    public CodeServerController(ICodeServerService CodeServerService)
    {
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
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(new GetLogResult { Success = false, Err = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<PostCmdResult> PostCmd(string cmd)
    {
        try
        {
            _CodeServerService.Input(cmd);
        }
        catch (Exception ex)
        {
            return BadRequest(new PostCmdResult() { Success = false, Err = ex.Message });
        }
        var res = new PostCmdResult() { Success = true, PostedCmd = cmd };
        return Ok(res);
    }

    [HttpPost]
    public ActionResult<StopResult> Stop()
    {
        try
        {
            _CodeServerService.Stop();
            var res = new StopResult() { Success = true };
            return res;
        }
        catch (Exception ex)
        {
            return BadRequest(new StopResult() { Success = false, Err = ex.Message });
        }
    }
}
