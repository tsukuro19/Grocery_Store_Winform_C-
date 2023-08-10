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
    public partial class Void : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        CancelOrder cancelOrder;
        public Void(CancelOrder cancel)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            txtName.Focus();
            cancelOrder = cancel;
        }

        private void btVoid_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text== cancelOrder.txtCancelBy.Text) 
                { 
                    MessageBox.Show("Username của người hủy phải trùng nhau","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                string username;
                cn.Open();
                cm = new SqlCommand("select * from tbUser where username=@username and password=@password", cn);
                cm.Parameters.AddWithValue("@username", txtName.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                dr = cm.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    username = dr["username"].ToString();
                    dr.Close();
                    cn.Close();
                    SaveCancelOrder(username);
                    if(cancelOrder.cbInventory.Text == "YES")
                    {
                        dBConnect.ExecuteQuerry("update tbInventory set qty = qty +" + cancelOrder.udCancelQty.Value + " where pcode = '" + cancelOrder.txtPcode.Text + "'");
                    }
                    dBConnect.ExecuteQuerry("delete tbCart where id like '" + cancelOrder.txtID.Text + "'");
                    MessageBox.Show("Hủy đơn hàng thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                    cancelOrder.ReloadSoldList();
                    cancelOrder.Dispose();
                }
                dr.Close();
                cn.Close() ;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Warning",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void SaveCancelOrder(string username)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("insert into tbCancel (transno, pcode, price, qty, total, sdate, voidby, cancelledby, reason, action) values (@transno, @pcode, @price, @qty, @total,@sdate, @voidby, @cancelledby, @reason, @action)",cn);
                cm.Parameters.AddWithValue("@transno",cancelOrder.txtTransno.Text);
                cm.Parameters.AddWithValue("@pcode", cancelOrder.txtPcode.Text);
                cm.Parameters.AddWithValue("@price", double.Parse(cancelOrder.txtPrice.Text));
                cm.Parameters.AddWithValue("@total", double.Parse(cancelOrder.txtTotal.Text));
                cm.Parameters.AddWithValue("@qty", int.Parse(cancelOrder.txtQty.Text));
                cm.Parameters.AddWithValue("@sdate", DateTime.Now);
                cm.Parameters.AddWithValue("@voidby", username);
                cm.Parameters.AddWithValue("@cancelledby", cancelOrder.txtCancelBy.Text);
                cm.Parameters.AddWithValue("@reason", cancelOrder.txtReason.Text);
                cm.Parameters.AddWithValue("@action", cancelOrder.cbInventory.Text);
                cm.ExecuteNonQuery();
                cn.Close() ;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
