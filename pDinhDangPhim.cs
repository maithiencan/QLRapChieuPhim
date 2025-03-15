using QLHS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapChieuPhim
{
    public partial class pDinhDangPhim : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maDP = ""; 
        public pDinhDangPhim()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        private void pDinhDangPhim_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }


        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM DINHDANGPHIM");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;

            txtMaDP.DataBindings.Clear();
            txtDangPhim.DataBindings.Clear();
            txtMaDP.DataBindings.Add("Text", biding, @"MADP");
            txtDangPhim.DataBindings.Add("Text", biding, @"DANGPHIM");
        }
        private void BatTat(bool giaTri)
        {
            txtMaDP.Enabled = giaTri; txtDangPhim.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }


        #endregion

        #region Sự kiện
        private void btnThem_Click(object sender, EventArgs e)
        {
            maDP = "";
            txtMaDP.Clear();
            txtDangPhim.Clear();
            txtMaDP.Focus();

            BatTat(true);
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            maDP = txtMaDP.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa dạng phim '" + txtDangPhim.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM DINHDANGPHIM WHERE MADP = @MADP";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MADP", SqlDbType.NVarChar, 10).Value = txtMaDP.Text.ToUpper();
                dataTable.Update(cmd);
                pDinhDangPhim_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaDP.Text.Trim() == "")
                MessageBox.Show("Mã dạng phim không được bỏ trống!");
            else if (txtDangPhim.Text.Trim() == "")
                MessageBox.Show("Mã dạng phim không được bỏ trống!");
            else if (txtMaDP.Text.Trim().Length > 10)
                MessageBox.Show("Mã dạng phim phải nhập đúng 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    if (maDP == "")
                    {
                        string sql = @"INSERT INTO DINHDANGPHIM VALUES(@MADP, @DANGPHIM)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MADP", SqlDbType.NVarChar, 10).Value = txtMaDP.Text.ToUpper();
                        cmd.Parameters.Add("@DANGPHIM", SqlDbType.NVarChar, 20).Value = txtDangPhim.Text;
                        int result = dataTable.Update(cmd);
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm thành công!", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Thêm thất bại. Vui lòng thử lại!", "Thông báo");
                        }
                    }
                    else
                    {
                        string sql = @"UPDATE       DINHDANGPHIM
                                       SET          MADP = @MADPMOI,
                                                    DANGPHIM = @DANGPHIM
                                        WHERE       MADP =@MADPCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MADPCU", SqlDbType.NVarChar, 10).Value = txtMaDP.Text.ToUpper();
                        cmd.Parameters.Add("@MADPMOI", SqlDbType.NVarChar, 10).Value = txtMaDP.Text.ToUpper();
                        cmd.Parameters.Add("@DANGPHIM", SqlDbType.NVarChar, 20).Value = txtDangPhim.Text;
                        int result = dataTable.Update(cmd);
                        if (result > 0)
                        {
                            MessageBox.Show("Lưu thành công!", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Lưu thất bại. Vui lòng thử lại!", "Thông báo");
                        }
                    }
                    pDinhDangPhim_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            pDinhDangPhim_Load(sender, e);
        }

        #endregion
    }
}
