namespace Inventory_Management_System_of_Monastery
{
    partial class adjustment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(adjustment));
            this.rb_deduct = new System.Windows.Forms.RadioButton();
            this.rb_add = new System.Windows.Forms.RadioButton();
            this.cmb_product_name = new System.Windows.Forms.ComboBox();
            this.cmb_reason = new System.Windows.Forms.ComboBox();
            this.btn_adjust = new System.Windows.Forms.Button();
            this.cmb_category = new System.Windows.Forms.ComboBox();
            this.lbl_qty = new System.Windows.Forms.Label();
            this.lbl_price = new System.Windows.Forms.Label();
            this.lbl_category = new System.Windows.Forms.Label();
            this.lbl_product_name = new System.Windows.Forms.Label();
            this.txt_adjust_qty = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_close = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // rb_deduct
            // 
            this.rb_deduct.AutoSize = true;
            this.rb_deduct.Font = new System.Drawing.Font("Gontserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_deduct.Location = new System.Drawing.Point(369, 284);
            this.rb_deduct.Name = "rb_deduct";
            this.rb_deduct.Size = new System.Drawing.Size(75, 25);
            this.rb_deduct.TabIndex = 37;
            this.rb_deduct.TabStop = true;
            this.rb_deduct.Text = "Deduct";
            this.rb_deduct.UseVisualStyleBackColor = true;
            // 
            // rb_add
            // 
            this.rb_add.AutoSize = true;
            this.rb_add.Font = new System.Drawing.Font("Gontserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_add.Location = new System.Drawing.Point(297, 284);
            this.rb_add.Name = "rb_add";
            this.rb_add.Size = new System.Drawing.Size(55, 25);
            this.rb_add.TabIndex = 36;
            this.rb_add.TabStop = true;
            this.rb_add.Text = "Add";
            this.rb_add.UseVisualStyleBackColor = true;
            // 
            // cmb_product_name
            // 
            this.cmb_product_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_product_name.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_product_name.FormattingEnabled = true;
            this.cmb_product_name.Location = new System.Drawing.Point(46, 205);
            this.cmb_product_name.Name = "cmb_product_name";
            this.cmb_product_name.Size = new System.Drawing.Size(398, 35);
            this.cmb_product_name.TabIndex = 35;
            // 
            // cmb_reason
            // 
            this.cmb_reason.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_reason.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_reason.FormattingEnabled = true;
            this.cmb_reason.Items.AddRange(new object[] {
            "Damaged",
            "Defective",
            "Lost",
            "Other"});
            this.cmb_reason.Location = new System.Drawing.Point(46, 357);
            this.cmb_reason.Name = "cmb_reason";
            this.cmb_reason.Size = new System.Drawing.Size(398, 35);
            this.cmb_reason.TabIndex = 34;
            // 
            // btn_adjust
            // 
            this.btn_adjust.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_adjust.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(107)))), ((int)(((byte)(65)))));
            this.btn_adjust.FlatAppearance.BorderSize = 0;
            this.btn_adjust.FlatAppearance.MouseDownBackColor = System.Drawing.Color.CadetBlue;
            this.btn_adjust.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CadetBlue;
            this.btn_adjust.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_adjust.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_adjust.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_adjust.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_adjust.Location = new System.Drawing.Point(360, 432);
            this.btn_adjust.Name = "btn_adjust";
            this.btn_adjust.Size = new System.Drawing.Size(84, 34);
            this.btn_adjust.TabIndex = 33;
            this.btn_adjust.Text = "Adjust";
            this.btn_adjust.UseVisualStyleBackColor = false;
            this.btn_adjust.Click += new System.EventHandler(this.btn_adjust_Click);
            // 
            // cmb_category
            // 
            this.cmb_category.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_category.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_category.FormattingEnabled = true;
            this.cmb_category.Location = new System.Drawing.Point(46, 122);
            this.cmb_category.Name = "cmb_category";
            this.cmb_category.Size = new System.Drawing.Size(398, 35);
            this.cmb_category.TabIndex = 32;
            this.cmb_category.SelectedIndexChanged += new System.EventHandler(this.cmb_category_SelectedIndexChanged);
            // 
            // lbl_qty
            // 
            this.lbl_qty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_qty.AutoSize = true;
            this.lbl_qty.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_qty.Location = new System.Drawing.Point(41, 252);
            this.lbl_qty.Name = "lbl_qty";
            this.lbl_qty.Size = new System.Drawing.Size(79, 27);
            this.lbl_qty.TabIndex = 31;
            this.lbl_qty.Text = "Quantity";
            // 
            // lbl_price
            // 
            this.lbl_price.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_price.AutoSize = true;
            this.lbl_price.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_price.Location = new System.Drawing.Point(41, 327);
            this.lbl_price.Name = "lbl_price";
            this.lbl_price.Size = new System.Drawing.Size(70, 27);
            this.lbl_price.TabIndex = 30;
            this.lbl_price.Text = "Reason";
            // 
            // lbl_category
            // 
            this.lbl_category.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_category.AutoSize = true;
            this.lbl_category.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_category.Location = new System.Drawing.Point(41, 92);
            this.lbl_category.Name = "lbl_category";
            this.lbl_category.Size = new System.Drawing.Size(81, 27);
            this.lbl_category.TabIndex = 29;
            this.lbl_category.Text = "Category";
            // 
            // lbl_product_name
            // 
            this.lbl_product_name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_product_name.AutoSize = true;
            this.lbl_product_name.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_product_name.Location = new System.Drawing.Point(41, 175);
            this.lbl_product_name.Name = "lbl_product_name";
            this.lbl_product_name.Size = new System.Drawing.Size(75, 27);
            this.lbl_product_name.TabIndex = 28;
            this.lbl_product_name.Text = "Product";
            // 
            // txt_adjust_qty
            // 
            this.txt_adjust_qty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_adjust_qty.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_adjust_qty.Location = new System.Drawing.Point(46, 282);
            this.txt_adjust_qty.Name = "txt_adjust_qty";
            this.txt_adjust_qty.Size = new System.Drawing.Size(245, 27);
            this.txt_adjust_qty.TabIndex = 27;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(107)))), ((int)(((byte)(65)))));
            this.panel2.Controls.Add(this.btn_close);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(497, 69);
            this.panel2.TabIndex = 38;
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(107)))), ((int)(((byte)(65)))));
            this.btn_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_close.BackgroundImage")));
            this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_close.FlatAppearance.BorderSize = 0;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("Gontserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_close.Location = new System.Drawing.Point(442, 19);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(52, 36);
            this.btn_close.TabIndex = 2;
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Gontserrat", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 35);
            this.label1.TabIndex = 1;
            this.label1.Text = "Adjustment";
            // 
            // adjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(236)))));
            this.ClientSize = new System.Drawing.Size(497, 491);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.rb_deduct);
            this.Controls.Add(this.rb_add);
            this.Controls.Add(this.cmb_product_name);
            this.Controls.Add(this.cmb_reason);
            this.Controls.Add(this.btn_adjust);
            this.Controls.Add(this.cmb_category);
            this.Controls.Add(this.lbl_qty);
            this.Controls.Add(this.lbl_price);
            this.Controls.Add(this.lbl_category);
            this.Controls.Add(this.lbl_product_name);
            this.Controls.Add(this.txt_adjust_qty);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "adjustment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "adjustment";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rb_deduct;
        private System.Windows.Forms.RadioButton rb_add;
        public System.Windows.Forms.ComboBox cmb_product_name;
        public System.Windows.Forms.ComboBox cmb_reason;
        public System.Windows.Forms.Button btn_adjust;
        public System.Windows.Forms.ComboBox cmb_category;
        private System.Windows.Forms.Label lbl_qty;
        private System.Windows.Forms.Label lbl_price;
        private System.Windows.Forms.Label lbl_category;
        private System.Windows.Forms.Label lbl_product_name;
        public System.Windows.Forms.TextBox txt_adjust_qty;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label label1;
    }
}