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

namespace cuuhangluuniem2
{
    public partial class Brandmodule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        Brand brand;
        public Brandmodule(Brand br)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            brand = br;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            //To insert brand name to brand table
            try
            {
                if(MessageBox.Show("Bạn có chắc muốn lưu hãng sản xuất này không?", "",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm=new SqlCommand("insert into tbBrand(HangSanXuat)values(@hangsanxuat)",cn);
                    cm.Parameters.AddWithValue("@hangsanxuat", txtBrand.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Bản ghi đã được lưu lại", "Thông Báo");
                    Clear();
                    brand.LoadBrand();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            //Update brand name
            if (MessageBox.Show("Bạn có chắc muốn cập nhật bản ghi này không?", "Cập Nhật Bản Ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("update tbBrand set HangSanXuat=@hangsanxuat where id like '"+lbID.Text+"'",cn);
                cm.Parameters.AddWithValue("@hangsanxuat", txtBrand.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Hãng sản xuất đã được cập nhật", "Thông Báo");
                Clear();
                this.Dispose();//to close this form after update data
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy không?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                this.Dispose();
            }
        }

        public void Clear()
        {
            txtBrand.Clear();
            btUpdate.Enabled = false;
            btSave.Enabled = true;
            txtBrand.Focus();
        }
    }
}
