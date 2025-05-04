using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.Update;

public interface IUpdateExpenseUseCase
{
    Task ExecuteAsync(long id, RequestExpenseJson request);
}