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
using Microsoft.Reporting.WinForms;

namespace cuuhangluuniem2
{
    public partial class Receipt : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        string store = "Cửa Hàng Lưu Niệm QStar";
        string address = "43 Tân Lập, Phường Đông Hòa, Thành Phố Dĩ An, Tỉnh Bình Dương";
        Cashier cashier;
        public Receipt(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            cashier = cash;
        }

        private void Receipt_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        public void Load_Receipt(string pcash,string pchange)
        {
            ReportDataSource rptDataSource;
            try
            {
                //this.reportViewer1.LocalReport.ReportPath = Application.StartupPath + @"\Reports\rpReceipt.rdlc";
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds1 = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand("select c.id, c.transno, c.pcode, c.price, c.qty, c.disc, c.total, c.sdate, c.status, p.pdesc from tbCart as c inner join tbProduct1 as p on p.pcode=c.pcode where c.transno like '"+cashier.lbTranNo.Text+"'",cn);
                da.Fill(ds1.Tables["dtReceipt"]);
                cn.Close();

                ReportParameter pVatable= new ReportParameter("pVatable",cashier.lbTax.Text);
                ReportParameter pDiscount = new ReportParameter("pDiscount", cashier.lbDiscount.Text);
                ReportParameter pTotal = new ReportParameter("pTotal", cashier.lbVatable.Text);
                ReportParameter pCash = new ReportParameter("pCash", pcash);
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAddress", address);
                ReportParameter pTransaction = new ReportParameter("pTransaction", cashier.lbTranNo.Text);
                ReportParameter pCashier = new ReportParameter("pCashier", cashier.lbName.Text);
                ReportParameter pChange = new ReportParameter("pChange", pchange);

                reportViewer1.LocalReport.SetParameters(pVatable);
                reportViewer1.LocalReport.SetParameters(pDiscount);
                reportViewer1.LocalReport.SetParameters(pTotal);
                reportViewer1.LocalReport.SetParameters(pCash);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);
                reportViewer1.LocalReport.SetParameters(pTransaction);
                reportViewer1.LocalReport.SetParameters(pCashier);
                reportViewer1.LocalReport.SetParameters(pChange);

                rptDataSource = new ReportDataSource("DataSet1", ds1.Tables["dtReceipt"]);
                reportViewer1.LocalReport.DataSources.Add(rptDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 30;
            }
            catch(Exception ex) 
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Receipt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }
    }
}
