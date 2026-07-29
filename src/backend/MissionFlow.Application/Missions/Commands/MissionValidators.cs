using FluentValidation;
using MissionFlow.Application.Missions.Commands;

namespace MissionFlow.Application.Missions.Validators;

public sealed class CreateMissionCommandValidator : AbstractValidator<CreateMissionCommand>
{
    public CreateMissionCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Type).NotEmpty();
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty().GreaterThanOrEqualTo(x => x.StartDate);
    }
}

public sealed class RejectMissionCommandValidator : AbstractValidator<RejectMissionCommand>
{
    public RejectMissionCommandValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}

public sealed class CancelMissionCommandValidator : AbstractValidator<CancelMissionCommand>
{
    public CancelMissionCommandValidator()
    {
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(500);
    }
}
