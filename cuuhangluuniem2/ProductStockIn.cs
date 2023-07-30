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
    public partial class ProductStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        stock stockIn;
        public ProductStockIn(stock stk)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadProduct();
            stockIn= stk;
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadProduct()
        {
            int i = 0;
            dtgvProduct.Rows.Clear();
            cm = new SqlCommand("select pcode, pdesc, qty from tbProduct1 where pdesc like '%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void dtgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvProduct.Columns[e.ColumnIndex].Name;
            if (colname == "Select")
            {
                if (stockIn.txtStockInBy.Text== String.Empty)
                {
                    MessageBox.Show("Bạn chưa nhập tên người nhập hàng?","Thông Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    stockIn.txtStockInBy.Focus();
                    this.Dispose();
                }
                if(MessageBox.Show("Bạn muốn thêm sản phẩm?","Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        cn.Open();
                        cm = new SqlCommand("insert into tbStockIn (refno,pcode,sdate,stockinby,supplierid) values (@refno,@pcode,@sdate,@stockinby,@supplierid)",cn);
                        cm.Parameters.AddWithValue("@refno",stockIn.txtRefNo.Text);
                        cm.Parameters.AddWithValue("@pcode", dtgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString());
                        cm.Parameters.AddWithValue("@sdate", stockIn.dtStockIn.Value);
                        cm.Parameters.AddWithValue("@stockinby", stockIn.txtStockInBy.Text);
                        cm.Parameters.AddWithValue("@supplierid", stockIn.lbid.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        stockIn.LoadStocIn();
                        MessageBox.Show("Đã thêm sản phẩm thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Thông Báo");
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
