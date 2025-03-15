using ClosedXML.Excel;
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
    public partial class fThongKeKhachHang : Form
    {
        public fThongKeKhachHang()
        {
            InitializeComponent();
        }

        MyDataTable dataTable = new MyDataTable();

        #region Functions
        private void LayDuLieu()
        {
            txtMaKH.Clear();
            SqlCommand cmd = new SqlCommand(@"
            SELECT 
                KHACHHANG.MAKH, 
                KHACHHANG.HOVATENKH, 
                KHACHHANG.DIACHI,
                KHACHHANG.NGAYSINH,
                KHACHHANG.SDT, 
                COUNT(VE.MAKH) AS SOLANMUA
            FROM KHACHHANG
            LEFT JOIN VE ON KHACHHANG.MAKH = VE.MAKH
            GROUP BY 
                KHACHHANG.MAKH, 
                KHACHHANG.HOVATENKH, 
                KHACHHANG.DIACHI,
                KHACHHANG.NGAYSINH,
                KHACHHANG.SDT;");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;
            int soLuong = DemSoLuongKhachHang();
            lbSoLuong.Text = soLuong.ToString().Trim();
            bindingNavigator1.BindingSource = biding;
        }

        private void LayDuLieu(string tuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT 
            KHACHHANG.MAKH, 
            KHACHHANG.HOVATENKH, 
            KHACHHANG.DIACHI,
            KHACHHANG.NGAYSINH,
            KHACHHANG.SDT, 
            COUNT(VE.MAKH) AS SOLANMUA
        FROM KHACHHANG
        LEFT JOIN VE ON KHACHHANG.MAKH = VE.MAKH
        WHERE KHACHHANG.MAKH LIKE '%" + tuKhoa + "%'  OR KHACHHANG.HOVATENKH LIKE N'%" + tuKhoa + "%'GROUP BY KHACHHANG.MAKH,KHACHHANG.HOVATENKH,KHACHHANG.DIACHI, KHACHHANG.NGAYSINH,KHACHHANG.SDT");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;
            int soLuong = DemSoLuongKhachHang();
            lbSoLuong.Text = soLuong.ToString().Trim();
            bindingNavigator1.BindingSource = biding;
        }

        private int DemSoLuongKhachHang()
        {
            int soLuong = 0;
            string sql = "SELECT COUNT(*) FROM KHACHHANG";
            using (SqlConnection conn = new SqlConnection("Server=LAPTOP-1S0RFBLD;Database=QLRapChieuPhim;Integrated Security=True;"))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    soLuong = (int)cmd.ExecuteScalar();
                }
            }
            return soLuong;
        }

        private void XuatExcel(DataGridView dataGridView, string filePath)
        {
            try
            {
                // Tạo workbook và worksheet mới
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Exported Data");

                    // Thiết lập tiêu đề cột
                    for (int i = 0; i < dataGridView.Columns.Count; i++)
                    {
                        var headerCell = worksheet.Cell(1, i + 1);
                        headerCell.Value = dataGridView.Columns[i].HeaderText;
                        headerCell.Style.Fill.BackgroundColor = XLColor.LightGray; // Màu nền tiêu đề
                        headerCell.Style.Font.Bold = true; // Chữ in đậm
                        headerCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center; // Căn giữa
                        headerCell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; // Viền mỏng
                    }

                    // Thêm dữ liệu từ DataGridView
                    for (int i = 0; i < dataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView.Columns.Count; j++)
                        {
                            var cellValue = dataGridView.Rows[i].Cells[j].Value;
                            var cell = worksheet.Cell(i + 2, j + 1); // Dòng bắt đầu từ 2
                            cell.Value = cellValue != null ? cellValue.ToString() : "";

                            // Tạo viền cho từng ô
                            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        }
                    }

                    // Tự động chỉnh độ rộng của cột
                    worksheet.Columns().AdjustToContents();

                    // Lưu file Excel
                    workbook.SaveAs(filePath);
                }

                MessageBox.Show("Xuất file Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Sự kiện

        private void btnTim_Click(object sender, EventArgs e)
        {
            LayDuLieu(txtMaKH.Text);
        }
        private void txtMaKH_TextChanged(object sender, EventArgs e)
        {
            string maKH = txtMaKH.Text.Trim();
            if (maKH == "")
            {
                fThongKeKhachHang_Load(sender, e);
            }
            else
            {
                string sql = "SELECT * FROM KHACHHANG WHERE MAKH LIKE @MAKH";
                SqlDataAdapter adapter = new SqlDataAdapter(sql, dataTable.connection);
                adapter.SelectCommand.Parameters.AddWithValue("@MAKH", "%" + maKH + "%"); 
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView.DataSource = table;
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            fThongKeKhachHang_Load(sender, e);
        }

        private void fThongKeKhachHang_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            int soLuong = DemSoLuongKhachHang();
            lbSoLuong.Text = soLuong.ToString();
        }

        private void btnXuatExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel|*.xlsx",
                Title = "Lưu file Excel"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                XuatExcel(dataGridView, saveFileDialog.FileName);
            }
        }
        #endregion


    }
}
