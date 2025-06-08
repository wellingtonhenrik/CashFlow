using CashFlow.Application.UseCases.User;
using CashFlow.Application.UseCases.User.Register;
using CashFlow.Communication.Requests;
using CashFlow.Exception.ExceptionsBase;
using CommomTestUtiilities.Cryptography;
using CommomTestUtiilities.Mapper;
using CommomTestUtiilities.Repositories;
using CommomTestUtiilities.Request;
using CommomTestUtiilities.Token;
using FluentAssertions;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Sucess()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var result = await useCase.ExecuteAsync(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        
        var useCase = CreateUseCase();
        var act = async() => await useCase.ExecuteAsync(request);
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains("Name é obrigatório"));
    }

    private RegisterUserUseCase CreateUseCase()
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var acessToken = JwtTokenGeneratorBuilder.Build();
        var readRepository = new UserReadOnlyRepositoryBuilder().Buiild();
        return new RegisterUserUseCase(mapper, passwordEncripter, readRepository, unitOfWork, userWriteOnlyRepository, acessToken);
    }
}