using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MissionFlow.Application.Common.DTOs;
using MissionFlow.Application.Missions.Commands;
using MissionFlow.Application.Missions.Queries;

namespace MissionFlow.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/missions")]
[ApiVersion("1.0")]
[Authorize]
public sealed class MissionsController : ControllerBase
{
    private readonly IMediator _mediator;
    public MissionsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMissionCommand command)
    {
        var mission = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = mission.Id }, ApiResponse<MissionDto>.Ok(mission, "Mission créée."));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var mission = await _mediator.Send(new GetMissionByIdQuery(id));
        return Ok(ApiResponse<MissionDto>.Ok(mission));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetMissionsQuery(page, pageSize, status));
        return Ok(result);
    }

    [HttpGet("mine")]
    public async Task<IActionResult> GetMine([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetMyMissionsQuery(page, pageSize));
        return Ok(result);
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id)
    {
        var mission = await _mediator.Send(new SubmitMissionCommand(id));
        return Ok(ApiResponse<MissionDto>.Ok(mission, "Mission soumise."));
    }

    [HttpPost("{id:guid}/approve")]
    [Authorize(Roles = "Manager,Director,HR,Finance,Administrator")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var mission = await _mediator.Send(new ApproveMissionCommand(id));
        return Ok(ApiResponse<MissionDto>.Ok(mission, "Mission approuvée."));
    }

    [HttpPost("{id:guid}/reject")]
    [Authorize(Roles = "Manager,Director,HR,Finance,Administrator")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] RejectRequest request)
    {
        var mission = await _mediator.Send(new RejectMissionCommand(id, request.Reason));
        return Ok(ApiResponse<MissionDto>.Ok(mission, "Mission rejetée."));
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelRequest request)
    {
        var mission = await _mediator.Send(new CancelMissionCommand(id, request.Reason));
        return Ok(ApiResponse<MissionDto>.Ok(mission, "Mission annulée."));
    }

    public sealed record RejectRequest(string Reason);
    public sealed record CancelRequest(string Reason);
}
