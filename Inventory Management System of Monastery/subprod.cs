using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Inventory_Management_System_of_Monastery
{
    public partial class subprod : Form
    {
        products p;
        // Gagamitin natin ito para malaman kung Edit o Add mode
        public string selected_product_id;

        public subprod(products p)
        {
            InitializeComponent();
            this.p = p;
        }

        public subprod()
        {
            InitializeComponent();
            this.p = null;
        }


        private void subprod_Load(object sender, EventArgs e)
        {
            // 1. Laging i-load ang Categories at Suppliers sa dropdown pagbukas
            LoadCategories();
            LoadSuppliers();

            // 2. Kung may laman ang product_id, ibig sabihin EDIT mode tayo
            if (!string.IsNullOrEmpty(this.selected_product_id))
            {
                LoadProductDetails(this.selected_product_id);
            }
        }

        // --- MGA HELPER FUNCTIONS ---

        public void LoadCategories()
        {
            using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
            {
                cn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT category_id, category_name FROM category ORDER BY category_name", cn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmb_category.DataSource = dt;
                cmb_category.DisplayMember = "category_name";
                cmb_category.ValueMember = "category_id";
                cmb_category.SelectedIndex = -1; // Default empty
            }
        }

        public void LoadSuppliers()
        {
            using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
            {
                cn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT supplier_id, supplier_name FROM supplier ORDER BY supplier_name", cn);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                cmb_supplier.DataSource = dt;
                cmb_supplier.DisplayMember = "supplier_name";
                cmb_supplier.ValueMember = "supplier_id";
                cmb_supplier.SelectedIndex = -1;
            }
        }

        public void LoadProductDetails(string id)
        {
            this.btn_save.Enabled = false;
            this.btn_update.Enabled = true;

            string query = "SELECT * FROM product WHERE product_id = @id";
            using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
            {
                cn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txt_product_name.Text = dr["product_name"].ToString();
                            txt_unit_price.Text = dr["unit_price"].ToString();
                            txt_stock_quantity.Text = dr["stock_quantity"].ToString();

                            // ✅ I-disable ang stock field sa edit mode
                            txt_stock_quantity.Enabled = false;
                            txt_stock_quantity.BackColor = System.Drawing.Color.LightGray;

                            ToolTip toolTip = new ToolTip();
                            toolTip.SetToolTip(txt_stock_quantity, "Stock quantity can only be changed via Stock Adjustment.");

                            cmb_category.SelectedValue = dr["category_id"];
                            cmb_supplier.SelectedValue = dr["supplier_id"];
                        }
                    }
                }
            }
        }

        // --- BUTTON CLICKS ---

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (IsInvalid()) return;

            try
            {
                string query = "INSERT INTO product (product_name, category_id, supplier_id, unit_price, stock_quantity, low_stock_threshold) " +
                               "VALUES (@name, @category, @supplier, @price, @stock, @threshold)";

                ExecuteQuery(query, "added");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (IsInvalid()) return;

            try
            {
                // ✅ Wala nang stock_quantity dito — hindi na natin ito tinutuching
                string query = "UPDATE product SET product_name=@name, category_id=@category, " +
                               "supplier_id=@supplier, unit_price=@price WHERE product_id=@id";

                ExecuteQuery(query, "updated");
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void ExecuteQuery(string query, string action)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseConnection.connectionString))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", txt_product_name.Text);
                    cmd.Parameters.AddWithValue("@category", cmb_category.SelectedValue);
                    cmd.Parameters.AddWithValue("@supplier", cmb_supplier.SelectedValue);
                    cmd.Parameters.AddWithValue("@price", decimal.Parse(txt_unit_price.Text));

                    // ✅ Ang @stock at @threshold — sa ADD mode lang
                    if (action == "added")
                    {
                        cmd.Parameters.AddWithValue("@stock", int.Parse(txt_stock_quantity.Text));
                        cmd.Parameters.AddWithValue("@threshold", 5);
                    }

                    if (action == "updated")
                        cmd.Parameters.AddWithValue("@id", selected_product_id);

                    cmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show($"Product {action} successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            p?.LoadProductData();
            this.Close();
        }

        private bool IsInvalid()
        {
            if (string.IsNullOrEmpty(txt_product_name.Text) || cmb_category.SelectedIndex == -1 || cmb_supplier.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all required fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            return false;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_stock_quantity_TextChanged(object sender, EventArgs e)
        {

        }
    }
}