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
    public partial class purchaseitem : Form
    {
        private purchaseorder po;
        private string selectedSupplierId;
        private string selectedSupplierName;

        public purchaseitem(purchaseorder po, string supplierId, string supplierName)
        {
            InitializeComponent();
            this.po = po;
            this.selectedSupplierId = supplierId;
            this.selectedSupplierName = supplierName;
            this.Text = $"Add Item  —  Supplier: {supplierName}";
        }

        // ── Inner class for ComboBox binding ─────────────────────────────────
        private class ProductItem
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public decimal UnitPrice { get; set; }   // Default cost price from product table
            public override string ToString() => Name;
        }

        // ── Form Load ─────────────────────────────────────────────────────────
        private void purchaseitem_Load(object sender, EventArgs e)
        {
            LoadProductsForComboBox();
        }

        // ── Load all active products into the ComboBox ────────────────────────
        public void LoadProductsForComboBox()
        {
            List<ProductItem> products = new List<ProductItem>();

            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();

                    // unit_price is the correct column name per the schema
                    string query = @"SELECT product_id, product_name, unit_price 
                                     FROM product 
                                     WHERE is_archived = 0 
                                       AND product_status = 'Active'
                                     ORDER BY product_name ASC";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new ProductItem
                            {
                                ID = reader["product_id"].ToString(),
                                Name = reader["product_name"].ToString(),
                                UnitPrice = reader.GetDecimal("unit_price")
                            });
                        }
                    }
                }

                

                cmb_product.DataSource = products;
                cmb_product.DisplayMember = "Name";
                cmb_product.ValueMember = "ID";
                cmb_product.SelectedIndex = -1;

                // Clear price field until user picks a product
                txt_price.Clear();
                txt_quantity.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading products: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // ── Auto-fill price when a product is selected ────────────────────────
        private void cmb_product_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_product.SelectedItem is ProductItem selected)
            {
                // Wala nang ID == "-1" check, wala nang sentinel
                txt_price.Text = selected.UnitPrice.ToString("F2");
                txt_quantity.Focus();
            }
            else
            {
                txt_price.Clear();
            }
        }

        // ── Add item to the parent purchase order ─────────────────────────────
        private void btn_add_Click(object sender, EventArgs e)
        {
            // ── Validation ───────────────────────────────────────────────────
            if (cmb_product.SelectedIndex == -1 || !(cmb_product.SelectedItem is ProductItem selectedProduct))
            {
                MessageBox.Show("Please select a product.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_product.Focus();
                return;
            }

            if (selectedProduct.ID == "-1")
            {
                MessageBox.Show("Please select a valid product.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmb_product.SelectedIndex = -1;
                return;
            }

            if (!int.TryParse(txt_quantity.Text.Trim(), out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Please enter a valid quantity (whole number, greater than 0).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_quantity.Focus();
                return;
            }

            if (!decimal.TryParse(txt_price.Text.Trim(), out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid unit price (greater than 0).", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_price.Focus();
                return;
            }

            // ── Pass data to parent purchaseorder form ───────────────────────
            decimal total = price * quantity;
            po.AddItemToPurchaseOrder(selectedProduct.ID, selectedProduct.Name, quantity, price, total);

            // ── Reset fields for the next item ───────────────────────────────
            cmb_product.SelectedIndex = -1;
            txt_quantity.Clear();
            txt_price.Clear();
            cmb_product.Focus();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_subtotal_TextChanged(object sender, EventArgs e)
        {
        }

        private void CalculateSubtotal()
        {
            // Susubukan nating i-parse ang values. Kung hindi number, 0 ang gagamitin.
            decimal price = 0;
            int qty = 0;

            decimal.TryParse(txt_price.Text, out price);
            int.TryParse(txt_quantity.Text, out qty);

            decimal subtotal = price * qty;

            // I-format natin as 2 decimal places
            txt_subtotal.Text = subtotal.ToString("F2");
        }

        private void txt_quantity_TextChanged(object sender, EventArgs e)
        {
            CalculateSubtotal();
        }

        private void txt_price_TextChanged(object sender, EventArgs e)
        {
            CalculateSubtotal();
        }

        private void btn_add_new_Click(object sender, EventArgs e)
        {
            subprod subprod = new subprod();
            subprod.ShowDialog();
            LoadProductsForComboBox();
        }
    }
}