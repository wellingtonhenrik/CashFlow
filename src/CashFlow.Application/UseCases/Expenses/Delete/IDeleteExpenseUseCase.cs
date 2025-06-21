namespace CashFlow.Application.UseCases.Expenses.Delete;

public interface IDeleteExpenseUseCase
{
    Task ExecuteAsync(long id);
}