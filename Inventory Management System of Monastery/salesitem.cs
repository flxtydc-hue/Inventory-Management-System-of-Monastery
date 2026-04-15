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
    public partial class salesitem : Form
    {
        private salesorder salesorder;
        private decimal itemPrice = 0.00m;
        private int maxStock = 0;
        private bool isUpdatingQuantity = false;

        public salesitem()
        {
            InitializeComponent();
        }

        public salesitem(salesorder salesorder)
        {
            InitializeComponent();
            this.salesorder = salesorder;

            txt_quantity.Text = "1"; // Default quantity
            txt_price.Text = "0.00"; // Default price
            btn_add.Enabled = false; // Disable Add button until item is selected
            LoadProducts();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (!ValidateQuantity()) return;

            DataRowView selectedRow = cmb_product.SelectedItem as DataRowView;
            if (selectedRow == null) return;

            string productID = selectedRow["product_id"].ToString();
            string productName = selectedRow["product_name"].ToString();
            int quantity = int.Parse(txt_quantity.Text);
            decimal price = itemPrice; 
            int currentStock = maxStock;

            // Tatawagin ang method sa main SalesOrder form
            salesorder.AddItemToOrder(productID, productName, quantity, price, currentStock);

            this.Close(); // Isara ang popup
        }

        private void LoadProducts()
        {
            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    // Siguraduhing active ang products at may stock para sa monasteryo inventory
                    string query = "SELECT product_id, product_name, unit_price, stock_quantity FROM product WHERE is_archived = FALSE AND stock_quantity > 0";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    {
                        DataTable dt = new DataTable();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(dt);

                        cmb_product.DataSource = dt;
                        cmb_product.DisplayMember = "product_name";
                        cmb_product.ValueMember = "product_id";
                        cmb_product.SelectedIndex = -1; // Default na walang pili
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_product_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmb_product.SelectedItem is DataRowView row)
            {
                itemPrice = Convert.ToDecimal(row["unit_price"]);
                maxStock = Convert.ToInt32(row["stock_quantity"]);

                // I-update ang bagong textbox price
                txt_price.Text = itemPrice.ToString("F2");
                txt_quantity.Text = "1";

                UpdateSubtotalAndValidation();
                btn_add.Enabled = true;
            }
        }

        private void UpdateSubtotalAndValidation()
        {
            if (int.TryParse(txt_quantity.Text, out int qty))
            {
                decimal subtotal = itemPrice * qty;
                txt_subtotal.Text = subtotal.ToString("F2"); // I-display sa bagong textbox
            }
            else
            {
                txt_subtotal.Text = "0.00";
            }

            ValidateQuantity();
        }

        private bool ValidateQuantity()
        {
            if (cmb_product.SelectedIndex == -1 || itemPrice == 0.00m)
            {
                lbl_warning.Text = "Select Product";
                lbl_warning.ForeColor = Color.Gray;
                btn_add.Enabled = false;
                return false;
            }

            if (!int.TryParse(txt_quantity.Text, out int qty))
            {
                lbl_warning.Text = "Invalid Qty";
                lbl_warning.ForeColor = Color.Orange;
                btn_add.Enabled = false;
                return false;
            }

            if (qty <= 0)
            {
                lbl_warning.Text = "Min Qty 1";
                lbl_warning.ForeColor = Color.Orange;
                btn_add.Enabled = false;
                return false;
            }

            if (qty > maxStock)
            {
                lbl_warning.Text = $"Out of Stock (Max: {maxStock})";
                lbl_warning.ForeColor = Color.Red;
                btn_add.Enabled = false;
                return false;
            }

            // Success state
            lbl_warning.Text = $"In Stock ({maxStock} left)";
            lbl_warning.ForeColor = Color.Green;
            btn_add.Enabled = true;
            return true;
        }

        private void txt_quantity_TextChanged(object sender, EventArgs e)
        {
            UpdateSubtotalAndValidation();
        }

        private void lbl_warning_Click(object sender, EventArgs e)
        {

        }
    }
}
