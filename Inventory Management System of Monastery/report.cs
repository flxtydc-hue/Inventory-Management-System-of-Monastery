using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Inventory_Management_System_of_Monastery
{
    public partial class report : Form
    {
        private dashboard dashboard;

        public report(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
            ReloadCurrentTab();
        }

        private void dgv_summary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void report_Load(object sender, EventArgs e)
        {
            cmb_period.Items.AddRange(new string[]
    {
        "Daily", "Weekly", "Bi-Weekly", "Monthly", "Custom"
    });
            cmb_period.SelectedIndex = 1;
            ReloadCurrentTab();
        }

        private void LoadReport(string procedureName, DataGridView dgv)
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(procedureName, cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("start_date", dtp_start.Value.Date);
                        cmd.Parameters.AddWithValue("end_date", dtp_end.Value.Date);

                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgv.AutoGenerateColumns = false;
                            dgv.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Para sa Inventory Tab
        private void LoadInventoryReport()
        {
            LoadReport("GetInventoryReport", dgv_summary);
        }

        // Para sa Sales Tab
        private void LoadSalesReport()
        {
            LoadReport("GetSalesReport", dgv_sales);
        }

        private void tab_report_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_report.SelectedIndex == 0)
                LoadInventoryReport();
            else
                LoadSalesReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dt;
            string title;
            string reportType;

            if (tab_report.SelectedIndex == 0)
            {
                dt = (DataTable)dgv_summary.DataSource;
                title = "Inventory Summary Report";
                reportType = "Inventory";
            }
            else
            {
                dt = (DataTable)dgv_sales.DataSource;
                title = "Sales Report";
                reportType = "Sales";
            }

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Walang data para i-print. Mag-load muna ng report.",
                    "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ReportGenerator.GenerateReport(
                dt, title,
                dtp_start.Value.ToString("MM/dd/yyyy"),
                dtp_end.Value.ToString("MM/dd/yyyy"),
                reportType
            );
        }


        private void cmb_period_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;

            switch (cmb_period.SelectedItem.ToString())
            {
                case "Daily":
                    dtp_start.Value = today;
                    dtp_end.Value = today;
                    break;
                case "Weekly":
                    dtp_start.Value = today.AddDays(-7);
                    dtp_end.Value = today;
                    break;
                case "Bi-Weekly":
                    dtp_start.Value = today.AddDays(-14);
                    dtp_end.Value = today;
                    break;
                case "Monthly":
                    dtp_start.Value = today.AddMonths(-1);
                    dtp_end.Value = today;
                    break;
                case "Custom":
                    dtp_start.Enabled = true;
                    dtp_end.Enabled = true;
                    return;
            }

            dtp_start.Enabled = false;
            dtp_end.Enabled = false;
            ReloadCurrentTab();
        }

        private void ReloadCurrentTab()
        {
            if (tab_report.SelectedIndex == 0)
                LoadInventoryReport();
            else
                LoadSalesReport();
        }
        private void btn_print_Click(object sender, EventArgs e)
        {

        }

        private void dgv_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dtp_start_ValueChanged(object sender, EventArgs e)
        {
            if (cmb_period.SelectedItem.ToString() == "Custom")
                ReloadCurrentTab();
        }

        private void dtp_end_ValueChanged(object sender, EventArgs e)
        {
            if (cmb_period.SelectedItem.ToString() == "Custom")
                ReloadCurrentTab();
        }
    }
    }

