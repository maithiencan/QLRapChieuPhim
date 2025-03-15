using QLHS;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RapChieuPhim
{
    public partial class fDatVe : Form
    {
        #region Biến toàn cục
        private ArrayList seatButtons = new ArrayList();
        private bool isLoaded = false;
        MyDataTable myData = new MyDataTable();
        MyDataTable dataDGV = new MyDataTable();
        MyDataTable cboLV = new MyDataTable();
        MyDataTable cboKH = new MyDataTable();
        MyDataTable cboNV = new MyDataTable();
        MyDataTable dataPhim = new MyDataTable();
        string maVe = "";
        string maLV = "";
        string maNV = "";
        string maKH = "";
        string khoiChieu = "";
        string tenPhim = "";
        private string maSuatHienTai = "";
        private bool isFormVisible = false;
        private Button selectedButton = null;
        #endregion
        public fDatVe()
        {
            InitializeComponent();
            dataDGV.OpenConnection();
            cboLV.OpenConnection();
            cboNV.OpenConnection();
            cboKH.OpenConnection();
            myData.OpenConnection();
            dataPhim.OpenConnection();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Sự kiện
        private void pVe_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            btnXuatHoaDon.Enabled = false;
            pnThongTin.Visible = false;
            pnChonGhe.Visible = false;
            flowSeats.Visible = false;
            BatTat(false);
            btnThemKhachHang.Enabled = false;
            if (!isLoaded)
            {

                TaoGhe(); ;
                isLoaded = true;
            }
            btnThem.Click += btnThemGhe_Click;
        }

        private void Ghe_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                if (selectedButton != null)
                {
                    selectedButton.BackColor = Color.LightGray;  
                    selectedButton.Enabled = true;  
                }
                clickedButton.BackColor = Color.DodgerBlue; 
                lbGhe.Text = $"{clickedButton.Text}";
                lbGhe.ForeColor = Color.Green;
                selectedButton = clickedButton;
            }
        }


        private void btnThemGhe_Click(object sender, EventArgs e)
        {
            foreach (Button btn in seatButtons)
            {
                btn.Enabled = true;
                btn.BackColor = Color.LightGray;
            }
        }





        private void btnThem_Click(object sender, EventArgs e)
        {
            string maVeMoi = TaoMaVe();
            txtMaVe.Text = maVeMoi;
            maVe = "";
            maLV = "";
            maKH = "";
            maNV = "";
            cboNhanVien.Text = "";
            cboLoaiVe.Text = "";
            cboKhachHang.Text = "";
            lbGhe.Text = "Vui lòng chọn ghế!";
            dtpNgayBan.Text = "";
            txtMaVe.Focus();
            pnChonGhe.Visible = true;
            flowSeats.Visible = true;
            btnThemKhachHang.Enabled = true;
            BatTat(true);
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            pVe_Load(sender, e);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string seatNumber = lbGhe.Text.Replace("Ghế: ", "");
            if (txtMaVe.Text.Trim() == "")
                MessageBox.Show("Mã vé không được bỏ trống!");
            else if (string.IsNullOrEmpty(maSuatHienTai))
            {
                MessageBox.Show("Vui lòng chọn suất chiếu và nhập đầy đủ thông tin!", "Thông báo");
                return;
            }
            else
            {
                try
                {
                    if (maVe == "" || maLV == "" || maKH == "" || maNV == "")
                    {
                        string sql = @"INSERT INTO VE VALUES(@MAVE, @MALV, @MANV, @MAKH, @GHE, @MASUAT, @NGAYBANVE, @TRANGTHAI)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAVE", SqlDbType.NVarChar, 10).Value = txtMaVe.Text.ToUpper();
                        cmd.Parameters.Add("@MALV", SqlDbType.NVarChar, 10).Value = cboLoaiVe.SelectedValue.ToString();
                        cmd.Parameters.Add("@MANV", SqlDbType.NVarChar, 10).Value = cboNhanVien.SelectedValue.ToString();
                        cmd.Parameters.Add("@MAKH", SqlDbType.NVarChar, 10).Value = cboKhachHang.SelectedValue.ToString();
                        cmd.Parameters.Add("@GHE", SqlDbType.Int).Value = int.Parse(seatNumber);
                        cmd.Parameters.Add("@MASUAT", SqlDbType.NVarChar, 10).Value = maSuatHienTai;
                        cmd.Parameters.Add("@NGAYBANVE", SqlDbType.DateTime).Value = dtpNgayBan.Value;
                        cmd.Parameters.Add("@TRANGTHAI", SqlDbType.TinyInt).Value = chkTrangThai.Checked ? 1 : 0;
                        int result = dataDGV.Update(cmd);

                        if (result > 0)
                        {
                            MessageBox.Show("Thêm vé thành công!", "Thông báo");
                            btnLuu.Enabled = false;
                            btnXuatHoaDon.Enabled = true;
                        }                       
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnThemKhachHang_Click(object sender, EventArgs e)
        {
            DoiMauNut(sender as Button);
            HienForm(new pThemKhachHang());
             
        }

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView.CurrentRow != null)
            {
                DataGridViewRow row = dataGridView.CurrentRow;
                lbPhong.Text = row.Cells["TENPHONG"].Value?.ToString() ?? "N/A";
                maSuatHienTai = dataGridView.CurrentRow.Cells["MASUAT1"].Value.ToString();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            pVe_Load(sender, e);
        }



        private void btnTim_Click(object sender, EventArgs e)
        {
            LayDuLieu(cboPhim.Text);
        }



        private void cboLoaiVe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboLoaiVe.SelectedValue != null)
            {
                string maLoaiVe = cboLoaiVe.SelectedValue.ToString();

                // Tìm đơn giá dựa vào mã loại vé
                DataRow[] selectedRows = cboLV.Select($"MALV = '{maLoaiVe}'");
                if (selectedRows.Length > 0)
                {
                    // Hiển thị đơn giá vào Label
                    string donGia = selectedRows[0]["DONGIA"].ToString();
                    lbGiaVe.Text = $"{donGia} VND";

                }
            }
        }

        private void btnXuatHoaDon_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count > 0)
            {
                fHoaDon fHoaDon = new fHoaDon();

                fHoaDon.MaVe = txtMaVe.Text;
                fHoaDon.TenKhachHang = cboKhachHang.Text;
                fHoaDon.Phim = cboPhim.Text;
                fHoaDon.KhoiChieu = khoiChieu;
                fHoaDon.LoaiVe = cboLoaiVe.Text;
                fHoaDon.Phong = lbPhong.Text;
                fHoaDon.Ghe = lbGhe.Text;
                fHoaDon.Gia = lbGiaVe.Text;
                fHoaDon.NgayBan = dtpNgayBan.Text;
                fHoaDon.Phim = tenPhim;
                fHoaDon.LoadData();
                fHoaDon.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một phim để tiếp tục!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                khoiChieu = dataGridView.Rows[e.RowIndex].Cells["KHOICHIEU"].Value.ToString();
                tenPhim = dataGridView.Rows[e.RowIndex].Cells["TENPHIM"].Value.ToString();
                pnThongTin.Visible = true;
            }
        }
        #endregion

        #region Functions
        private void LayDuLieu()
        {
            SqlCommand cmdDGV = new SqlCommand(@"SELECT T.*, P.TENPHONG, PH.TENPHIM
                                                 FROM SUATCHIEU T, PHONGCHIEU P, PHIM PH
                                                 WHERE T.MAPHIM = PH.MAPHIM AND T.MAPHONG = P.MAPHONG");
            dataDGV.Fill(cmdDGV);
            BindingSource bidingDGV = new BindingSource();
            bidingDGV.DataSource = dataDGV;
            dataGridView.DataSource = bidingDGV;

            SqlCommand cmdLoaiVe = new SqlCommand(@"SELECT * FROM LOAIVE");
            cboLV.Fill(cmdLoaiVe);
            BindingSource bindingLoaiVe = new BindingSource();
            bindingLoaiVe.DataSource = cboLV;
            cboLoaiVe.DataSource = bindingLoaiVe;
            cboLoaiVe.DisplayMember = @"TENLV";
            cboLoaiVe.ValueMember = @"MALV";

            SqlCommand cmdNhanVien = new SqlCommand(@"SELECT *  FROM NHANVIEN");
            cboNV.Fill(cmdNhanVien);
            BindingSource bindingNhanVien = new BindingSource();
            bindingNhanVien.DataSource = cboNV;
            cboNhanVien.DataSource = bindingNhanVien;
            cboNhanVien.DisplayMember = @"HOVATENNV";
            cboNhanVien.ValueMember = @"MANV";

            SqlCommand cmdKhachHang = new SqlCommand(@"SELECT *  FROM KHACHHANG");
            cboKH.Fill(cmdKhachHang);
            BindingSource bindingKhachHang = new BindingSource();
            bindingKhachHang.DataSource = cboKH;
            cboKhachHang.DataSource = bindingKhachHang;
            cboKhachHang.DisplayMember = @"HOVATENKH";
            cboKhachHang.ValueMember = @"MAKH";

            SqlCommand cmdPhim = new SqlCommand(@"SELECT MAPHIM, TENPHIM FROM PHIM");
            dataPhim.Fill(cmdPhim);
            BindingSource bindingPhim = new BindingSource();
            bindingPhim.DataSource = dataPhim;
            cboPhim.DataSource = bindingPhim;
            cboPhim.DisplayMember = @"TENPHIM";
            cboPhim.ValueMember = @"MAPHIM";
            cboPhim.Text = "";
        }

        private void LayDuLieu(string tuKhoa)
        {
            SqlCommand cmdDGV = new SqlCommand(@"SELECT T.*, P.TENPHONG, PH.TENPHIM
                                                 FROM SUATCHIEU T, PHONGCHIEU P, PHIM PH
                                                 WHERE T.MAPHIM = PH.MAPHIM AND T.MAPHONG = P.MAPHONG AND (PH.TENPHIM LIKE N'%" + tuKhoa + "%')");
            dataDGV.Fill(cmdDGV);
            BindingSource bidingDGV = new BindingSource();
            bidingDGV.DataSource = dataDGV;
            dataGridView.DataSource = bidingDGV;
        }

        private void BatTat(bool giaTri)
        {

            cboKhachHang.Enabled = giaTri;
            cboLoaiVe.Enabled = giaTri;
            cboNhanVien.Enabled = giaTri;
            chkTrangThai.Enabled = giaTri;
            chkTrangThai.Checked = true;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri;
        }


        private void TaoGhe()
        {
            int totalSeats = 120;
            for (int i = 1; i <= totalSeats; i++)
            {
                Button seatButton = new Button();
                seatButton.Text = i.ToString();
                seatButton.Width = 45;
                seatButton.Height = 45;
                seatButton.Tag = i;
                seatButton.BackColor = Color.LightGray;
                seatButton.Cursor = Cursors.Hand;
                seatButton.Click += Ghe_Click;
                // Thêm ghế vào ArrayList và FlowLayoutPanel
                seatButtons.Add(seatButton);
                flowSeats.Controls.Add(seatButton);
            }
        }

        private void HienForm(Form formToShow)
        {
            if (isFormVisible)
            {
                // Nếu form đang hiển thị, thì ẩn nó đi
                panel6.Controls.Clear();
                isFormVisible = false;
            }
            else
            {
                panel6.Controls.Clear();

                // Thiết lập Form con
                formToShow.TopLevel = false;   // Không phải là Form độc lập
                formToShow.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền

                // Thêm Form vào Panel
                panel6.Controls.Add(formToShow);
                formToShow.Show(); // Hiển thị Form
                isFormVisible = true;
            }
        }

        private string TaoMaVe()
        {
            MyDataTable dataTable = new MyDataTable();
            string maxMaVe = null;
            dataTable.OpenConnection();
            string sql = "SELECT MAX(CAST(SUBSTRING(MAVE, 2, LEN(MAVE) - 1) AS INT)) AS MaxNumber FROM VE";
            SqlCommand cmd = new SqlCommand(sql);
            dataTable.Fill(cmd);

            object result = cmd.ExecuteScalar();
            if (result != null && result != DBNull.Value)
            {
                // Lấy số lớn nhất và tăng lên 1
                int maxNumber = Convert.ToInt32(result);
                return "V" + (maxNumber + 1).ToString();
            }

            // Nếu không có dữ liệu trong bảng, trả về giá trị mặc định
            return "V1";
        }

        private void DoiMauNut(Button button)
        {
            if (button != null)
            {
                if (button.BackColor == Color.White)
                {
                    button.BackColor = Color.Blue;
                    button.ForeColor = Color.White;
                }
                else
                {
                    button.BackColor = Color.White;
                    button.ForeColor = Color.Black;
                }
            }
        }
        #endregion       
    }
 }




