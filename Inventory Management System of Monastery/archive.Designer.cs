namespace Inventory_Management_System_of_Monastery
{
    partial class archive
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(archive));
            this.lbl_archive = new System.Windows.Forms.Label();
            this.dgv_archived = new System.Windows.Forms.DataGridView();
            this.product_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.unit_price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stock_quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.low_stock_threshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplier_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_archived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.restore = new System.Windows.Forms.DataGridViewImageColumn();
            this.btn_search = new System.Windows.Forms.Button();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_close = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_archived)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_archive
            // 
            this.lbl_archive.AutoSize = true;
            this.lbl_archive.Font = new System.Drawing.Font("Gontserrat", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_archive.ForeColor = System.Drawing.Color.Black;
            this.lbl_archive.Location = new System.Drawing.Point(21, 9);
            this.lbl_archive.Name = "lbl_archive";
            this.lbl_archive.Size = new System.Drawing.Size(206, 35);
            this.lbl_archive.TabIndex = 17;
            this.lbl_archive.Text = "Archived Products";
            // 
            // dgv_archived
            // 
            this.dgv_archived.AllowUserToAddRows = false;
            this.dgv_archived.AllowUserToDeleteRows = false;
            this.dgv_archived.AllowUserToResizeRows = false;
            this.dgv_archived.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_archived.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgv_archived.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_archived.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_archived.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_archived.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_archived.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.product_id,
            this.product_name,
            this.category_name,
            this.supplier_name,
            this.unit_price,
            this.stock_quantity,
            this.product_status,
            this.low_stock_threshold,
            this.supplier_id,
            this.is_archived,
            this.restore});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_archived.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_archived.GridColor = System.Drawing.SystemColors.Control;
            this.dgv_archived.Location = new System.Drawing.Point(27, 99);
            this.dgv_archived.MultiSelect = false;
            this.dgv_archived.Name = "dgv_archived";
            this.dgv_archived.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.ButtonShadow;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_archived.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_archived.RowHeadersVisible = false;
            this.dgv_archived.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_archived.Size = new System.Drawing.Size(909, 417);
            this.dgv_archived.TabIndex = 18;
            this.dgv_archived.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_archive_CellContentClick);
            // 
            // product_id
            // 
            this.product_id.DataPropertyName = "product_id";
            this.product_id.HeaderText = "Column1";
            this.product_id.Name = "product_id";
            this.product_id.ReadOnly = true;
            this.product_id.Visible = false;
            // 
            // product_name
            // 
            this.product_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.product_name.DataPropertyName = "product_name";
            this.product_name.HeaderText = "Name";
            this.product_name.Name = "product_name";
            this.product_name.ReadOnly = true;
            // 
            // category_name
            // 
            this.category_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.category_name.DataPropertyName = "category_name";
            this.category_name.HeaderText = "Category";
            this.category_name.Name = "category_name";
            this.category_name.ReadOnly = true;
            this.category_name.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // supplier_name
            // 
            this.supplier_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.supplier_name.DataPropertyName = "supplier_name";
            this.supplier_name.HeaderText = "Supplier";
            this.supplier_name.Name = "supplier_name";
            this.supplier_name.ReadOnly = true;
            // 
            // unit_price
            // 
            this.unit_price.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.unit_price.DataPropertyName = "unit_price";
            this.unit_price.HeaderText = "Price";
            this.unit_price.Name = "unit_price";
            this.unit_price.ReadOnly = true;
            // 
            // stock_quantity
            // 
            this.stock_quantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.stock_quantity.DataPropertyName = "stock_quantity";
            this.stock_quantity.HeaderText = "Stock";
            this.stock_quantity.Name = "stock_quantity";
            this.stock_quantity.ReadOnly = true;
            // 
            // product_status
            // 
            this.product_status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.product_status.DataPropertyName = "product_status";
            this.product_status.HeaderText = "Status";
            this.product_status.Name = "product_status";
            this.product_status.ReadOnly = true;
            this.product_status.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.product_status.Width = 85;
            // 
            // low_stock_threshold
            // 
            this.low_stock_threshold.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.low_stock_threshold.DataPropertyName = "low_stock_threshold";
            this.low_stock_threshold.HeaderText = "Threshold";
            this.low_stock_threshold.Name = "low_stock_threshold";
            this.low_stock_threshold.ReadOnly = true;
            this.low_stock_threshold.Visible = false;
            // 
            // supplier_id
            // 
            this.supplier_id.DataPropertyName = "supplier_id";
            this.supplier_id.HeaderText = "Supplier ID";
            this.supplier_id.Name = "supplier_id";
            this.supplier_id.ReadOnly = true;
            this.supplier_id.Visible = false;
            // 
            // is_archived
            // 
            this.is_archived.DataPropertyName = "is_archived";
            this.is_archived.HeaderText = "Archive";
            this.is_archived.Name = "is_archived";
            this.is_archived.ReadOnly = true;
            this.is_archived.Visible = false;
            // 
            // restore
            // 
            this.restore.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.restore.DataPropertyName = "edit";
            this.restore.HeaderText = "";
            this.restore.Image = ((System.Drawing.Image)(resources.GetObject("restore.Image")));
            this.restore.Name = "restore";
            this.restore.ReadOnly = true;
            this.restore.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.restore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.restore.Width = 19;
            // 
            // btn_search
            // 
            this.btn_search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_search.BackColor = System.Drawing.SystemColors.Window;
            this.btn_search.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_search.BackgroundImage")));
            this.btn_search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_search.FlatAppearance.BorderSize = 0;
            this.btn_search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_search.Location = new System.Drawing.Point(685, 58);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(32, 23);
            this.btn_search.TabIndex = 24;
            this.btn_search.UseVisualStyleBackColor = false;
            // 
            // txt_search
            // 
            this.txt_search.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_search.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_search.Location = new System.Drawing.Point(684, 56);
            this.txt_search.Name = "txt_search";
            this.txt_search.Size = new System.Drawing.Size(252, 27);
            this.txt_search.TabIndex = 25;
            this.txt_search.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txt_search.TextChanged += new System.EventHandler(this.txt_search_TextChanged);
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_close.BackgroundImage")));
            this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Location = new System.Drawing.Point(884, 8);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(52, 36);
            this.btn_close.TabIndex = 26;
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // archive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(236)))));
            this.ClientSize = new System.Drawing.Size(962, 528);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.txt_search);
            this.Controls.Add(this.dgv_archived);
            this.Controls.Add(this.lbl_archive);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "archive";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "archive";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.archive_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_archived)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_archive;
        private System.Windows.Forms.DataGridView dgv_archived;
        private System.Windows.Forms.Button btn_search;
        public System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn category_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn unit_price;
        private System.Windows.Forms.DataGridViewTextBoxColumn stock_quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_status;
        private System.Windows.Forms.DataGridViewTextBoxColumn low_stock_threshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplier_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_archived;
        private System.Windows.Forms.DataGridViewImageColumn restore;
    }
}