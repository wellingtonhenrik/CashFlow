using CashFlow.Application.UseCases.User;
using CommomTestUtiilities.Request;
using FluentAssertions;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Sucess()
    {
        //Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        
        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.Should().BeTrue();

    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Name_Empty(string name)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(a=>a.ErrorMessage == "Name é obrigatório");
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Error_Email_Empty(string email)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = email;
        
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(a=>a.ErrorMessage == "Informe email");
    }
    
    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "invalid.com";
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(a=>a.ErrorMessage == "Informe email");
    }
    
    [Fact]
    public void Error_Password_Invalid()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;
        var result = validator.Validate(request);
        
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(a=>a.ErrorMessage == "informe senha valida");
    }
}