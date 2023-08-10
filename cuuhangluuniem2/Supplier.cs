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
    public partial class Supplier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public Supplier()
        {
            cn = new SqlConnection(dBConnect.myConnect());
            InitializeComponent();
            LoadSupplier();
        }

        public void LoadSupplier()
        {
            dtgvSuplier.Rows.Clear();
            int i = 0;
            cn.Open();
            cm=new SqlCommand("select * from tbSupplier",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvSuplier.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btAdd_Brand_Click(object sender, EventArgs e)
        {
            SuplierModule suplierModule = new SuplierModule(this);
            suplierModule.ShowDialog();
        }

        private void dtgvSuplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvSuplier.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {
                if(MessageBox.Show("Bạn có chắc muốn xóa bản ghi này không?","Xóa Bản Ghi",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    //delete where id in database
                    cm = new SqlCommand("delete from tbSupplier where id like '" + dtgvSuplier.Rows[e.RowIndex].Cells[1].Value.ToString()+"'",cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Nhà cung cấp đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (colname == "Edit")
            {
                SuplierModule suplier = new SuplierModule(this);
                suplier.lbID.Text = dtgvSuplier.Rows[e.RowIndex].Cells[1].Value.ToString();
                suplier.txtSupplier.Text = dtgvSuplier.Rows[e.RowIndex].Cells[2].Value.ToString();
                suplier.txtAddress.Text = dtgvSuplier.Rows[e.RowIndex].Cells[3].Value.ToString();
                suplier.txtContact_person.Text = dtgvSuplier.Rows[e.RowIndex].Cells[4].Value.ToString();
                suplier.txtPhone.Text = dtgvSuplier.Rows[e.RowIndex].Cells[5].Value.ToString();
                suplier.txtEmail.Text = dtgvSuplier.Rows[e.RowIndex].Cells[6].Value.ToString();

                suplier.btSave.Enabled = false;
                suplier.btUpdate.Enabled = true;
                suplier.ShowDialog();
            }
            LoadSupplier();
        }
    }
}
