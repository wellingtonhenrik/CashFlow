using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IExpensesWriteOnlyRepository _expensesWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;  
    public RegisterExpenseUseCase(IUnitOfWork unitOfWork, IMapper mapper, IExpensesWriteOnlyRepository expensesWriteOnlyRepository, IUserReadOnlyRepository userReadOnlyRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _expensesWriteOnlyRepository = expensesWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task<ResponseRegisterExpenseJson> ExecuteAsync(RequestExpenseJson request, string userEmail)
    {
        await Validate(request);
        var expense = _mapper.Map<Expense>(request);
        var user = await _userReadOnlyRepository.GetUserByEmail(userEmail);
        expense.UserId = user!.Id;
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