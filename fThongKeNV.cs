﻿using ClosedXML.Excel;
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
    public partial class fThongKeNV : Form
    {
        MyDataTable dataTable = new MyDataTable();
        public fThongKeNV()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
        }

        #region Functions
        private void LayDuLieu()
        {
            txtMaNV.Clear();
            SqlCommand cmd = new SqlCommand(@"SELECT N.*, C.TENCHUCVU FROM NHANVIEN N, CHUCVU C WHERE N.MACV = C.MACV");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;
            int soLuong = DemSoLuongNhanVien();
            lbSoLuong.Text = soLuong.ToString().Trim();

            bindingNavigator1.BindingSource = biding;
        }

        private void LayDuLieu(string tuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT N.*, C.TENCHUCVU FROM NHANVIEN N, CHUCVU C WHERE N.MACV = C.MACV AND (N.MANV LIKE '%"+tuKhoa+ "%' OR N.HOVATENNV LIKE N'%"+tuKhoa+"%')");
            dataTable.Fill(cmd);

            BindingSource biding = new BindingSource();
            biding.DataSource = dataTable;

            dataGridView.DataSource = biding;
            int soLuong = DemSoLuongNhanVien();
            lbSoLuong.Text = soLuong.ToString().Trim();

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





        private int DemSoLuongNhanVien()
        {
            int soLuong = 0;
            string sql = "SELECT COUNT(*) FROM NHANVIEN";
            MyDataTable dataDem = new MyDataTable();
            dataDem.OpenConnection();
            SqlCommand cmd = new SqlCommand(sql);
            dataDem.Fill(cmd);
            soLuong = (int)cmd.ExecuteScalar();
            return soLuong;
        }

        private int TinhTongLuong()
        {
            int tongLuong = 0;
            string sql = "SELECT SUM(LUONG) FROM NHANVIEN";
            MyDataTable dataLuong = new MyDataTable();
            dataLuong.OpenConnection(); // Mở kết nối
            SqlCommand cmd = new SqlCommand(sql);
            dataLuong.Fill(cmd);
            object result = cmd.ExecuteScalar();
            if (result != DBNull.Value) // Kiểm tra nếu không null
            {
                tongLuong = Convert.ToInt32(result);
            }
            return tongLuong;
        }


        #endregion

        #region Sự kiện
        private void fThongKeNV_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            dataGridView.CellFormatting += dgvDSNhanVien_CellFormatting;
            int soLuong = DemSoLuongNhanVien();
            lbSoLuong.Text = soLuong.ToString();
            int tongLuong = TinhTongLuong();
            lbLuong.Text = tongLuong.ToString()+" đồng";
        }
        private void btnTim_Click(object sender, EventArgs e)
        {
            LayDuLieu(txtMaNV.Text);
        }

        private void txtMaNV_TextChanged(object sender, EventArgs e)
        {
           LayDuLieu(txtMaNV.Text);
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

        private void btnTaiLai_Click_1(object sender, EventArgs e)
        {
            fThongKeNV_Load(sender, e);
        }
        #endregion

       
    }
}