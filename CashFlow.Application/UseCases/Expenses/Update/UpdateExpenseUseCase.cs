using AutoMapper;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Update;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IExpensesUpdateOnlyRepository _expensesUpdateOnlyRepository;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase(IUnitOfWork unitOfWork, IExpensesUpdateOnlyRepository expensesUpdateOnlyRepository,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _expensesUpdateOnlyRepository = expensesUpdateOnlyRepository;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(long id, RequestExpenseJson request)
    {
        Validate(request);
        var expense = await _expensesUpdateOnlyRepository.GetByIdAsync(id);
        if (expense is null) throw new NotFoundException(ResourcesErrorMessages.DESPESA_NAO_ENCONTRADA);

        _mapper.Map(request, expense);
        _expensesUpdateOnlyRepository.Updateasync(expense);
        await _unitOfWork.CommitAsync();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var result = validator.Validate(request);
        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}