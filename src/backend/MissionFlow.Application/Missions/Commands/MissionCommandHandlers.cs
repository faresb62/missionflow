using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MissionFlow.Application.Common.DTOs;
using MissionFlow.Application.Common.Interfaces;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Enums;
using MissionFlow.Domain.Interfaces;
using MissionFlow.Domain.ValueObjects;

namespace MissionFlow.Application.Missions.Commands;

public sealed class CreateMissionCommandHandler : IRequestHandler<CreateMissionCommand, MissionDto>
{
    private readonly IMissionRepository _missionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public CreateMissionCommandHandler(IMissionRepository missionRepository, IUnitOfWork unitOfWork, ICurrentUser currentUser, IMapper mapper)
    {
        _missionRepository = missionRepository; _unitOfWork = unitOfWork; _currentUser = currentUser; _mapper = mapper;
    }

    public async Task<MissionDto> Handle(CreateMissionCommand request, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated) throw new UnauthorizedAccessException();
        var type = Enum.Parse<MissionType>(request.Type);
        var transportMode = Enum.Parse<VehicleType>(request.TransportMode);
        var period = new DateRange(DateOnly.FromDateTime(request.StartDate), DateOnly.FromDateTime(request.EndDate));
        Address? address = request.DestinationCity is not null ? new Address(request.DestinationStreet ?? "", request.DestinationCity, request.DestinationWilaya ?? "") : null;
        Money? budget = request.EstimatedBudget.HasValue ? Money.DZD(request.EstimatedBudget.Value) : null;
        var mission = new Mission(request.Title, type, period, _currentUser.UserId!.Value, transportMode, request.Description, request.Objective, address, budget);
        await _missionRepository.AddAsync(mission, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<MissionDto>(mission);
    }
}

public sealed class SubmitMissionCommandHandler : IRequestHandler<SubmitMissionCommand, MissionDto>
{
    private readonly IMissionRepository _missionRepository; private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
    public SubmitMissionCommandHandler(IMissionRepository missionRepository, IUnitOfWork unitOfWork, IMapper mapper)
    { _missionRepository = missionRepository; _unitOfWork = unitOfWork; _mapper = mapper; }
    public async Task<MissionDto> Handle(SubmitMissionCommand request, CancellationToken ct)
    {
        var mission = await _missionRepository.GetByIdAsync(request.MissionId, ct) ?? throw new KeyNotFoundException();
        mission.Submit(); _missionRepository.Update(mission); await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<MissionDto>(mission);
    }
}

public sealed class ApproveMissionCommandHandler : IRequestHandler<ApproveMissionCommand, MissionDto>
{
    private readonly IMissionRepository _missionRepository; private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser; private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
    public ApproveMissionCommandHandler(IMissionRepository missionRepository, IUserRepository userRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork, IMapper mapper)
    { _missionRepository = missionRepository; _userRepository = userRepository; _currentUser = currentUser; _unitOfWork = unitOfWork; _mapper = mapper; }
    public async Task<MissionDto> Handle(ApproveMissionCommand request, CancellationToken ct)
    {
        var mission = await _missionRepository.GetByIdAsync(request.MissionId, ct) ?? throw new KeyNotFoundException();
        var approver = await _userRepository.GetByIdAsync(_currentUser.UserId!.Value, ct) ?? throw new UnauthorizedAccessException();
        switch (mission.Status)
        {
            case MissionStatus.Submitted: mission.ApproveByManager(approver); break;
            case MissionStatus.ApprovedByManager: mission.ApproveByHR(approver); break;
            case MissionStatus.ApprovedByHR: mission.ApproveByFinance(approver); break;
            default: throw new InvalidOperationException();
        }
        _missionRepository.Update(mission); await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<MissionDto>(mission);
    }
}

public sealed class RejectMissionCommandHandler : IRequestHandler<RejectMissionCommand, MissionDto>
{
    private readonly IMissionRepository _missionRepository; private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser; private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
    public RejectMissionCommandHandler(IMissionRepository missionRepository, IUserRepository userRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork, IMapper mapper)
    { _missionRepository = missionRepository; _userRepository = userRepository; _currentUser = currentUser; _unitOfWork = unitOfWork; _mapper = mapper; }
    public async Task<MissionDto> Handle(RejectMissionCommand request, CancellationToken ct)
    {
        var mission = await _missionRepository.GetByIdAsync(request.MissionId, ct) ?? throw new KeyNotFoundException();
        var approver = await _userRepository.GetByIdAsync(_currentUser.UserId!.Value, ct) ?? throw new UnauthorizedAccessException();
        mission.Reject(approver, request.Reason); _missionRepository.Update(mission); await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<MissionDto>(mission);
    }
}

public sealed class CancelMissionCommandHandler : IRequestHandler<CancelMissionCommand, MissionDto>
{
    private readonly IMissionRepository _missionRepository; private readonly IUnitOfWork _unitOfWork; private readonly IMapper _mapper;
    public CancelMissionCommandHandler(IMissionRepository missionRepository, IUnitOfWork unitOfWork, IMapper mapper)
    { _missionRepository = missionRepository; _unitOfWork = unitOfWork; _mapper = mapper; }
    public async Task<MissionDto> Handle(CancelMissionCommand request, CancellationToken ct)
    {
        var mission = await _missionRepository.GetByIdAsync(request.MissionId, ct) ?? throw new KeyNotFoundException();
        mission.Cancel(request.Reason); _missionRepository.Update(mission); await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<MissionDto>(mission);
    }
}
