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
    public partial class fPhong : Form
    {
        MyDataTable dataTable = new MyDataTable();
        string maPhong = "";
        public fPhong()
        {
            InitializeComponent();
            dataTable.OpenConnection();
        }
        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM PHONGCHIEU");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;

            txtMaPhong.DataBindings.Clear();
            txtTenPhong.DataBindings.Clear();
            txtMaPhong.DataBindings.Add("Text", biding, @"MAPHONG");
            txtTenPhong.DataBindings.Add("Text", biding, @"TENPHONG");
        }

        private void BatTat(bool giaTri)
        {
            txtMaPhong.Enabled = giaTri; txtTenPhong.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }
        #endregion

        #region Sự kiện
        private void btnThem_Click(object sender, EventArgs e)
        {
            maPhong = "";
            txtMaPhong.Clear();
            txtTenPhong.Clear();
            txtMaPhong.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maPhong = txtMaPhong.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa phòng '" + txtTenPhong.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM PHONGCHIEU WHERE MAPHONG = @MAPHONG";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MAPHONG", SqlDbType.NVarChar, 10).Value = txtMaPhong.Text.ToUpper();
                dataTable.Update(cmd);
                fPhong_Load(sender,e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaPhong.Text.Trim() == "")
                MessageBox.Show("Mã phòng không được bỏ trống!");
            else if (txtTenPhong.Text.Trim() == "")
                MessageBox.Show("Tên phòng không được bỏ trống!");
            else if (txtMaPhong.Text.Trim().Length > 10)
                MessageBox.Show("Mã phòng phải nhập tối thiểu 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    if (maPhong == "")
                    {
                        string sql = @"INSERT INTO PHONGCHIEU VALUES(@MAPHONG, @TENPHONG)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAPHONG", SqlDbType.NVarChar, 10).Value = txtMaPhong.Text.ToUpper();
                        cmd.Parameters.Add("@TENPHONG", SqlDbType.NVarChar, 20).Value = txtTenPhong.Text;
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
                        string sql = @"UPDATE       PHONGCHIEU
                                       SET          MAPHONG = @MAPHONGMOI,
                                                    TENPHONG = @TENPHONG
                                        WHERE       MAPHONG =@MAPHONGCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAPHONGMOI", SqlDbType.NVarChar, 10).Value = txtMaPhong.Text.ToUpper();
                        cmd.Parameters.Add("@MAPHONGCU", SqlDbType.NVarChar, 10).Value = txtMaPhong.Text.ToUpper();
                        cmd.Parameters.Add("@TENPHONG", SqlDbType.NVarChar, 20).Value = txtTenPhong.Text;
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
                    fPhong_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fPhong_Load(sender, e);
        }

        private void fPhong_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }

        #endregion
    }
}
