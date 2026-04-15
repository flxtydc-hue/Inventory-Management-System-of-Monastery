using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Inventory_Management_System_of_Monastery
{
    public partial class security: Form
    {
        private int _userId;
        private int _questionId;

        public security(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void security_question_Load(object sender, EventArgs e)
        {

        }

        private void LoadSecurityQuestion()
        {
            string query = @"SELECT sq.question_id, sq.question_text 
                         FROM security_questions sq
                         INNER JOIN user_security_answers usa ON sq.question_id = usa.question_id
                         WHERE usa.user_id = @userId
                         LIMIT 1";

            using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
            {
                cn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@userId", _userId);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            _questionId = Convert.ToInt32(dr["question_id"]);

                            // ✅ Iisa lang ang lalabas — yung pinili nila
                            cmb_questions.Items.Clear();
                            cmb_questions.Items.Add(dr["question_text"].ToString());
                            cmb_questions.SelectedIndex = 0;
                            cmb_questions.Enabled = false; // Read-only — hindi na kailangan pamiliin
                        }
                        else
                        {
                            MessageBox.Show("No security question set for this account.\nPlease contact your Admin.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                }
            }
        }

        private void btn_verify_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_answer.Text))
            {
                MessageBox.Show("Please enter your answer.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = @"SELECT COUNT(*) FROM user_security_answers 
                         WHERE user_id = @userId 
                         AND question_id = @questionId 
                         AND answer_hash = @answer";

            using (MySqlConnection cn = new MySqlConnection(DatabaseConnection.connectionString))
            {
                cn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@userId", _userId);
                    cmd.Parameters.AddWithValue("@questionId", _questionId);
                    cmd.Parameters.AddWithValue("@answer", txt_answer.Text.Trim().ToLower());

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        reset rp = new reset(_userId);
                        rp.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect answer. Please try again.", "Access Denied",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txt_answer.Clear();
                        txt_answer.Focus();
                    }
                }
            }
        }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void security_Load(object sender, EventArgs e)
        {

        }

        private void security_Shown(object sender, EventArgs e)
        {
            LoadSecurityQuestion();

        }


    }
}