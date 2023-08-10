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
    public partial class Categarymodule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        Category category;
        public Categarymodule(Category ct)
        {
            cn = new SqlConnection(dBConnect.myConnect());
            InitializeComponent();
            category = ct;
        }

        public void Clear()
        {
            txtCategory.Clear();
            txtCategory.Focus();
            btSave.Enabled = true;
            btUpdate.Enabled = false;
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            //To insert category name to category table
            try
            {
                if (MessageBox.Show("Bạn có chắc muốn lưu loại sản phẩm này không?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("insert into tbCategory(LoaiSanPham)values(@loaisanpham)", cn);
                    cm.Parameters.AddWithValue("@loaisanpham", txtCategory.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Bản ghi đã được lưu lại", "Thông Báo");
                    Clear();
                }
                category.LoadCategory();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy không?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                this.Dispose();
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            //Update category name
            if (MessageBox.Show("Bạn có chắc muốn cập nhật bản ghi này không?", "Cập Nhật Bản Ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("update tbCategory set LoaiSanPham=@loaisanpham where id like '" + lbID.Text + "'", cn);
                cm.Parameters.AddWithValue("@loaisanpham", txtCategory.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Loại sản phẩm đã được cập nhật", "Thông Báo");
                Clear();
                this.Dispose();//to close this form after update data
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
