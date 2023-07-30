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
    public partial class Brand : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public Brand()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadBrand();
        }

        //Data retrieve from table Brand in SQL to dgbBrand on Brand form
        public void LoadBrand()
        {
            int i = 0;
            dtgvBrand.Rows.Clear();
            cn.Open();
            cm= new SqlCommand("select * from tbBrand",cn);
            dr=cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dtgvBrand.Rows.Add(i, dr["id"].ToString(), dr["HangSanXuat"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //For update and delete brand by cell click from tbBrand
            string colName = dtgvBrand.Columns[e.ColumnIndex].Name;
            if (colName == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi này không?", "Xóa Bản Ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tbBrand where id like '" + dtgvBrand[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Hãng sản xuất đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colName == "Edit")
            {
                Brandmodule brandmodule = new Brandmodule(this);
                brandmodule.lbID.Text = dtgvBrand[1, e.RowIndex].Value.ToString();
                brandmodule.txtBrand.Text = dtgvBrand[2, e.RowIndex].Value.ToString();
                brandmodule.btSave.Enabled = false;
                brandmodule.btUpdate.Enabled = true;
                brandmodule.ShowDialog();
            }
            LoadBrand();
        }

        private void btAdd_Click(object sender, EventArgs e)
        {
            Brandmodule moduleForm = new Brandmodule(this);
            moduleForm.ShowDialog();
        }

    }
}
