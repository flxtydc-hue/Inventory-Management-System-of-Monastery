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
    public partial class supplier : Form
    {
        private purchase purchase;
        public supplier(purchase purchase)
        {
            InitializeComponent();
            this.purchase = purchase;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_supplier_name.Text) || string.IsNullOrEmpty(txt_contact_person.Text) || string.IsNullOrEmpty(txt_contact_number.Text) || string.IsNullOrEmpty(txt_address.Text))
            {
                MessageBox.Show("Please fill in all fields.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "INSERT INTO supplier (supplier_name, contact_person, contact_number, address) VALUES (@name, @contact_person, @contact_number, @address)";
                using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@name", txt_supplier_name.Text);
                        cmd.Parameters.AddWithValue("@contact_person", txt_contact_person.Text);
                        cmd.Parameters.AddWithValue("@contact_number", txt_contact_number.Text);
                        cmd.Parameters.AddWithValue("@address", txt_address.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Supplier added successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        LoadManageSupplier();
                        this.btn_save.Enabled = true;
                        this.btn_update.Enabled = false;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Error saving supplier: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_supplier_name.Text) || string.IsNullOrEmpty(txt_contact_person.Text) || string.IsNullOrEmpty(txt_contact_number.Text) || string.IsNullOrEmpty(txt_address.Text))
            {
                MessageBox.Show("Please fill in all fields.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string query = "UPDATE supplier SET supplier_name = @name, contact_person = @contact_person, contact_number = @contact_number, address = @address WHERE supplier_id = @id";
                using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
                {
                    cn.Open();
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@name", txt_supplier_name.Text);
                        cmd.Parameters.AddWithValue("@contact_person", txt_contact_person.Text);
                        cmd.Parameters.AddWithValue("@contact_number", txt_contact_number.Text);
                        cmd.Parameters.AddWithValue("@address", txt_address.Text);
                        cmd.Parameters.AddWithValue("@id", lbl_supplier_id.Text);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Supplier updated successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearFields();
                        LoadManageSupplier();
                        this.btn_save.Enabled = false;
                        this.btn_update.Enabled = true;
                    }
                }
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show("Error saving supplier: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadManageSupplier()
        {
            dgv_supplier.Rows.Clear();
            using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
            {
                try
                {
                    cn.Open();
                    string query = @" SELECT
                supplier_id, supplier_name, contact_person, contact_number, address
                FROM supplier WHERE is_active = 1
                ORDER BY supplier_id;
            ";
                    using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                    using (MySql.Data.MySqlClient.MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            dgv_supplier.Rows.Add(
                                dr["supplier_id"].ToString(),
                                dr["supplier_name"].ToString(),
                                dr["contact_person"].ToString(),
                                dr["contact_number"].ToString(),
                                dr["address"].ToString()
                            );
                        }
                    }
                }
                catch (MySql.Data.MySqlClient.MySqlException ex)
                {
                    MessageBox.Show("Error loading suppliers: " + ex.Message, title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dgv_supplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;


                string colName = dgv_supplier.Columns[e.ColumnIndex].Name;

                if (colName == "edit")
                {
                    string supplier_id = dgv_supplier.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    string supplier_name = dgv_supplier.Rows[e.RowIndex].Cells["supplier_name"].Value.ToString();
                    string contact_person = dgv_supplier.Rows[e.RowIndex].Cells["contact_person"].Value.ToString();
                    string contact_number = dgv_supplier.Rows[e.RowIndex].Cells["contact_number"].Value.ToString();
                    string address = dgv_supplier.Rows[e.RowIndex].Cells["address"].Value.ToString();


                    lbl_supplier_id.Text = supplier_id;
                    txt_supplier_name.Text = supplier_name;
                    txt_contact_person.Text = contact_person;
                    txt_contact_number.Text = contact_number;
                    txt_address.Text = address;
                    this.btn_save.Enabled = false;
                    this.btn_update.Enabled = true;
                }
                else if (colName == "delete")
                {
                    string supplier_id = dgv_supplier.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    string supplier_name = dgv_supplier.Rows[e.RowIndex].Cells["supplier_name"].Value.ToString();
                    string contact_person = dgv_supplier.Rows[e.RowIndex].Cells["contact_person"].Value.ToString();
                    string contact_number = dgv_supplier.Rows[e.RowIndex].Cells["contact_number"].Value.ToString();
                    string address = dgv_supplier.Rows[e.RowIndex].Cells["address"].Value.ToString();


                    if (MessageBox.Show("Are you sure you want to delete the supplier: " + supplier_name + "?", title.name, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        string query = "UPDATE supplier SET is_active = 0 WHERE supplier_id = @id";
                        using (MySql.Data.MySqlClient.MySqlConnection cn = DatabaseConnection.GetConnection())
                        {
                            cn.Open();
                            using (MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand(query, cn))
                            {
                                cmd.Parameters.AddWithValue("@id", supplier_id);
                                cmd.ExecuteNonQuery();
                                MessageBox.Show("Supplier deleted successfully!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadManageSupplier();
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

        public void ClearFields()
        {
            txt_supplier_name.Clear();
            txt_contact_person.Clear();
            txt_contact_number.Clear();
            txt_address.Clear();
        }

        private void supplier_Load(object sender, EventArgs e)
        {
            LoadManageSupplier();
            btn_save.Enabled = true;
            btn_update.Enabled = false;
            ClearFields();
        }
    }
}
