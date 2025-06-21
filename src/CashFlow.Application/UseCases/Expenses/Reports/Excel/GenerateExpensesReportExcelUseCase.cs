using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Reports.Excel;

public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private const string DollarFormat = "$";

    public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
    {
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
    }

    public async Task<byte[]> ExecuteAsync(DateOnly date)
    {
        var expenses = await _expensesReadOnlyRepository.FilterByMonthAsync(date);
        if (!expenses.Any()) return [];

        using var workbook = new XLWorkbook();

        workbook.Author = "Wellington";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Times New Roman";

        var worksheet = workbook.Worksheets.Add(date.ToString("Y"));

        InsertHeader(worksheet);
        var raw = 2;

        expenses.ForEach(f =>
        {
            worksheet.Cell($"A{raw}").Value = f.Title;
            worksheet.Cell($"B{raw}").Value = f.Date;
            worksheet.Cell($"C{raw}").Value = f.PaymentType.ConvertPaymentType();

            worksheet.Cell($"D{raw}").Value = f.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{DollarFormat} #,##0.00";
            worksheet.Cell($"E{raw}").Value = f.Description;
        });
        var file = new MemoryStream();
        worksheet.Columns().AdjustToContents();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = "Titulo";
        worksheet.Cell("B1").Value = "Date";
        worksheet.Cell("C1").Value = "PaymentType";
        worksheet.Cell("D1").Value = "Amount";
        worksheet.Cell("E1").Value = "Descrição";
        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#FF0000");

        worksheet.Cells("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cells("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
    }
}