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
    public partial class stock : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public stock()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadSupplier();
            GetRefNo();
        }

        public void GetRefNo()
        {
            Random rd= new Random();
            txtRefNo.Clear();
            txtRefNo.Text += rd.Next();
        }

        public void LoadSupplier()
        {
            cbSupplier.Items.Clear();
            cbSupplier.DataSource = dBConnect.getTable("select * from tbSupplier");
            cbSupplier.DisplayMember = "supplier";
        }

        private void cbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            cn.Open();
            cm=new SqlCommand("select * from tbSupplier where supplier like '"+cbSupplier.Text+"'",cn);
            dr= cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                lbid.Text = dr["id"].ToString();
                txtContact.Text = dr["contactperson"].ToString();
                txtAddress.Text = dr["address"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void cbSupplier_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void linkGenerate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GetRefNo();
        }

        public void LoadStocIn()
        {
            int i = 0;
            dtgvStockin.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockIn where refno like '"+txtRefNo.Text+"' and status like 'Chua Giai Quyet'",cn);
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dtgvStockin.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString());
            }
            dr.Close (); 
            cn.Close();
        }

        private void linkProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProductStockIn productStock = new ProductStockIn(this);
            productStock.ShowDialog();
        }

        private void btEntry_Click(object sender, EventArgs e)
        {
            try
            {
                if (dtgvStockin.Rows.Count > 0)
                {
                    if (MessageBox.Show("Bạn có muốn lưu bản ghi này không?","Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = 0;i<dtgvStockin.Rows.Count;i++)
                        {
                            //update product quantity
                            cn.Open();
                            cm=new SqlCommand("update tbProduct1 set qty = qty + "+ int.Parse(dtgvStockin.Rows[i].Cells[5].Value.ToString())+", reorder=reorder+1 where pcode like '" + dtgvStockin.Rows[i].Cells[3].Value.ToString()+"'",cn);
                            cm.ExecuteNonQuery();
                            cn.Close();

                            //update stockin quantity
                            cn.Open ();
                            cm = new SqlCommand("update tbStockIn set qty= qty +"+ int.Parse(dtgvStockin.Rows[i].Cells[5].Value.ToString()) +",status='Hoan Thanh' where id like '" + dtgvStockin.Rows[i].Cells[1].Value.ToString()+"'",cn);
                            cm.ExecuteNonQuery();
                            cn.Close();
                        }
                        Clear();
                        LoadStocIn();
                    }
                }

            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message,"Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        public void Clear()
        {
            txtRefNo.Clear();
            txtStockInBy.Clear();
            dtStockIn.Value=DateTime.Now;
        }

        private void dtgvStockin_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvStockin.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {
                if (MessageBox.Show("Bạn muốn xóa sản phẩm được chọn?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tbStockIn where id= '" + dtgvStockin.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery(); 
                    cn.Close();
                    MessageBox.Show("Sản phẩm đã được xóa","Thông Báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadStocIn() ;
                }
            }
        }

        private void btLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                dtgvStockHistory.Rows.Clear();
                cn.Open();
                cm = new SqlCommand("select * from vwStockIn where cast(sdate as date) between '"+dtFrom.Value.ToString("yyyy-MM-dd") + "' and '"+dtTo.Value.ToString("yyyy-MM-dd") + "' and status like 'Hoan Thanh'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dtgvStockHistory.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), DateTime.Parse(dr[5].ToString()).ToShortDateString(), dr[6].ToString(), dr[8].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
