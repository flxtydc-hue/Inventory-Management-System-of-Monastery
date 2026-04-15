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
    public partial class reset : Form
    {
        private int _userId;

        public reset(int userId)    
        {
            InitializeComponent();
            _userId = userId;

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void btn_confirm_Click(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(txt_new_pin.Text) ||
                    string.IsNullOrWhiteSpace(txt_confirm_pin.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txt_new_pin.Text != txt_confirm_pin.Text)
                {
                    MessageBox.Show("PINs do not match. Please try again.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_confirm_pin.Clear();
                    txt_confirm_pin.Focus();
                    return;
                }

                if (txt_new_pin.Text.Length < 4)
                {
                    MessageBox.Show("PIN must be at least 4 digits.", "Warning",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "UPDATE users SET pin_code = @pin WHERE user_id = @userId";

                using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
                {
                    cn.Open();
                    using (MySqlCommand cmd = new MySqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@pin", txt_new_pin.Text);
                        cmd.Parameters.AddWithValue("@userId", _userId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("PIN reset successfully! Please log in again.", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }

            private void btn_cancel_Click(object sender, EventArgs e)
            {
                this.Close();
            }
    }
    }

