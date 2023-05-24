using Microsoft.Win32;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using Sales.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Sales.Views;

/// <summary>
/// Interaction logic for Statistics.xaml
/// </summary>
public partial class Statistics : UserControl {
    private Document document;
    private Table table;

    public Statistics() {
        InitializeComponent();

        this.document = new();
        this.table = new();
    }

    private void GetSalesClick(object sender, RoutedEventArgs e) {
        StatisticsViewModel context = (StatisticsViewModel)DataContext;

        context.GetGetSales();
    }

    private void GetPdf(object sender, RoutedEventArgs e) {
        StatisticsViewModel context = (StatisticsViewModel)DataContext;

        this.document = new();
        this.document.Info.Title = $"Salgstatistik for {context.SelectedSalesManName}";

        DefineStyles();
        CreatePage();
        FillPageContext();

        // Need text encoding for pdf or it will throw an error
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        PdfDocumentRenderer renderer = new(true) {
            Document = this.document
        };

        renderer.RenderDocument();

        string filename = $"{DateTime.Now.Date.ToShortDateString()}_{context.SelectedSalesManName}.pdf";

        SaveFileDialog saveFileDialog = new() {
            Filter = "Pdf Files|*.pdf",
            FilterIndex = 2,
            RestoreDirectory = true,
            FileName = filename
        };

        if (saveFileDialog.ShowDialog() == null) return;

        // Save the document
        renderer.PdfDocument.Save(saveFileDialog.FileName);


        // Ready print process
        Process p = new() {
            StartInfo = new ProcessStartInfo() {
                UseShellExecute = true,
                CreateNoWindow = true,
                Verb = "print",
                FileName = saveFileDialog.FileName
            }
        };
        
        try {
            p.Start();
            // Discard the exception, it will only throw if the user hasnt set a specific print program
        } catch { }
    }

    private void DefineStyles() {
        // Get the predefined style Normal.
        MigraDoc.DocumentObjectModel.Style style = this.document.Styles["Normal"];
        // Because all styles are derived from Normal, the next line changes the 
        // font of the whole document. Or, more exactly, it changes the font of
        // all styles and paragraphs that do not redefine the font.
        style.Font.Name = "Verdana";

        // Create a new style called Table based on style Normal
        style = this.document.Styles.AddStyle("Table", "Normal");
        style.Font.Name = "Verdana";
        style.Font.Size = 9;

        // Create a new style called Reference based on style Normal
        style = this.document.Styles.AddStyle("Reference", "Normal");
        style.ParagraphFormat.SpaceBefore = "5mm";
        style.ParagraphFormat.SpaceAfter = "5mm";
        style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
    }

    private void CreatePage() {
        StatisticsViewModel context = (StatisticsViewModel)DataContext;

        // Each MigraDoc document needs at least one section.
        Section section = this.document.AddSection();

        // Add the title
        Paragraph paragraph = section.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.AddFormattedText($"Salgsstatistik for {context.SelectedSalesManName}", TextFormat.Bold);

        // Add the total sales
        paragraph = section.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.AddFormattedText("Total salg (DKK):", TextFormat.Bold);
        paragraph.AddTab();
        paragraph.AddFormattedText($"{context.TotalSalesPrice:F2}");

        // Add the date field
        paragraph = section.AddParagraph();
        paragraph.Style = "Reference";
        paragraph.AddFormattedText("Periode:", TextFormat.Bold);
        paragraph.AddTab();
        paragraph.AddFormattedText($"{context.StartDate.Date:dd.MM.yyyy} - {context.EndDate.Date:dd.MM.yyyy}");

        // Create the item table
        this.table = section.AddTable();
        table.Style = "Table";
        table.Borders.Color = Colors.Black;
        table.Borders.Width = 0.25;
        table.Borders.Left.Width = 0.5;
        table.Borders.Right.Width = 0.5;
        table.Rows.LeftIndent = 0;

        // Before you can add a row, you must define the columns

        // Product ID
        Column column = table.AddColumn("2cm");
        column.Format.Alignment = ParagraphAlignment.Center;

        // Product name
        column = table.AddColumn("4cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        // Sales date
        column = table.AddColumn("2.5cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        // Price
        column = table.AddColumn("2.5cm");
        column.Format.Alignment = ParagraphAlignment.Right;

        // Amount
        column = table.AddColumn("2.5cm");
        column.Format.Alignment = ParagraphAlignment.Left;

        // Total
        column = table.AddColumn("2.5cm");
        column.Format.Alignment = ParagraphAlignment.Right;

        Row row = table.AddRow();
        row.HeadingFormat = true;
        row.Format.Alignment = ParagraphAlignment.Center;
        row.Format.Font.Bold = true;
        row.Shading.Color = Colors.White;

        row.Cells[0].AddParagraph("Produkt ID");
        row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
        row.Cells[0].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
        row.Cells[1].AddParagraph("Produkt navn");
        row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[2].AddParagraph("Salgsdato");
        row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[3].AddParagraph("Pris (DKK)");
        row.Cells[3].Format.Alignment = ParagraphAlignment.Right;
        row.Cells[4].AddParagraph("Mængde");
        row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[5].AddParagraph("Total (DKK)");
        row.Cells[5].Format.Alignment = ParagraphAlignment.Right;
    }

    private void FillPageContext() {
        StatisticsViewModel context = (StatisticsViewModel)DataContext;

        foreach (var productLine in context.ProductLines) {
            Row row = this.table.AddRow();
            row.TopPadding = 1.5;

            row.Cells[0].AddParagraph(productLine.ProductId.ToString());
            row.Cells[1].AddParagraph(productLine.ProductName);
            row.Cells[2].AddParagraph(productLine.SalesDate.Date.ToString("dd.MM.yyyy"));
            row.Cells[3].AddParagraph(productLine.Price.ToString("F2"));
            row.Cells[4].AddParagraph(productLine.Amount.ToString());
            row.Cells[5].AddParagraph(productLine.TotalPrice.ToString("F2"));

            this.table.SetEdge(0, this.table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
        }

        Row invisibleRow = this.table.AddRow();
        invisibleRow.Borders.Visible = false;
    }
}
