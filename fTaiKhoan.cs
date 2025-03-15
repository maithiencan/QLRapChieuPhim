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
using BC = BCrypt.Net.BCrypt;

namespace RapChieuPhim
{
    public partial class fTaiKhoan : Form
    {
        MyDataTable dataTable = new MyDataTable();
        MyDataTable dataNhanVien = new MyDataTable();
        string taiKhoan ="";
        public fTaiKhoan()
        {
            InitializeComponent();
            dataTable.OpenConnection();
            dataNhanVien.OpenConnection();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmdNV = new SqlCommand(@"SELECT MANV, HOVATENNV FROM NHANVIEN");
            dataNhanVien.Fill(cmdNV);
            BindingSource bindingNV = new BindingSource();
            bindingNV.DataSource = dataNhanVien;
            cboNhanVien.DataSource = bindingNV;
            cboNhanVien.DisplayMember = "HOVATENNV";
            cboNhanVien.ValueMember = "MANV";

            SqlCommand cmd = new SqlCommand(@"SELECT T.*, N.HOVATENNV, C.TENCHUCVU 
                                              FROM TAIKHOAN T, NHANVIEN N, CHUCVU C
                                              WHERE T.MANV = N.MANV AND N.MACV = C.MACV");
            dataTable.Fill(cmd);
            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;
            dataGridView.DataSource = biding;

            txtTaiKhoan.DataBindings.Clear();
            txtMatKhau.DataBindings.Clear();
            cboNhanVien.DataBindings.Clear();
            cboQuyen.DataBindings.Clear();
            txtTaiKhoan.DataBindings.Add("Text", biding, @"TENTAIKHOAN");
            txtMatKhau.DataBindings.Add("Text", biding, @"MATKHAU");
            cboNhanVien.DataBindings.Add("SelectedValue", biding, @"MANV");
            cboQuyen.DataBindings.Add("Text", biding, @"QUYENHAN");
        }

        private void BatTat(bool giaTri)
        {
            txtMatKhau.Enabled = giaTri; txtTaiKhoan.Enabled = giaTri;
            cboNhanVien.Enabled = giaTri; cboQuyen.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuy.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }

        #endregion

        #region Sự kiện
        private void fTaiKhoan_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }

        private void chkHienMK_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHienMK.Checked)
            {
                txtMatKhau.UseSystemPasswordChar = false;
            }
            else
            {
                txtMatKhau.UseSystemPasswordChar = true;
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            taiKhoan = ""; 
            txtTaiKhoan.Clear();
            txtMatKhau.Clear();
            txtTaiKhoan.Focus();
            cboNhanVien.Text = "";
            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            taiKhoan = txtTaiKhoan.Text;
            txtMatKhau.Clear();
            BatTat(true);
            txtTaiKhoan.Enabled = false;
            cboNhanVien.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa tài khoản '" + txtTaiKhoan.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM TAIKHOAN WHERE TENTAIKHOAN = @TENTAIKHOAN";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@TENTAIKHOAN", SqlDbType.NVarChar, 50).Value = txtTaiKhoan.Text;
                dataTable.Update(cmd);
                fTaiKhoan_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtTaiKhoan.Text.Trim() == "")
                MessageBox.Show("Tài khoản không được bỏ trống!");
            else if (txtMatKhau.Text.Trim() == "")
                MessageBox.Show("Mật khẩu không được bỏ trống!");
            else if (cboNhanVien.Text == "")
                MessageBox.Show("Nhân viên không được bỏ trống!");
            else if (cboQuyen.Text == "")
                MessageBox.Show("Quyền không được bỏ trống!");
            else
            {
                try
                {
                    if (taiKhoan == "")
                    {
                        string sql = @"INSERT INTO TAIKHOAN VALUES(@MANV, @TENTAIKHOAN, @MATKHAU, @QUYENHAN )";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 10).Value = cboNhanVien.SelectedValue.ToString();
                        cmd.Parameters.Add("@TENTAIKHOAN", SqlDbType.NVarChar, 50).Value = txtTaiKhoan.Text;
                        cmd.Parameters.Add("@MATKHAU", SqlDbType.NVarChar, 50).Value = txtMatKhau.Text/*BC.HashPassword(txtMatKhau.Text)*/;
                        cmd.Parameters.Add("@QUYENHAN", SqlDbType.NVarChar, 30).Value = cboQuyen.Text;
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
                        string sql = @"UPDATE       TAIKHOAN
                                       SET          MANV = @MANV,
                                                    TENTAIKHOAN = @TENTAIKHOANMOI,
                                                    MATKHAU = @MATKHAU, QUYENHAN = @QUYENHAN                                                 
                                        WHERE       TENTAIKHOAN = @TENTAIKHOANCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 10).Value = cboNhanVien.SelectedValue.ToString();
                        cmd.Parameters.Add("@TENTAIKHOANMOI", SqlDbType.NVarChar, 50).Value = txtTaiKhoan.Text;
                        cmd.Parameters.Add("@TENTAIKHOANCU", SqlDbType.NVarChar, 50).Value = txtTaiKhoan.Text;
                        cmd.Parameters.Add("@MATKHAU", SqlDbType.NVarChar, 50).Value = txtMatKhau.Text; /*BC.HashPassword(txtMatKhau.Text)*/;
                        cmd.Parameters.Add("@QUYENHAN", SqlDbType.NVarChar, 30).Value = cboQuyen.Text;
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

                    fTaiKhoan_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fTaiKhoan_Load(sender, e);
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "MATKHAU")
            {
                e.Value = "••••••••••";
            }
        }

        #endregion
    }
}

