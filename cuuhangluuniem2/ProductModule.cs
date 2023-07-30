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
    public partial class ProductModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        Product product;
        public ProductModule(Product pd)
        {
            cn = new SqlConnection(dBConnect.myConnect());
            InitializeComponent();
            product = pd;
            LoadBrand();
            LoadCategory();
        }

        public void LoadCategory()
        {
            cbCategory.Items.Clear();
            cbCategory.DataSource = dBConnect.getTable("select * from tbCategory");
            cbCategory.DisplayMember = "LoaiSanPham";
            cbCategory.ValueMember = "id";
        }

        public void LoadBrand()
        {
            cbBrand.Items.Clear();
            cbBrand.DataSource = dBConnect.getTable("select * from tbBrand");
            cbBrand.DisplayMember = "HangSanXuat";
            cbBrand.ValueMember = "id";
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void Clear()
        {
            txtPcode.Clear();
            txtBarcode.Clear();
            txtPdesc.Clear();
            txtPrice.Clear();
            cbBrand.SelectedIndex = 0;
            cbCategory.SelectedIndex = 0;
            udreorder.Value = 1;

            txtPcode.Enabled = true;
            txtPcode.Focus();
            btSave.Enabled = true;
            btUpdate.Enabled = false;

        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Bạn có muốn lưu sản phẩm này không?","Lưu Sản Phẩm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm=new SqlCommand("insert into tbProduct1(pcode,barcode,pdesc,bid,cid,price,reorder) values (@pcode,@barcode,@pdesc,@bid,@cid,@price,@reorder)",cn);
                    cm.Parameters.AddWithValue("@pcode",txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", cbBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@cid", cbCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", udreorder.Value);
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Sản phẩm đã được lưu thành công","Thông Báo");
                    Clear();
                    product.LoadProduct();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Bạn có muốn cập nhật sản phẩm này không?", "Cập nhật Sản Phẩm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("update tbProduct1 set barcode=@barcode,pdesc=@pdesc,bid=@bid,cid=@cid,price=@price,reorder=@reorder where pcode like @pcode",cn);
                    cm.Parameters.AddWithValue("@pcode", txtPcode.Text);
                    cm.Parameters.AddWithValue("@barcode", txtBarcode.Text);
                    cm.Parameters.AddWithValue("@pdesc", txtPdesc.Text);
                    cm.Parameters.AddWithValue("@bid", cbBrand.SelectedValue);
                    cm.Parameters.AddWithValue("@cid", cbCategory.SelectedValue);
                    cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
                    cm.Parameters.AddWithValue("@reorder", udreorder.Value);
                    cn.Open();
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Sản phẩm đã được cập nhật thành công", "Thông Báo");
                    Clear();
                    this.Dispose();

                }
            }
            catch (Exception ex ) 
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
    }
}
