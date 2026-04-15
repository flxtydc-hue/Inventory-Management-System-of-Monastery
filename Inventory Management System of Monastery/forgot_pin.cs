using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Inventory_Management_System_of_Monastery
{
    public partial class forgot_pin : Form
    {
        private int _userId;
        private int _questionId;

        public forgot_pin()
        {
            InitializeComponent();
        }

        private void forgot_pin_Load(object sender, EventArgs e)
        {
        }

        // ─── STEP 0: Validate Username ───────────────────────────
        private void btn_next_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_username.Text))
            {
                MessageBox.Show("Please enter your username.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "SELECT user_id FROM users WHERE username = @username AND is_active = 1";

            using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
            {
                cn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@username", txt_username.Text.Trim());
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        int userId = Convert.ToInt32(result);

                        // ✅ Buksan ang Form 2 — security question
                        security sq = new security(userId);
                        sq.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Username not found or account is inactive.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_username.Clear();
                        txt_username.Focus();
                    }
                }
            }
        }


        // ─── LOAD SECURITY QUESTION ──────────────────────────────


        // ─── CANCEL BUTTONS ──────────────────────────────────────
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_cancel0_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_confirm_pin_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
