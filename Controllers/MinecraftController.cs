using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PDLiSiteAPI.Services;
using PDLiSiteAPI.Models;
using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace PDLiSiteAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("Api/[controller]/[action]")]
public class MinecraftController : ControllerBase
{
    private readonly ILogger<MinecraftController> _logger;
    private readonly IMinecraftService _minecraftService;

    public MinecraftController(
        ILogger<MinecraftController> logger,
        IMinecraftService minecraftService
    )
    {
        _logger = logger;
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
            _logger.LogError(ex, "POST /Api/Minecraft/Start ERROR");
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
        _logger.LogInformation("POST /Api/Minecraft/Start {res}", JsonSerializer.Serialize(res));
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
            _logger.LogInformation(
                "GET /Api/Minecraft/GetLog {res}",
                JsonSerializer.Serialize(res)
            );
            return Ok(res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /Api/Minecraft/GetLog ERROR");
            return BadRequest(new GetLogResult { Success = false, Err = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<PostCmdResult> PostCmd([Required] string cmd)
    {
        try
        {
            _minecraftService.Input(cmd);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GET /Api/Minecraft/PostCmd?cmd={cmd} ERROR", cmd);
            return BadRequest(new PostCmdResult() { Success = false, Err = ex.Message });
        }
        var res = new PostCmdResult() { Success = true, PostedCmd = cmd };
        _logger.LogInformation(
            "POST /Api/Minecraft/PostCmd?cmd={cmd} {res}",
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
            _minecraftService.Stop();
            var res = new StopResult() { Success = true };
            _logger.LogInformation("POST /Api/Minecraft/Stop {res}", JsonSerializer.Serialize(res));
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "POST /Api/Minecraft/Stop ERROR");
            return BadRequest(new StopResult() { Success = false, Err = ex.Message });
        }
    }
}
