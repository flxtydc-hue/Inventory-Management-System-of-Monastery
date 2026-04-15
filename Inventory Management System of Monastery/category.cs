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
    public partial class category : Form
    {

        public category(products products)
        {
            InitializeComponent();
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_category_name.Clear();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_category_name.Text))
            {
                MessageBox.Show("Please enter a category name.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "INSERT INTO category (category_name) VALUES (@name)";
                using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@name", txt_category_name.Text);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Category added successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_category_name.Clear();
                        LoadManageCategory();
                        this.btn_save.Enabled = true;
                        this.btn_update.Enabled = false;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Error saving category: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

        public void LoadManageCategory()
        {
            dgv_category.Rows.Clear();
            using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
            {
                try
                {
                    cn.Open();
                    string query = @" SELECT
                category_id, category_name
                FROM category WHERE is_active = 1
                ORDER BY category_id;
            ";
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                    using (MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dgv_category.Rows.Add(
                                dr["category_id"].ToString(),
                                dr["category_name"].ToString()
                            );
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error loading categories: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void category_Load(object sender, EventArgs e)
        {
            LoadManageCategory();
            btn_save.Enabled = true;
            btn_update.Enabled = false;
            txt_category_name.Clear();
        }

        private void dgv_category_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;


                string colName = dgv_category.Columns[e.ColumnIndex].Name;

                if (colName == "edit")
                {
                    string category_id = dgv_category.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    string category_name = dgv_category.Rows[e.RowIndex].Cells["category_name"].Value.ToString();
                    lbl_category_id.Text = category_id;
                    txt_category_name.Text = category_name;
                    this.btn_save.Enabled = false;
                    this.btn_update.Enabled = true;
                }
                else if (colName == "delete")
                {
                    string category_id = dgv_category.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    string category_name = dgv_category.Rows[e.RowIndex].Cells["category_name"].Value.ToString();
                    if (MessageBox.Show("Are you sure you want to delete the category: " + category_name + "?", title.name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "UPDATE category SET is_active = 0 WHERE category_id = @id";
                        using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
                        {
                            cn.Open();
                            using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                            {
                                cmd.Parameters.AddWithValue("@id", category_id);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Category deleted successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadManageCategory();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing request: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txt_category_name.Text))
            {
                MessageBox.Show("Please enter a category name.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (MessageBox.Show("Are you sure you want to update this category?", title.name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string query = "UPDATE category SET category_name = @name WHERE category_id = @id";
                    using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
                    {
                        cn.Open();
                        using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                        {
                            cmd.Parameters.AddWithValue("@name", txt_category_name.Text);
                            cmd.Parameters.AddWithValue("@id", lbl_category_id.Text);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Category updated successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txt_category_name.Clear();
                            LoadManageCategory();
                            this.btn_update.Enabled = true;
                            this.btn_save.Enabled = false;
                        }
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Error updating category: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
