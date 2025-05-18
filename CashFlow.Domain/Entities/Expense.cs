using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;

public class Expense
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
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