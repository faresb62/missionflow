using AutoMapper;
using MediatR;
using MissionFlow.Application.Common.DTOs;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Application.Users.Queries;

public sealed class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, IReadOnlyList<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUsersByRoleQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository; _mapper = mapper;
    }

    public async Task<IReadOnlyList<UserDto>> Handle(GetUsersByRoleQuery request, CancellationToken ct)
    {
        var users = await _userRepository.GetByRoleAsync(request.Role, ct);
        return _mapper.Map<IReadOnlyList<UserDto>>(users);
    }
}
