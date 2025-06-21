using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.User.Login;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAcessTokenGenerator _acessTokenGenerator;

    public DoLoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, IPasswordEncripter passwordEncripter,
        IAcessTokenGenerator acessTokenGenerator)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _passwordEncripter = passwordEncripter;
        _acessTokenGenerator = acessTokenGenerator;
    }

    public async Task<ResponseRegisteredUserJson> ExecuteAsync(RequestLoginJson request)
    {
        var user = await _userReadOnlyRepository.GetUserByEmail(request.Email);
        if (user is null) throw new InvalidLoginException();

        var passwordMath = _passwordEncripter.Verify(request.Password, user.Password);
        if (!passwordMath) throw new InvalidLoginException();

        return new ResponseRegisteredUserJson()
        {
            Name = user.Name,
            Token = _acessTokenGenerator.Generate(user),
        };
    }
}