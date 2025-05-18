using System.Reflection;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesReadOnlyRepository _expensesReadOnlyRepository;
    private const int HEIGHT_ROW_EXPENSE_TABLE = 25;

    public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
    {
        _expensesReadOnlyRepository = expensesReadOnlyRepository;
    }

    public async Task<byte[]> ExecuteAsync(DateOnly month)
    {
        var expenses = await _expensesReadOnlyRepository.FilterByMonthAsync(month);
        if (!expenses.Any()) return [];

        var document = CreateDocument(month);
        var page = CreatePage(document);
        CreateHeaderWithProfileAndName(page);
        CreateTotalSpentSection(page, month, expenses.Sum(s => s.Amount));
        expenses.ForEach(f =>
        {
            var table = CreateExpenseTable(page);
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            AddExpenseTitle(row[0], f.Title);
            AddHeaderForAmount(row[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_EXPENSE_TABLE;

            row.Cells[0].AddParagraph(f.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row[0]);
            row.Cells[0].Format.LeftIndent = 20;

            row.Cells[1].AddParagraph(f.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row[1]);

            row.Cells[2].AddParagraph($"-{f.PaymentType.ConvertPaymentType()}");
            SetStyleBaseForExpenseInformation(row[2]);

            AddAmountForExpense(row[3], f.Amount);

            if (!string.IsNullOrWhiteSpace(f.Description))
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = HEIGHT_ROW_EXPENSE_TABLE;

                descriptionRow.Cells[0].AddParagraph(f.Description);
                descriptionRow[0].Format.Font = new Font { Size = 10, Color = ColorsHelper.BLACK };
                descriptionRow[0].Shading.Color = ColorsHelper.GRENN_LIGHT;
                descriptionRow[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow[0].MergeRight = 2;
                descriptionRow[0].Format.LeftIndent = 20;

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        });
        return RenderDocument(document);
    }

    private void AddAmountForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"-{amount} R$");
        cell.Format.Font = new Font { Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Format.Font = new Font { Size = 12, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Size = 14, Color = ColorsHelper.BLACK };
        cell.Shading.Color = ColorsHelper.RED_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph("Amount");
        cell.Format.Font = new Font { Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void CreateTotalSpentSection(Section page, DateOnly month, decimal totalAmount)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        paragraph.AddFormattedText(month.ToString("Y"), new Font { Size = 15 });
        paragraph.AddLineBreak();
        paragraph.AddFormattedText($"{totalAmount} R$", new Font { Size = 50 });
    }

    private void CreateHeaderWithProfileAndName(Section page)
    {
        var table = page.AddTable();
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        //var assembly = Assembly.GetExecutingAssembly();
        //var directorName = Path.Combine(assembly.Location);
        //colocando imagem ao PDF
        //row.Cells[0].AddImage(Path.Combine(directorName,"Foto","ProfileFoto.png"));

        row.Cells[1].AddParagraph("Olá Wellington");
        row.Cells[1].Format.Font = new Font { Size = 16 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"Titulo {month.ToString("Y")}";
        document.Info.Author = "Wellington";

        var style = document.Styles["Normal"];
        //style!.Font.Name = FontHelper.RelawayRegular;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();

        section.PageSetup.PageFormat = PageFormat.A4;

        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document
        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }

    private Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();
        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }
}