using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapChieuPhim
{
    public partial class fHoaDon : Form
    {
        public fHoaDon()
        {
            InitializeComponent();
        }

        #region Lấy thông tin
        public string MaVe { get; set; }
        public string TenKhachHang { get; set; }
        public string Phim { get; set; }
        public string KhoiChieu { get; set; }
        public string LoaiVe { get; set; }
        public string Phong { get; set; }
        public string Ghe { get; set; }
        public string Gia { get; set; }
        public string NgayBan { get; set; }
        #endregion

        #region Sự kiện
        public void LoadData()
        {
            lbMaVe.Text = MaVe;
            lbTenKhachHang.Text = TenKhachHang;
            lbPhim.Text = Phim;
            lbKhoiChieu.Text = KhoiChieu;
            lbLoaiVe.Text = LoaiVe;
            lbPhong.Text = Phong;
            lbGia.Text = Gia;
            lbNgayBan.Text = NgayBan;
            lbGhe.Text = Ghe;
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.Title = "Lưu Form Dưới Dạng Hình Ảnh";
            saveFileDialog.FileName = lbMaVe.Text + ".png";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                using (Bitmap bmp = new Bitmap(pnHoaDon.Width, pnHoaDon.Height))
                {
                    pnHoaDon.DrawToBitmap(bmp, new Rectangle(0, 0, pnHoaDon.Width, pnHoaDon.Height));
                    bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                }

                MessageBox.Show("Hóa Đơn đã được lưu thành công dưới dạng hình ảnh!", "Thông báo");
            }

        }
        #endregion
    }
}
