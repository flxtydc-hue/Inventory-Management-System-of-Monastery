using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Inventory_Management_System_of_Monastery
{
    public partial class sales : Form
    {
        private dashboard dashboard;
        public sales()
        {
            InitializeComponent();
        }

        public sales(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
            LoadSalesOrder();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            salesorder so = new salesorder(this);
            this.dashboard.ShowForm(so);
        }

        private void dgv_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                string colName = dgv_sales.Columns[e.ColumnIndex].Name;
                string salesId = dgv_sales.Rows[e.RowIndex].Cells["sales_id"].Value.ToString();
                string status = dgv_sales.Rows[e.RowIndex].Cells["status"].Value.ToString();

                if (colName == "edit")
                {
                    // Ngayon, 'Pending' na ang hahanapin natin para payagan ang Edit
                    if (status == "Pending" || status == "Paid")
                    {
                        salesorder so = new salesorder(this, salesId, false);
                        this.dashboard.ShowForm(so);
                    }
                    else
                    {
                        // Mas malinaw na warning para sa staff
                        MessageBox.Show($"Cannot edit. Order status is already '{status}'. Only 'Pending' orders can be modified.",
                                        "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else if (colName == "view")
                {
                    // Papayagan ba ang delete kahit hindi pending? 
                    // Kadalasan oo, pero may confirmation prompt dapat sa loob ng salesorder form.
                    salesorder so = new salesorder(this, salesId, true);
                    this.dashboard.ShowForm(so);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void LoadSalesOrder()
        {
            try
            {
                dgv_sales.Rows.Clear();
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // In-adjust base sa SQL columns mo: sales_id, grand_total, status
                    string query = @"SELECT sales_id, customer_name, grand_total, order_date, status 
                             FROM sales_order 
                             ORDER BY sales_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgv_sales.Rows.Add(
                                reader["sales_id"].ToString(),
                                reader["customer_name"]?.ToString() ?? "Guest",
                                Convert.ToDecimal(reader["grand_total"]).ToString("N2"),
                                Convert.ToDateTime(reader["order_date"]).ToString("yyyy-MM-dd"),
                                reader["status"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SearchSales(string searchTerm)
        {
            try
            {
                dgv_sales.Rows.Clear();
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    // In-adjust ang WHERE clause para sa sales_id at customer_name
                    string query = @"SELECT sales_id, customer_name, grand_total, order_date, status 
                             FROM sales_order 
                             WHERE customer_name LIKE @searchTerm OR sales_id LIKE @searchTerm 
                             ORDER BY sales_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgv_sales.Rows.Add(
                                    reader["sales_id"].ToString(),
                                    reader["customer_name"]?.ToString() ?? "Guest",
                                    Convert.ToDecimal(reader["grand_total"]).ToString("N2"),
                                    Convert.ToDateTime(reader["order_date"]).ToString("yyyy-MM-dd"),
                                    reader["status"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching: " + ex.Message, "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            SearchSales(txt_search.Text.Trim());
        }

        private void sales_Load(object sender, EventArgs e)
        {
            LoadSalesOrder();
        }
    }
}
    

