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

namespace DVLD
{
    public partial class frmAddUpdateUser : Form
    {
        public enum enMode { AddMode = 0, UpdateMode = 1 };
        private enMode _Mode;
        private int _UserID = -1;
        clsUser _User;

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _Mode = enMode.UpdateMode;
            _UserID = UserID;

        }
        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddMode;

        }

        private void _ResetDefaultValues()
        {
            //this will initialize the reset the default values

            if (_Mode == enMode.AddMode)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();

                tpLoginInfo.Enabled = false;

                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;


            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true;


        }
        private void _LoadData()
        {

            _User = clsUser.FindByUserID(_UserID);
            //tcUserInfo.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _User, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            //the following code will not be executed if the person was not found
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
        }
        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.UpdateMode)
                _LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        //private void AddNewUser_Load(object sender, EventArgs e)
        //{


        //    if (_Mode == enMode.AddMode)
        //    {
        //        lblTitle.Text = "Add New User";
        //        _User = new clsUser();
        //        return;
        //    }

        //    lblTitle.Text = "Udate User";
        //    _User = clsUser.FindByUserID(_UserID);

        //    if (_UserID == null)
        //    {
        //        Console.WriteLine("This form will be close because No User with " + _UserID);
        //        this.Close();

        //        return;
        //    }

        //    clsPersonInfoWithFilter1.loadInformation(_User.PersonID);

        //    //الشق الثاني
        //    label_ID.Text = _User.UserID.ToString();
        //    txt_UserName.Text = _User.UserName.ToString();
        //    txt_Password.Text = _User.Password.ToString();
        //    txt_Confirm.Text = _User.Password.ToString();

        //    if (_User.IsActive == 1)
        //    {
        //        chk_ISActive.Checked = true;
        //    }
        //    else
        //    {
        //        chk_ISActive.Checked = false;
        //    }


        //}



        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //    if (tabControl.SelectedTab == tabPage_LoginInfo)
        //    {
        //        //_User = new clsUser();

        //        _User.UserName = txt_UserName.Text;
        //        _User.Password = txt_Password.Text;
        //        _User.PersonID = clsPersonInfoWithFilter1.PersonID;
        //        if (chk_ISActive.Checked)
        //        {
        //            _User.IsActive = 1;
        //        }
        //        else
        //        {
        //            _User.IsActive = 0;
        //        }

        //        if (_User.SaveUsers())

        //            MessageBox.Show("Data Saved Successfully.");
        //        else
        //            MessageBox.Show("Error: Data Is not Saved Successfully.");

        //        _Mode = enMode.UpdateMode;


        //        label_ID.Text = _User.UserID.ToString();
        //    }
        //}



        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };
        }
        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            };


            if (_Mode == enMode.AddMode)
            {

                if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                };
            }
            else
            {
                //incase update make sure not to use anothers user name
                if (_User.UserName != txtUserName.Text.Trim())
                {
                    if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    };
                }
            }
        }


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we don't continue because the form is not valid
                MessageBox.Show("Some fields are not valid!, put the mouse over the red icon(s) to see the error",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _User.UserName = txtUserName.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.IsActive = chkIsActive.Checked;


            if (_User.SaveUsers())
            {
                lblUserID.Text = _User.UserID.ToString();
                //change form mode to update.
                _Mode = enMode.UpdateMode;
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.UpdateMode)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsUser.isUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {

                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();

            }
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
