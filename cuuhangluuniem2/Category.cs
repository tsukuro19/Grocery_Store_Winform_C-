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
    public partial class Category : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public Category()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadCategory();
        }
        //Data retrieve from table Brand in SQL to dgbCategory on Category form
        public void LoadCategory()
        {
            int i = 0;
            dtgvCategory.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from tbCategory", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvCategory.Rows.Add(i, dr["id"].ToString(), dr["LoaiSanPham"].ToString());
            }
            dr.Close();
            cn.Close();
        }
        private void dtgvBrand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //For update and delete category by cell click from tbCategory
            string colName = dtgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi này không?", "Xóa Bản Ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tbCategory where id like '" + dtgvCategory[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Loại sản phẩm đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colName == "Edit")
            {
                Categarymodule categarymodule = new Categarymodule(this);
                categarymodule.lbID.Text = dtgvCategory[1, e.RowIndex].Value.ToString();
                categarymodule.txtCategory.Text = dtgvCategory[2, e.RowIndex].Value.ToString();
                categarymodule.btSave.Enabled = false;
                categarymodule.btUpdate.Enabled = true;
                categarymodule.ShowDialog();
            }
            LoadCategory();
        }

        private void btAdd_Category_Click(object sender, EventArgs e)
        {
            Categarymodule moduleForm = new Categarymodule(this);
            moduleForm.ShowDialog();
        }
    }
}
