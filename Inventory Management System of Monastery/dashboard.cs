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
    public partial class dashboard : Form
    {
        private Form currentForm;
        public dashboard()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ShowForm(new subdash(this)); // ✅ Dito na para tapos na ang InitializeComponent
        }

        public void ShowForm(Form form)
        {
            if (currentForm != null)
            {
                currentForm.Close();
                panel3.Controls.Remove(currentForm);
                currentForm.Dispose();
            }

            currentForm = form;
            form.TopLevel = false;
            form.Dock = DockStyle.Fill; // ✅ Mapupuno ang buong panel
            panel3.Controls.Add(form);
            form.BringToFront();
            form.Show();
        }
        private void btn_dashboard_Click(object sender, EventArgs e)
        {
            ShowForm(new subdash(this));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ShowForm(new sales(this));
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?", title.name, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 f = new Form1();
                f.Show();
                this.Hide();
            }
        }

        private void btn_purchase_Click(object sender, EventArgs e)
        {
            ShowForm(new purchase(this));

        }

        private void btn_history_Click(object sender, EventArgs e)
        {
            ShowForm(new history(this));

        }

        private void btn_user_Click(object sender, EventArgs e)
        {
            ShowForm(new user(this));

        }

        private void btn_report_Click(object sender, EventArgs e)
        {
            ShowForm(new report(this));
        }

        private void btn_products_Click(object sender, EventArgs e)
        {
            products p = new products(this);
            ShowForm(p);
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
