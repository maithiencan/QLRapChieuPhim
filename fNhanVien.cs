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
    
    public partial class fNhanVien : Form
    {
        MyDataTable dataTable = new MyDataTable();
        MyDataTable dataCboCHUCVU = new MyDataTable();
        string maNV = "";
        public fNhanVien()
        {
            InitializeComponent();
            dataTable.OpenConnection();
            dataCboCHUCVU.OpenConnection();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmdCBO = new SqlCommand(@"SELECT * FROM CHUCVU");
            dataCboCHUCVU.Fill(cmdCBO);
            BindingSource bindingcbo = new BindingSource();
            bindingcbo.DataSource = dataCboCHUCVU;
            cboChucVu.DataSource = bindingcbo;
            cboChucVu.DisplayMember = "TENCHUCVU";
            cboChucVu.ValueMember = "MACV";

            SqlCommand cmd = new SqlCommand(@"SELECT N.*,  C.TENCHUCVU
                                            FROM NHANVIEN N, CHUCVU C
                                            WHERE N.MACV = C.MACV");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;

            txtMaNV.DataBindings.Clear();
            txtDiaChi.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            txtTenNV.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();
            numLuong.DataBindings.Clear();
            cboCa.DataBindings.Clear();
            radNam.DataBindings.Clear();
            radNu.DataBindings.Clear();
            cboChucVu.DataBindings.Clear();
            txtMaNV.DataBindings.Add("Text", biding, @"MANV");
            txtTenNV.DataBindings.Add("Text", biding, @"HOVATENNV");
            txtSDT.DataBindings.Add("Text", biding, @"SDT");
            dtpNgaySinh.DataBindings.Add("Value", biding, @"NGAYSINH");
            cboCa.DataBindings.Add("Text", biding, @"CA");
            txtDiaChi.DataBindings.Add("Text", biding, @"DIACHI");
            numLuong.DataBindings.Add("Value", biding, @"LUONG");
            Binding nam = new Binding("Checked", dataGridView.DataSource, "PHAI", false , DataSourceUpdateMode.Never); nam.Format += (s, evt) => { evt.Value = Convert.ToInt32(evt.Value) == 1; }; radNam.DataBindings.Add(nam);
            Binding nu = new Binding("Checked", dataGridView.DataSource, "PHAI", false, DataSourceUpdateMode.Never); nu.Format += (s, evt) => { evt.Value = Convert.ToInt32(evt.Value) == 0; }; radNu.DataBindings.Add(nu);
            cboChucVu.DataBindings.Add("SelectedValue", dataGridView.DataSource, @"MACV");
        }

        private void BatTat(bool giaTri)
        {
            txtMaNV.Enabled = giaTri; txtDiaChi.Enabled = giaTri; txtSDT.Enabled = giaTri; txtTenNV.Enabled = giaTri; cboChucVu.Enabled = giaTri;
            cboCa.Enabled = giaTri; dtpNgaySinh.Enabled = giaTri; radNu.Enabled = giaTri; radNam.Enabled = giaTri;  numLuong.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }
        #endregion

        #region Sự kiện
        private void fNhanVien_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
            dataGridView.CellFormatting += dgvDSNhanVien_CellFormatting;
        }
        private void dgvDSNhanVien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) 
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "PHAI")
            {
                // Kiểm tra giá trị trước khi xử lý
                if (e.Value != null && e.Value is byte)
                {
                    e.Value = (byte)e.Value == 1 ? "Nam" : "Nữ";
                    e.FormattingApplied = true; // Xác nhận định dạng đã được áp dụng
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            maNV = "";
            txtMaNV.Clear();
            txtDiaChi.Clear();
            txtTenNV.Clear();
            txtSDT.Clear();
            cboCa.Text = "Sáng";
            dtpNgaySinh.Text = DateTime.Now.ToString();
            radNam.Checked = true;
            numLuong.Value = 1000000;
            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maNV = txtMaNV.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa nhân viên '" + txtTenNV.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM NHANVIEN WHERE MANV = @MANV";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 10).Value = txtMaNV.Text.ToUpper();
                dataTable.Update(cmd);
                fNhanVien_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaNV.Text.Trim() == "")
                MessageBox.Show("Mã nhân viên không được bỏ trống!");
            else if (txtDiaChi.Text.Trim() == "")
                MessageBox.Show("Địa chỉ không được bỏ trống!");
            else if (txtTenNV.Text.Trim() == "")
                MessageBox.Show("Tên nhân viên không được bỏ trống!");
            else if (txtSDT.Text.Trim() == "")
                MessageBox.Show("Số  điện thoại không được bỏ trống!");
            else if (txtDiaChi.Text.Trim() == "")
                MessageBox.Show("Địa chỉ không được bỏ trống!");
            else if (txtMaNV.Text.Trim().Length > 10)
                MessageBox.Show("Mã nhân viên phải nhập đúng 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!txtMaNV.Text.StartsWith("NV") && txtMaNV.Text.Length >= 2)
            {
                MessageBox.Show("Mã nhân viên phải bắt đầu bằng 'NV'!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaNV.Text = "NV"; // Reset lại thành "NV"
                txtMaNV.SelectionStart = txtMaNV.Text.Length;
            }
            else
            {
                try
                {
                    if (maNV == "")
                    {
                        string sql = @"INSERT INTO NHANVIEN VALUES (@MANV, @HOVATENNV, @DIACHI, @SDT, @NGAYSINH,@PHAI, @LUONG, @CA,@MACV)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 10).Value = txtMaNV.Text.ToUpper();
                        cmd.Parameters.Add("@HOVATENNV", SqlDbType.NVarChar, 50).Value = txtTenNV.Text;
                        cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100).Value = txtDiaChi.Text;
                        cmd.Parameters.Add("@SDT", SqlDbType.NVarChar, 10).Value = txtSDT.Text;
                        cmd.Parameters.Add("@NGAYSINH", SqlDbType.DateTime).Value = dtpNgaySinh.Text;
                        cmd.Parameters.Add("@LUONG", SqlDbType.Int).Value = numLuong.Value;
                        cmd.Parameters.Add("@CA", SqlDbType.NVarChar, 10).Value = cboCa.Text;
                        cmd.Parameters.Add("@PHAI", SqlDbType.TinyInt).Value = radNam.Checked ? 1 : 0;
                        cmd.Parameters.Add("@MACV", SqlDbType.NVarChar, 10).Value = cboChucVu.SelectedValue;
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
                        string sql = @"UPDATE       NHANVIEN
                                       SET          MANV = @MANVMOI,
                                                    HOVATENNV = @HOVATENNV,
                                                    DIACHI = @DIACHI,
                                                    SDT = @SDT,
                                                    NGAYSINH = @NGAYSINH,
                                                    LUONG = @LUONG,
                                                    CA = @CA,
                                                    PHAI =@PHAI,
                                                    MACV = @MACV
                                        WHERE       MANV =@MANVCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MANVMOI", SqlDbType.NVarChar, 10).Value = txtMaNV.Text.ToUpper();
                        cmd.Parameters.Add("@MANVCU", SqlDbType.NVarChar, 10).Value = txtMaNV.Text.ToUpper();
                        cmd.Parameters.Add("@HOVATENNV", SqlDbType.NVarChar, 50).Value = txtTenNV.Text;
                        cmd.Parameters.Add("@DIACHI", SqlDbType.NVarChar, 100).Value = txtDiaChi.Text;
                        cmd.Parameters.Add("@SDT", SqlDbType.NVarChar, 10).Value = txtSDT.Text;
                        cmd.Parameters.Add("@NGAYSINH", SqlDbType.DateTime).Value = dtpNgaySinh.Text;
                        cmd.Parameters.Add("@LUONG", SqlDbType.Int).Value = numLuong.Value;
                        cmd.Parameters.Add("@CA", SqlDbType.NVarChar, 10).Value = cboCa.Text;
                        cmd.Parameters.Add("@PHAI", SqlDbType.TinyInt).Value = radNam.Checked ? 1 : 0;
                        cmd.Parameters.Add("@MACV", SqlDbType.NVarChar, 10).Value = cboChucVu.SelectedValue;
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
                    fNhanVien_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fNhanVien_Load(sender, e);
        }
        #endregion
    }
}

