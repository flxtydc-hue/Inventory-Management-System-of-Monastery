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
    public partial class subdash : Form
    {
        private dashboard dashboard;
        public subdash(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private void subdash_Load(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            LoadStats();
            LoadLowStockTable();
            LoadRecentSales();
        }

        private void LoadStats()
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("GetDashboardStats", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                lbl_total_products.Text = dr["total_products"].ToString();
                                lbl_total_categories.Text = dr["total_categories"].ToString();
                                lbl_low_stock.Text = dr["low_stock_count"].ToString();
                                lbl_todays_sales.Text = $"₱{Convert.ToDecimal(dr["todays_sales"]):N2}";

                                // Kung may low stock, gawing red ang label
                                int lowStockCount = Convert.ToInt32(dr["low_stock_count"]);
                                lbl_low_stock.ForeColor = lowStockCount > 0 ? Color.Red : Color.Green;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading stats: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =====================
        // LOW STOCK TABLE
        // =====================
        private void LoadLowStockTable()
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("GetLowStockItems", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgv_lowstock.AutoGenerateColumns = false; 
                            dgv_lowstock.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading low stock: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // =====================
        // RECENT SALES TABLE
        // =====================
        private void LoadRecentSales()
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand("GetRecentSales", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            dgv_recentsales.AutoGenerateColumns = false;
                            dgv_recentsales.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading recent sales: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Refresh button
        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadDashboard();
        }

        private void dgv_lowstock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgv_recentsales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
   
