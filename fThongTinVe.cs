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
    public partial class fThongTinVe : Form
    {
        MyDataTable dataTable = new MyDataTable();
        public fThongTinVe()
        {
            InitializeComponent();
            dataTable.OpenConnection();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Functions
        private void LayDuLieu()
        {
            txtTim.Clear();
            SqlCommand cmd = new SqlCommand(@"SELECT V.*, K.HOVATENKH, L.TENLV, P.TENPHONG, PH.TENPHIM, L.DONGIA, S.KHOICHIEU
                                            FROM VE V, KHACHHANG K, PHIM PH, PHONGCHIEU P, SUATCHIEU S, LOAIVE L
                                            WHERE V.MASUAT = S.MASUAT AND V.MAKH = K.MAKH AND V.MALV = L.MALV AND S.MAPHONG = P.MAPHONG AND S.MAPHIM = PH.MAPHIM");
            dataTable.Fill(cmd);
            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;
            dataGridView.DataSource = biding;
            bindingNavigator1.BindingSource = biding;
        }

        private void LayDuLieu(string tuKhoa)
        {
            txtTim.Clear();
            SqlCommand cmd = new SqlCommand(@"SELECT V.*, K.HOVATENKH, L.TENLV, P.TENPHONG, PH.TENPHIM, L.DONGIA, S.KHOICHIEU
                                            FROM VE V, KHACHHANG K, PHIM PH, PHONGCHIEU P, SUATCHIEU S, LOAIVE L
                                            WHERE V.MASUAT = S.MASUAT AND V.MAKH = K.MAKH AND V.MALV = L.MALV AND S.MAPHONG = P.MAPHONG AND S.MAPHIM = PH.MAPHIM
                                            AND(V.MAVE LIKE '%"+tuKhoa+ "%' OR K.HOVATENKH LIKE N'%"+tuKhoa+"%')");
            dataTable.Fill(cmd);
            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;
            dataGridView.DataSource = biding;
            bindingNavigator1.BindingSource = biding;
        }
        #endregion

        #region Sự kiện
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "TRANGTHAI")
            {
                // Kiểm tra giá trị trước khi xử lý
                if (e.Value != null && e.Value is byte)
                {
                    e.Value = (byte)e.Value == 1 ? "Đã thanh toán" : "Chưa thanh toán";
                    e.FormattingApplied = true; // Xác nhận định dạng đã được áp dụng
                }
            }
        }

        private void fThongTinVe_Load(object sender, EventArgs e)
        {
            LayDuLieu();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            fThongTinVe_Load(sender, e);
        }

        #endregion

        private void btnTim_Click(object sender, EventArgs e)
        {
            LayDuLieu(txtTim.Text);
        }
    }
}
