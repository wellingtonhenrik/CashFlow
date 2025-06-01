using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IAcessTokenGenerator _acessTokenGenerator;

    public RegisterUserUseCase(IMapper mapper, IPasswordEncripter passwordEncripter, IUserReadOnlyRepository userReadOnlyRepository, IUnitOfWork unitOfWork, IUserWriteOnlyRepository userWriteOnlyRepository, IAcessTokenGenerator acessTokenGenerator)
    {
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _acessTokenGenerator = acessTokenGenerator;
    }

    public async Task<ResponseRegisterUserJson> ExecuteAsync(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();
        await _userWriteOnlyRepository.AddAsync(user);
        await _unitOfWork.CommitAsync();
        return new ResponseRegisterUserJson()
        {
            Name = user.Name,
            Token = _acessTokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var result = new UserValidator().Validate(request);
        var emailExiste = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if (emailExiste)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, "Este email já está sendo utilizado"));
        }
        
        if (!result.IsValid)
        {
            var errors = result.Errors.Select(s => s.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}   