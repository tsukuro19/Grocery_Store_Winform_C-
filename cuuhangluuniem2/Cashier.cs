using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuuhangluuniem2
{
    public partial class Cashier : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;

        int qty;
        string id;
        string price;
        public Cashier()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            getTransNo();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn muốn thoát phần mềm?","Xác Nhận",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        public void slide(Button btn)
        {
            panelSlide.BackColor=Color.White;
            panelSlide.Height=btn.Height;
            panelSlide.Top=btn.Top;
        }
        #region button_effect
        private void btNTran_Click(object sender, EventArgs e)
        {
            slide(btNTran);
            getTransNo();
        }

        private void btSearch_Click(object sender, EventArgs e)
        {
            slide(btSearch);
            LookupProduct lookup = new LookupProduct(this);
            lookup.LoadProduct();
            lookup.ShowDialog();
        }

        private void btDIscount_Click(object sender, EventArgs e)
        {
            slide(btDIscount);
            Discount discount = new Discount(this);
            discount.lbid.Text = id;
            discount.txtTotal.Text = price;
            discount.ShowDialog();
        }

        private void btSettle_Click(object sender, EventArgs e)
        {
            slide(btSettle);
            Settle settle = new Settle(this);
            settle.txtSale.Text = lbVatable.Text;
            settle.ShowDialog();
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            slide(btClear);
            if(MessageBox.Show("Xóa tất cả giao dịch trong giỏ hàng?","Xác Nhận",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                cn.Open();
                cm = new SqlCommand("delete from tbCart where transno like '"+lbTranNo.Text+"'",cn);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Tất cả giao dịch đã được xóa", "Xóa Giao Dịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCart();
            }
        }

        private void btDSales_Click(object sender, EventArgs e)
        {
            slide(btDSales);
            DailySale dailySale = new DailySale();
            dailySale.solduser=lbName.Text;
            dailySale.dtFrom.Enabled = false;
            dailySale.dtTo.Enabled = false;
            dailySale.cbCashier.Enabled = false;
            dailySale.cbCashier.Text=lbUserName.Text;
            dailySale.ShowDialog();
        }

        private void btPass_Click(object sender, EventArgs e)
        {
            slide(btPass);
            ChangePassword change = new ChangePassword(this);
            change.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            slide(btnLogout);
            if(dtgvCashier.Rows.Count > 0)
            {
                MessageBox.Show("Vui lòng xử lý tất cả đơn hàng", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn muốn đăng xuất tài khoản", "Đăng Xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }
        #endregion button_effect
        
        public void LoadCart()
        {
            Boolean hascart=false;
            try
            {
                int i = 0;
                double total = 0;
                double discount = 0;
                dtgvCashier.Rows.Clear();
                cn.Open();
                cm = new SqlCommand("select c.id, c.pcode, p.pdesc, c.price,c.qty, c.disc, c.total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where c.transno like @transno and c.status like 'Chua Giai Quyet'", cn);
                cm.Parameters.AddWithValue("@transno", lbTranNo.Text);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    total += Convert.ToDouble(dr["total"].ToString());
                    discount += Convert.ToDouble(dr["disc"].ToString());
                    dtgvCashier.Rows.Add(i, dr["id"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));//
                    hascart = true;
                }
                dr.Close();
                cn.Close();
                lbSaleTotal.Text = total.ToString("#,##0.00");
                lbDiscount.Text = discount.ToString("#,##0.00");
                GetCartTotal();
                if (hascart) {btClear.Enabled = true;btSettle.Enabled = true;btDIscount.Enabled= true;}
                else { btClear.Enabled = false;btSettle.Enabled = false; btDIscount.Enabled = false;}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông Báo");
            }
        }

        public void GetCartTotal()
        {
            double discount=double.Parse(lbDiscount.Text);
            double sales=double.Parse(lbSaleTotal.Text)-discount;
            double vat = sales * 0.12;
            double vatable = sales + vat;

            lbTax.Text = vat.ToString("#,##0.00");
            lbVatable.Text = vatable.ToString("#,##0.00");
            lbDisplayTotal.Text = sales.ToString("#,##0.00");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTimer.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        public void getTransNo()
        {
            try
            {
                string sdate = DateTime.Now.ToString("yyyyMMdd");
                string sdate_real = DateTime.Now.ToString("ddMMyyyy");
                int count;
                string transno;
                cn.Open();
                cm = new SqlCommand("select top 1 transno from tbCart where transno like '"+sdate+"%' order by id desc",cn);
                dr=cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows) 
                {
                    transno = dr[0].ToString();
                    count=int.Parse(transno.Substring(8,4));
                    lbTranNo.Text = sdate + (count+1);
                    lbDate.Text = sdate_real;
                }
                else
                {
                    transno = sdate + "1001";
                    lbTranNo.Text= transno;
                    lbDate.Text = sdate_real;
                }
                dr.Close();
                cn.Close();
            }
            catch(Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Thông Báo");
            }
        }

        private void txtBarcode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtBarcode.Text == String.Empty)
                {
                    return;
                }
                else
                {
                    string _pcode;
                    double _price;
                    int _qty;
                    cn.Open();
                    cm = new SqlCommand("select * from tbProduct1 where barcode like '"+txtBarcode.Text+"'",cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if(dr.HasRows)
                    {
                        qty = int.Parse(dr["qty"].ToString());
                        _pcode = dr["pcode"].ToString();
                        _price = double.Parse(dr["price"].ToString());
                        _qty=int.Parse(txtQty.Text);
                        dr.Close();
                        cn.Close();
                        //insert tbCart
                        Add_To_Cart(_pcode, _price, _qty);
                    }
                    dr.Close() ;
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void Add_To_Cart(string _pcode,double _price,int _qty)
        {
            try
            {
                string id = "";
                int cart_qty = 0;
                bool found=false;
                cn.Open();
                cm = new SqlCommand("select * from tbCart where transno = @transno and pcode=@pcode",cn);
                cm.Parameters.AddWithValue("@transno",lbTranNo.Text);
                cm.Parameters.AddWithValue("@pcode", _pcode);
                dr = cm.ExecuteReader();
                dr.Read() ;
                if(dr.HasRows)
                {
                    id = dr["id"].ToString();
                    cart_qty = int.Parse(dr["qty"].ToString());
                    found = true;
                }else found = false;
                dr.Close();
                cn.Close();

                if (found)
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Không thể tiến hành số lượng còn lại hiện tại là " + qty, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    cn.Open();
                    cm = new SqlCommand("update tbCart set qty = (qty + " + _qty + ") where id like '" + id + "'", cn);
                    cm.ExecuteReader();
                    cn.Close();
                    txtBarcode.SelectionStart= 0;
                    txtBarcode.SelectionLength= txtBarcode.Text.Length;
                    LoadCart();
                }
                else
                {
                    if (qty < (int.Parse(txtQty.Text) + cart_qty))
                    {
                        MessageBox.Show("Không thể tiến hành số lượng còn lại hiện tại là " + qty, "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    cn.Open();
                    cm = new SqlCommand("insert into tbCart(transno,pcode,price,qty,sdate,cashier) values (@transno,@pcode,@price,@qty,@sdate,@cashier)", cn);
                    cm.Parameters.AddWithValue("@transno", lbTranNo.Text);
                    cm.Parameters.AddWithValue("@pcode", _pcode);
                    cm.Parameters.AddWithValue("@price", _price);
                    cm.Parameters.AddWithValue("@qty", _qty);
                    cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                    cm.Parameters.AddWithValue("@cashier", lbUserName.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    LoadCart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Thông Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dtgvCashier_SelectionChanged(object sender, EventArgs e)
        {
            int i = dtgvCashier.CurrentRow.Index;
            id = dtgvCashier[1,i].Value.ToString();
            price = dtgvCashier[7,i].Value.ToString();
        }

        private void dtgvCashier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvCashier.Columns[e.ColumnIndex].Name;
            if (colname == "Delete")
            {
                if (MessageBox.Show("Bạn muốn xóa giao dịch này?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dBConnect.ExecuteQuerry("delete from tbCart where id like '" + dtgvCashier.Rows[e.RowIndex].Cells[1].Value.ToString() + "'");
                    MessageBox.Show("Giao dịch này đã được xóa", "Xóa Giao Dịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCart();
                }
            }
            else if(colname == "colAdd")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select sum(qty) as qty from tbProduct1 where pcode like '" + dtgvCashier.Rows[e.RowIndex].Cells[2].Value.ToString() + "' group by pcode", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();

                if (int.Parse(dtgvCashier.Rows[e.RowIndex].Cells[5].Value.ToString())<i) 
                {
                    dBConnect.ExecuteQuerry("update tbCart set qty = qty +" + int.Parse(txtQty.Text) + "where transno like '" + lbTranNo.Text + "' and pcode like '"+ dtgvCashier.Rows[e.RowIndex].Cells[2].Value.ToString() +"'");
                    LoadCart() ;
                }
                else
                {
                    MessageBox.Show("Số lượng sản phẩm còn lại trong kho là "+i,"Cảnh Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if(colname == "colReduce")
            {
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select sum(qty) as qty from tbCart where pcode like '" + dtgvCashier.Rows[e.RowIndex].Cells[2].Value.ToString() + "' group by pcode", cn);
                i = int.Parse(cm.ExecuteScalar().ToString());
                cn.Close();
                if (i > 1)
                {
                    dBConnect.ExecuteQuerry("update tbCart set qty = qty -" + int.Parse(txtQty.Text) + "where transno like '" + lbTranNo.Text + "' and pcode like '" + dtgvCashier.Rows[e.RowIndex].Cells[2].Value.ToString() + "'");
                    LoadCart();
                }
                else
                {
                    MessageBox.Show("Số lượng sản phẩm của giao dịch phải lớn hơn hoặc bằng 1", "Cảnh Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void Cashier_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the user is closing the form using the Alt+F4 key combination
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                // Perform any necessary cleanup or save operations
                // ...

                // Exit the application
                if (MessageBox.Show("Bạn muốn thoát phần mềm?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }
    }
}
