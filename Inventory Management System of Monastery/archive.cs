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
    public partial class archive : Form
    {
        private MySql.Data.MySqlClient.MySqlConnection cn;
        private products p;
        public archive(products p)
        {
            InitializeComponent();
            this.p = p;
            LoadArchivedProducts();
        }

        private void dgv_archive_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;

                string colName = dgv_archived.Columns[e.ColumnIndex].Name;

                if (colName == "restore")
                {

                    string ID = dgv_archived.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string product_name = dgv_archived.Rows[e.RowIndex].Cells[1].Value.ToString();

                    confirmation c = new confirmation(ID, product_name, this.p, this);
                    c.ShowDialog();

                    LoadArchivedProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing request: " + ex.Message);

            }
        }

        public void LoadArchivedProducts()
        {
            dgv_archived.Rows.Clear();
            using (MySqlConnection cn = DatabaseConnection.GetConnection())
            {
                try
                {
                    cn.Open();
                    // DINAGDAG: JOIN sa supplier table at s.supplier_name sa SELECT
                    string query = @"SELECT p.product_id, p.product_name, c.category_name, s.supplier_name, p.unit_price, p.stock_quantity
                             FROM product p
                             LEFT JOIN category c ON p.category_id = c.category_id
                             LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                             WHERE p.is_archived = 1
                             ORDER BY p.product_id";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dgv_archived.Rows.Add(
                                dr["product_id"].ToString(),
                                dr["product_name"].ToString(),
                                dr["category_name"] == DBNull.Value ? "Uncategorized" : dr["category_name"].ToString(),
                                dr["supplier_name"] == DBNull.Value ? "No Supplier" : dr["supplier_name"].ToString(), // DINAGDAG ITO
                                dr["unit_price"].ToString(),
                                dr["stock_quantity"].ToString(),
                                "Archived"
                            );
                        }
                    }
                    dgv_archived.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading archives: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        public void SearchArchivedProducts(string searchTerm)
        {
            dgv_archived.Rows.Clear();
            using (MySqlConnection cn = DatabaseConnection.GetConnection())
            {
                try
                {
                    cn.Open();
                    // Sa loob ng SearchArchivedProducts method, palitan ang query:
                    string query = @"SELECT p.product_id, p.product_name, c.category_name, s.supplier_name, p.unit_price, p.stock_quantity
                 FROM product p
                 LEFT JOIN category c ON p.category_id = c.category_id
                 LEFT JOIN supplier s ON p.supplier_id = s.supplier_id
                 WHERE p.is_archived = 1, product_status = 'Archived'   
                 AND (p.product_name LIKE @search OR c.category_name LIKE @search OR s.supplier_name LIKE @search)
                 ORDER BY p.product_id";
                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchTerm + "%");
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dgv_archived.Rows.Add(
                                    dr["product_id"].ToString(),
                                    dr["product_name"].ToString(),
                                    dr["category_name"] == DBNull.Value ? "Uncategorized" : dr["category_name"].ToString(),
                                    dr["supplier_name"] == DBNull.Value ? "No Supplier" : dr["supplier_name"].ToString(),
                                    dr["unit_price"].ToString(),
                                    dr["stock_quantity"].ToString(),
                                    "Archived"
                                );
                            }
                        }
                    }
                    dgv_archived.ClearSelection();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error searching archives: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            SearchArchivedProducts(txt_search.Text.Trim());
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void archive_Load(object sender, EventArgs e)
        {

        }
    }
}
