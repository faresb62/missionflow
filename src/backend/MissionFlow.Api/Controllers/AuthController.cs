using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MissionFlow.Application.Users.Commands;
using MissionFlow.Application.Users.Queries;
using MissionFlow.Application.Common.DTOs;

namespace MissionFlow.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var response = await _mediator.Send(command);
        return Ok(ApiResponse<LoginResponse>.Ok(response, "Connexion réussie."));
    }

    [HttpPost("setup")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Setup([FromBody] CreateAdminCommand command)
    {
        var user = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCurrentUser), null, ApiResponse<UserDto>.Ok(user, "Administrateur créé avec succès."));
    }

    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser()
    {
        var user = await _mediator.Send(new GetCurrentUserQuery());
        return Ok(ApiResponse<UserDto>.Ok(user));
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(ApiResponse<LoginResponse>.Ok(response, "Token rafraîchi avec succès."));
    }

    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout()
    {
        await _mediator.Send(new LogoutCommand());
        return Ok(new { success = true, message = "Déconnexion réussie." });
    }
}
