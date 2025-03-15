using QLHS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BC = BCrypt.Net.BCrypt;

namespace RapChieuPhim
{
    public partial class fManHinhChinh : Form
    {
        #region Biến toàn cục
        MyDataTable dataTable = new MyDataTable();
        fPhong fPhong = null;
        menuPhim frmPhim = null;
        fNhanVien fNhanVien = null;
        AboutBox frmAbout = null;
        pThemKhachHang pKhach = null;
        fKhachHang fKhachHang = null;
        fTaiKhoan fTaiKhoan = null;
        fThongKeNV fThongKeNV = null;
        fThongKeKhachHang fThongKeKhachHang = null;
        fLoaiVe fLoaiVe = null;
        fDangNhap dangNhap = null;
        string hoVaTen = "";
        fDatVe pDatVe = null;
        fThongKeVe fThongKeVe = null;
        fThongTinVe fThongTinVe = null;
        menuThongKe fmenuTK = null;
        private bool isFormVisible = false;
        #endregion

        public fManHinhChinh()
        {
            InitializeComponent();
            timer.Start();
            Flash flash = new Flash();
            flash.ShowDialog();
            dataTable.OpenConnection();
        }





        #region Sự kiện

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            HienForm(new pStart());
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            if (fmenuTK == null || fmenuTK.IsDisposed)
            {
                fmenuTK = new menuThongKe();
                fmenuTK.MdiParent = this;
                fmenuTK.Show();
            }

        }
        private void tHOÁTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tHOÁTToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            var kq = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pHÒNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fPhong == null || fPhong.IsDisposed)
            {
                fPhong = new fPhong();
                fPhong.MdiParent = this;
                fPhong.Show();
            }
        }

        private void pHIMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmPhim == null || frmPhim.IsDisposed)
            {
                frmPhim = new menuPhim();
                frmPhim.MdiParent = this;
                frmPhim.Show();
            }
        }

        private void nHÂNVIÊNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fNhanVien == null || fNhanVien.IsDisposed)
            {
                fNhanVien = new fNhanVien();
                fNhanVien.MdiParent = this;
                fNhanVien.Show();
            }
        }

        private void vÉToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fLoaiVe == null || fLoaiVe.IsDisposed)
            {
                fLoaiVe = new fLoaiVe();
                fLoaiVe.MdiParent = this;
                fLoaiVe.Show();
            }
        }


        private void aBOUTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmAbout == null || frmAbout.IsDisposed)
            {
                frmAbout = new AboutBox();
                frmAbout.MdiParent = this;
                frmAbout.Show();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToString("hh:mm tt / dd-MM-yyyy");
        }



        private void fManHinhChinh_Load(object sender, EventArgs e)
        {
            //PlayOpenFormSound();
            ChuaDangNhap();
            DangNhap();
        }

        private void kHÁCHHÀNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fKhachHang == null || fKhachHang.IsDisposed)
            {
                fKhachHang = new fKhachHang();
                fKhachHang.MdiParent = this;
                fKhachHang.Show();
            }
        }



        private void tÀIKHOẢNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fTaiKhoan == null || fTaiKhoan.IsDisposed)
            {
                fTaiKhoan = new fTaiKhoan();
                fTaiKhoan.MdiParent = this;
                fTaiKhoan.Show();
            }
        }

        private void nHÂNVIÊNToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fThongKeNV == null || fThongKeNV.IsDisposed)
            {
                fThongKeNV = new fThongKeNV();
                fThongKeNV.MdiParent = this;
                fThongKeNV.Show();
            }
        }

        private void kHÁCHHÀNGToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (fThongKeKhachHang == null || fThongKeKhachHang.IsDisposed)
            {
                fThongKeKhachHang = new fThongKeKhachHang();
                fThongKeKhachHang.MdiParent = this;
                fThongKeKhachHang.Show();
            }
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DangNhap();
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            var kq = MessageBox.Show("Bạn có muốn đăng xuất không?","Đăng xuất",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                foreach (Form child in MdiChildren)
                {
                    child.Close();
                }
                ChuaDangNhap();
            }
        }

        private void btnDatVe_Click(object sender, EventArgs e)
        {
            if (pDatVe == null || pDatVe.IsDisposed)
            {
                pDatVe = new fDatVe();
                pDatVe.MdiParent = this;
                pDatVe.Show();
            }
        }

        private void mneBCVE_Click(object sender, EventArgs e)
        {
            if (fThongKeVe == null || fThongKeVe.IsDisposed)
            {
                fThongKeVe = new fThongKeVe();
                fThongKeVe.MdiParent = this;
                fThongKeVe.Show();
            }
        }

        private void btnTrangThai_Click(object sender, EventArgs e)
        {
            if (fThongTinVe == null || fThongTinVe.IsDisposed)
            {
                fThongTinVe = new fThongTinVe();
                fThongTinVe.MdiParent = this;
                fThongTinVe.Show();
            }
        }
        #endregion

        #region Functions

        private void HienForm(Form formToShow)
        {
            if (isFormVisible)
            {

                pnStart.Controls.Clear();
                isFormVisible = false;

                pnStart.Visible = false;
            }
            else
            {
                pnStart.Controls.Clear();

                formToShow.TopLevel = false;  
                formToShow.FormBorderStyle = FormBorderStyle.None; 

                pnStart.Controls.Add(formToShow);
                formToShow.Show();
                isFormVisible = true;

                pnStart.Visible = true;
            }
        }

        private void PlayOpenFormSound()
        {
            try
            {
                var soundStream = Properties.Resources.oxp;

                SoundPlayer player = new SoundPlayer(soundStream);

                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể phát âm thanh: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ChuaDangNhap()
        {
            btnDangNhap.Enabled = true;
            btnDangXuat.Enabled = false;
            mneAdmin.Visible = false;           
            mneHeThong.Visible = true;
            mneTaiKhoan.Visible = false;
            mneBaoCao.Visible = false;
            btnDatVe.Enabled = false;
            btnThongKe.Enabled = false;
            btnTrangThai.Enabled = false;
            lbTaiKhoan.Text = "Chưa đăng nhập";
        }

        public void QuanLi()
        {
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            mneAdmin.Visible = true;
            mneHeThong.Visible = true;
            mneTaiKhoan.Visible = true;
            btnDatVe.Enabled = true;
            mneBaoCao.Visible = true;
            btnThongKe.Enabled = true;
            btnTrangThai.Enabled = true;
            lbTaiKhoan.Text = "QUẢN LÍ: " + hoVaTen;
        }

        public void NhanVien()
        {
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            mneAdmin.Visible = false;
            mneBaoCao.Visible = false;
            btnDatVe.Enabled = true;
            btnThongKe.Enabled = false;
            btnTrangThai.Enabled = true;
            lbTaiKhoan.Text = "NHÂN VIÊN: " + hoVaTen;
        }

        public void KeToan()
        {
            btnDangNhap.Enabled = false;
            btnDangXuat.Enabled = true;
            mneHeThong.Visible = false;
            mneAdmin.Visible = false;
            mneBaoCao.Visible = true;
            btnDatVe.Enabled = false;
            btnThongKe.Enabled = true;
            btnTrangThai.Enabled = false;
            lbTaiKhoan.Text = "KẾ TOÁN: " + hoVaTen;
        }

        private void DangNhap()
        {
        LamLai:
            if (dangNhap == null || dangNhap.IsDisposed)
                dangNhap = new fDangNhap();
            if (dangNhap.ShowDialog() == DialogResult.OK)
            {
                if (dangNhap.txtTenDangNhap.Text.Trim() == "")
                {
                    MessageBox.Show("Tên đăng nhập không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dangNhap.txtTenDangNhap.Focus();
                    goto LamLai;
                }
                else if (dangNhap.txtMatKhau.Text.Trim() == "")
                {
                    MessageBox.Show("Mật khẩu không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dangNhap.txtMatKhau.Focus();
                    goto LamLai;
                }
                else
                {
                    MyDataTable dataTable = new MyDataTable();
                    dataTable.OpenConnection();
                    SqlCommand cmd = new SqlCommand("SELECT T.TENTAIKHOAN,T.MATKHAU, T.QUYENHAN, N.HOVATENNV " +
                        "                            FROM TAIKHOAN T , NHANVIEN N " +
                        "                            WHERE T.TENTAIKHOAN = @TTK AND T.MATKHAU = @MK AND T.MANV=N.MANV");
                    cmd.Parameters.Add("@TTK", SqlDbType.NVarChar, 50).Value = dangNhap.txtTenDangNhap.Text;
                    cmd.Parameters.Add("@MK", SqlDbType.NVarChar, 100).Value = dangNhap.txtMatKhau.Text;
                    dataTable.Fill(cmd);
                    if (dataTable.Rows.Count > 0)
                    {
                        hoVaTen = dataTable.Rows[0]["HOVATENNV"].ToString();
                        string quyenHan = dataTable.Rows[0]["QUYENHAN"].ToString();
                        if (quyenHan == "Admin")
                            QuanLi();
                        else if (quyenHan == "Kế toán")
                            KeToan();
                        else if (quyenHan == "Nhân viên")
                            NhanVien();
                        else
                            ChuaDangNhap();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        dangNhap.txtTenDangNhap.Focus();
                        goto LamLai;
                    }
                }
            }
        }


        #endregion

       
    }
}
