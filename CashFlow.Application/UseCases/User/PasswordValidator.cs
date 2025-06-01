using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCases.User;

public class PasswordValidator<T> : PropertyValidator<T, string>
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

        if (Regex.IsMatch(password, @"[A-Z]+") == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        if (Regex.IsMatch(password, @"[a-z]+") == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }


        if (Regex.IsMatch(password, @"[1-9]+") == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        if (Regex.IsMatch(password, @"[\!\?\*\.]+") == false)
        {
            context.MessageFormatter.AppendArgument(ErrorMessage, "informa uma senha valida");
            return false;
        }

        return true;
    }

    public override string Name { get; } = "PasswordValidator";
}