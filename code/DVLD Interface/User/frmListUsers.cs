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
using The_DVLD_Project.User;

namespace DVLD
{
    public partial class frmListUsers : Form
    {
        private static DataTable _dtAllUsers;



        public frmListUsers()
        {
            InitializeComponent();
        }


        ////اذا هو رقم ال اي دي فقط دخل ارقام
        //private void txt_Filter_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (cbFilterBy.Text == "User ID" || cbFilterBy.Text == "Person ID")
        //    {
        //        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        //        {
        //            e.Handled = true; // Ignore the key
        //        }
        //    }
        //}
        ////هنا فقط اختار نوع الفلتر وكل الشغل في التكيست بوك
        //private void combFilter_SelectedIndexChanged_1(object sender, EventArgs e)
        //{
        //    _RefreshUserList();

        //    if ((cbFilterBy.SelectedItem.ToString() != "None") && (cbFilterBy.SelectedItem.ToString() != "Is Active"))
        //    {
        //        txtFilterValue.Text = "";

        //        txtFilterValue.Visible = true;
        //        txtFilterValue.Focus();

        //        //MessageBox.Show(combFilter.Text);           
        //    }
        //    else if ((cbFilterBy.SelectedItem.ToString() == "Is Active"))
        //    {
        //        txtFilterValue.Visible = false;
        //        cbIsActive.Visible = true;
        //        cbIsActive.Focus();
        //    }
        //    else
        //    {
        //        cbIsActive.Visible = false;
        //        txtFilterValue.Visible = false;
        //    }
        //}
        //private void txt_Filter_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txtFilterValue.Text) && cbFilterBy.Text == "User ID")
        //    {
        //        int UserID;
        //        if (int.TryParse(txtFilterValue.Text, out UserID))
        //        {
        //            DataTable User = clsUser.GetByUserID(UserID);

        //            if (User != null)
        //            {
        //                dgvUsers.DataSource = User;
        //            }
        //            else
        //            {
        //                dgvUsers.DataSource = null;

        //            }
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(txtFilterValue.Text) && cbFilterBy.Text == "Full Name")
        //    {
        //        DataTable User = clsUser.GetByFullName(txtFilterValue.Text.Trim());

        //        if (User != null && User.Rows.Count > 0)
        //        {
        //            dgvUsers.DataSource = User;
        //        }
        //        else
        //        {
        //            dgvUsers.DataSource = null;
        //        }

        //    }
        //    if (!string.IsNullOrWhiteSpace(txtFilterValue.Text) && cbFilterBy.Text == "User Name")
        //    {

        //        DataTable User = clsUser.GetByUserName(txtFilterValue.Text);

        //        if (User != null)
        //        {
        //            dgvUsers.DataSource = User;
        //        }
        //        else
        //        {
        //            dgvUsers.DataSource = null;

        //        }

        //    }

        //    if (!string.IsNullOrWhiteSpace(txtFilterValue.Text) && cbFilterBy.Text == "Person ID")
        //    {

        //        int PersonID;
        //        if (int.TryParse(txtFilterValue.Text, out PersonID))
        //        {
        //            DataTable User = clsUser.GetByPersonID(PersonID);

        //            if (User != null)
        //            {
        //                dgvUsers.DataSource = User;
        //            }
        //            else
        //            {
        //                dgvUsers.DataSource = null;

        //            }
        //        }

        //    }


        //    //اذا هو فاضي اعمل رفريش
        //    if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
        //    {
        //        _RefreshUserList();
        //    }
        //}
        //private void combIsActive_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cbFilterBy.Text == "Is Active")
        //    {
        //        DataTable users = null;



        //        if (cbIsActive.Text == "All")
        //        {
        //            users = clsUser.GetALlUsers();
        //        }
        //        else if (cbIsActive.Text == "Yes")
        //        {
        //            users = clsUser.GetByIsActive(1);
        //        }
        //        else if (cbIsActive.Text == "No")
        //        {
        //            users = clsUser.GetByIsActive(0);
        //        }

