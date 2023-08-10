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
    public partial class Product : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public Product()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadProduct();
        }

        public void LoadProduct()
        {
            int i = 0;
            dtgvProduct.Rows.Clear();
            cm=new SqlCommand("select p.pcode, p.barcode, p.pdesc,b.HangSanXuat,c.LoaiSanPham,p.price,p.reorder from tbProduct1 as p inner join tbBrand as b on b.id=p.bid inner join tbCategory as c on c.id = p.cid where concat(p.pdesc,b.HangSanXuat,c.LoaiSanPham) like '%" + txtSearch.Text+"%'",cn);
            cn.Open();
            dr=cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dtgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void btAdd_Brand_Click(object sender, EventArgs e)
        {
            ProductModule productModule = new ProductModule(this);
            productModule.udreorder.Enabled = false;
            productModule.ShowDialog();
        }


        

        private void btAdd_Brand_Click_1(object sender, EventArgs e)
        {
            ProductModule productModule = new ProductModule(this);
            productModule.udreorder.Enabled = false;
            productModule.ShowDialog();
        }
        //Configure edit and delete record in database
        private void dtgvProduct_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvProduct.Columns[e.ColumnIndex].Name;
            if (colname == "Edit")
            {
                ProductModule product = new ProductModule(this);
                product.txtPcode.Text = dtgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
                product.txtBarcode.Text = dtgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
                product.txtPdesc.Text = dtgvProduct.Rows[e.RowIndex].Cells[3].Value.ToString();
                product.cbBrand.Text = dtgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();
                product.cbCategory.Text = dtgvProduct.Rows[e.RowIndex].Cells[5].Value.ToString();
                product.txtPrice.Text = dtgvProduct.Rows[e.RowIndex].Cells[6].Value.ToString();
                product.udreorder.Value = int.Parse(dtgvProduct.Rows[e.RowIndex].Cells[7].Value.ToString());

                product.txtPcode.Enabled = false;
                product.btSave.Enabled = false;
                product.btUpdate.Enabled = true;
                product.udreorder.Enabled = true;
                product.ShowDialog();
            }
            else if (colname == "Delete")
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa bản ghi này không?", "Xóa Bản Ghi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("delete from tbProduct1 where pcode like '" + dtgvProduct[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("delete from tbInventory where pcode like '" + dtgvProduct[1, e.RowIndex].Value.ToString() + "'", cn);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Sản phẩm đã được xóa khỏi danh sách", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            LoadProduct();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            LoadProduct();
        }
    }
}
