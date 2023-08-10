using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace cuuhangluuniem2
{
    public partial class Form1 : Form
    {
        SqlConnection cn = new SqlConnection();
        SqlCommand cm = new SqlCommand();
        DBConnect dBConnect = new DBConnect();
        public string _pass;
        public Form1()
        {
            InitializeComponent();
            customize_Design();
            cn = new SqlConnection(dBConnect.myConnect());
            cn.Open();
        }


        #region panelSlide
        private void customize_Design()
        {
            panel_SubProduct.Visible = false;
            panel_Subrecord.Visible = false;
            panel_SubSetting.Visible = false;
            panel_SubStock.Visible = false;
        }

        private void hide_submenu()
        {
            if(panel_SubProduct.Visible == true)
                panel_SubProduct.Visible=false;
            if (panel_Subrecord.Visible == true)
                panel_Subrecord.Visible = false;
            if (panel_SubSetting.Visible == true)
                panel_SubSetting.Visible = false;
            if (panel_SubStock.Visible == true)
                panel_SubStock.Visible = false;
        }

        private void show_menu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hide_submenu();
                submenu.Visible = true;
            }else
                submenu.Visible=false;
        }
        #endregion
        private Form activeForm = null;
        public void openChildForm(Form childForm
            )
        {
            if (activeForm != null)
                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            //display name child form
            label_Title.Text = childForm.Text;
            panel_Main.Controls.Add(childForm);
            panel_Main.Tag= childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btDashboard_Click(object sender, EventArgs e)
        {
            openChildForm(new DashBoard());
            hide_submenu() ;
        }

        private void btProduct_Click(object sender, EventArgs e)
        {
            show_menu(panel_SubProduct);//used to show sub_menu of product
        }

        private void btListProduct_Click(object sender, EventArgs e)
        {
            openChildForm(new Product());
            hide_submenu();//used to hide sub_menu
        }

        private void btCategory_Product_Click(object sender, EventArgs e)
        {
            openChildForm(new Category());
            hide_submenu();
        }

        private void btBrand_Click(object sender, EventArgs e)
        {
            openChildForm(new Brand());
            hide_submenu();
        }

        private void btStock_Click(object sender, EventArgs e)
        {
            show_menu(panel_SubStock);
        }

        private void btEntry_Click(object sender, EventArgs e)
        {
            openChildForm(new stock());
            hide_submenu();
        }

        private void btAdjust_Click(object sender, EventArgs e)
        {
            openChildForm(new Adjustment(this));
            hide_submenu();
        }

        private void btSupplier_Click(object sender, EventArgs e)
        {
            openChildForm(new Supplier());
            hide_submenu();
        }

        private void btRecord_Click(object sender, EventArgs e)
        {
            show_menu(panel_Subrecord);
        }

        private void btHistory_Click(object sender, EventArgs e)
        {
            DailySale daily = new DailySale();
            daily.ShowDialog();
            hide_submenu();
        }

        private void bt_Product_of_sale_Click(object sender, EventArgs e)
        {
            openChildForm(new dtFromStockIn());
            hide_submenu();
        }

        private void btSetting_Click(object sender, EventArgs e)
        {
            show_menu(panel_SubSetting);
        }

        private void bt_User_Click(object sender, EventArgs e)
        {
            openChildForm(new UserAccount(this));
            hide_submenu();
        }

        private void bt_Logout_Click(object sender, EventArgs e)
        {
            hide_submenu();
            if (MessageBox.Show("Bạn muốn đăng xuất tài khoản", "Đăng Xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Hide();
                Login login = new Login();
                login.ShowDialog();
            }
        }

        private void panel_Main_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát phần mềm?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the user is closing the form using the Alt+F4 key combination
            if (e.CloseReason == CloseReason.UserClosing && (Control.ModifierKeys & Keys.Alt) == Keys.Alt)
            {
                // Perform any necessary cleanup or save operations
                // ...

                // Exit the application
                if (MessageBox.Show("Bạn muốn thoát phần mềm?", "Xác Nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
        }

        private void btStore_Click(object sender, EventArgs e)
        {
            hide_submenu();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btDashboard.PerformClick();
        }
    }
}
