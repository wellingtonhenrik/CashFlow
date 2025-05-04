using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesUpdateOnlyRepository
{
    void Updateasync(Expense expenses);
    Task<Expense?> GetByIdAsync(long id);
}