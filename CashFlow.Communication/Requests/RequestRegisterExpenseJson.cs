using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Requests;

public class RequestRegisterExpenseJson
{
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public PaymentType PaymentType { get; set; }
}