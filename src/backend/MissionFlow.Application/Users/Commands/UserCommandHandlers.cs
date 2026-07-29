using AutoMapper;
using MediatR;
using MissionFlow.Application.Common.DTOs;
using MissionFlow.Application.Common.Interfaces;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Interfaces;
using MissionFlow.Domain.Enums;
using MissionFlow.Domain.ValueObjects;

namespace MissionFlow.Application.Users.Commands;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoginCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository; _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService; _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork; _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct) ?? throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
        if (!_passwordHasher.Verify(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Email ou mot de passe incorrect.");
        if (!user.IsActive) throw new UnauthorizedAccessException("Ce compte est desactive.");
        var accessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email.Value, user.Role.ToString(), user.PreferredLanguage.ToString());
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = _jwtTokenService.GetTokenExpiration(accessToken);
        var tokenEntity = new RefreshToken(user.Id, _passwordHasher.Hash(refreshToken), DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(tokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        user.RecordLogin();
        _userRepository.Update(user);
        await _unitOfWork.SaveChangesAsync(ct);
        return new LoginResponse(accessToken, refreshToken, expiresAt, _mapper.Map<UserDto>(user));
    }
}

public sealed class CreateAdminCommandHandler : IRequestHandler<CreateAdminCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateAdminCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository; _passwordHasher = passwordHasher; _unitOfWork = unitOfWork; _mapper = mapper;
    }

    public async Task<UserDto> Handle(CreateAdminCommand request, CancellationToken ct)
    {
        var email = EmailAddress.Create(request.Email);
        var hashedPassword = _passwordHasher.Hash(request.Password);
        var user = new User(request.FirstName, request.LastName, email, hashedPassword, UserRole.Administrator);
        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return _mapper.Map<UserDto>(user);
    }
}
