using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MissionFlow.Infrastructure.Persistence;

namespace MissionFlow.Api.Controllers;

[ApiController]
[Route("health")]
ApiExplorerSettings(IgnoreApi = true)]
public sealed class HealthController : ControllerBase
{
    private readonly MissionFlowDbContext _context;
    private readonly Logger<HealthController> _logger;

    public HealthController(MissionFlowDbContext context, Logger<HealthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        string dbStatus;
        try
        {
            dbStatus = await _context.Database.CanConnectAsync() ? "connected" : "disconnected";
        }
        catch
        {
            dbStatus = "error";
        }

        return Ok(new {
            status = dbStatus == "connected" ? "healthy" : "degraded",
            timestamp = DateTime.UtcNow,
            version = "1.0.0",
            database = dbStatus
        });
    }
}
