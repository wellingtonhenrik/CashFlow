using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository, IMapper mapper)
    {
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenseJson> ExecuteAsync(long expenseId)
    {
        var result = await _expensesReadOnlyRepository.GetByIdAsync(expenseId);

        return _mapper.Map<ResponseExpenseJson>(result);

    }
}