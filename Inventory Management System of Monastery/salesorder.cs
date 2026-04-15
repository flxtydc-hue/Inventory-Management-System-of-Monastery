using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Inventory_Management_System_of_Monastery
{
    public partial class salesorder : Form
    {
        private sales sales;
        public string id { get; private set; }
        private bool isViewMode = false;
        private bool isEditMode = false;
        private decimal currentGrandTotal = 0.00m;

        // Track the original status loaded from the DB so we can detect changes
        private string originalStatus = string.Empty;
        public salesorder(sales sales)
        {
            InitializeComponent();
            this.sales = sales;
            this.id = string.Empty;
            this.Text = "New Sales Order";
        }


        public salesorder(sales sales, string salesId, bool isViewMode)
        {
            InitializeComponent();
            this.sales = sales;
            this.id = salesId;
            this.isViewMode = isViewMode;
            this.isEditMode = !isViewMode;
            this.Text = isViewMode ? $"View Sales Order: {salesId}" : $"Edit Sales Order: {salesId}";

            this.Load += salesorder_Load;

            if (this.isViewMode)
            {
                DisableControls();
            }

        }

        private void DisableControls()
        {
            txt_customer_name.ReadOnly = true;
            dtp_order_date.Enabled = false;
            btn_add.Enabled = false;
            btn_save.Enabled = false;
            btn_cancel.Enabled = true;

            cmb_status.Enabled = false;
            cmb_status.Visible = false;
            lbl_status.Visible = true;          // shown, text set in LoadOrderDetails

            dgv_sales_order.ReadOnly = true;
            dgv_sales_order.AllowUserToAddRows = false;
            dgv_sales_order.AllowUserToDeleteRows = false;

            if (dgv_sales_order.Columns.Contains("remove"))
                dgv_sales_order.Columns["remove"].Visible = false;

        }

        private void LoadStatuses()
        {
            cmb_status.Items.Clear();
            cmb_status.Items.AddRange(new string[] { "Pending", "Paid", "Refunded", "Cancelled" });
        }

        private void ClearFields()
        {
            txt_customer_name.Clear();
            dtp_order_date.Value = DateTime.Today;
            dgv_sales_order.Rows.Clear();
            lbl_grand_total.Text = "0.00";

        }

        private void dgv_sales_order_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || isViewMode) return;

            string colName = dgv_sales_order.Columns[e.ColumnIndex].Name;

            // 2. Check "remove" column
            if (colName == "remove")
            {
                // Safe check if value is not null
                var cellVal = dgv_sales_order.Rows[e.RowIndex].Cells["product_name"].Value;
                string productName = cellVal != null ? cellVal.ToString() : "this item";

                if (MessageBox.Show($"Are you sure you want to remove '{productName}'?",
                    title.name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dgv_sales_order.Rows.RemoveAt(e.RowIndex);
                    UpdateGrandTotal();
                }
            }
        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void salesorder_Load(object sender, EventArgs e)
        {
            LoadStatuses();

            if (!string.IsNullOrEmpty(this.id))
            {
                // Existing order: load header then items
                LoadOrderDetails(this.id);
                LoadOrderItems(this.id);

                // Only allow the combo in edit mode
                cmb_status.Enabled = isEditMode;
                cmb_status.Visible = isEditMode;
                lbl_status.Visible = isViewMode;
            }
            else
            {
                // New order defaults
                dtp_order_date.Value = DateTime.Now;
                txt_customer_name.Text = "Guest";
                cmb_status.SelectedItem = "Pending";
                cmb_status.Visible = false;     // status combo hidden for new orders
                lbl_status.Visible = false;
            }
        }

        private void LoadOrderDetails(string salesId)
        {
            try
            {
                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();
                    const string query =
                        "SELECT customer_name, order_date, grand_total, status " +
                        "FROM sales_order WHERE sales_id = @sales_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@sales_id", MySqlDbType.Int32).Value = Convert.ToInt32(salesId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txt_customer_name.Text = reader.GetString("customer_name");
                                dtp_order_date.Value = reader.GetDateTime("order_date");
                                currentGrandTotal = reader.GetDecimal("grand_total");
                                lbl_grand_total.Text = currentGrandTotal.ToString("N2");

                                string dbStatus = reader.GetString("status");

                                // Remember the DB status so we can detect changes on save
                                originalStatus = dbStatus;

                                // Combo (edit mode)
                                cmb_status.SelectedItem = cmb_status.Items.Contains(dbStatus)
                                    ? dbStatus : "Pending";

                                // Label (view mode)
                                lbl_status.Text = "Status: " + dbStatus;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order details:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadOrderItems(string salesId)
        {
            try
            {
                dgv_sales_order.Rows.Clear();
                currentGrandTotal = 0;

                using (MySqlConnection conn = DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    const string query =
                        @"SELECT si.sales_item_id, si.product_id, p.product_name,
                                 si.quantity, si.unit_price, si.subtotal
                          FROM   sales_items si
                          JOIN   product     p  ON si.product_id = p.product_id
                          WHERE  si.sales_id = @sales_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Add("@sales_id", MySqlDbType.Int32).Value = Convert.ToInt32(salesId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                dgv_sales_order.Rows.Add(
                                    reader["sales_item_id"],
                                    reader["product_id"],
                                    reader["product_name"],
                                    reader["quantity"],
                                    reader["unit_price"],
                                    reader["subtotal"]
                                );
                            }
                        }
                    }
                }

                UpdateGrandTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order items:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            new salesitem(this).ShowDialog();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_customer_name.Text))
            {
                MessageBox.Show("Please enter a customer name.",
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgv_sales_order.Rows.Count == 0)
            {
                MessageBox.Show("Please add at least one item.",
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Determine new status
            string newStatus = cmb_status.SelectedItem?.ToString() ?? "Pending";
            bool isNewOrder = string.IsNullOrEmpty(id);

            // ── Status-change guards ──────────────────────────────────────────────
            // Prevent going backwards from a terminal state
            if (!isNewOrder)
            {
                if (originalStatus == "Paid" && newStatus == "Pending")
                {
                    MessageBox.Show("A Paid order cannot be moved back to Pending.\n" +
                                    "Use 'Refunded' or 'Cancelled' instead.",
                                    "Invalid Status Change",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if ((originalStatus == "Refunded" || originalStatus == "Cancelled") &&
                     newStatus != originalStatus)
                {
                    MessageBox.Show($"A {originalStatus} order cannot be changed to {newStatus}.",
                                    "Invalid Status Change",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            using (MySqlConnection conn = DatabaseConnection.GetConnection())
            {
                conn.Open();
                using (var trans = conn.BeginTransaction())
                {
                    try
                    {

                        // ── 1. Upsert the sales_order header ─────────────────────
                        UpsertSalesOrderHeader(conn, trans, isNewOrder, newStatus);

                        // ── 2. Re-insert all line-items ──────────────────────────
                        DeleteAndReinsertItems(conn, trans);

                        // ── 3. Stock adjustment based on status transition ────────
                        HandleStockAdjustment(conn, trans, isNewOrder, originalStatus, newStatus);

                        trans.Commit();

                        // Remember the new status as baseline
                        originalStatus = newStatus;

                        MessageBox.Show($"Order saved successfully.\nOrder ID: {id}",
                                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        sales?.LoadSalesOrder();
                        Close();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        MessageBox.Show("Save failed:\n" + ex.Message,
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ── INSERT or UPDATE the sales_order header row ───────────────────────────
        private void UpsertSalesOrderHeader(MySqlConnection conn, MySqlTransaction trans,
                                            bool isNewOrder, string status)
        {
            string query = isNewOrder
                ? "INSERT INTO sales_order (customer_name, order_date, grand_total, status) " +
                  "VALUES (@name, @date, @total, @status)"
                : "UPDATE sales_order " +
                  "SET customer_name=@name, order_date=@date, grand_total=@total, status=@status " +
                  "WHERE sales_id=@id";

            using (var cmd = new MySqlCommand(query, conn, trans))
            {
                if (!isNewOrder)
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(id);

                cmd.Parameters.Add("@name", MySqlDbType.VarChar).Value = txt_customer_name.Text.Trim();
                cmd.Parameters.Add("@date", MySqlDbType.DateTime).Value = dtp_order_date.Value;
                cmd.Parameters.Add("@total", MySqlDbType.Decimal).Value = currentGrandTotal;
                cmd.Parameters.Add("@status", MySqlDbType.VarChar).Value = status;
                cmd.ExecuteNonQuery();

                // Retrieve the auto-generated ID after insert
                if (isNewOrder)
                    id = cmd.LastInsertedId.ToString();
            }
        }

        // Helper method para iwas redundancy sa query string


        private void DeleteAndReinsertItems(MySqlConnection conn, MySqlTransaction trans)
        {
            // Delete existing items
            using (var del = new MySqlCommand(
                       "DELETE FROM sales_items WHERE sales_id = @id", conn, trans))
            {
                del.Parameters.Add("@id", MySqlDbType.Int32).Value = Convert.ToInt32(id);
                del.ExecuteNonQuery();
            }

            // Insert current grid rows
            foreach (DataGridViewRow row in dgv_sales_order.Rows)
            {
                if (row.IsNewRow) continue;

                string productId = row.Cells["product_id"].Value?.ToString();
                if (string.IsNullOrEmpty(productId)) continue;

                int qty = Convert.ToInt32(row.Cells["quantity"].Value);
                decimal price = Convert.ToDecimal(row.Cells["unit_price"].Value);
                decimal subtotal = qty * price;

                // Keep subtotal cell in sync
                row.Cells["subtotal"].Value = subtotal;

                using (var ins = new MySqlCommand(
                           @"INSERT INTO sales_items (sales_id, product_id, quantity, unit_price, subtotal)
                             VALUES (@sid, @pid, @qty, @price, @sub)", conn, trans))
                {
                    ins.Parameters.Add("@sid", MySqlDbType.Int32).Value = Convert.ToInt32(id);
                    ins.Parameters.Add("@pid", MySqlDbType.Int32).Value = Convert.ToInt32(productId);
                    ins.Parameters.Add("@qty", MySqlDbType.Int32).Value = qty;
                    ins.Parameters.Add("@price", MySqlDbType.Decimal).Value = price;
                    ins.Parameters.Add("@sub", MySqlDbType.Decimal).Value = subtotal;
                    ins.ExecuteNonQuery();
                }
            }
        }

        private void HandleStockAdjustment(MySqlConnection conn, MySqlTransaction trans,
                                   bool isNewOrder, string prevStatus, string newStatus)
        {
            bool becomesPaid = newStatus == "Paid" && (isNewOrder || prevStatus != "Paid");
            bool becomesRefunded = !isNewOrder && prevStatus == "Paid"
                                    && (newStatus == "Refunded" || newStatus == "Cancelled");

            if (!becomesPaid && !becomesRefunded) return;

            foreach (DataGridViewRow row in dgv_sales_order.Rows)
            {
                if (row.IsNewRow) continue;

                string productId = row.Cells["product_id"].Value?.ToString();
                if (string.IsNullOrEmpty(productId)) continue;

                int qty = Convert.ToInt32(row.Cells["quantity"].Value);
                int pId = Convert.ToInt32(productId);

                // ── Get stock BEFORE adjustment ────────────────────────────
                int stockBefore = 0;
                string getStock = "SELECT stock_quantity FROM product WHERE product_id = @prodId";
                using (MySqlCommand cmd = new MySqlCommand(getStock, conn, trans))
                {
                    cmd.Parameters.Add("@prodId", MySqlDbType.Int32).Value = pId;
                    stockBefore = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (becomesPaid)
                {
                    // ── Deduct stock ───────────────────────────────────────
                    using (var cmd = new MySqlCommand(
                               "UPDATE product SET stock_quantity = stock_quantity - @q " +
                               "WHERE product_id = @id AND stock_quantity >= @q",
                               conn, trans))
                    {
                        cmd.Parameters.Add("@q", MySqlDbType.Int32).Value = qty;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = pId;
                        if (cmd.ExecuteNonQuery() == 0)
                            throw new Exception($"Insufficient stock for product ID: {productId}");
                    }

                    // ── Log Stock Out ──────────────────────────────────────
                    string logStockOut = @"INSERT INTO transaction_history 
                                    (product_id, user_id, type, quantity,
                                     quantity_before, quantity_after,
                                     reference_id, reference_type, remarks)
                                    VALUES
                                    (@prodId, @userId, 'Stock Out', @qty,
                                     @before, @after,
                                     @refId, 'Sales', @remarks)";

                    using (MySqlCommand cmd = new MySqlCommand(logStockOut, conn, trans))
                    {
                        cmd.Parameters.Add("@prodId", MySqlDbType.Int32).Value = pId;
                        cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = GlobalUser.ID;
                        cmd.Parameters.Add("@qty", MySqlDbType.Int32).Value = qty;
                        cmd.Parameters.Add("@before", MySqlDbType.Int32).Value = stockBefore;
                        cmd.Parameters.Add("@after", MySqlDbType.Int32).Value = stockBefore - qty;
                        cmd.Parameters.Add("@refId", MySqlDbType.Int32).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@remarks", MySqlDbType.VarChar).Value =
                            $"Stock Out from Sale #{id}";
                        cmd.ExecuteNonQuery();
                    }
                }
                else // becomesRefunded / Cancelled — restore stock
                {
                    // ── Restore stock ──────────────────────────────────────
                    using (var cmd = new MySqlCommand(
                               "UPDATE product SET stock_quantity = stock_quantity + @q " +
                               "WHERE product_id = @id",
                               conn, trans))
                    {
                        cmd.Parameters.Add("@q", MySqlDbType.Int32).Value = qty;
                        cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = pId;
                        cmd.ExecuteNonQuery();
                    }

                    // ── Log Adjustment (Refund/Cancel) ─────────────────────
                    string logRefund = @"INSERT INTO transaction_history 
                                  (product_id, user_id, type, quantity,
                                   quantity_before, quantity_after,
                                   reference_id, reference_type, remarks)
                                  VALUES
                                  (@prodId, @userId, 'Adjustment', @qty,
                                   @before, @after,
                                   @refId, 'Sales', @remarks)";

                    using (MySqlCommand cmd = new MySqlCommand(logRefund, conn, trans))
                    {
                        cmd.Parameters.Add("@prodId", MySqlDbType.Int32).Value = pId;
                        cmd.Parameters.Add("@userId", MySqlDbType.Int32).Value = GlobalUser.ID;
                        cmd.Parameters.Add("@qty", MySqlDbType.Int32).Value = qty;
                        cmd.Parameters.Add("@before", MySqlDbType.Int32).Value = stockBefore;
                        cmd.Parameters.Add("@after", MySqlDbType.Int32).Value = stockBefore + qty;
                        cmd.Parameters.Add("@refId", MySqlDbType.Int32).Value = Convert.ToInt32(id);
                        cmd.Parameters.Add("@remarks", MySqlDbType.VarChar).Value =
                            $"Stock restored from {newStatus} Sale #{id}";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }










        public void AddItemToOrder(string productID, string productName,
                                   int quantity, decimal price, int stock)
        {
            if (quantity <= 0 || price <= 0)
            {
                MessageBox.Show("Invalid quantity or price.",
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // If the product is already in the grid, accumulate
            foreach (DataGridViewRow row in dgv_sales_order.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["product_id"].Value?.ToString() == productID)
                {
                    int existing = Convert.ToInt32(row.Cells["quantity"].Value);
                    int newQty = existing + quantity;

                    if (newQty > stock)
                    {
                        MessageBox.Show(
                            $"Cannot add {quantity} unit(s). Only {stock - existing} more in stock.",
                            "Stock Limit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    row.Cells["quantity"].Value = newQty;
                    row.Cells["subtotal"].Value = newQty * price;
                    UpdateGrandTotal();
                    return;
                }
            }

            // New product row – leave sales_item_id blank (DB auto-assigns or we omit it)
            dgv_sales_order.Rows.Add(
                DBNull.Value,   // sales_item_id: not yet persisted
                productID,
                productName,
                quantity,
                price,
                quantity * price
            );

            UpdateGrandTotal();
        }

        public void UpdateGrandTotal()
        {
            currentGrandTotal = 0;

            foreach (DataGridViewRow row in dgv_sales_order.Rows)
            {
                if (row.IsNewRow) continue;

                if (decimal.TryParse(row.Cells["subtotal"].Value?.ToString(),
                                     out decimal sub))
                    currentGrandTotal += sub;
            }

            lbl_grand_total.Text = currentGrandTotal.ToString("N2");
        }

        private void UpdateRowSubtotal(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= dgv_sales_order.Rows.Count) return;

            DataGridViewRow row = dgv_sales_order.Rows[rowIndex];

            if (int.TryParse(row.Cells["quantity"].Value?.ToString(), out int quantity) &&
                decimal.TryParse(row.Cells["unit_price"].Value?.ToString(), out decimal price))
            {
                row.Cells["subtotal"].Value = (quantity * price).ToString("F2");
            }
        }

        private void dgv_sales_order_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgv_sales_order.Rows[e.RowIndex];

            // Guard: quantity must be a positive integer
            if (int.TryParse(row.Cells["quantity"].Value?.ToString(), out int qty) && qty <= 0)
            {
                MessageBox.Show("Quantity must be greater than zero.",
                                "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                row.Cells["quantity"].Value = 1;
            }

            // Delegate math to UpdateRowSubtotal so logic stays in one place
            UpdateRowSubtotal(e.RowIndex);
            UpdateGrandTotal();
        }



        private void btn_cancel_Click(object sender, EventArgs e)
        {
            ClearFields();
            this.Close();
        }
    }
}