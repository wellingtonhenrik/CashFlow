using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommomTestUtiilities.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        var mock = new Mock<IPasswordEncripter>();
        mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("!AV54asadsadas");
        return mock.Object;
    }
}