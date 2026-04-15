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
    public partial class purchase : Form
    {
        private dashboard dashboard;

        public purchase(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private void purchase_Load(object sender, EventArgs e)
        {
            LoadPurchaseOrders();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            purchaseorder po = new purchaseorder(this);
            this.dashboard.ShowForm(po);
            LoadPurchaseOrders(); // Refresh after dialog closes
        }

        private void btn_supplier_Click(object sender, EventArgs e)
        {
            supplier supplier = new supplier(this);
            supplier.ShowDialog();
        }

        private void dgv_purchase_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Ignore header row clicks or invalid indexes
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                string colName = dgv_purchase.Columns[e.ColumnIndex].Name;

                // Safely read po_id and status from the clicked row
                object poIdVal = dgv_purchase.Rows[e.RowIndex].Cells["po_id"].Value;
                object statusVal = dgv_purchase.Rows[e.RowIndex].Cells["status"].Value;

                if (poIdVal == null || statusVal == null) return;

                string poId = poIdVal.ToString();
                string status = statusVal.ToString();

                if (colName == "edit")
                {
                    if (status == "Pending")
                    {
                        // isReadOnly = false → Edit mode
                        purchaseorder po = new purchaseorder(this, poId, false);
                        this.dashboard.ShowForm(po);
                    }
                    else
                    {
                        MessageBox.Show(
                            $"Cannot edit. This purchase order is already '{status}'.",
                            "Restricted",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                }
                else if (colName == "view")
                {
                    // isReadOnly = true → View-only mode
                    purchaseorder po = new purchaseorder(this, poId, true);
                    po.ShowDialog();
                }

                // Refresh the list after dialog closes
                LoadPurchaseOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error processing request: " + ex.Message,
                    "System Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void LoadPurchaseOrders()
        {
            try
            {
                dgv_purchase.Rows.Clear();

                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT 
                                        po.po_id, 
                                        s.supplier_name, 
                                        po.order_date, 
                                        po.grand_total, 
                                        po.status 
                                     FROM purchase_order po
                                     JOIN supplier s ON po.supplier_id = s.supplier_id 
                                     ORDER BY po.po_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgv_purchase.Rows.Add(
                                reader["po_id"].ToString(),
                                reader["supplier_name"].ToString(),
                                Convert.ToDateTime(reader["order_date"]).ToString("MMM dd, yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                                Convert.ToDecimal(reader["grand_total"]).ToString("N2"),
                                reader["status"].ToString()
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading purchase orders: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void SearchPurchase(string searchTerm)
        {
            try
            {
                dgv_purchase.Rows.Clear();

                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"SELECT 
                                        po.po_id, 
                                        s.supplier_name, 
                                        po.order_date, 
                                        po.grand_total, 
                                        po.status 
                                     FROM purchase_order po
                                     JOIN supplier s ON po.supplier_id = s.supplier_id 
                                     WHERE po.po_id LIKE @search 
                                        OR s.supplier_name LIKE @search
                                     ORDER BY po.po_id DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgv_purchase.Rows.Add(
                                    reader["po_id"].ToString(),
                                    reader["supplier_name"].ToString(),
                                    Convert.ToDateTime(reader["order_date"]).ToString("MMM dd, yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture),
                                    Convert.ToDecimal(reader["grand_total"]).ToString("N2"),
                                    reader["status"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error searching purchase orders: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // FIX: Now actually calls SearchPurchase or LoadPurchaseOrders based on input
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            string term = txt_search.Text.Trim();

            if (string.IsNullOrEmpty(term))
                LoadPurchaseOrders();
            else
                SearchPurchase(term);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            // Mag-reload tuwing magiging visible ulit ang purchase form
            if (this.Visible)
                LoadPurchaseOrders();
        }
    }
}