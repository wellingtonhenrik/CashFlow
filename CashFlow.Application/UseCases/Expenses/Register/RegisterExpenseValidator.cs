using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseValidator : AbstractValidator<RequestRegisterExpenseJson>
{
    public RegisterExpenseValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage(ResourcesErrorMessages.VALOR_MENOS_ZERO);
        RuleFor(x => x.Title).NotEmpty().WithMessage(ResourcesErrorMessages.TITULO_OBRIGATORIO);
        RuleFor(x => x.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourcesErrorMessages.DATAFUTURAINVALIDA);
        RuleFor(x => x.PaymentType).IsInEnum().WithMessage(ResourcesErrorMessages.TIPO_PAGAMENTO_INVALIDO);
    }
}