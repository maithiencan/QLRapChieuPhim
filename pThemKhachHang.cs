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
    public partial class pThemKhachHang : Form
    {
        MyDataTable dataTable = new MyDataTable();
        public pThemKhachHang()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }
        #region Functions
        private void Reset()
        {
            txtMaKH.Text = TaoMaKH();
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            txtMaKH.Focus();
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
        private void pKhachHang_Load(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
           
            txtMaKH.Text = TaoMaKH();
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            txtMaKH.Focus();
        }



        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaKH.Text.Trim() == "")
                MessageBox.Show("Mã khách hàng không được bỏ trống!");
            else if (txtTenKH.Text.Trim() == "")
                MessageBox.Show("Tên khách hàng không được bỏ trống!");
            else if (txtSDT.Text.Trim() == "")
                MessageBox.Show("SDT khách hàng không được bỏ trống!");
            else if (txtDiaChi.Text.Trim() == "")
                MessageBox.Show("Địa chỉ khách hàng không được bỏ trống!");
            else if (txtMaKH.Text.Trim().Length > 10)
                MessageBox.Show("Mã khách hàng phải nhập đúng 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!txtMaKH.Text.StartsWith("KH") && txtMaKH.Text.Length >= 2)
            {
                MessageBox.Show("Mã khách hàng phải bắt đầu bằng 'KH'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaKH.Text = "KH"; // Reset lại thành "KH"
                txtMaKH.SelectionStart = txtMaKH.Text.Length;
            }
            else
            {
                try
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
                    pKhachHang_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            pKhachHang_Load(sender, e);
        }

        #endregion
    }
}
