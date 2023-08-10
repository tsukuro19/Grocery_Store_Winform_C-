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
    public partial class SuplierModule : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        Supplier supplier;
        public SuplierModule(Supplier sp)
        {
            cn = new SqlConnection(dBConnect.myConnect());
            InitializeComponent();
            supplier = sp;
        }

        public void Clear()
        {
            txtAddress.Clear();
            txtContact_person.Clear();
            txtEmail.Clear();
            txtSupplier.Clear();
            txtPhone.Clear();

            btSave.Enabled = true;
            btUpdate.Enabled = false;
            txtSupplier.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Bạn có muốn lưu nhà cung cấp này không?", "Lưu Nhà Cung Cấp", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm=new SqlCommand("insert into tbSupplier (supplier,address,contactperson,phone,email) values (@supplier,@address,@contactperson,@phone,@email)",cn);
                    cm.Parameters.AddWithValue("@supplier", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtContact_person.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Bản ghi đã được lưu", "Lưu Bản Ghi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    supplier.LoadSupplier();

                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message,"Thông Báo");
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc muốn hủy không?","Cảnh Báo",MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
            {
                this.Dispose();
            }
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(MessageBox.Show("Bạn có muốn cập nhật bản ghi này không ? ","Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    cn.Open();
                    cm=new SqlCommand("update tbSupplier set supplier=@supplier, address=@address,contactperson=@contactperson,phone=@phone,email=@email where id=@id",cn);
                    cm.Parameters.AddWithValue("@id", lbID.Text);
                    cm.Parameters.AddWithValue("@supplier", txtSupplier.Text);
                    cm.Parameters.AddWithValue("@address", txtAddress.Text);
                    cm.Parameters.AddWithValue("@contactperson", txtContact_person.Text);
                    cm.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cm.Parameters.AddWithValue("@email", txtEmail.Text);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Bản ghi đã được cập nhật", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Cảnh Báo");
            }
        }
    }
}
