using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MissionFlow.Application.Common.DTOs;
using MissionFlow.Application.Common.Interfaces;
using MissionFlow.Domain.Entities;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Application.Users.Commands;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RefreshTokenCommandHandler(IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = userRepository; _refreshTokenRepository = refreshTokenRepository;
        _jwtTokenService = jwtTokenService; _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork; _mapper = mapper;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand request, CancellationToken ct)
    {
        var (userId, email, role) = _jwtTokenService.ValidateExpiredToken(request.AccessToken);
        var hashedToken = _passwordHasher.Hash(request.RefreshToken);
        var storedToken = await _refreshTokenRepository.GetByTokenHashAsync(hashedToken, ct);
        if (storedToken is null || !storedToken.IsActive) throw new UnauthorizedAccessException("Refresh token invalide ou expire.");
        var user = await _userRepository.GetByIdAsync(userId, ct) ?? throw new UnauthorizedAccessException("Utilisateur introuvable.");
        storedToken.Revoke("Refreshed");
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user.Id, user.Email.Value, user.Role.ToString(), user.PreferredLanguage.ToString());
        var newRefreshToken = _jwtTokenService.GenerateRefreshToken();
        var expiresAt = _jwtTokenService.GetTokenExpiration(newAccessToken);
        var newTokenEntity = new RefreshToken(user.Id, _passwordHasher.Hash(newRefreshToken), DateTime.UtcNow.AddDays(7));
        await _refreshTokenRepository.AddAsync(newTokenEntity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return new LoginResponse(newAccessToken, newRefreshToken, expiresAt, _mapper.Map<UserDto>(user));
    }
}
