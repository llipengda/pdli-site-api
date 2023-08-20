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

    [HttpPost]
    public ActionResult<StartResult> Start()
    {
        try
        {
            _minecraftService.Start();
        }
        catch (Exception ex)
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

    [HttpGet]
    [AllowAnonymous]
    public ActionResult<GetLogResult> GetLog()
    {
        try
        {
            var res = new GetLogResult()
            {
                Success = true,
                Log = _minecraftService.OutPut.ToString(),
                Err = _minecraftService.IsRunning() ? null : "Minecraft is not Running"
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
            _minecraftService.Input(cmd);
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
            _minecraftService.Stop();
            return new StopResult() { Success = true };
        }
        catch (Exception ex)
        {
            return BadRequest(new StopResult() { Success = false, Err = ex.Message });
        }
    }
}
