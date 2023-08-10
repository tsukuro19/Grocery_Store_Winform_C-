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
    public partial class PrintSoldItems : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        public PrintSoldItems()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
        }

        private void PrintSoldItems_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadSoldItems(string sql,string param)
        {
            ReportDataSource rptDataSource;
            try
            {
                this.reportViewer1.LocalReport.DataSources.Clear();

                DataSet1 ds1 = new DataSet1();
                SqlDataAdapter da = new SqlDataAdapter();

                cn.Open();
                da.SelectCommand = new SqlCommand(sql, cn);
                da.Fill(ds1.Tables["dtSoldItems"]);
                cn.Close();

                ReportParameter pDate = new ReportParameter("pDate", param);

                reportViewer1.LocalReport.SetParameters(pDate);

                rptDataSource = new ReportDataSource("DataSet1", ds1.Tables["dtSoldItems"]);
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
