namespace RapChieuPhim
{
    partial class menuThongKe
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnThongKeNV = new System.Windows.Forms.Button();
            this.btnThongKeKH = new System.Windows.Forms.Button();
            this.btnThongKeVe = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnThongKeNV);
            this.panel1.Controls.Add(this.btnThongKeKH);
            this.panel1.Controls.Add(this.btnThongKeVe);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(152, 589);
            this.panel1.TabIndex = 2;
            // 
            // btnThongKeNV
            // 
            this.btnThongKeNV.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThongKeNV.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThongKeNV.Location = new System.Drawing.Point(-2, 8);
            this.btnThongKeNV.Margin = new System.Windows.Forms.Padding(2);
            this.btnThongKeNV.Name = "btnThongKeNV";
            this.btnThongKeNV.Size = new System.Drawing.Size(152, 52);
            this.btnThongKeNV.TabIndex = 1;
            this.btnThongKeNV.Text = "NHÂN VIÊN";
            this.btnThongKeNV.UseVisualStyleBackColor = true;
            this.btnThongKeNV.Click += new System.EventHandler(this.btnThongKeNV_Click);
            // 
            // btnThongKeKH
            // 
            this.btnThongKeKH.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThongKeKH.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThongKeKH.Location = new System.Drawing.Point(-2, 65);
            this.btnThongKeKH.Margin = new System.Windows.Forms.Padding(2);
            this.btnThongKeKH.Name = "btnThongKeKH";
            this.btnThongKeKH.Size = new System.Drawing.Size(152, 52);
            this.btnThongKeKH.TabIndex = 2;
            this.btnThongKeKH.Text = "KHÁCH HÀNG";
            this.btnThongKeKH.UseVisualStyleBackColor = true;
            this.btnThongKeKH.Click += new System.EventHandler(this.btnThongKeKH_Click);
            // 
            // btnThongKeVe
            // 
            this.btnThongKeVe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnThongKeVe.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnThongKeVe.Location = new System.Drawing.Point(-2, 122);
            this.btnThongKeVe.Margin = new System.Windows.Forms.Padding(2);
            this.btnThongKeVe.Name = "btnThongKeVe";
            this.btnThongKeVe.Size = new System.Drawing.Size(152, 52);
            this.btnThongKeVe.TabIndex = 0;
            this.btnThongKeVe.Text = "VÉ";
            this.btnThongKeVe.UseVisualStyleBackColor = true;
            this.btnThongKeVe.Click += new System.EventHandler(this.btnThongKeVe_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(152, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(979, 589);
            this.panel2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(317, 213);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(462, 88);
            this.label1.TabIndex = 1;
            this.label1.Text = "CHỌN MENU BÊN TRÁI \r\nĐỂ QUẢN LÍ NHÉ";
            // 
            // menuThongKe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 589);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "menuThongKe";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "THỐNG KÊ";
            this.Load += new System.EventHandler(this.menuThongKe_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnThongKeNV;
        private System.Windows.Forms.Button btnThongKeKH;
        private System.Windows.Forms.Button btnThongKeVe;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
    }
}