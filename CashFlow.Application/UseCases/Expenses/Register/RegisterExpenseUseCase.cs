using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository;
    public RegisterExpenseUseCase(IUnitOfWork unitOfWork, IMapper mapper, IExpensesWriteOnlyRepository expensesWriteOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
    }

    public async Task<ResponseRegisterExpenseJson> ExecuteAsync(RequestExpenseJson request)
    {
        await Validate(request);
        var expense = _mapper.Map<Expense>(request);
        await _expensesWriteOnlyRepository.AddAsync(expense);
        await _unitOfWork.CommitAsync();
        return _mapper.Map<ResponseRegisterExpenseJson>(expense);
    }

    private async Task Validate(RequestExpenseJson request)
    {
        var validator = new ExpenseValidator();
        var result = await validator.ValidateAsync(request);
        if (!result.IsValid)
        {
            var errorsMessages = result.Errors.Select(s => s.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorsMessages);
        } 
    }
}