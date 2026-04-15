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
    public partial class confirmation : Form
    {
        private string ID;
        private products p;
        private archive a;
        private MySqlConnection cn;
        public confirmation()
        {
            InitializeComponent();
        }

        public confirmation(string ID, string product_name, products p, archive a)
        {
            InitializeComponent();
            cn = DatabaseConnection.GetConnection();
            this.ID = ID;
            this.p = p;
            this.a = a;

            lbl_product_name.Text = "Product Name: " + product_name;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txt_stock.Text, out int stockQuantity) || stockQuantity < 0)
            {
                MessageBox.Show("Please enter a valid positive integer for stock.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    string query = "UPDATE product SET is_archived = 0, stock_quantity = @stock WHERE product_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@stock", stockQuantity);
                        cmd.Parameters.AddWithValue("@id", ID);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Product restored successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (this.p != null) this.p.LoadProductData();
                        if (this.a  != null)  this.a.LoadArchivedProducts();
                        this.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error restoring product: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_stock_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txt_stock.Text, out int stockQuantity))
            {
                if (stockQuantity > 0)
                {
                    lbl_status2.Text = "In Stock";
                    lbl_status2.ForeColor = Color.Green;
                }
                else
                {
                    lbl_status2.Text = "Out of Stock";
                    lbl_status2.ForeColor = Color.Red;
                }
            }
            else if (string.IsNullOrWhiteSpace(txt_stock.Text))
            {
                lbl_status2.Text = "Enter Stock Qty";
                lbl_status2.ForeColor = Color.Gray;
            }
            else
            {
                lbl_status2.Text = "Invalid";
                lbl_status2.ForeColor = Color.Orange;
            }
        
    }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
