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
    public partial class history : Form
    {
        private dashboard dashboard;

        public history(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private bool _isLoaded = false;

        private void History_Load(object sender, EventArgs e)
        {
          

            // Default date range — buong current month
            dtp_from.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            dtp_to.Value = DateTime.Today;
            _isLoaded = true;  

            LoadTransactionHistory();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible && _isLoaded)
                LoadTransactionHistory();
        }

        // ── Main loader — handles search + type filter + date range ──────────
        public void LoadTransactionHistory()
        {
            if (!_isLoaded) return; // Iwas crash habang nag-i-initialize

            try
            {
                dgv_transaction_history.Rows.Clear();

                string searchTerm = txt_search.Text.Trim();
                string typeFilter = cmb_type.SelectedItem?.ToString() ?? "All";
                DateTime dateFrom = dtp_from.Value.Date;
                DateTime dateTo = dtp_to.Value.Date.AddDays(1).AddSeconds(-1);

                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    // SQL Query - Siniguro nating tama ang logic ng parenthesis sa AND/OR
                    string query = @"SELECT 
                                th.transaction_id, th.product_id, p.product_name,
                                th.user_id, u.username, th.type,
                                th.quantity_before, th.quantity, th.quantity_after,
                                th.reference_id, th.reference_type, th.remarks, th.created_at
                            FROM transaction_history th
                            JOIN product p ON th.product_id = p.product_id
                            JOIN users u ON th.user_id = u.user_id
                            WHERE (th.created_at BETWEEN @dateFrom AND @dateTo)
                                AND (@type = 'All' OR th.type = @type)
                                AND (
                                    p.product_name LIKE @search
                                    OR u.username LIKE @search
                                    OR th.type LIKE @search
                                    OR th.reference_type LIKE @search
                                    OR th.remarks LIKE @search
                                )
                            ORDER BY th.created_at DESC";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@dateFrom", dateFrom);
                        cmd.Parameters.AddWithValue("@dateTo", dateTo);
                        cmd.Parameters.AddWithValue("@type", typeFilter);
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string referenceDisplay = "-";
                                bool hasRefType = reader["reference_type"] != DBNull.Value;
                                bool hasRefId = reader["reference_id"] != DBNull.Value;

                                if (hasRefType && hasRefId)
                                    referenceDisplay = $"{reader["reference_type"]} #{reader["reference_id"]}";
                                else if (hasRefType)
                                    referenceDisplay = reader["reference_type"].ToString();

                                dgv_transaction_history.Rows.Add(
                                    reader["transaction_id"].ToString(),
                                    reader["product_id"].ToString(),
                                    reader["product_name"].ToString(),
                                    reader["user_id"].ToString(),
                                    reader["username"].ToString(),
                                    reader["type"].ToString(),
                                    reader["quantity_before"].ToString(),
                                    reader["quantity"].ToString(),
                                    reader["quantity_after"].ToString(),
                                    hasRefId ? reader["reference_id"].ToString() : "",
                                    referenceDisplay,
                                    reader["remarks"] == DBNull.Value ? "-" : reader["remarks"].ToString(),
                                    Convert.ToDateTime(reader["created_at"]).ToString("MMM dd, yyyy HH:mm")
                                );
                            }
                        }
                    }
                }
                int count = dgv_transaction_history.Rows.Count;
                lbl_transaction_count.Text = $"Total Transactions: {count}";
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        // ── All controls trigger the same LoadTransactionHistory ─────────────
        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            LoadTransactionHistory();
        }

        private void cmb_type_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTransactionHistory();
        }

        private bool _isUpdatingDate = false;
        private void dtp_from_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdatingDate) return;
            _isUpdatingDate = true;

            if (dtp_from.Value.Date > dtp_to.Value.Date)
                dtp_to.Value = dtp_from.Value;

            _isUpdatingDate = false;
            LoadTransactionHistory();
        }

        private void dtp_to_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdatingDate) return;
            _isUpdatingDate = true;

            if (dtp_to.Value.Date < dtp_from.Value.Date)
                dtp_from.Value = dtp_to.Value;

            _isUpdatingDate = false;
            LoadTransactionHistory();
        }

        private void dgv_transaction_history_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Reserved for future use
        }
    }
}