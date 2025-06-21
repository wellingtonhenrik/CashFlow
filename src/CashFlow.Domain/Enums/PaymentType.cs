namespace CashFlow.Domain.Enums;

public enum PaymentType
{
    Cash = 0,
    CreditCard = 1,
    DebitCard = 2,
    EletronicTransfer = 3,
}

public static class PaymentTypeExtension
{
    public static string ConvertPaymentType(this PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.CreditCard => "Credit Card",
            PaymentType.DebitCard => "Debit Card",
            PaymentType.Cash => "Cash",
            PaymentType.EletronicTransfer => "Eletronic Transfer",
            _ => string.Empty
        };
    }
}