using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuuhangluuniem2
{
    public partial class Adjustment : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        Form1 main;
        int _qty;
        public Adjustment(Form1 mainform)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            ReferenceNo();
            LoadStock();
            main = mainform;
            lbUsername.Text=main.lbUsername.Text;
        }

        public void ReferenceNo()
        {
            Random rnd= new Random();
            lbRefno.Text=rnd.Next().ToString();
        }

        public void LoadStock()
        {
            int i = 0;
            dtgvAdjustment.Rows.Clear();
            cm = new SqlCommand("select p.pcode, p.barcode, p.pdesc,b.HangSanXuat,c.LoaiSanPham,p.price,p.qty from tbProduct1 as p inner join tbBrand as b on b.id=p.bid inner join tbCategory as c on c.id = p.cid where concat(p.pdesc,b.HangSanXuat,c.LoaiSanPham) like '%" + txtSearch.Text + "%'", cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvAdjustment.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());

            }
            dr.Close();
            cn.Close();
        }

        private void dtgvAdjustment_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvAdjustment.Columns[e.ColumnIndex].Name;
            lbPcode.Text = dtgvAdjustment.Rows[e.RowIndex].Cells[1].Value.ToString();
            lbProduct_Name.Text = dtgvAdjustment.Rows[e.RowIndex].Cells[3].Value.ToString() ;
            _qty = int.Parse(dtgvAdjustment.Rows[e.RowIndex].Cells[7].Value.ToString() + " ");
            btSave.Enabled = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadStock();
        }

        public void Clear()
        {
            lbProduct_Name.Text = "";
            lbPcode.Text = "";
            txtQty.Clear();
            txtRemark.Clear();
            cbAction.Text = "";
            ReferenceNo();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                //validation for empty field
                if (cbAction.Text == "")
                {
                    MessageBox.Show("Vui lòng chọn hành động của bạn", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (txtQty.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập số lượng", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQty.Focus();
                    return;
                }

                if (txtRemark.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập lý do", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtRemark.Focus();
                    return;
                }

                //update stock
                
                if(cbAction.Text== "Xóa khỏi hàng tồn kho")
                {
                    if (int.Parse(txtQty.Text) > _qty)
                    {
                        MessageBox.Show("Số lượng hàng điều chỉnh phải nhỏ hơn hoặc bằng số lượng trong kho", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        dBConnect.ExecuteQuerry("update tbProduct1 set qty = (qty -" + int.Parse(txtQty.Text) + ") where pcode like '" + lbPcode.Text + "'");
                    }
                }
                else if(cbAction.Text== "Thêm vào hàng tồn kho")
                {
                    dBConnect.ExecuteQuerry("update tbProduct1 set qty = (qty -" + int.Parse(txtQty.Text) + ") where pcode like '" + lbPcode.Text + "'");
                    dBConnect.ExecuteQuerry("update tbInventory set qty = (qty +" + int.Parse(txtQty.Text) + ") where pcode like '" + lbPcode.Text + "'");
                }

                dBConnect.ExecuteQuerry("insert into tbAdjustment(referenceno, pcode, qty, action, remark, sdate, [user]) values ('"+lbRefno.Text+"','"+lbPcode.Text+"','"+int.Parse(txtQty.Text)+"','"+cbAction.Text+"','"+txtRemark.Text+"','"+DateTime.Now.ToString("yyyyMMdd")+"','"+lbUsername+"')");
                MessageBox.Show("Kho đã được điều chỉnh thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadStock();
                Clear();
                btSave.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
