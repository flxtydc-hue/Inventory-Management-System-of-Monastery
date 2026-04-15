using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
namespace Inventory_Management_System_of_Monastery
{
    public partial class PrintPreviewReport : Form
    {

        private DataTable _reportData;
        private string _reportTitle;
        private string _startDate;
        private string _endDate;
        private string _reportPath;
        public PrintPreviewReport(DataTable reportData, string reportTitle,
            string startDate, string endDate, string reportPath)
        {
            InitializeComponent();
            _reportData = reportData;
            _reportTitle = reportTitle;
            _startDate = startDate;
            _endDate = endDate;
            _reportPath = reportPath;

        }

        private void PrintPreviewReport_Load(object sender, EventArgs e)
        {
            try
            {
                reportViewer1.LocalReport.ReportEmbeddedResource =
                    $"Inventory_Management_System_of_Monastery.{_reportPath}";

                ReportDataSource rds = new ReportDataSource("DataSet1", _reportData);
                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                ReportParameter[] parameters = new ReportParameter[]
                {
                    new ReportParameter("ReportTitle", _reportTitle),
                    new ReportParameter("StartDate", _startDate),
                    new ReportParameter("EndDate", _endDate)
                };

                reportViewer1.LocalReport.SetParameters(parameters);
                reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
