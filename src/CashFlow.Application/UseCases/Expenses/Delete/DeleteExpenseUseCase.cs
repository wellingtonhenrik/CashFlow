using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository;

    public DeleteExpenseUseCase( IUnitOfWork unitOfWork, IExpensesWriteOnlyRepository expensesWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
    }

    public async Task ExecuteAsync(long id)
    {
        var result = await _expensesWriteOnlyRepository.DeleteAsync(id);
        if (!result) throw new NotFoundException(ResourcesErrorMessages.DESPESA_NAO_ENCONTRADA);
        await _unitOfWork.CommitAsync();
    }
}