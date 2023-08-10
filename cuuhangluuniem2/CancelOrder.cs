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
    public partial class CancelOrder : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;

        DailySale dailySale;
        public CancelOrder(DailySale sale)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            dailySale = sale;
        }

        private void btCOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReason.Text == "")
                {
                    MessageBox.Show("Vui lòng nhập lý do","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                if(cbInventory.Text!=string.Empty && udCancelQty.Value>0 && txtReason.Text != string.Empty) 
                {
                    if(int.Parse(txtQty.Text)>=udCancelQty.Value)
                    {
                        Void @void = new Void(this);
                        @void.txtName.Focus();
                        @void.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        public void ReloadSoldList()
        {
            dailySale.LoadSold();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void cbInventory_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
