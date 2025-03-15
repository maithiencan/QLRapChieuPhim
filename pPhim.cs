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
    
    public partial class pPhim : Form
    {
        #region Biến toàn cục
        MyDataTable dataTabledgv = new MyDataTable();
        MyDataTable dataTablecbodp = new MyDataTable();
        MyDataTable dataTablecbotlp = new MyDataTable();
        string maPhim = "";

        #endregion

        public pPhim()
        {
            InitializeComponent();
            dataGridView.AutoGenerateColumns = false;
            dataTabledgv.OpenConnection();
            dataTablecbodp.OpenConnection();
            dataTablecbotlp.OpenConnection();
        }

       

        #region Functions 
        private void LayDuLieu()
        {   //DataGridview
            SqlCommand cmd = new SqlCommand(@"SELECT P.*, D.DANGPHIM, T.LOAIPHIM
                                              FROM PHIM P, DINHDANGPHIM D, THELOAIPHIM T
                                              WHERE P.MALP = T.MALP AND P.MADP = D.MADP");
            dataTabledgv.Fill(cmd);
            BindingSource bidingDGV = new BindingSource();
            bidingDGV.DataSource = dataTabledgv;
            dataGridView.DataSource = bidingDGV;

            //cboDangPhim
            SqlCommand cmddp = new SqlCommand(@"SELECT *  FROM DINHDANGPHIM");
            dataTablecbodp.Fill(cmddp);
            BindingSource bidingcboDP = new BindingSource();
            bidingcboDP.DataSource = dataTablecbodp;
            cboDangPhim.DataSource = bidingcboDP;
            cboDangPhim.DisplayMember = @"DANGPHIM";
            cboDangPhim.ValueMember = @"MADP";

            //cbotheloaiphim
            SqlCommand cmdlp = new SqlCommand(@"SELECT *  FROM THELOAIPHIM");
            dataTablecbotlp.Fill(cmdlp);
            BindingSource bidingcboLP = new BindingSource();
            bidingcboLP.DataSource = dataTablecbotlp;
            cboLoaiPhim.DataSource = bidingcboLP;
            cboLoaiPhim.DisplayMember = @"LOAIPHIM";
            cboLoaiPhim.ValueMember = @"MALP";

            txtMaPhim.DataBindings.Clear();
            cboDangPhim.DataBindings.Clear();
            cboLoaiPhim.DataBindings.Clear();
            txtTenPhim.DataBindings.Clear();
            txtNhaSX.DataBindings.Clear();
            txtMaPhim.DataBindings.Add("Text", bidingDGV, @"MAPHIM");
            cboDangPhim.DataBindings.Add("SelectedValue", bidingDGV, @"MADP");
            cboLoaiPhim.DataBindings.Add("SelectedValue", bidingDGV, @"MALP");
            txtTenPhim.DataBindings.Add("Text", bidingDGV, @"TENPHIM");
            txtNhaSX.DataBindings.Add("Text", bidingDGV, @"NHASX");
        }

        private void BatTat(bool giaTri)
        {
            txtMaPhim.Enabled = giaTri;
            cboDangPhim.Enabled = giaTri;
            cboLoaiPhim.Enabled = giaTri;
            txtTenPhim.Enabled = giaTri;
            txtNhaSX.Enabled = giaTri;
            btnLuu.Enabled = giaTri; btnHuyBo.Enabled = giaTri;
            btnThem.Enabled = !giaTri; btnSua.Enabled = !giaTri; btnXoa.Enabled = !giaTri;
        }
        #endregion

        #region Sự kiện
        private void pPhim_Load(object sender, EventArgs e)
        {
            LayDuLieu();
            BatTat(false);
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            maPhim = "";
            txtMaPhim.Clear();
            cboDangPhim.Text = "";
            cboLoaiPhim.Text = "";
            txtTenPhim.Clear();
            txtNhaSX.Clear();
            txtMaPhim.Focus();

            BatTat(true);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            maPhim = txtMaPhim.Text;
            BatTat(true);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult kq;
            kq = MessageBox.Show("Bạn có muốn xóa phim '" + txtTenPhim.Text + "' không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (kq == DialogResult.Yes)
            {
                string sql = @"DELETE FROM PHIM WHERE MAPHIM = @MAPHIM";
                SqlCommand cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@MAPHIM", SqlDbType.NVarChar, 10).Value = txtMaPhim.Text.ToUpper();
                dataTabledgv.Update(cmd);
                pPhim_Load(sender, e);

            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (txtMaPhim.Text.Trim() == "")
                MessageBox.Show("Mã dạng phim không được bỏ trống!");
            else if (txtTenPhim.Text.Trim() == "")
                MessageBox.Show("Tên phim không được bỏ trống!");
            else if (txtNhaSX.Text.Trim() == "")
                MessageBox.Show("Nhà sản xuất phim không được bỏ trống!");
            else if (txtMaPhim.Text.Trim().Length >= 10)
                MessageBox.Show("Mã phim phải nhập đúng 10 ký tự!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                try
                {
                    if (maPhim == "")
                    {
                        string sql = @"INSERT INTO PHIM VALUES(@MAPHIM, @MALP, @MADP, @TENPHIM, @NHASX)";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAPHIM", SqlDbType.NVarChar, 10).Value = txtMaPhim.Text.ToUpper();
                        cmd.Parameters.Add("@MALP", SqlDbType.NVarChar, 10).Value = cboLoaiPhim.SelectedValue.ToString();
                        cmd.Parameters.Add("@MADP", SqlDbType.NVarChar, 10).Value = cboDangPhim.SelectedValue.ToString();
                        cmd.Parameters.Add("@TENPHIM", SqlDbType.NVarChar, 100).Value = txtTenPhim.Text;
                        cmd.Parameters.Add("@NHASX", SqlDbType.NVarChar, 100).Value = txtNhaSX.Text;
                        int result = dataTabledgv.Update(cmd);
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
                        string sql = @"UPDATE       PHIM
                                       SET          MAPHIM = @MAPHIMMOI,
                                                    MADP = @MADP,
                                                    MALP = @MALP,
                                                    TENPHIM = @TENPHIM,
                                                    NHASX = @NHASX
                                        WHERE       MAPHIM =@MAPHIMCU";
                        SqlCommand cmd = new SqlCommand(sql);
                        cmd.Parameters.Add("@MAPHIMMOI", SqlDbType.NVarChar, 10).Value = txtMaPhim.Text.ToUpper();
                        cmd.Parameters.Add("@MAPHIMCU", SqlDbType.NVarChar, 10).Value = txtMaPhim.Text.ToUpper();
                        cmd.Parameters.Add("@MADP", SqlDbType.NVarChar, 10).Value = cboDangPhim.SelectedValue.ToString();
                        cmd.Parameters.Add("@MALP", SqlDbType.NVarChar, 10).Value = cboLoaiPhim.SelectedValue.ToString();
                        cmd.Parameters.Add("@TENPHIM", SqlDbType.NVarChar, 100).Value = txtTenPhim.Text;
                        cmd.Parameters.Add("@NHASX", SqlDbType.NVarChar, 100).Value = txtNhaSX.Text;
                        int result = dataTabledgv.Update(cmd);
                        if (result > 0)
                        {
                            MessageBox.Show("Lưu thành công!", "Thông báo");
                        }
                        else
                        {
                            MessageBox.Show("Lưu thất bại. Vui lòng thử lại!", "Thông báo");
                        }
                    }
                    pPhim_Load(sender, e);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            pPhim_Load(sender, e);
        }
        #endregion
    }
}
