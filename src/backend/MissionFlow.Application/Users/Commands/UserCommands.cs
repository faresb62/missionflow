using MediatR;
using MissionFlow.Application.Common.DTOs;

namespace MissionFlow.Application.Users.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
public sealed record CreateAdminCommand(string FirstName, string LastName, string Email, string Password) : IRequest<UserDto>;
public sealed record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<LoginResponse>;
