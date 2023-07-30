using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuuhangluuniem2
{
    class DBConnect
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        private string connect;
        
        public string myConnect()
        {
            connect = @"Data Source=DESKTOP-1DI3VRS;Initial Catalog=DBPoS;Integrated Security=True";
            return connect;
        }

        public DataTable getTable(string query)
        {
            cn.ConnectionString = myConnect();
            cm=new SqlCommand(query,cn);
            SqlDataAdapter adapter=new SqlDataAdapter(cm);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public void ExecuteQuerry(String sql)
        {
            try
            {
                cn.ConnectionString = myConnect();
                cn.Open();
                cm=new SqlCommand(sql,cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public String getPassword(string username)
        {
            string password="";
            cn.ConnectionString=myConnect();
            cn.Open() ;
            cm=new SqlCommand("select password from tbUser where username = '"+username+"'",cn);
            dr=cm.ExecuteReader();
            dr.Read();
            if(dr.HasRows)
            {
                password = dr["password"].ToString();
            }
            dr.Close();
            cn.Close() ;
            return password;
        }

        public double ExtractData(string sql1)
        {
            cn=new SqlConnection();
            cn.ConnectionString = myConnect();
            cn.Open();
            cm=new SqlCommand(sql1,cn);
            double data=double.Parse(cm.ExecuteScalar().ToString());
            cn.Close();
            return data;
        }
    }
}
