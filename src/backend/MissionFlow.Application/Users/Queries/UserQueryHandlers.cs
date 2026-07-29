using AutoMapper;
using MediatR;
using MissionFlow.Application.Common.DTOs;
using MissionFlow.Application.Common.Interfaces;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Application.Users.Queries;

public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(IUserRepository userRepository, ICurrentUser currentUser, IMapper mapper)
    {
        _userRepository = userRepository; _currentUser = currentUser; _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken ct)
    {
        if (_currentUser.UserId is null) throw new UnauthorizedAccessException();
        var user = await _userRepository.GetByIdAsync(_currentUser.UserId.Value, ct) ?? throw new KeyNotFoundException("Utilisateur introuvable.");
        return _mapper.Map<UserDto>(user);
    }
}

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository; _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, ct) ?? throw new KeyNotFoundException("Utilisateur introuvable.");
        return _mapper.Map<UserDto>(user);
    }
}
