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
    public partial class purchaseorder : Form
    {
        private purchase purchase;

        private bool isEditMode = false;
        private bool isViewMode = false;
        public string id { get; private set; }

        // ── Constructor: ADD NEW PO ──────────────────────────────────────────
        public purchaseorder(purchase purchase)
        {
            InitializeComponent();
            this.purchase = purchase;
            this.id = string.Empty;
            this.Load += PurchaseOrder_Load;
        }

        // ── Constructor: VIEW or EDIT existing PO ───────────────────────────
        public purchaseorder(purchase purchase, string id, bool isViewMode = false)
        {
            InitializeComponent();
            this.purchase = purchase;
            this.id = id;
            this.isViewMode = isViewMode;
            this.isEditMode = !isViewMode;
            this.Text = isViewMode ? $"View Purchase Order - {id}" : $"Edit Purchase Order - {id}";

            this.Load += PurchaseOrder_Load;


            if (isViewMode)
                DisableControls();
            else
            {
                btn_save.Enabled = true;
                btn_add.Enabled = true;
                dtp_order_date.Enabled = true;
            }
        }

        // ── Disable all inputs in view-only mode ─────────────────────────────
        private void DisableControls()
        {
            btn_add.Enabled = false;
            dtp_order_date.Enabled = false;
            btn_cancel.Enabled = false;
            btn_save.Enabled = false;

            if (cmb_status != null)
            {
                cmb_status.Enabled = false;
                cmb_status.Visible = true;
            }

            if (cmb_supplier_name != null)
            {
                cmb_supplier_name.Enabled = false;
                cmb_supplier_name.Visible = true;
            }
        }

        // ── Inner class for ComboBox binding ────────────────────────────────
        private class SupplierItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }

        // ── Load status dropdown ─────────────────────────────────────────────
        private void LoadStatuses()
        {
            List<string> statuses = new List<string> { "Pending", "Completed", "Cancelled" };

            if (cmb_status != null)
                cmb_status.DataSource = statuses;
        }

        // ── Load active suppliers into ComboBox ──────────────────────────────
        private void LoadSuppliers()
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    string query = "SELECT supplier_id, supplier_name FROM supplier WHERE is_active = 1 ORDER BY supplier_name";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<SupplierItem> suppliers = new List<SupplierItem>();

                        while (reader.Read())
                        {
                            suppliers.Add(new SupplierItem
                            {
                                ID = reader.GetInt32("supplier_id"),
                                Name = reader.GetString("supplier_name")
                            });
                        }

                        if (cmb_supplier_name != null)
                        {
                            cmb_supplier_name.DataSource = suppliers;
                            cmb_supplier_name.DisplayMember = "Name";
                            cmb_supplier_name.ValueMember = "ID";
                            cmb_supplier_name.SelectedIndex = -1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading suppliers: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Form Load ────────────────────────────────────────────────────────
        private void PurchaseOrder_Load(object sender, EventArgs e)
        {
            LoadStatuses();
            LoadSuppliers();

            if (!string.IsNullOrEmpty(this.id))
            {
                // Existing PO: load header then items
                LoadPurchaseDetails(this.id);
                LoadPurchaseItems(this.id);

                // Supplier cannot be changed on an existing PO
                cmb_supplier_name.Enabled = false;

                if (isViewMode)
                {
                    DisableControls();
                    cmb_status.Enabled = false;
                }
                else if (isEditMode)
                {
                    cmb_status.Enabled = true;
                }
            }
            else
            {
                // New PO defaults
                dtp_order_date.Value = DateTime.Today;
                dgv_purchase_order.Rows.Clear();
                lbl_grand_total.Text = "0.00";

                if (cmb_supplier_name != null)
                    cmb_supplier_name.SelectedIndex = -1;

                if (cmb_status != null)
                {
                    cmb_status.SelectedItem = "Pending";
                    cmb_status.Visible = false;   // Hidden for new POs (always Pending)
                    cmb_status.Enabled = false;
                }
            }
        }

        // ── Load PO header (supplier, date, grand total, status) ─────────────
        private void LoadPurchaseDetails(string id)
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();

                    // FIX: grand_total is now included in SELECT
                    string query = @"SELECT 
                                        po.supplier_id, 
                                        s.supplier_name, 
                                        po.order_date, 
                                        po.grand_total,
                                        po.status
                                     FROM purchase_order po
                                     JOIN supplier s ON po.supplier_id = s.supplier_id
                                     WHERE po.po_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    {
                        // FIX: parameter name now matches the query placeholder (@id)
                        cmd.Parameters.AddWithValue("@id", id);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // ── Supplier ────────────────────────────────
                                string supplierName = reader["supplier_name"].ToString();
                                List<SupplierItem> suppliersList = cmb_supplier_name.DataSource as List<SupplierItem>;
                                SupplierItem selectedSupplier = suppliersList?.FirstOrDefault(s => s.Name == supplierName);

                                if (selectedSupplier != null)
                                    cmb_supplier_name.SelectedItem = selectedSupplier;
                                else
                                    cmb_supplier_name.Text = supplierName; // Fallback for inactive suppliers

                                // ── Order Date ──────────────────────────────
                                dtp_order_date.Value = Convert.ToDateTime(reader["order_date"]);

                                // ── Grand Total ─────────────────────────────
                                // FIX: grand_total now exists in the SELECT result
                                decimal grandTotal = reader.GetDecimal("grand_total");
                                lbl_grand_total.Text = grandTotal.ToString("F2");

                                // ── Status ──────────────────────────────────
                                string status = reader.GetString("status");
                                if (cmb_status != null && cmb_status.Items.Contains(status))
                                    cmb_status.SelectedItem = status;
                                else if (cmb_status != null)
                                    cmb_status.SelectedItem = "Pending"; // Safe fallback
                            }
                            else
                            {
                                MessageBox.Show("Purchase order not found.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error loading purchase details: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading purchase details: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Load PO line items into the DataGridView ──────────────────────────
        public void LoadPurchaseItems(string purchaseId)
        {
            try
            {
                dgv_purchase_order.Rows.Clear();

                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();

                    // FIX: correct table name (po_items, not purchase_order_item)
                    // FIX: correct column aliases that match reader.Get calls below
                    string query = @"SELECT 
                                        poi.po_item_id,
                                        poi.product_id, 
                                        p.product_name, 
                                        poi.quantity, 
                                        poi.unit_price, 
                                        (poi.quantity * poi.unit_price) AS total_price
                                     FROM po_items poi
                                     JOIN product p ON poi.product_id = p.product_id
                                     WHERE poi.po_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    {
                        // FIX: parameter name matches @id in the query
                        cmd.Parameters.AddWithValue("@id", purchaseId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // FIX: reader column names now match the SELECT aliases
                                dgv_purchase_order.Rows.Add(
                                    reader.GetInt32("po_item_id"),           // hidden row ID
                                    reader["product_id"].ToString(),
                                    reader["product_name"].ToString(),
                                    reader.GetInt32("quantity"),
                                    reader.GetDecimal("unit_price").ToString("F2"),
                                    reader.GetDecimal("total_price").ToString("F2")
                                );
                            }
                        }
                    }
                }

                dgv_purchase_order.ClearSelection();
                UpdateGrandTotal(); // Recalculate total from loaded rows
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Database error loading purchase items: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading purchase items: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Open purchaseitem dialog to add a product ─────────────────────────
        private void btn_add_Click(object sender, EventArgs e)
        {
            if (cmb_supplier_name.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier first.", title.name);
                return;
            }

            string supplierId = cmb_supplier_name.SelectedValue.ToString();
            string supplierName = cmb_supplier_name.Text;

            purchaseitem pi = new purchaseitem(this, supplierId, supplierName);
            pi.ShowDialog();
        }

        // ── Add a product row to the DGV (called from purchaseitem form) ──────
        public void AddItemToPurchaseOrder(string productID, string productName, int quantity, decimal price, decimal total)
        {
            // If the product already exists in the grid, just increase its quantity
            foreach (DataGridViewRow row in dgv_purchase_order.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["product_id"].Value != null &&
                    row.Cells["product_id"].Value.ToString() == productID)
                {
                    int existingQty = Convert.ToInt32(row.Cells["quantity"].Value);
                    int newQty = existingQty + quantity;

                    row.Cells["quantity"].Value = newQty;
                    row.Cells["unit_price"].Value = price.ToString("F2");
                    row.Cells["subtotal"].Value = (price * newQty).ToString("F2");

                    UpdateGrandTotal();
                    return;
                }
            }

            // New product — add a fresh row
            // Column order must match DGV designer: po_item_id | product_id | product_name | quantity | unit_price | subtotal
            dgv_purchase_order.Rows.Add(
                0,                          // po_item_id (0 = not yet saved)
                productID,
                productName,
                quantity,
                price.ToString("F2"),
                total.ToString("F2")
            );

            UpdateGrandTotal();
        }

        // ── Recalculate grand total from all rows ────────────────────────────
        public void UpdateGrandTotal()
        {
            decimal grandTotal = 0m;

            foreach (DataGridViewRow row in dgv_purchase_order.Rows)
            {
                if (row.IsNewRow) continue;

                if (row.Cells["subtotal"].Value != null &&
                    decimal.TryParse(row.Cells["subtotal"].Value.ToString(), out decimal rowTotal))
                {
                    grandTotal += rowTotal;
                }
            }

            lbl_grand_total.Text = grandTotal.ToString("F2");
        }

        // ── Recalculate a single row's subtotal after inline edit ────────────
        private void UpdateRowSubtotal(int rowIndex)
        {
            DataGridViewRow row = dgv_purchase_order.Rows[rowIndex];

            if (int.TryParse(row.Cells["quantity"].Value?.ToString(), out int quantity) &&
                decimal.TryParse(row.Cells["unit_price"].Value?.ToString(), out decimal price))
            {
                row.Cells["subtotal"].Value = (quantity * price).ToString("F2");
            }
        }

        private void dgv_purchase_order_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                UpdateRowSubtotal(e.RowIndex);
                UpdateGrandTotal();
            }
        }

        // ── Save (Insert or Update) Purchase Order ────────────────────────────
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (cmb_supplier_name.SelectedValue == null)
            {
                MessageBox.Show("Please select a supplier.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool hasItems = false;
            foreach (DataGridViewRow row in dgv_purchase_order.Rows)
            {
                if (!row.IsNewRow) { hasItems = true; break; }
            }

            if (!hasItems)
            {
                MessageBox.Show("Please add at least one item.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedSupplierID = Convert.ToInt32(cmb_supplier_name.SelectedValue);

            if (!decimal.TryParse(lbl_grand_total.Text, out decimal grandTotal) || grandTotal <= 0)
            {
                MessageBox.Show("Grand total is invalid. Please check the items.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string newStatus = string.IsNullOrEmpty(this.id)
                               ? "Pending"
                               : (cmb_status.SelectedItem?.ToString() ?? "Pending");

            using (MySqlConnection cn = DatabaseConnection.GetConnection())
            {
                cn.Open();
                MySqlTransaction transaction = cn.BeginTransaction();

                try
                {
                    int currentPoId;

                    if (string.IsNullOrEmpty(this.id))
                    {
                        // ════ INSERT new PO header ════════════════════════════
                        string insertHeader = @"INSERT INTO purchase_order 
                                            (supplier_id, order_date, grand_total, status) 
                                        VALUES 
                                            (@supplierId, @orderDate, @grandTotal, @status);
                                        SELECT LAST_INSERT_ID();";

                        using (MySqlCommand cmd = new MySqlCommand(insertHeader, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@supplierId", selectedSupplierID);
                            cmd.Parameters.AddWithValue("@orderDate", dtp_order_date.Value);
                            cmd.Parameters.AddWithValue("@grandTotal", grandTotal);
                            cmd.Parameters.AddWithValue("@status", newStatus);

                            currentPoId = Convert.ToInt32(cmd.ExecuteScalar());
                            this.id = currentPoId.ToString();
                        }
                    }
                    else
                    {
                        // ════ UPDATE existing PO ══════════════════════════════
                        currentPoId = Convert.ToInt32(this.id);

                        // A. Get old status
                        string getOldStatus = "SELECT status FROM purchase_order WHERE po_id = @poId";
                        string oldStatus = "";

                        using (MySqlCommand cmd = new MySqlCommand(getOldStatus, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@poId", currentPoId);
                            oldStatus = cmd.ExecuteScalar()?.ToString() ?? "";
                        }

                        // B. If was Completed — check stock then reverse
                        if (oldStatus == "Completed")
                        {
                            // 1. Check if reversing causes negative stock
                            string checkStock = @"
                        SELECT p.product_name, p.stock_quantity, poi.quantity, poi.product_id
                        FROM product p
                        JOIN po_items poi ON p.product_id = poi.product_id
                        WHERE poi.po_id = @poId";

                            // Store items for logging after reversal
                            List<(int productId, int quantity, int stockNow)> itemsToReverse
                                = new List<(int, int, int)>();

                            using (MySqlCommand cmd = new MySqlCommand(checkStock, cn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@poId", currentPoId);

                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        int currentStock = Convert.ToInt32(reader["stock_quantity"]);
                                        int qtyToRemove = Convert.ToInt32(reader["quantity"]);
                                        string productName = reader["product_name"].ToString();
                                        int pId = Convert.ToInt32(reader["product_id"]);

                                        if (currentStock - qtyToRemove < 0)
                                        {
                                            throw new Exception(
                                                $"Cannot update purchase order.\n\n" +
                                                $"Reversing stock for '{productName}' would result in negative inventory.\n" +
                                                $"Current Stock: {currentStock}\n" +
                                                $"Required to remove: {qtyToRemove}");
                                        }

                                        itemsToReverse.Add((pId, qtyToRemove, currentStock));
                                    }
                                }
                            }

                            // 2. Reverse stock
                            string reverseStock = @"
                        UPDATE product p 
                        JOIN po_items poi ON p.product_id = poi.product_id 
                        SET p.stock_quantity = p.stock_quantity - poi.quantity 
                        WHERE poi.po_id = @poId";

                            using (MySqlCommand cmd = new MySqlCommand(reverseStock, cn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@poId", currentPoId);
                                cmd.ExecuteNonQuery();
                            }

                            // 3. Log reversal per item
                            foreach (var item in itemsToReverse)
                            {
                                string logReversal = @"INSERT INTO transaction_history 
                                                (product_id, user_id, type, quantity,
                                                 quantity_before, quantity_after,
                                                 reference_id, reference_type, remarks)
                                                VALUES
                                                (@prodId, @userId, 'Adjustment', @qty,
                                                 @before, @after,
                                                 @refId, 'Purchase Order', @remarks)";

                                using (MySqlCommand cmd = new MySqlCommand(logReversal, cn, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@prodId", item.productId);
                                    cmd.Parameters.AddWithValue("@userId", GlobalUser.ID);
                                    cmd.Parameters.AddWithValue("@qty", item.quantity);
                                    cmd.Parameters.AddWithValue("@before", item.stockNow);
                                    cmd.Parameters.AddWithValue("@after", item.stockNow - item.quantity);
                                    cmd.Parameters.AddWithValue("@refId", currentPoId);
                                    cmd.Parameters.AddWithValue("@remarks", $"Stock reversed from PO #{currentPoId} edit");
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        // C. Delete old line items
                        string deleteItems = "DELETE FROM po_items WHERE po_id = @poId";
                        using (MySqlCommand cmd = new MySqlCommand(deleteItems, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@poId", currentPoId);
                            cmd.ExecuteNonQuery();
                        }

                        // D. Update PO header
                        string updateHeader = @"UPDATE purchase_order 
                                        SET supplier_id = @supId, 
                                            order_date  = @oDate, 
                                            grand_total = @gTotal, 
                                            status      = @status 
                                        WHERE po_id = @poId";

                        using (MySqlCommand cmd = new MySqlCommand(updateHeader, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@supId", selectedSupplierID);
                            cmd.Parameters.AddWithValue("@oDate", dtp_order_date.Value);
                            cmd.Parameters.AddWithValue("@gTotal", grandTotal);
                            cmd.Parameters.AddWithValue("@status", newStatus);
                            cmd.Parameters.AddWithValue("@poId", currentPoId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // ════ INSERT line items & conditionally update stock ══════
                    // ✅ OUTSIDE ng lahat ng if/else — palaging tumatakbo
                    foreach (DataGridViewRow row in dgv_purchase_order.Rows)
                    {
                        if (row.IsNewRow) continue;

                        if (!int.TryParse(row.Cells["product_id"].Value?.ToString(), out int productId)) continue;
                        if (!int.TryParse(row.Cells["quantity"].Value?.ToString(), out int qty) || qty <= 0) continue;
                        if (!decimal.TryParse(row.Cells["unit_price"].Value?.ToString(), out decimal uPrice)) continue;

                        // Insert into po_items
                        string insertItem = @"INSERT INTO po_items 
                                        (po_id, product_id, quantity, unit_price) 
                                     VALUES 
                                        (@poId, @prodId, @qty, @price)";

                        using (MySqlCommand cmd = new MySqlCommand(insertItem, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@poId", currentPoId);
                            cmd.Parameters.AddWithValue("@prodId", productId);
                            cmd.Parameters.AddWithValue("@qty", qty);
                            cmd.Parameters.AddWithValue("@price", uPrice);
                            cmd.ExecuteNonQuery();
                        }

                        // Add to stock ONLY when Completed
                        if (newStatus == "Completed")
                        {
                            // Get stock before update
                            int stockBefore = 0;
                            string getStock = "SELECT stock_quantity FROM product WHERE product_id = @prodId";
                            using (MySqlCommand cmd = new MySqlCommand(getStock, cn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@prodId", productId);
                                stockBefore = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            // Add to stock
                            string addStock = @"UPDATE product 
                                       SET stock_quantity = stock_quantity + @qty 
                                       WHERE product_id = @prodId";

                            using (MySqlCommand cmd = new MySqlCommand(addStock, cn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@qty", qty);
                                cmd.Parameters.AddWithValue("@prodId", productId);
                                cmd.ExecuteNonQuery();
                            }

                            // Log Stock In
                            string logTrans = @"INSERT INTO transaction_history 
                                        (product_id, user_id, type, quantity,
                                         quantity_before, quantity_after,
                                         reference_id, reference_type, remarks)
                                        VALUES
                                        (@prodId, @userId, 'Stock In', @qty,
                                         @before, @after,
                                         @refId, 'Purchase Order', @remarks)";

                            using (MySqlCommand cmd = new MySqlCommand(logTrans, cn, transaction))
                            {
                                cmd.Parameters.AddWithValue("@prodId", productId);
                                cmd.Parameters.AddWithValue("@userId", GlobalUser.ID);
                                cmd.Parameters.AddWithValue("@qty", qty);
                                cmd.Parameters.AddWithValue("@before", stockBefore);
                                cmd.Parameters.AddWithValue("@after", stockBefore + qty);
                                cmd.Parameters.AddWithValue("@refId", currentPoId);
                                cmd.Parameters.AddWithValue("@remarks", $"Stock In from PO #{currentPoId}");
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    // ✅ COMMIT — nasa labas ng lahat ng if/else
                    transaction.Commit();

                    MessageBox.Show(
                        "Purchase order saved successfully!",
                        title.name,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.purchase?.LoadPurchaseOrders();
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show(
                        "Error saving purchase order: " + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_purchase_order_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Reserved for future use (e.g. remove row button)
        }
    }
}