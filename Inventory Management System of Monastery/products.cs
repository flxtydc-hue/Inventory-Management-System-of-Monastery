using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Inventory_Management_System_of_Monastery
{
    public partial class products : Form
    {
        private dashboard dashboard;
        // Gagamitin natin ang connection string mula sa iyong DatabaseConnection class
        private string connString = DatabaseConnection.connectionString;

        public products(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;

            // Mahalaga: I-bind ang event handler
            dgv_products.CellFormatting += dgv_products_CellFormatting;
        }

        public void LoadProductData()
        {
            // Gagamitin natin ang CASE logic para laging updated ang Status base sa current stock vs threshold
            // Sa LoadProductData at SearchProducts ng products.cs
            string query = @"SELECT 
                    p.product_id, 
                    p.product_name, 
                    c.category_name, 
                    s.supplier_name, 
                    p.unit_price, 
                    p.stock_quantity, 
                    CASE 
                        WHEN p.stock_quantity <= 0 THEN 'Out of Stock'
                        WHEN p.stock_quantity <= p.low_stock_threshold THEN 'Low Stock' 
                        ELSE 'Active' 
                    END AS product_status,
                    p.low_stock_threshold, 
                    p.is_archived 
                 FROM product p 
                 JOIN category c ON p.category_id = c.category_id 
                 JOIN supplier s ON p.supplier_id = s.supplier_id 
                 WHERE p.is_archived = FALSE 
                 ORDER BY p.product_name ASC";

            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    dgv_products.AutoGenerateColumns = false; // Pigilan ang pagdoble ng columns

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dgv_products.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgv_products_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // FIX: Siniguro na tama ang column name reference
            if (dgv_products.Columns[e.ColumnIndex].Name == "stock_quantity" && e.Value != null)
            {
                try
                {
                    int stock = Convert.ToInt32(e.Value);
                    // Kinukuha ang threshold value mula sa hidden column sa parehong row
                    int threshold = Convert.ToInt32(dgv_products.Rows[e.RowIndex].Cells["low_stock_threshold"].Value);

                    if (stock <= threshold)
                    {
                        e.CellStyle.ForeColor = Color.Red;
                        e.CellStyle.SelectionForeColor = Color.Red; // Para red pa rin kahit i-click ng user
                        e.CellStyle.Font = new Font(dgv_products.Font, FontStyle.Bold);
                    }
                }
                catch { /* Iwas error kung sakaling hindi integer ang value */ }
            }
        }

        private void dgv_products_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string colName = dgv_products.Columns[e.ColumnIndex].Name;

            if (colName == "edit")
            {
                int prodID = Convert.ToInt32(dgv_products.Rows[e.RowIndex].Cells["product_id"].Value);

                subprod sp = new subprod(this);
                sp.btn_save.Enabled = false;
                sp.btn_update.Enabled = true;

                sp.selected_product_id = prodID.ToString();
                sp.LoadCategories();

                // FIX: Ipasa ang ID dito para hindi mag-error
                sp.LoadProductDetails(sp.selected_product_id);

                sp.ShowDialog();
            }
            else if (colName == "delete")
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to archive this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.Yes)
                {
                    int prodID = Convert.ToInt32(dgv_products.Rows[e.RowIndex].Cells["product_id"].Value);
                    ArchiveProduct(prodID);
                }
            }
        }

        private void ArchiveProduct(int id)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();
                    string query = "UPDATE product SET is_archived = TRUE WHERE product_id = @id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Product archived successfully.");
                    LoadProductData(); // Refresh table
                }
            }
            catch (Exception ex) { MessageBox.Show("Archive failed: " + ex.Message); }
        }

        private void products_Load(object sender, EventArgs e)
        {
            LoadProductData();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            subprod sp = new subprod(this);
            sp.btn_update.Enabled = false;
            sp.btn_save.Enabled = true;
            sp.LoadCategories();
            sp.ShowDialog();
        }

        private void btn_category_Click(object sender, EventArgs e)
        {
            category c = new category(this);
            c.FormClosed += (s, args) => LoadProductData();
            c.ShowDialog();
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            SearchProducts(txt_search.Text.Trim());
        }

        public void SearchProducts(string keyword)
        {
            string query = @"SELECT 
                        p.product_id, 
                        p.product_name, 
                        c.category_name, 
                        s.supplier_name, 
                        p.unit_price, 
                        p.stock_quantity, 
                        CASE 
                            WHEN p.stock_quantity <= 0 THEN 'Archived'
                            WHEN p.stock_quantity <= p.low_stock_threshold THEN 'Low Stock' 
                            ELSE 'Active' 
                        END AS product_status,
                        p.low_stock_threshold, 
                        p.supplier_id, 
                        p.is_archived 
                     FROM product p 
                     JOIN category c ON p.category_id = c.category_id 
                     JOIN supplier s ON p.supplier_id = s.supplier_id 
                     WHERE p.is_archived = FALSE AND (p.product_name LIKE @keyword OR c.category_name LIKE @keyword)";
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dgv_products.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_view_Click(object sender, EventArgs e)
        {
            archive a = new archive(this);
            this.dashboard.ShowForm(a);
        }

        private void btn_adjust_Click(object sender, EventArgs e)
        {
            adjustment adj = new adjustment(this);
            adj.ShowDialog();
        }
    }
}