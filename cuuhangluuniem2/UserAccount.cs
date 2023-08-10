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
    public partial class UserAccount : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        SqlDataReader dr;
        Form1 main;
        public string username;
        string fullname;
        string role;
        string phone;
        string identity_number;
        string acc_status;
        public UserAccount(Form1 mainform)
        {
            InitializeComponent();
            cn=new SqlConnection(dBConnect.myConnect());
            main= mainform;
            LoadUser();
        }

        public void LoadUser()
        {
            int i = 0;
            dtgvUser.Rows.Clear();
            cm = new SqlCommand("select * from tbUser" , cn);
            cn.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dtgvUser.Rows.Add(i, dr[0].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[2].ToString());

            }
            dr.Close();
            cn.Close();
        }

        public void Clear()
        {
            txtUser.Clear();
            txtName.Clear();
            txtPassword.Clear();
            cbRole.Text = "";
            txtRe_typePass.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtIdentity.Clear();
        }

        private void btAccSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text!=txtRe_typePass.Text)
                {
                    MessageBox.Show("Mật khẩu nhập lại không trùng nhau","Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                //check account exist
                cn.Open();
                cm=new SqlCommand("select username from tbUser where username = '"+txtUser.Text+"'", cn);
                string check_username = (string)cm.ExecuteScalar();
                cn.Close() ;
                if(check_username==txtUser.Text)
                {
                    MessageBox.Show("Tài khoản đã tồn tại", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                cn.Open();
                cm=new SqlCommand("insert into tbUser (username,password,role,fullname,phone,identity_card) values(@username,@password,@role,@fullname,@phone,@identity_card)",cn);
                cm.Parameters.AddWithValue("@username",txtUser.Text);
                cm.Parameters.AddWithValue("@password", txtPassword.Text);
                cm.Parameters.AddWithValue("@role", cbRole.Text);
                cm.Parameters.AddWithValue("@fullname", txtName.Text);
                cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                cm.Parameters.AddWithValue("@identity_card", txtIdentity.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Tài khoản mới đã được lưu","Lưu Tài Khoản",MessageBoxButtons.OK,MessageBoxIcon.Information);
                LoadUser();
                Clear();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Cảnh Báo");
            }
        }

        private void btAccCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn hủy không?", "Cảnh Báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                Clear();
            }
        }

        private void btPass_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReccentPass.Text != main._pass)
                {
                    MessageBox.Show("Mật khẩu gần đây không chính xác","Thông Báo",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(txtNewPass.Text != txtConfirm.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận không trùng nhau", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                dBConnect.ExecuteQuerry("update tbUser set password = '"+txtNewPass.Text+"' where username = '"+lbUsername.Text+"'");
                MessageBox.Show("Mật khẩu đã được cập nhật thành công","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                ClearCP();
            }
            catch( Exception ex ) 
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UserAccount_Load(object sender, EventArgs e)
        {
            lbUsername.Text = main.lbUsername.Text;
        }

        private void btCancel_Pass_Click(object sender, EventArgs e)
        {
            ClearCP();
        }

        public void ClearCP()
        {
            txtReccentPass.Clear();
            txtNewPass.Clear();
            txtConfirm.Clear();
        }

        private void dtgvUser_SelectionChanged(object sender, EventArgs e)
        {
            int i=dtgvUser.CurrentRow.Index;
            username = dtgvUser[1,i].Value.ToString();
            fullname = dtgvUser[2,i].Value.ToString();
            phone= dtgvUser[3,i].Value.ToString();
            identity_number = dtgvUser[4,i].Value.ToString();
            acc_status = dtgvUser[5,i].Value.ToString();
            role = dtgvUser[6,i].Value.ToString();
            if (lbUsername.Text == username)
            {
                btRemove.Enabled = false;
                btReste_Password.Enabled = false;
                gbUser.Text = "Mật Khẩu cho " + username;
                lbAccNote.Text = "Bạn không thể thay đổi mật khẩu cho " + username + " vui lòng chọn reset password";
            }
            else
            {
                btRemove.Enabled = true;
                btReste_Password.Enabled = true;
                gbUser.Text = "Mật Khẩu cho " + username;
                lbAccNote.Text = "Để thay đổi mật khẩu cho " + username + " vui lòng chọn reset password";
            }
        }

        private void btRemove_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn muốn xóa tài khoản này khỏi hệ thống?","Xác Nhận",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dBConnect.ExecuteQuerry("delete from tbUser where username= '" + username + "'");
                MessageBox.Show("Tài khoản đã được xóa khỏi hệ thống","Thông Báo",MessageBoxButtons.OK,MessageBoxIcon.Information);
                LoadUser();
            }
        }

        private void btReste_Password_Click(object sender, EventArgs e)
        {
            ResetPassword reset = new ResetPassword(this);
            reset.ShowDialog();
        }

        private void btProperties_Click(object sender, EventArgs e)
        {
            UserProperties properties = new UserProperties(this);
            properties.txtName.Text = username;
            properties.txtPhone.Text = phone;
            properties.txtIdentity.Text = identity_number;
            properties.cbRole.Text= role;
            properties.cbActive.Text = acc_status;
            properties.username=username;
            properties.ShowDialog();
        }
    }
}
