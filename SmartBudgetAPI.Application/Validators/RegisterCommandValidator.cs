using FluentValidation;
using SmartBudgetAPI.Application.Features.Auth.Commands.Register;

namespace SmartBudgetAPI.Application.Validators;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(v => v.RegisterDto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(v => v.RegisterDto.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

        RuleFor(v => v.RegisterDto.ConfirmPassword)
            .Equal(v => v.RegisterDto.Password).WithMessage("Passwords do not match.");

        RuleFor(v => v.RegisterDto.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(v => v.RegisterDto.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");

        RuleFor(v => v.RegisterDto.DefaultCurrency)
            .NotEmpty().WithMessage("Default currency is required.")
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.");
    }
}
