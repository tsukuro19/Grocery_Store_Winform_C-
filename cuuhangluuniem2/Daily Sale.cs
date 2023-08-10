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
    public partial class DailySale : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public string solduser;
        public DailySale()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadCashier();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCashier()
        {
            cbCashier.Items.Clear();
            cbCashier.Items.Add("Tất Cả Nhân Viên");
            cn.Open();
            cm=new SqlCommand("select * from tbUser where role like 'Nhân Viên'",cn);
            dr=cm.ExecuteReader();
            while(dr.Read()) 
            {
                cbCashier.Items.Add(dr["username"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        public void LoadSold()
        {
            int i = 0;
            double total=0;
            dtgvSold.Rows.Clear();
            cn.Open();
            if(cbCashier.Text=="Tất Cả Nhân Viên")
            {
                cm=new SqlCommand("select c.id,c.transno,c.pcode,p.pdesc,c.price,c.qty,c.disc,c.total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where status like 'Da Ban' and sdate between '"+dtFrom.Value.ToString("yyyy-MM-dd")+"' and '"+dtTo.Value.ToString("yyyy-MM-dd") + "'",cn);
            }
            else
            {
                cm = new SqlCommand("select c.id,c.transno,c.pcode,p.pdesc,c.price,c.qty,c.disc,c.total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where status like 'Da Ban' and sdate between '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and '" + dtTo.Value.ToString("yyyy-MM-dd") + "' and cashier like '"+cbCashier.Text+"'", cn);
            }
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                total += double.Parse(dr["total"].ToString());
                dtgvSold.Rows.Add(i, dr["id"].ToString(), dr["transno"].ToString(), dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["price"].ToString(), dr["qty"].ToString(), dr["disc"].ToString(), dr["total"].ToString()) ;
            }
            dr.Close (); 
            cn.Close();
            lbTotal.Text = total.ToString("#,##0.00");
        }

        private void cbCashier_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            LoadSold();
        }

        private void dtgvSold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void dtgvSold_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colname = dtgvSold.Columns[e.ColumnIndex].Name;
            if(colname == "Cancel")
            {
                CancelOrder cancelOrder = new CancelOrder(this);
                cancelOrder.txtID.Text = dtgvSold.Rows[e.RowIndex].Cells[1].Value.ToString();
                cancelOrder.txtTransno.Text = dtgvSold.Rows[e.RowIndex].Cells[2].Value.ToString();
                cancelOrder.txtPcode.Text = dtgvSold.Rows[e.RowIndex].Cells[3].Value.ToString();
                cancelOrder.txtDesc.Text = dtgvSold.Rows[e.RowIndex].Cells[4].Value.ToString();
                cancelOrder.txtPrice.Text = dtgvSold.Rows[e.RowIndex].Cells[5].Value.ToString();
                cancelOrder.txtQty.Text = dtgvSold.Rows[e.RowIndex].Cells[6].Value.ToString();
                cancelOrder.txtDisc.Text = dtgvSold.Rows[e.RowIndex].Cells[7].Value.ToString();
                cancelOrder.txtTotal.Text = dtgvSold.Rows[e.RowIndex].Cells[8].Value.ToString();
                cancelOrder.txtCancelBy.Text = solduser;
                cancelOrder.ShowDialog();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            POSReport report = new POSReport();
            string param = "Từ ngày : " + dtFrom.Value.ToShortDateString() + " Đến: " + dtTo.Value.ToShortDateString();
            if (cbCashier.Text == "Tất Cả Nhân Viên")
            {
                report.LoadDailyReport("select c.id,c.transno,c.pcode,p.pdesc,c.price,c.qty,c.disc as discount,c.total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where status like 'Da Ban' and sdate between '" + dtFrom.Value.ToString("yyyyMMdd") + "' and '" + dtTo.Value.ToString("yyyyMMdd") + "'", param,cbCashier.Text);
            }
            else
            {
                report.LoadDailyReport("select c.id,c.transno,c.pcode,p.pdesc,c.price,c.qty,c.disc as discount,c.total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where status like 'Da Ban' and sdate between '" + dtFrom.Value.ToString("yyyyMMdd") + "' and '" + dtTo.Value.ToString("yyyyMMdd") + "' and cashier like '" + cbCashier.Text + "'", param, cbCashier.Text);
            }
            report.ShowDialog();
        }
    }
}