        //        dgvUsers.DataSource = users ?? null;
        //    }

        //}


        //private void addNewPersonToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    _AddUser();
        //    _RefreshUserList();
        //}

        //private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    frmAddUpdateUser frm = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);

        //    frm.ShowDialog();

        //    //اول ما ينهي من الفورم على طول يعمل تجديد
        //    _RefreshUserList();
        //}

        //private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    //dataGridView1.CurrentRow.Cells[0].Value اول عنصر من الصف وهو ال اي دي
        //    //مهم جدا تحدف للذي ليس له ارتباط مع اي جدول لكي نحقق Refrential Integrity  
        //    int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;

        //    if (MessageBox.Show("Are yot sure you want ot delete user [" + UserID + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
        //    {

        //        if (!clsUser.IsUserHasLinkedToOtherTables(UserID))
        //        {
        //            //بكره استخدم جوين لكل الجداول المرتبطه بهذا
        //            if (clsUser.DeleteUser(UserID))
        //            {
        //                MessageBox.Show("user Deleted Successfully.", "Successfuly", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                _RefreshUserList();
        //            }
        //            else
        //            {
        //                MessageBox.Show("user was not Deleted", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("user was not Deleted, because it has data linked to it.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        //private void toolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    chnagePassword frm = new chnagePassword((int)dgvUsers.CurrentRow.Cells[0].Value);

        //    frm.ShowDialog();
        //    frm.StartPosition = FormStartPosition.CenterScreen;
        //    //اول ما ينهي من الفورم على طول يعمل تجديد
        //    _RefreshUserList();
        //}

        //private void sToolStripMenuItemShowDetails_Click(object sender, EventArgs e)
        //{
        //    ShowUserDetails frm = new ShowUserDetails((int)dgvUsers.CurrentRow.Cells[0].Value);
        //    frm.StartPosition = FormStartPosition.CenterScreen;

        //    frm.ShowDialog();

        //    _RefreshUserList();
        //}

        //private void sendEmailToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("This feature is Not Implementd Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        //}

        //private void phoneCallToolStripMenuItem1_Click(object sender, EventArgs e)
        //{
        //    MessageBox.Show("This feature is Not Implementd Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        //}

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            _dtAllUsers = clsUser.GetAllUsers();
            dgvUsers.DataSource = _dtAllUsers;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();

            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[0].Width = 110;

                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[1].Width = 120;

                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[2].Width = 350;

                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[3].Width = 120;

                dgvUsers.Columns[4].HeaderText = "Is Active";
                dgvUsers.Columns[4].Width = 120;
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            }

            else

            {

                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsActive.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "UserName":
                    FilterColumn = "UserName";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }


            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
        }
        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue = cbIsActive.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                _dtAllUsers.DefaultView.RowFilter = "";
            else
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();


        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnAddUser_Click_1(object sender, EventArgs e)
        {
            frmAddUpdateUser Frm1 = new frmAddUpdateUser();
            Frm1.ShowDialog();
            frmListUsers_Load(null, null); 
        }

        private void sToolStripMenuItemShowDetails_Click(object sender, EventArgs e)
        {
            frmUserInfo Frm1 = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();
        }
        private void addNewPersonToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser Frm1 = new frmAddUpdateUser();
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser Frm1 = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();

            //طريقة اخرة للفريش داتا
            frmListUsers_Load(null, null);
        }
        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            if (clsUser.DeleteUser(UserID))
            {
                MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmListUsers_Load(null, null);
            }

            else
                MessageBox.Show("User is not delted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        private void ChangePasswordToStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmChangePassword Frm1 = new frmChangePassword(UserID);
            Frm1.Show();
        }
        private void sendEmailToolStripMenuItem1_Click(object sender, EventArgs e)
        {
                MessageBox.Show("This feature is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
        private void phoneCallToolStripMenuItem1_Click(object sender, EventArgs e)
        {
                MessageBox.Show("This feature is Not Implementd Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }

        private void dgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmUserInfo Frm1 = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();
        }
    }

}
