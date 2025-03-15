using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RapChieuPhim
{
    public partial class menuPhim : Form
    {
        private Button selectedButton = null;
        public menuPhim()
        {
            InitializeComponent();
        }
        #region Functions
        private void HienForm(Form formToShow)
        {
            // Xóa Form cũ trong Panel
            panel2.Controls.Clear();

            // Thiết lập Form con
            formToShow.TopLevel = false;   // Không phải là Form độc lập
            formToShow.FormBorderStyle = FormBorderStyle.None; // Loại bỏ viền
            formToShow.Dock = DockStyle.Fill;  // Tự động co giãn theo Panel

            // Thêm Form vào Panel
            panel2.Controls.Add(formToShow);
            formToShow.Show(); // Hiển thị Form
        }
        #endregion

        #region Sự kiện
        private void Button_Click(object sender, EventArgs e)
        {
            // Lấy Button hiện tại được bấm
            Button currentButton = sender as Button;

            // Đổi màu Button hiện tại và chữ trắng
            currentButton.BackColor = Color.Blue;
            currentButton.ForeColor = Color.White;

            // Đổi màu Button trước đó về trạng thái ban đầu
            if (selectedButton != null && selectedButton != currentButton)
            {
                selectedButton.BackColor = SystemColors.Control; // Màu nền mặc định
                selectedButton.ForeColor = SystemColors.ControlText; // Màu chữ mặc định
            }

            // Cập nhật Button đã bấm
            selectedButton = currentButton;
            
        }

        private void btnPhim_Click(object sender, EventArgs e)
        {
            HienForm(new pPhim());
        }

        private void btnTheLoaiPhim_Click(object sender, EventArgs e)
        {
            HienForm(new pTheLoaiPhim());
        }

        private void btnDinhDangPhim_Click(object sender, EventArgs e)
        {
            HienForm(new pDinhDangPhim());
        }

        private void frmPhim_Load(object sender, EventArgs e)
        {
            btnPhim.Click += Button_Click;
            btnDinhDangPhim.Click += Button_Click;
            btnTheLoaiPhim.Click += Button_Click;
            btnSuatChieu.Click += Button_Click;
        }

        private void btnSuatChieu_Click(object sender, EventArgs e)
        {
            HienForm(new pSuatChieuPhim());
        }
        #endregion
    }
}
