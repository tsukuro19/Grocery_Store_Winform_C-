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
    public partial class UserProperties : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;

        UserAccount account;
        public string username;
        public UserProperties(UserAccount user)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            account = user;
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                if ((MessageBox.Show("Bạn có muốn thay đổi thông tin tài khoản không?", "Thông Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)) 
                {
                    cn.Open();
                    cm = new SqlCommand("update tbUser set fullname=@fullname, phone=@phone, identity_card=@identity_card,role=@role,isactive=@isactive where username='" + username + "'", cn);
                    cm.Parameters.AddWithValue("@fullname", txtName.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@identity_card", txtIdentity.Text);
                    cm.Parameters.AddWithValue("@role", cbRole.Text);
                    cm.Parameters.AddWithValue("@isactive", cbActive.Text);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Chỉnh sửa thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    account.LoadUser();
                    this.Dispose();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
    }
}

