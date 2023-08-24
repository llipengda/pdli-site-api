using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Models;
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

    /// <summary>
    /// Start the CodeServer Service
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Failed to start</response>
    /// <response code="401">Unauthorized</response>
    /// <returns>a <see cref="StartResult"/> object</returns>
    [HttpPost]
    public ActionResult<StartResult> Start()
    {
        try
        {
            _CodeServerService.StartAsync();
        }
        catch (InvalidOperationException ex)
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

    /// <summary>
    /// Get the log of CodeServer Service
    /// </summary>
    /// <response code="200">Ok(Maybe)</response>
    /// <response code="400">Failed to get log</response>
    /// <returns>a <see cref="GetLogResult"/> object</returns>
    [HttpGet]
    [AllowAnonymous]
    public ActionResult<GetLogResult> GetLog()
    {
        try
        {
            bool isRunning = _CodeServerService.IsRunning();
            var res = new GetLogResult()
            {
                Success = isRunning,
                Log = _CodeServerService.OutPut.ToString(),
                Err = isRunning ? null : "CodeServer is not running"
            };
            return Ok(res);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GetLogResult { Success = false, Err = ex.Message });
        }
    }

    /// <summary>
    /// Post a command to CodeServer Service
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Failed to post command</response>
    /// <response code="401">Unauthorized</response>
    /// <returns>a <see cref="PostCmdResult"/> object</returns>
    [HttpPost]
    public ActionResult<PostCmdResult> PostCmd([Required] string cmd)
    {
        try
        {
            _CodeServerService.Input(cmd);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new PostCmdResult() { Success = false, Err = ex.Message });
        }
        var res = new PostCmdResult() { Success = true, PostedCmd = cmd };
        return Ok(res);
    }

    /// <summary>
    /// Stop the CodeServer Service
    /// </summary>
    /// <response code="200">Ok</response>
    /// <response code="400">Failed to stop</response>
    /// <response code="401">Unauthorized</response>
    /// <returns>a <see cref="StopResult"/> object</returns>
    [HttpPost]
    public ActionResult<StopResult> Stop()
    {
        try
        {
            _CodeServerService.StopAsync();
            return new StopResult() { Success = true };
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new StopResult() { Success = false, Err = ex.Message });
        }
    }
}
