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
    public partial class POSReport : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        string store = "Cửa Hàng Lưu Niệm QStar";
        string address = "43 Tân Lập, Phường Đông Hòa, Thành Phố Dĩ An, Tỉnh Bình Dương";
        public POSReport()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            
        }

        private void POSReport_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadDailyReport(string sql, string param,string cashier)
        {
            ReportDataSource rptDataSource;
            try
            {
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds1 = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds1.Tables["dtSoldReport"]);
                cn.Close();

                ReportParameter pDate= new ReportParameter("pDate",param);
                ReportParameter pCashier = new ReportParameter("pCashier", cashier);
                ReportParameter pHeader = new ReportParameter("pHeader", "Báo Cáo Doanh Thu Hàng Ngày");
                ReportParameter pStore = new ReportParameter("pStore", store);
                ReportParameter pAddress = new ReportParameter("pAddress", address);

                reportViewer1.LocalReport.SetParameters(pDate);
                reportViewer1.LocalReport.SetParameters(pCashier);
                reportViewer1.LocalReport.SetParameters(pHeader);
                reportViewer1.LocalReport.SetParameters(pStore);
                reportViewer1.LocalReport.SetParameters(pAddress);

                rptDataSource = new ReportDataSource("DataSet1", ds1.Tables["dtSoldReport"]);
                reportViewer1.LocalReport.DataSources.Add(rptDataSource);
                reportViewer1.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout);
                reportViewer1.ZoomMode = ZoomMode.Percent;
                reportViewer1.ZoomPercent = 30;
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, "Error");
            }
        }
        
    }
}
