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
    public partial class adjustment : Form
    {
        private products p;
        public adjustment(products p)
        {
            InitializeComponent();
            this.p = p;
            LoadAllCategories();
        }

        private void btn_adjust_Click(object sender, EventArgs e)
        {
            // ── Validations ───────────────────────────────────────────────────
            if (cmb_category.SelectedIndex == -1 || cmb_product_name.SelectedIndex == -1
                || string.IsNullOrWhiteSpace(txt_adjust_qty.Text))
            {
                MessageBox.Show("Please fill in all fields.", title.name,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txt_adjust_qty.Text, out int adjustQty) || adjustQty <= 0)
            {
                MessageBox.Show("Please enter a valid adjustment quantity.", title.name,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!rb_add.Checked && !rb_deduct.Checked)
            {
                MessageBox.Show("Please select whether to Add or Deduct.", title.name,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rb_deduct.Checked && cmb_reason.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a reason for deduction.", title.name,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cmb_product_name.SelectedValue == null) return;

            string product_id = cmb_product_name.SelectedValue.ToString();
            string reason = cmb_reason.Text;
            string sqlOperator = rb_add.Checked ? "+" : "-";

            using (MySqlConnection connection = DatabaseConnection.GetConnection())
            {
                try
                {
                    connection.Open();

                    // ── Get stock BEFORE adjustment ───────────────────────────
                    int stockBefore = 0;
                    string selectQuery = "SELECT stock_quantity FROM product WHERE product_id = @product_id";
                    using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@product_id", product_id);
                        object res = selectCmd.ExecuteScalar();
                        stockBefore = (res != null) ? Convert.ToInt32(res) : 0;
                    }

                    // ── Check if deduction is valid ───────────────────────────
                    if (rb_deduct.Checked && stockBefore < adjustQty)
                    {
                        MessageBox.Show($"Deduction failed. Current stock is only {stockBefore}.",
                            title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // ── Update product stock ───────────────────────────────────
                    string updateQuery = $"UPDATE product SET stock_quantity = stock_quantity {sqlOperator} @adjust_qty WHERE product_id = @product_id";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@adjust_qty", adjustQty);
                        updateCmd.Parameters.AddWithValue("@product_id", product_id);
                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            int stockAfter = rb_add.Checked
                                ? stockBefore + adjustQty
                                : stockBefore - adjustQty;

                            // ── Log to transaction_history ─────────────────────
                            string logQuery = @"INSERT INTO transaction_history 
                                                (product_id, user_id, type, quantity,
                                                 quantity_before, quantity_after,
                                                 reference_id, reference_type, remarks)
                                                VALUES 
                                                (@pid, @uid, 'Adjustment', @qty,
                                                 @before, @after,
                                                 NULL, 'Manual Adjustment', @remarks)";

                            using (MySqlCommand logCmd = new MySqlCommand(logQuery, connection))
                            {
                                logCmd.Parameters.AddWithValue("@pid", product_id);
                                logCmd.Parameters.AddWithValue("@uid", GlobalUser.ID);
                                logCmd.Parameters.AddWithValue("@qty", adjustQty);
                                logCmd.Parameters.AddWithValue("@before", stockBefore);
                                logCmd.Parameters.AddWithValue("@after", stockAfter);
                                logCmd.Parameters.AddWithValue("@remarks", rb_add.Checked
                                    ? $"Stock Added: Manual Adjustment by {GlobalUser.Name}"
                                    : $"Stock Deducted: {reason} by {GlobalUser.Name}");
                                logCmd.ExecuteNonQuery();
                            }

                            // ── Check and update archive status ────────────────
                            UpdateStockAndCheckArchive(product_id, stockAfter);

                            MessageBox.Show("Stock adjusted successfully.", title.name,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.p.LoadProductData();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Adjustment failed: No matching product found.",
                                title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, title.name,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cmb_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_category.SelectedValue != null)
            {
                string category_id = cmb_category.SelectedValue.ToString();
                LoadProductsByCategories(category_id);
            }
            else
            {
                cmb_product_name.DataSource = null;
            }
        }

        public void LoadProductsByCategories(string category_id)
        {
            cmb_product_name.DataSource = null;

            string query = "SELECT product_id, product_name FROM product WHERE category_id = @category_id AND is_archived = 0 ORDER BY product_name";

            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@category_id", category_id);
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            cmb_product_name.DisplayMember = "product_name";
                            cmb_product_name.ValueMember = "product_id";
                            cmb_product_name.DataSource = dt;
                            cmb_product_name.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, title.name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadAllCategories()
        {
            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT category_id, category_name FROM category ORDER BY category_name";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        cmb_category.DisplayMember = "category_name";
                        cmb_category.ValueMember = "category_id";
                        cmb_category.DataSource = dt;
                        cmb_category.SelectedIndex = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, title.name,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateStockAndCheckArchive(string productID, int newStock)
        {
            int currentArchiveStatus = 0;
            string checkStatusQuery = "SELECT is_archived FROM product WHERE product_id = @id";

            using (MySqlConnection cn = DatabaseConnection.GetConnection())
            {
                try
                {
                    cn.Open();
                    using (MySqlCommand checkCmd = new MySqlCommand(checkStatusQuery, cn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", productID);
                        currentArchiveStatus = Convert.ToInt32(checkCmd.ExecuteScalar());
                    }

                    string archiveQuery = "";
                    string archiveType = "";
                    string archiveRemarks = "";

                    // ── Auto-archive if stock hits 0 ───────────────────────────
                    if (newStock == 0 && currentArchiveStatus == 0)
                    {
                        archiveQuery = "UPDATE product SET is_archived = 1 WHERE product_id = @id";
                        archiveType = "Archived";
                        archiveRemarks = "System Auto-Archive: Stock reached 0.";
                    }
                    // ── Auto-restore if stock replenished ──────────────────────
                    else if (newStock > 0 && currentArchiveStatus == 1)
                    {
                        archiveQuery = "UPDATE product SET is_archived = 0 WHERE product_id = @id";
                        archiveType = "Restored";
                        archiveRemarks = "System Auto-Restore: Stock replenished.";
                    }

                    if (!string.IsNullOrEmpty(archiveQuery))
                    {
                        using (MySqlCommand cmd = new MySqlCommand(archiveQuery, cn))
                        {
                            cmd.Parameters.AddWithValue("@id", productID);
                            cmd.ExecuteNonQuery();
                        }

                        // ── Log archive/restore event ──────────────────────────
                        string logQuery = @"INSERT INTO transaction_history 
                                            (product_id, user_id, type, quantity,
                                             quantity_before, quantity_after,
                                             reference_id, reference_type, remarks)
                                            VALUES 
                                            (@pid, @uid, @type, @qty,
                                             @before, @after,
                                             NULL, 'System', @remarks)";

                        using (MySqlCommand logCmd = new MySqlCommand(logQuery, cn))
                        {
                            logCmd.Parameters.AddWithValue("@pid", productID);
                            logCmd.Parameters.AddWithValue("@uid", GlobalUser.ID);
                            logCmd.Parameters.AddWithValue("@type", archiveType);
                            logCmd.Parameters.AddWithValue("@qty", newStock);
                            logCmd.Parameters.AddWithValue("@before", newStock);
                            logCmd.Parameters.AddWithValue("@after", newStock);
                            logCmd.Parameters.AddWithValue("@remarks", archiveRemarks);
                            logCmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Archive Sync Error: " + ex.Message);
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}