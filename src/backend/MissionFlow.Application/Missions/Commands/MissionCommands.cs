using MediatR;
using MissionFlow.Application.Common.DTOs;

namespace MissionFlow.Application.Missions.Commands;

public sealed record CreateMissionCommand(string Title, string Type, DateTime StartDate, DateTime EndDate, string? Description = null, string? Objective = null, string? DestinationCity = null, string? DestinationWilaya = null, string? DestinationStreet = null, decimal? EstimatedBudget = null, string TransportMode = "PersonalCar") : IRequest<MissionDto>;
public sealed record SubmitMissionCommand(Guid MissionId) : IRequest<MissionDto>;
public sealed record ApproveMissionCommand(Guid MissionId) : IRequest<MissionDto>;
public sealed record RejectMissionCommand(Guid MissionId, string Reason) : IRequest<MissionDto>;
public sealed record CancelMissionCommand(Guid MissionId, string Reason) : IRequest<MissionDto>;
