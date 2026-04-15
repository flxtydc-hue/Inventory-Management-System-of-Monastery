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
    public partial class user : Form
    {
        private dashboard dashboard;

        public user(dashboard dashboard)
        {
            InitializeComponent();
            this.dashboard = dashboard;
        }

        private void dgv_users_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
