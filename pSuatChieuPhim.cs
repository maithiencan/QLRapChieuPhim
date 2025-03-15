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
    public partial class pSuatChieuPhim : Form
    {
        #region Biến toàn cục
        MyDataTable dataPhong = new MyDataTable();
        MyDataTable dataPhim = new MyDataTable();
        MyDataTable dataTTV = new MyDataTable();
        string maSuat = "";
        #endregion
        public pSuatChieuPhim()
        {
            InitializeComponent();
            dataPhong.OpenConnection();
            dataPhim.OpenConnection();
            dataTTV.OpenConnection();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Functions
        private void LayDuLieu()
        {
            

            SqlCommand cmdPhongChieu = new SqlCommand(@"SELECT * FROM PHONGCHIEU");
            dataPhong.Fill(cmdPhongChieu);
            BindingSource bindingPhongChieu = new BindingSource();
            bindingPhongChieu.DataSource = dataPhong;
            cboPhong.DataSource = bindingPhongChieu;
            cboPhong.DisplayMember = @"TENPHONG";
            cboPhong.ValueMember = @"MAPHONG";

            SqlCommand cmdPhim = new SqlCommand(@"SELECT * FROM PHIM");
            dataPhim.Fill(cmdPhim);
            BindingSource bindingPhim = new BindingSource();
            bindingPhim.DataSource = dataPhim;
            cboPhim.DataSource = bindingPhim;
            cboPhim.DisplayMember = @"TENPHIM";
            cboPhim.ValueMember = @"MAPHIM";

            SqlCommand cmdSC = new SqlCommand(@"SELECT T.*, P.TENPHONG, PH.TENPHIM
                                                 FROM SUATCHIEU T, PHONGCHIEU P, PHIM PH
                                                 WHERE T.MAPHIM = PH.MAPHIM AND T.MAPHONG = P.MAPHONG");
            dataTTV.Fill(cmdSC);
            BindingSource bindingSource = new BindingSource();
            bindingSource.DataSource = dataTTV;
            dataGridView.DataSource = bindingSource;

            txtMaSuat.DataBindings.Clear();
            cboPhim.DataBindings.Clear();
            cboPhong.DataBindings.Clear();
            dtpKhoiChieu.DataBindings.Clear();
            txtMaSuat.DataBindings.Add("Text", bindingSource, @"MASUAT");
            cboPhim.DataBindings.Add("SelectedValue", bindingSource, @"MAPHIM");
            cboPhong.DataBindings.Add("SelectedValue", bindingSource, @"MAPHONG");
            dtpKhoiChieu.DataBindings.Add("Value", bindingSource, @"KHOICHIEU");
        }

        private void BatTat(bool giaTri)
        {
            txtMaSuat.Enabled = giaTri;
            cboPhim.Enabled = giaTri;
            cboPhong.Enabled = giaTri;
            dtpKhoiChieu.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }
        #endregion

        #region Sự kiện
        private void pChonPhim_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maSuat = "";
            txtMaSuat.Text ="";
            cboPhim.Text = "";
            cboPhong.Text = "";
            dtpKhoiChieu.Value = DateTime.Now;
            txtMaSuat.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maSuat = txtMaSuat.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa suất '" + txtMaSuat.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM SUATCHIEU WHERE MASUAT = @MASUAT";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MASUAT", SqlDbType.NVarChar, 10).Value = txtMaSuat.Text.ToUpper();
                dataTTV.Update(cmd);
                pChonPhim_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaSuat.Text.Trim() == "")
                MessageBox.Show("Mã suất không được bỏ trống!");
            else if (cboPhong.Text.Trim() == "")
                MessageBox.Show("Phòng không được bỏ trống!");
            else if (cboPhim.Text.Trim() == "")
                MessageBox.Show("Phim không được bỏ trống!");
            else if (txtMaSuat.Text.Trim().Length > 10)
                MessageBox.Show("Mã suất phải nhập đúng 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!txtMaSuat.Text.StartsWith("S") && txtMaSuat.Text.Length >= 1)
            {
                MessageBox.Show("Mã nhân viên phải bắt đầu bằng 'S'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSuat.Text = "S"; // Reset lại thành "NV"
                txtMaSuat.SelectionStart = txtMaSuat.Text.Length;
            }

            else
            {
                try
                {
                    if (maSuat == "")
                    {
                        string sql = @"INSERT INTO SUATCHIEU VALUES(@MASUAT, @MAPHIM, @MAPHONG, @KHOICHIEU)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MASUAT", SqlDbType.NVarChar, 10).Value = txtMaSuat.Text;
                        cmd.Parameters.Add("@MAPHONG", SqlDbType.NVarChar, 10).Value = cboPhong.SelectedValue.ToString();
                        cmd.Parameters.Add("@MAPHIM", SqlDbType.NVarChar, 10).Value = cboPhim.SelectedValue.ToString();
                        cmd.Parameters.Add("@KHOICHIEU", SqlDbType.DateTime, 100).Value = dtpKhoiChieu.Text;
                        int result = dataTTV.Update(cmd);
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
                        string sql = @"UPDATE       SUATCHIEU
                                       SET          MASUAT =@MASUATMOI,
                                                    MAPHIM = @MAPHIM,
                                                    MAPHONG = @MAPHONG,
                                                    KHOICHIEU =@KHOICHIEU
                                        WHERE       MASUAT =@MASUATCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MASUATMOI", SqlDbType.NVarChar, 10).Value = txtMaSuat.Text;
                        cmd.Parameters.Add("@MASUATCU", SqlDbType.NVarChar, 10).Value = txtMaSuat.Text;
                        cmd.Parameters.Add("@MAPHONG", SqlDbType.NVarChar, 10).Value = cboPhong.SelectedValue.ToString();
                        cmd.Parameters.Add("@MAPHIM", SqlDbType.NVarChar, 10).Value = cboPhim.SelectedValue.ToString();
                        cmd.Parameters.Add("@KHOICHIEU", SqlDbType.DateTime, 100).Value = dtpKhoiChieu.Text;
                        int result = dataTTV.Update(cmd);
                        if (result > 0)
                        {
                            MessageBox.Show("Lưu thành công!", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Lưu thất bại. Vui lòng thử lại!", "Thông báo");
                        }
                    }
                    pChonPhim_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            pChonPhim_Load(sender, e);
        }
        #endregion
    }
}

