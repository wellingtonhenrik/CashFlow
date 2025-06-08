using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommomTestUtiilities.Token;

public class JwtTokenGeneratorBuilder
{
    public static IAcessTokenGenerator Build()
    {
        var mock = new Mock<IAcessTokenGenerator>();
        mock.Setup(acessTokenGenerator => acessTokenGenerator.Generate(It.IsAny<User>())).Returns("token");
        return mock.Object;
    }
}