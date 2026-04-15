using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.BC;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void NumberButton_Click (object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (txt_pin.Text.Length < 4)
            {
                txt_pin.Text += btn.Text;
            }
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            string enteredPin = txt_pin.Text;

            if(string.IsNullOrEmpty(enteredPin))
            {
                MessageBox.Show("Please enter a PIN.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(DatabaseConnection.connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM users WHERE pin_code = @pin";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@pin", enteredPin);

                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string username = reader["username"].ToString();
                    string role  = reader["role"].ToString();

                    GlobalUser.ID = Convert.ToInt32(reader["user_id"]);
                    GlobalUser.Name = reader["username"].ToString();

                    MessageBox.Show("Welcome, " + username + "!", title.name, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Pagkatapos ng successful login at nakuha mo yung ID galing sa database:

                    dashboard d = new dashboard();
                    d.Show();
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Invalid PIN. Please try again.", title.name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_pin.Clear();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txt_pin.PasswordChar = checkbox.Checked ? '\0' : '*';
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            txt_pin.Clear();
            txt_pin.Focus();
        }

        private void txt_pin_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lbl_forgot_Click(object sender, EventArgs e)
        {
            this.Hide(); // ✅ Itago muna ang login
            forgot_pin fp = new forgot_pin();
            fp.ShowDialog();
            this.Show();

        }
    }
}
