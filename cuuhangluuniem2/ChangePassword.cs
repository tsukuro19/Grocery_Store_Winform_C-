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
    public partial class ChangePassword : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        Cashier cashier;
        public ChangePassword(Cashier cash)
        {
            InitializeComponent();
            cn = new SqlConnection(dBConnect.myConnect());
            cashier =cash;
            lbUsername.Text = cashier.lbUserName.Text;
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btNext_Click(object sender, EventArgs e)
        {
            try
            {
                string oldpass=dBConnect.getPassword(lbUsername.Text);
                if (oldpass != txtPass.Text)
                {
                    MessageBox.Show("Sai mật khẩu vui lòng thử lại","Thông Báo",MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    txtPass.Visible = false;
                    btNext.Visible = false;
                    txtNewPass.Visible = true;
                    txtConfirm.Visible = true;
                    btSave.Visible = true;
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.Message,"Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewPass.Text != string.Empty && txtConfirm.Text != string.Empty)
                {
                    if (txtNewPass.Text != txtConfirm.Text)
                    {
                        MessageBox.Show("Mật khẩu nhập lại không trùng nhau", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (MessageBox.Show("Bạn chắc chắn muốn đổi mật khẩu?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            dBConnect.ExecuteQuerry("update tbUser set password='" + txtNewPass.Text + "' where username = '" + lbUsername.Text + "'");
                            MessageBox.Show("Cập nhật mật khẩu thành công", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Dispose();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
