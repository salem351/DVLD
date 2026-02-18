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
using The_DVLD_Project.People;
using The_DVLD_Project.People.Controls;

namespace DVLD
{
    public partial class frmListPeople : Form
    {

        private static DataTable _dtAllPeople = clsPerson.GetAllPeople();

        //only select the columns that you want to show in the grid
        private DataTable _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                         "FirstName", "SecondName", "ThirdName", "LastName",
                                                         "GendorCaption", "DateOfBirth", "CountryName",
                                                         "Phone", "Email");



        public frmListPeople()
        {
            InitializeComponent();
        }

        private void _RefreshPeopleList()
        {
            _dtAllPeople = clsPerson.GetAllPeople();
            _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                       "FirstName", "SecondName", "ThirdName", "LastName",
                                                       "GendorCaption", "DateOfBirth", "CountryName",
                                                       "Phone", "Email");

            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }
        private void Manage_People_Load(object sender, EventArgs e)
        {

            dgvPeople.DataSource = _dtPeople;
            cbFilterBy.SelectedIndex = 0;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";
                dgvPeople.Columns[0].Width = 110;

                dgvPeople.Columns[1].HeaderText = "National No.";
                dgvPeople.Columns[1].Width = 120;


                dgvPeople.Columns[2].HeaderText = "First Name";
                dgvPeople.Columns[2].Width = 120;

                dgvPeople.Columns[3].HeaderText = "Second Name";
                dgvPeople.Columns[3].Width = 140;


                dgvPeople.Columns[4].HeaderText = "Third Name";
                dgvPeople.Columns[4].Width = 120;

                dgvPeople.Columns[5].HeaderText = "Last Name";
                dgvPeople.Columns[5].Width = 120;

                dgvPeople.Columns[6].HeaderText = "Gendor";
                dgvPeople.Columns[6].Width = 120;

                dgvPeople.Columns[7].HeaderText = "Date Of Birth";
                dgvPeople.Columns[7].Width = 140;

                dgvPeople.Columns[8].HeaderText = "Nationality";
                dgvPeople.Columns[8].Width = 120;


                dgvPeople.Columns[9].HeaderText = "Phone";
                dgvPeople.Columns[9].Width = 120;


                dgvPeople.Columns[10].HeaderText = "Email";
                dgvPeople.Columns[10].Width = 170;
            }
        }


        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshPeopleList();  
        }
       


        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void addNewPersonToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshPeopleList();
        }
        private void editToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Form frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshPeopleList();
        }
        private void deleteToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {

                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {


                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeopleList();
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }
        private void sendEmailToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        private void phoneCallToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }





        //هنا فقط اختار نوع الفلتر وكل الشغل في التكيست بوك
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        //private void txbFilter_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
        //    {
        //        _RefreshPeopleList();
        //        return;
        //    }

        //    string filterType = cbFilterBy.Text;
        //    string filterValue = txtFilterValue.Text;
        //    DataTable person = null;

        //    switch (filterType)
        //    {
        //        case "Person ID":
        //            if (int.TryParse(filterValue, out int personID))
        //            {
        //                //person = clsPerson.FindPersonID(personID);
        //            }
        //            break;

        //        case "National No":
        //            //person = clsPerson.FindNationalNo(filterValue);
        //            break;

        //        case "First Name":
        //            //person = clsPerson.FindFirstName(filterValue);
        //            break;

        //        case "Second Name":
        //            //person = clsPerson.FindSecondName(filterValue);
        //            break;

        //        case "Third Name":
        //           // person = clsPerson.FindThirdName(filterValue);
        //            break;

        //        case "Last Name":
        //            //person = clsPerson.FindLastName(filterValue);
        //            break;

        //        case "Nationality":
        //            //person = clsPerson.FindNationality(filterValue);
        //            break;

        //        case "Gendor":
        //            //person = clsPerson.FindGendor(filterValue);
        //            break;

        //        case "Phone":
        //            //person = clsPerson.FindPhone(filterValue);
        //            break;

        //        case "Email":
        //            //person = clsPerson.FindEmail(filterValue);
        //            break;
        //    }

        //    if (person != null && person.Rows.Count > 0)
        //    {
        //        dgvPeople.DataSource = person;
        //    }
        //    else
        //    {
        //        _RefreshPeopleList(); // fallback if no match found
        //    }


        //}
        //اذا هو رقم ال اي دي فقط دخل ارقام
        private void txbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void txbFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.

                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();

        }



        private void dgvPeople_DoubleClick(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }





        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }

        
    }
}


