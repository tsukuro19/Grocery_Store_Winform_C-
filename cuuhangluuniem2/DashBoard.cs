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
    public partial class DashBoard : Form
    {

        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public DashBoard()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
        }

        private void DashBoard_Load(object sender, EventArgs e)
        {
            string sdate = DateTime.Now.ToString("yyyy-MM-dd");
            lbDailySale.Text = dBConnect.ExtractData("select isnull(sum(total),0) as total from tbCart where status like 'Da Ban' and sdate between '" + sdate + "' and '" + sdate + "'").ToString("#,##0.00");
            lbTotalProduct.Text = dBConnect.ExtractData("select count(*) from tbProduct1").ToString("#,##0");
            lbStockOnHand.Text = dBConnect.ExtractData("select isnull(sum(qty),0) as qty from tbProduct1").ToString("#,##0");
            lbCritical.Text= dBConnect.ExtractData("select count(*) from vwCriticalItems").ToString("#,##0"); ;
        }
    }
}
