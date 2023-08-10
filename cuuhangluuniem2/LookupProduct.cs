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
    public partial class LookupProduct : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        Cashier cashier;
        public LookupProduct(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            cashier = cash;
            LoadProduct();
        }

        public void LoadProduct()
        {
            int i = 0;
            dtgvProduct.Rows.Clear();
            cm = new SqlCommand("select p.pcode, p.barcode, p.pdesc,b.HangSanXuat,c.LoaiSanPham,p.price,p.qty from tbProduct1 as p inner join tbBrand as b on b.id=p.bid inner join tbCategory as c on c.id = p.cid where concat(p.pdesc,b.HangSanXuat,c.LoaiSanPham) like '%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dtgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvProduct.Columns[e.ColumnIndex].Name;
            if (colname == "Select")
            {
                Qty qty = new Qty(cashier);
                qty.ProductDetails(dtgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString(), double.Parse(dtgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString()), cashier.lbTranNo.Text, int.Parse(dtgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString()));
                qty.ShowDialog();
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        private void dtgvProduct_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
            {
                this.Dispose();
            }
        }
    }
}
