using QLHS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapChieuPhim
{
    public partial class fLoaiVe : Form
    {

        MyDataTable dataTable = new MyDataTable();
        string maLV = "";
        public fLoaiVe()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM LOAIVE");
            dataTable.Fill(cmd);
            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;
            dataGridView.DataSource = biding;

            txtMaLV.DataBindings.Clear();
            txtTenLV.DataBindings.Clear();
            numDonGia.DataBindings.Clear();
            txtMaLV.DataBindings.Add("Text", biding, @"MALV");
            txtTenLV.DataBindings.Add("Text", biding, @"TENLV");
            numDonGia.DataBindings.Add("Value", biding, @"DONGIA");
        }

        private void BatTat(bool giaTri)
        {
            txtMaLV.Enabled = giaTri; txtTenLV.Enabled = giaTri; numDonGia.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }
        #endregion


        #region Sự kiện

        private void btnThem_Click(object sender, EventArgs e)
        {
            maLV = "";
            txtMaLV.Clear();
            txtTenLV.Clear();
            numDonGia.Value = 10000;
            txtMaLV.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maLV = txtMaLV.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa loại vé'" + txtTenLV.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM LOAIVE WHERE MALV = @MALV";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MALV", SqlDbType.NVarChar, 10).Value = txtMaLV.Text.ToUpper();
                dataTable.Update(cmd);
                fLoaiVe_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaLV.Text.Trim() == "")
                MessageBox.Show("Mã loại vé không được bỏ trống!");
            else if (txtTenLV.Text.Trim() == "")
                MessageBox.Show("Tên loại vé không được bỏ trống!");
            else if (txtMaLV.Text.Trim().Length > 10)
                MessageBox.Show("Mã loại vé phải nhập tối thiểu 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    if (maLV == "")
                    {
                        string sql = @"INSERT INTO LOAIVE VALUES(@MALV, @TENLV, @DONGIA)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MALV", SqlDbType.NVarChar, 10).Value = txtMaLV.Text.ToUpper();
                        cmd.Parameters.Add("@TENLV", SqlDbType.NVarChar, 50).Value = txtTenLV.Text;
                        cmd.Parameters.Add("@DONGIA", SqlDbType.Int).Value = numDonGia.Value;
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
                        string sql = @"UPDATE       LOAIVE
                                       SET          MALV = @MALVMOI,
                                                    TENLV = @TENLV,
                                                    DONGIA = @DONGIA
                                        WHERE       MALV =@MALVCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MALVCU", SqlDbType.NVarChar, 10).Value = txtMaLV.Text.ToUpper();
                        cmd.Parameters.Add("@MALVMOI", SqlDbType.NVarChar, 10).Value = txtMaLV.Text.ToUpper();
                        cmd.Parameters.Add("@TENLV", SqlDbType.NVarChar, 50).Value = txtTenLV.Text;
                        cmd.Parameters.Add("@DONGIA", SqlDbType.Int).Value = numDonGia.Value;
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
                    fLoaiVe_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fLoaiVe_Load(sender, e);
        }

        private void fLoaiVe_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }

        #endregion


    }
}
