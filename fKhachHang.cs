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
    public partial class fKhachHang : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maKH = "";
        public fKhachHang()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }

        #region Functions
        private void BatTat(bool giaTri)
        {
            txtMaKH.Enabled = giaTri;
            txtDiaChi.Enabled = giaTri;
            txtSDT.Enabled = giaTri;
            txtTenKH.Enabled = giaTri;
            dtpNgaySinh.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }

        private void LayDuLieu()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM KHACHHANG");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;

            txtMaKH.DataBindings.Clear();
            txtTenKH.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();
            txtMaKH.DataBindings.Add("Text", biding, @"MAKH");
            txtTenKH.DataBindings.Add("Text", biding, @"HOVATENKH");
            txtSDT.DataBindings.Add("Text", biding, @"SDT");
            txtDiaChi.DataBindings.Add("Text", biding, @"DIACHI");
            dtpNgaySinh.DataBindings.Add("Value", biding, @"NGAYSINH");
 
        }

        private string TaoMaKH()
        {
            MyDataTable dataTable = new MyDataTable();
            string maxMaVe = null;
            dataTable.OpenConnection();
            string sql = "SELECT MAX(CAST(SUBSTRING(MAKH, 3, LEN(MAKH) - 2) AS INT)) AS MaxNumber FROM KHACHHANG";
            SqlCommand cmd = new SqlCommand(sql);
            dataTable.Fill(cmd);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                // Lấy số lớn nhất và tăng lên 1
                int maxNumber = Convert.ToInt32(result);
                return "KH" + (maxNumber + 1).ToString();
            }

            // Nếu không có dữ liệu trong bảng, trả về giá trị mặc định
            return "KH1";
        }
        #endregion


        #region Sự kiện
        private void fKhachHang_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            string maKHmoi = TaoMaKH();
            maKH = "";
            txtMaKH.Text = maKHmoi;
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            txtMaKH.Focus();
            BatTat(true);
        }



        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text.Trim() == "")
                MessageBox.Show("Mã khách hàng không được bỏ trống!");
            else if (txtTenKH.Text.Trim() == "")
                MessageBox.Show("Họ và tên khách hàng không được bỏ trống!");
            else if (txtSDT.Text.Trim() == "")
                MessageBox.Show("Số điện thoại không được bỏ trống!");
            else if (txtDiaChi.Text.Trim() == "")
                MessageBox.Show("Địa chỉ khách hàng không được bỏ trống!");
            else if (txtMaKH.Text.Trim().Length > 10)
                MessageBox.Show("Mã khách hàng phải nhập tối thiểu 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!txtMaKH.Text.StartsWith("KH") && txtMaKH.Text.Length >= 2)
            {
                MessageBox.Show("Mã nhân viên phải bắt đầu bằng 'KH'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKH.Text = "KH"; // Reset lại thành "KH"
                txtMaKH.SelectionStart = txtMaKH.Text.Length;
            }
            else
            {
                try
                {
                    if (maKH == "")
                    {
                        string sql = @"INSERT INTO KHACHHANG VALUES(@MAKH, @HOVATENKH, @SDT, @DIACHI, @NGAYSINH)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAKH", SqlDbType.NVarChar, 10).Value = txtMaKH.Text.ToUpper();
                        cmd.Parameters.Add("@HOVATENKH", SqlDbType.NVarChar, 50).Value = txtTenKH.Text;
                        cmd.Parameters.Add("@SDT", SqlDbType.NVarChar, 10).Value = txtSDT.Text;
                        cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100).Value = txtDiaChi.Text;
                        cmd.Parameters.Add("@NGAYSINH", SqlDbType.DateTime).Value = dtpNgaySinh.Value;
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
                        string sql = @"UPDATE       KHACHHANG
                                       SET          MAKH = @MAKHMOI,
                                                    HOVATENKH = @HOVATENKH,
                                                    SDT = @SDT,
                                                    DIACHI = @DIACHI,
                                                    NGAYSINH = @NGAYSINH
                                        WHERE       MAKH =@MAKHCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAKHMOI", SqlDbType.NVarChar, 10).Value = txtMaKH.Text.ToUpper();
                        cmd.Parameters.Add("@MAKHCU", SqlDbType.NVarChar, 10).Value = txtMaKH.Text.ToUpper();
                        cmd.Parameters.Add("@HOVATENKH", SqlDbType.NVarChar, 50).Value = txtTenKH.Text;
                        cmd.Parameters.Add("@SDT", SqlDbType.NVarChar, 10).Value = txtSDT.Text;
                        cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100).Value = txtDiaChi.Text;
                        cmd.Parameters.Add("@NGAYSINH", SqlDbType.DateTime).Value = dtpNgaySinh.Value;
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
                    fKhachHang_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }

        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fKhachHang_Load(sender, e);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maKH = txtMaKH.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa khách hàng '" + txtTenKH.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM KHACHHANG WHERE MAKH = @MAKH";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MAKH", SqlDbType.NVarChar, 10).Value = txtMaKH.Text.ToUpper();
                dataTable.Update(cmd);
                fKhachHang_Load(sender, e);

            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            fKhachHang_Load(sender, e);
        }

        #endregion
    }
}
