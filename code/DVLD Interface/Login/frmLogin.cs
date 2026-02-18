using DVLD_MyBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using DVLD.Classes;

namespace DVLD
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            if (user != null)
            {

                if (chkRememberMe.Checked)
                {
                    //store username and password
                    clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                }
                else
                {
                    //store empty username and password
                    clsGlobal.RememberUsernameAndPassword("", "");

                }

                //incase the user is not active
                if (!user.IsActive)
                {

                    txtUserName.Focus();
                    MessageBox.Show("Your account is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = user;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();

                this.Close();

            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintails", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //private void LoginAccount_Load(object sender, EventArgs e)
        //{
        //    string filePath = @"D:\شهادات كورسات محمد ابو هدهود\كورس 19\UserNameAndPassword.txt";

        //    if (File.Exists(filePath))
        //    {
        //        string[] lines = File.ReadAllLines(filePath);

        //        foreach (string line in lines)
        //        {
        //            if (line.StartsWith("Username: "))
        //                txtUserName.Text = line.Substring("Username: ".Length);
        //            else if (line.StartsWith("Password: "))
        //                txtPassword.Text = line.Substring("Password: ".Length);
        //        }
        //    }
        //}

        //private void chb_Me_CheckedChanged(object sender, EventArgs e)
        //{
        //    //but if it doesn't checked, deleted the text while click to ligon button
        //    if (chkRememberMe.Checked)
        //    {
        //        string input = "Username: " + txtUserName.Text + "\nPassword: " + txtPassword.Text;

        //        // Choose a path to save the file (you can change this)
        //        string filePath = @"D:\شهادات كورسات محمد ابو هدهود\كورس 19\UserNameAndPassword.txt";

        //        // Save the input to the file
        //        File.WriteAllText(filePath, input);

        //        //MessageBox.Show("Input saved successfully.");
        //    }

        //}


        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";

            if (clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                chkRememberMe.Checked = true;
            }
            else
                chkRememberMe.Checked = false;

        }




    }


}


