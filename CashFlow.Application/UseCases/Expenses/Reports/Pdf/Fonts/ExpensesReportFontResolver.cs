using System.Reflection;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;

public class ExpensesReportFontResolver : IFontResolver
{
    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName);

        //stream ??= ReadFontFile(FontHelper.DefaultFont);

        var length = (int)stream!.Length;

        var data = new byte[length];

        stream.Read(data, 0, length);
        return data;
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream(
            $"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts..{faceName}.ttf");
    }
}