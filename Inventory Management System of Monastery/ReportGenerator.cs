using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management_System_of_Monastery
{
    public class ReportGenerator
    {

        public static void GenerateReport(DataTable dt, string title,
        string startDate, string endDate, string reportType)
        {
            string filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"{reportType}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            Document doc = new Document(PageSize.A4.Rotate(), 20f, 20f, 30f, 30f);
            PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
            doc.Open();

            // === TITLE ===
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
            var subFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            doc.Add(new Paragraph(title, titleFont)
            { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph($"Period: {startDate} - {endDate}", subFont)
            { Alignment = Element.ALIGN_CENTER });
            doc.Add(new Paragraph(" ")); // spacer

            // === TABLE ===
            PdfPTable table = new PdfPTable(dt.Columns.Count);
            table.WidthPercentage = 100;

            // Header
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8,
                BaseColor.WHITE);
            foreach (DataColumn col in dt.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(col.ColumnName, headerFont));
                cell.BackgroundColor = new BaseColor(41, 128, 185); // #2980B9
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Padding = 5;
                table.AddCell(cell);
            }

            // Rows
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 8);
            bool alternate = false;
            foreach (DataRow row in dt.Rows)
            {
                alternate = !alternate;
                foreach (var item in row.ItemArray)
                {
                    PdfPCell cell = new PdfPCell(
                        new Phrase(item?.ToString() ?? "", cellFont));
                    cell.BackgroundColor = alternate
                        ? BaseColor.WHITE
                        : new BaseColor(235, 245, 255);
                    cell.Padding = 4;
                    table.AddCell(cell);
                }
            }

            doc.Add(table);

            // Footer
            doc.Add(new Paragraph(" "));
            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 7,
                BaseColor.GRAY);
            doc.Add(new Paragraph(
                $"Generated on: {DateTime.Now:MM/dd/yyyy hh:mm tt}", footerFont));

            doc.Close();

            // Auto-open the PDF
            Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });

            MessageBox.Show($"Report saved to Desktop:\n{Path.GetFileName(filePath)}",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}

