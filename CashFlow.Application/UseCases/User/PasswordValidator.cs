using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.User;

public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ErrorMessage = "ErrorMessage";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ErrorMessage}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informe senha valida");
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        if (!UpperCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        if (!LowerCaseLetter().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }


        if (!Numbers().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        if (!SpecialSymbols().IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
    
    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();

    [GeneratedRegex(@"[1-9]+")]
    private static partial Regex Numbers();
    
    [GeneratedRegex(@"[\!\?\*\.]+")]
    private static partial Regex SpecialSymbols();

    
    public override string Name { get; } = "PasswordValidator";
}