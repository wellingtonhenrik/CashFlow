using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.User;

public class UserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UserValidator()
    {
        RuleFor(a => a.Name).NotEmpty().WithMessage("Name é obrigatório");
        RuleFor(a => a.Email).NotEmpty().WithMessage("Informe email").EmailAddress().WithMessage("Informe um email valido");
        RuleFor(a => a.Password).NotEmpty().WithMessage("Informe email valido");
        
        RuleFor(a=>a.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        
    }
}