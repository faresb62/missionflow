using MediatR;
using MissionFlow.Application.Common.DTOs;

namespace MissionFlow.Application.Users.Queries;

public sealed record GetCurrentUserQuery : IRequest<UserDto>;
public sealed record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;
public sealed record GetUsersByRoleQuery(string Role) : IRequest<IReadOnlyList<UserDto>>;
