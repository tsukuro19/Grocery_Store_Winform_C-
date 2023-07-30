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
    public partial class dtFromStockIn : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public dtFromStockIn()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            LoadCriticalItems();
            LoadInvetoryList();
        }

        public void LoadTopSelling()
        {
            int i = 0;
            dtgvTopSelling.Rows.Clear();
            cn.Open();
            if (cbTopSell.Text == "Sắp xếp theo số lượng")
            {
                cm = new SqlCommand("select top 10 pcode, pdesc, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwTopSoldItems where sdate between '" + dtFromTopSell.Value.ToString("yyyy-MM-dd") + "' and '" + dtToTopSell.Value.ToString("yyyy-MM-dd") + "' and status like 'Da Ban' group by pcode, pdesc order by qty desc", cn);

            }
            else if(cbTopSell.Text == "Sắp xếp theo doanh thu")
            {
                cm = new SqlCommand("select top 10 pcode, pdesc, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwTopSoldItems where sdate between '" + dtFromTopSell.Value.ToString("yyyy-MM-dd") + "' and '" + dtToTopSell.Value.ToString("yyyy-MM-dd") + "' and status like 'Da Ban' group by pcode, pdesc order by total desc", cn);
            }
            dr=cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dtgvTopSelling.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), dr["qty"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
            }
            dr.Close();
            cn.Close();
        }

        private void btLoadTopSell_Click(object sender, EventArgs e)
        {
            if(cbTopSell.Text== "Chọn kiểu sắp xếp")
            {
                MessageBox.Show("Vui lòng chọn kiểu sắp xếp","Thông Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbTopSell.Focus();
                return;
            }
            LoadTopSelling();
        }

        public void LoadSoldItems()
        {
            try
            {
                dtgvSoldItems.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select c.pcode, p.pdesc, c.price, sum(c.qty) as qty, sum(c.disc) as disc,sum(c.total) as total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where status like 'Da Ban' and sdate between '" + dtFromSoldItems.Value.ToString("yyyy-MM-dd") + "' and '" + dtToSoldItems.Value.ToString("yyyy-MM-dd") + "' group by c.pcode, p.pdesc, c.price", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dtgvSoldItems.Rows.Add(i, dr["pcode"].ToString(), dr["pdesc"].ToString(), double.Parse(dr["price"].ToString()).ToString("#,##0.00"), dr["qty"].ToString(), dr["disc"].ToString(), double.Parse(dr["total"].ToString()).ToString("#,##0.00"));
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select isnull(sum(total),0) from tbCart where status like 'Da Ban' and sdate between '" + dtFromSoldItems.Value.ToString("yyyy-MM-dd") + "' and '" + dtToSoldItems.Value.ToString("yyyy-MM-dd") + "'", cn);
                lbTotal.Text = double.Parse(cm.ExecuteScalar().ToString()).ToString("#,##0.00");
                cn.Close();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void btLoadSoldItems_Click(object sender, EventArgs e)
        {
            LoadSoldItems();
        }

        public void LoadCriticalItems()
        {
            try
            {
                dtgvCriticalItems.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select * from vwCriticalItems",cn);
                dr=cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dtgvCriticalItems.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[7].ToString(), dr[6].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void LoadInvetoryList()
        {
            try
            {
                dtgvInventory.Rows.Clear();
                int i = 0;
                cn.Open();
                cm = new SqlCommand("select * from vwInventoryList", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dtgvInventory.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[7].ToString(), dr[6].ToString());
                }
                dr.Close();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        public void LoadCancelItems()
        {
            int i = 0;
            dtgvCancel.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwCancel where sdate between '"+dtFromCancel.Value.ToString("yyyy-MM-dd")+"' and '"+dtToCancel.Value.ToString("yyyy-MM-dd")+"'",cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvCancel.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString(), dr[9].ToString());
            }
            dr.Close (); 
            cn.Close();
        }
        private void btLoadCancel_Click(object sender, EventArgs e)
        {
            LoadCancelItems();
        }

        public void LoadStockInHist()
        {
            int i = 0;
            dtgvStockIn.Rows.Clear();
            cn.Open();
            cm = new SqlCommand("select * from vwStockIn where cast(sdate as date) between '" + dtFromStockIn2.Value.ToString("yyyy-MM-dd") + "' and '" + dtToStockIn.Value.ToString("yyyy-MM-dd") + "' and status like 'Hoan Thanh'", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvStockIn.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void btLoadStockIn_Click(object sender, EventArgs e)
        {
            LoadStockInHist();
        }

        private void btnPrintTopSale_Click(object sender, EventArgs e)
        {
            string param = "Từ Ngày :" + dtFromTopSell.Value.ToString() + " đến " + dtToTopSell.Value.ToString();
            PrintTopSelling report=new PrintTopSelling();
            if (cbTopSell.Text == "Sắp xếp theo số lượng")
            {
                report.LoadTopSelling("select top 10 pcode, pdesc, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwTopSoldItems where sdate between '" + dtFromTopSell.Value.ToString("yyyy-MM-dd") + "' and '" + dtToTopSell.Value.ToString("yyyy-MM-dd") + "' and status like 'Da Ban' group by pcode, pdesc order by qty desc", param,"Sản Phẩm Bán Chạy Sắp Xếp Theo Số Lượng");

            }
            else if (cbTopSell.Text == "Sắp xếp theo doanh thu")
            {
               report.LoadTopSelling("select top 10 pcode, pdesc, isnull(sum(qty),0) as qty, isnull(sum(total),0) as total from vwTopSoldItems where sdate between '" + dtFromTopSell.Value.ToString("yyyy-MM-dd") + "' and '" + dtToTopSell.Value.ToString("yyyy-MM-dd") + "' and status like 'Da Ban' group by pcode, pdesc order by total desc", param,"Sản Phẩm Bán Chạy Sắp Xếp Theo Doanh Thu");
            }
            report.ShowDialog();
        }

        private void btPrintSoldItems_Click(object sender, EventArgs e)
        {
            string param = "Từ Ngày :" + dtFromTopSell.Value.ToString() + " đến " + dtToTopSell.Value.ToString();
            PrintSoldItems report=new PrintSoldItems();
            report.LoadSoldItems("select c.pcode, p.pdesc, c.price, sum(c.qty) as qty, sum(c.disc) as disc,sum(c.total) as total from tbCart as c inner join tbProduct1 as p on c.pcode=p.pcode where status like 'Da Ban' and sdate between '" + dtFromSoldItems.Value.ToString("yyyy-MM-dd") + "' and '" + dtToSoldItems.Value.ToString("yyyy-MM-dd") + "' group by c.pcode, p.pdesc, c.price", param);
            report.ShowDialog();
        }

        private void btPrintInventory_Click(object sender, EventArgs e)
        {
            PrintInventoryList report=new PrintInventoryList();
            report.LoadInventoryList("select * from vwInventoryList");
            report.ShowDialog();
        }

        private void btPrintCancel_Click(object sender, EventArgs e)
        {
            string param = "Từ Ngày :" + dtFromTopSell.Value.ToString() + " đến " + dtToTopSell.Value.ToString();
            PrintCancel report=new PrintCancel();
            report.LoadCancel("select * from vwCancel where sdate between '" + dtFromCancel.Value.ToString("yyyy-MM-dd") + "' and '" + dtToCancel.Value.ToString("yyyy-MM-dd") + "'",param);
            report.ShowDialog();
        }

        private void btPrintStockIn_Click(object sender, EventArgs e)
        {
            string param = "Từ Ngày :" + dtFromTopSell.Value.ToString() + " đến " + dtToTopSell.Value.ToString();
            printStockIn report=new printStockIn();
            report.LoadStockIn("select * from vwStockIn where cast(sdate as date) between '" + dtFromStockIn2.Value.ToString("yyyy-MM-dd") + "' and '" + dtToStockIn.Value.ToString("yyyy-MM-dd") + "' and status like 'Hoan Thanh'",param);
            report.ShowDialog();
        }
    }
}
