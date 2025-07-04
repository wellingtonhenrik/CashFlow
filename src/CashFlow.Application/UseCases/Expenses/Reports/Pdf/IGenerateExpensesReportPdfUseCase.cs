namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public interface IGenerateExpensesReportPdfUseCase
{
    Task<byte[]> ExecuteAsync(DateOnly month);
}