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
    public partial class Discount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        Cashier cashier;
        public Discount(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            cashier = cash;
            txtDiscount.Focus();
            this.KeyPreview = true;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void Discount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Dispose();
            else if (e.KeyCode == Keys.Enter) btConfirm.PerformClick();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double disc=double.Parse(txtTotal.Text)*double.Parse(txtDiscount.Text)*0.01;
                txtDisAmount.Text=disc.ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                txtDisAmount.Text = "0.00";
            }
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("update tbCart set disc_percent=@disc_percent where id=@id",cn);
                cm.Parameters.AddWithValue("@disc_percent", double.Parse(txtDiscount.Text));
                cm.Parameters.AddWithValue("@id", int.Parse(lbid.Text));
                cm.ExecuteNonQuery();
                cn.Close();
                cashier.LoadCart();
                this.Dispose();
            }
            catch (Exception ex)
            {
                cn.Close() ;
                MessageBox.Show(ex.Message, "Thông Báo");
            }

        }

       
    }
}
