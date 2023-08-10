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
    public partial class ResetPassword : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        UserAccount user;
        public ResetPassword(UserAccount userAccount)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            user = userAccount;
        }

        private void btConfirm_Click(object sender, EventArgs e)
        {
            if (txtNewPass.Text!=txtConfirm.Text)
            {
                MessageBox.Show("Mật khẩu nhập lại không trùng nhau","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            else
            {
                if(MessageBox.Show("Bạn chắc chắn muốn thay đổi mật khẩu","Xác Nhận",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    dBConnect.ExecuteQuerry("update tbUser set password = '" + txtNewPass.Text + "' where username = '" + user.username + "'");
                    MessageBox.Show("Mật khẩu đã được thay đổi", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                } 
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
