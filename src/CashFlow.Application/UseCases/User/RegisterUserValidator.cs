using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.User;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(a => a.Name).NotEmpty().WithMessage("Name é obrigatório");
        RuleFor(a => a.Email)
            .NotEmpty()
            .WithMessage("Informe email")
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage("Informe um email valido");
        
        RuleFor(a=>a.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        
    }
}