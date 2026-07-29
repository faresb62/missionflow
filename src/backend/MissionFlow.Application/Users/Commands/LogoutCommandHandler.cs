using MediatR;
using MissionFlow.Application.Common.Interfaces;
using MissionFlow.Domain.Interfaces;

namespace MissionFlow.Application.Users.Commands;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICurrentUser currentUser, IUnitOfWork unitOfWork)
    {
        _refreshTokenRepository = refreshTokenRepository; _currentUser = currentUser; _unitOfWork = unitOfWork;
    }

    public async Task Handle(LogoutCommand request, CancellationToken ct)
    {
        if (_currentUser.UserId is not null)
        {
            await _refreshTokenRepository.RevokeAllForUserAsync(_currentUser.UserId.Value, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
