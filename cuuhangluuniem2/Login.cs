using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.PerformanceData;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cuuhangluuniem2
{
    public partial class Login : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;

        public string _pass = "";
        public bool _isactive;
        public Login()
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            txtName.Focus();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn muốn thoát ứng dụng","Thông Báo",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btLogin_Click(object sender, EventArgs e)
        {
            string _username = "", _name = "", _role = "";
            try
            {
                bool found;
                cn.Open();
                cm=new SqlCommand("select * from tbUser where username=@username and password=@password",cn);
                cm.Parameters.AddWithValue("@username",txtName.Text);
                cm.Parameters.AddWithValue("@password", txtPass.Text);
                dr= cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    found = true;
                    _username = dr["username"].ToString();
                    _name = dr["fullname"].ToString();
                    _role = dr["role"].ToString();
                    _pass = dr["password"].ToString();
                    _isactive = bool.Parse(dr["isactive"].ToString());
                }
                else
                {
                    found=false;
                }
                dr.Close();
                cn.Close();

                if (found)
                {
                    if (!_isactive)
                    {
                        MessageBox.Show("Tài khoản chưa được kích hoạt","Kích hoạt tài khoản",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    if (_role=="Nhân Viên")
                    {
                        MessageBox.Show("Chào Mừng "+_name,"Thông báo",MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Clear();
                        txtPass.Clear();
                        this.Hide();
                        Cashier cashier=new Cashier();
                        cashier.lbUserName.Text = _username;
                        cashier.lbName.Text = _name + " | " + _role;
                        cashier.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Chào Mừng " + _name, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtName.Clear();
                        txtPass.Clear();
                        this.Hide();
                        Form1 main=new Form1();
                        main.label_User.Text = _name;
                        main.label_Admin.Text = _role;
                        main.lbUsername.Text = _username;
                        main._pass = _pass;
                        main.ShowDialog();

                    }
                }
                else
                {
                    MessageBox.Show("Sai mật khẩu hoặc tên tài khoản","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát ứng dụng", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
            
        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btLogin.PerformClick();
            }
        }
    }
}
