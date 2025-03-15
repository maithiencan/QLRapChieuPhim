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
    public partial class pTheLoaiPhim : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maLP = "";
        public pTheLoaiPhim()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

       
        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM THELOAIPHIM");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;

            txtMaLP.DataBindings.Clear();
            txtLoaiPhim.DataBindings.Clear();
            txtMaLP.DataBindings.Add("Text", biding, @"MALP");
            txtLoaiPhim.DataBindings.Add("Text", biding, @"LOAIPHIM");
        }

        private void BatTat(bool giaTri)
        {
            txtMaLP.Enabled = giaTri; txtLoaiPhim.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }
        #endregion


        #region Sự kiện
        private void pTheLoaiPhim_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maLP = "";
            txtMaLP.Clear();
            txtLoaiPhim.Clear();
            txtMaLP.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maLP = txtMaLP.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa thể loại '" + txtLoaiPhim.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM THELOAIPHIM WHERE MALP = @MALP";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MALP", SqlDbType.NVarChar, 10).Value = txtMaLP.Text.ToUpper();
                dataTable.Update(cmd);
                pTheLoaiPhim_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaLP.Text.Trim() == "")
                MessageBox.Show("Mã thể loại không được bỏ trống!");
            else if (txtLoaiPhim.Text.Trim() == "")
                MessageBox.Show("Dạng phim không được bỏ trống!");
            else if (txtMaLP.Text.Trim().Length >= 10)
                MessageBox.Show("Mã loại phim phải nhập đúng 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    if (maLP == "")
                    {
                        string sql = @"INSERT INTO THELOAIPHIM VALUES(@MALP, @DANGPHIM)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MALP", SqlDbType.NVarChar, 10).Value = txtMaLP.Text.ToUpper();
                        cmd.Parameters.Add("@DANGPHIM", SqlDbType.NVarChar, 50).Value = txtLoaiPhim.Text;
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
                        string sql = @"UPDATE       THELOAIPHIM
                                       SET          MALP = @MALPMOI,
                                                    DANGPHIM = @DANGPHIM
                                        WHERE       MALP =@MALPCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MALPCU", SqlDbType.NVarChar, 10).Value = txtMaLP.Text.ToUpper();
                        cmd.Parameters.Add("@MALPMOI", SqlDbType.NVarChar, 10).Value = txtMaLP.Text.ToUpper();
                        cmd.Parameters.Add("@DANGPHIM", SqlDbType.NVarChar, 50).Value = txtLoaiPhim.Text;
                        int result = dataTable.Update(cmd);
                        if (result > 0)
                        {
                            MessageBox.Show("L thành công!", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Lưu thất bại. Vui lòng thử lại!", "Thông báo");
                        }
                    }
                    pTheLoaiPhim_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            pTheLoaiPhim_Load(sender, e);
        }
        #endregion
    }
}
