using FluentValidation;
using MissionFlow.Application.Users.Commands;

namespace MissionFlow.Application.Users.Validators;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email valide requis.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Mot de passe requis.");
    }
}

public sealed class CreateAdminCommandValidator : AbstractValidator<CreateAdminCommand>
{
    public CreateAdminCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("Mot de passe: 8 caracteres minimum.");
    }
}
