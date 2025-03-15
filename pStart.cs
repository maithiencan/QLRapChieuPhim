using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapChieuPhim
{
    public partial class pStart : Form
    {
        AboutBox AboutBox =  new AboutBox();
        public pStart()
        {
            InitializeComponent();
        }



        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutBox.ShowDialog();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            var kq = MessageBox.Show("Bạn có muốn thoát không?","Thoát",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
