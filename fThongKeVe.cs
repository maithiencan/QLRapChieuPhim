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
using static System.Net.Mime.MediaTypeNames;
using ClosedXML.Excel;

namespace RapChieuPhim
{
    public partial class fThongKeVe : Form
    {
        MyDataTable dataTable = new MyDataTable();
        public fThongKeVe()
        {
            InitializeComponent();
            dataTable.OpenConnection();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Function
        private void LayDuLieu()
        {
            txtMaVe.Clear();
            SqlCommand cmd = new SqlCommand(@"SELECT V.*, K.HOVATENKH, L.DONGIA, L.TENLV
                                            FROM VE V, KHACHHANG K,LOAIVE L
                                            WHERE V.MAKH = K.MAKH AND V.MALV = L.MALV");
            dataTable.Fill(cmd);
            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;
            dataGridView.DataSource = biding;
            bindingNavigator1.BindingSource = biding;
        }

        private void LayDuLieu(string tuKhoa)
        {

            SqlCommand cmd = new SqlCommand(@"SELECT V.*, K.HOVATENKH, L.DONGIA, L.TENLV
                                            FROM VE V, KHACHHANG K,LOAIVE L
                                            WHERE V.MAKH = K.MAKH AND V.MALV = L.MALV AND(V.MAVE LIKE '%" + tuKhoa + "%'  OR K.HOVATENKH LIKE N'%" + tuKhoa + "%')");
            dataTable.Fill(cmd);
            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;
            dataGridView.DataSource = biding;
            bindingNavigator1.BindingSource = biding;
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

        private int DemSoLuongVe()
        {
            int soLuong = 0;
            string sql = "SELECT COUNT(*) FROM VE";
            MyDataTable dataDem = new MyDataTable();
            dataDem.OpenConnection();
            SqlCommand cmd = new SqlCommand(sql);
            dataDem.Fill(cmd);
            soLuong = (int)cmd.ExecuteScalar();
            return soLuong;
        }

        private int TinhTongTien()
        {
            int tongTien = 0;
            string sql = "SELECT SUM(L.DONGIA) FROM VE V, LOAIVE L WHERE V.MALV = L.MALV ";
            MyDataTable dataTien = new MyDataTable();
            dataTien.OpenConnection(); // Mở kết nối
            SqlCommand cmd = new SqlCommand(sql);
            dataTien.Fill(cmd);
            object result = cmd.ExecuteScalar();
            if (result != DBNull.Value) // Kiểm tra nếu không null
            {
                tongTien = Convert.ToInt32(result);
            }
            return tongTien;
        }
        #endregion

        #region Sự kiện
        private void fThongKeVe_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            int soLuong = DemSoLuongVe();
            lbSoLuong.Text = soLuong.ToString();
            int tongTien = TinhTongTien();
            lbTien.Text = tongTien.ToString()+"VND";
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            fThongKeVe_Load(sender, e);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LayDuLieu(txtMaVe.Text);
        }

        #endregion
    }
}
