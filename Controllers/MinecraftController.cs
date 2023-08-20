using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;
using PDLiSiteAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace PDLiSiteAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("Api/[controller]/[action]")]
public class MinecraftController : ControllerBase
{
    private readonly IMinecraftService _minecraftService;

    public MinecraftController(IMinecraftService minecraftService)
    {
        _minecraftService = minecraftService;
    }

    /// <summary>
    /// Start the Minecraft Service
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
            _minecraftService.Start();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(
                new StartResult()
                {
                    Success = false,
                    Id = _minecraftService.Id,
                    Err = ex.Message
                }
            );
        }
        var res = new StartResult() { Success = true, Id = _minecraftService.Id };
        return Ok(res);
    }

    /// <summary>
    /// Get the log of Minecraft Service
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
            bool isRunning = _minecraftService.IsRunning();
            var res = new GetLogResult()
            {
                Success = isRunning,
                Log = _minecraftService.OutPut.ToString(),
                Err = isRunning ? null : "Minecraft is not running"
            };
            return Ok(res);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new GetLogResult { Success = false, Err = ex.Message });
        }
    }

    /// <summary>
    /// Post a command to Minecraft Service
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
            _minecraftService.Input(cmd);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new PostCmdResult() { Success = false, Err = ex.Message });
        }
        var res = new PostCmdResult() { Success = true, PostedCmd = cmd };
        return Ok(res);
    }

    /// <summary>
    /// Stop the Minecraft Service
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
            _minecraftService.Stop();
            return new StopResult() { Success = true };
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new StopResult() { Success = false, Err = ex.Message });
        }
    }
}
