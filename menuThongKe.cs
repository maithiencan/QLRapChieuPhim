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
    public partial class menuThongKe : Form
    {
        private Button selectedButton = null;
        public menuThongKe()
        {
            InitializeComponent();
        }

        #region Function
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
        private void btnThongKeNV_Click(object sender, EventArgs e)
        {
            HienForm(new fThongKeNV());
        }

        private void btnThongKeKH_Click(object sender, EventArgs e)
        {
            HienForm(new fThongKeKhachHang());
        }

        private void btnThongKeVe_Click(object sender, EventArgs e)
        {
            HienForm(new fThongKeVe());
        }

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

        private void menuThongKe_Load(object sender, EventArgs e)
        {

            btnThongKeKH.Click += Button_Click;
            btnThongKeNV.Click += Button_Click;
            btnThongKeVe.Click += Button_Click;
        }
        #endregion
    }
}
