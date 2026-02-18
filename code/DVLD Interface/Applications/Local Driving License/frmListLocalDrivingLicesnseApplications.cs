using DVLD_MyBusiness;
using cc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.License;
using The_DVLD_Project.Applications.Local_License;
using The_DVLD_Project.Licenses.Local_Licenses;
using The_DVLD_Project.Licenses;

namespace DVLD.Apply
{
    public partial class frmListLocalDrivingLicesnseApplications : Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplications;

        public frmListLocalDrivingLicesnseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtAllLocalDrivingLicenseApplications = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();
            dgvLocalDrivingLicenseApplications.DataSource = _dtAllLocalDrivingLicenseApplications;

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {

                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 300;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 150;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 350;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 170;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 150;
            }

            cbFilterBy.SelectedIndex = 0;

        }


        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {

                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;


                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                //in this case we deal with integer not string.
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase L.D.L.AppID id is selected.
            if (cbFilterBy.Text == "L.D.L.AppID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }


        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();
            frm.ShowDialog();
            //refresh
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm =
                       new frmLocalDrivingLicenseApplicationInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            //refresh
            frmListLocalDrivingLicenseApplications_Load(null, null);

        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmAddUpdateLocalDrivingLicesnseApplication frm =
                         new frmAddUpdateLocalDrivingLicesnseApplication(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();

            //refresh
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            int LicenseID = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(
               LocalDrivingLicenseApplicationID).GetActiveLicenseID();

            if (LicenseID != -1)
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("No License Found!", "No License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void CancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refresh the form again.
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void DeleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //refresh the form again.
                    frmListLocalDrivingLicenseApplications_Load(null, null);
                }
                else
                {
                    MessageBox.Show("Could not delete application, other data depends on it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(localDrivingLicenseApplication.ApplicantPersonID);
            frm.ShowDialog();

        }


        private void _ScheduleTest(clsTestType.enTestType TestType)
        {

            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingLicenseApplicationID, TestType);
            frm.ShowDialog();

            //refresh
            frmListLocalDrivingLicenseApplications_Load(null, null);

        }
        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }
        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }
        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }




        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication =
                    clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID
                                                    (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplication.IsLicenseIssued();

            //Enabled only if person passed all tests and Does not have license. 
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExists;

            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            editToolStripMenuItem.Enabled = !LicenseExists && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);
            ScheduleTestsMenue.Enabled = !LicenseExists;

            //Enable/Disable Cancel Menue Item
            //We only canel the applications with status=new.
            CancelApplicaitonToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            //Enable/Disable Delete Menue Item
            //We only allow delete incase the application status is new not complete or Cancelled.
            DeleteApplicationToolStripMenuItem.Enabled = (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);



            //Enable Disable Schedule menue and it's sub menue
            bool PassedVisionTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest); ;
            bool PassedWrittenTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.StreetTest);

            ScheduleTestsMenue.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LocalDrivingLicenseApplication.ApplicationStatus == clsApplication.enApplicationStatus.New);

            if (ScheduleTestsMenue.Enabled)
            {
                //To Allow Schdule vision test, Person must not passed the same test before.
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;

            }

        }






        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }








        //private void _RefreshApplicationsList()
        //{
        //    dataGridView1.DataSource = clsLocalDriving.GetAllApplications();
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //}


        //private void LocalDrivingLicesneApplication_Load(object sender, EventArgs e)
        //{
        //    DataTable dt = clsLocalDriving.GetAllApplications();

        //    dataGridView1.DataSource = dt;
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //    label_Records.Text = dt.Rows.Count.ToString();

        //    _RefreshApplicationsList();
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void _AddApplication()
        //{
        //    New_Local New_Local = new New_Local(-1);
        //    New_Local.StartPosition = FormStartPosition.CenterScreen;
        //    New_Local.ShowDialog();

        //}
        //private void btnApply_Click(object sender, EventArgs e)
        //{
        //    _AddApplication();
        //    _RefreshApplicationsList();
        //}



        //private void combFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    _RefreshApplicationsList();

        //    if ((combFilter.SelectedItem.ToString() != "None"))
        //    {
        //        txbFilter.Text = "";

        //        txbFilter.Visible = true;
        //        txbFilter.Focus();

        //        //MessageBox.Show(combFilter.Text);           
        //    }
        //    else
        //    {
        //        txbFilter.Visible = false;
        //    }
        //}
        //private void txbFilter_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (combFilter.Text == "L.D.L.AppID")
        //    {
        //        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        //        {
        //            e.Handled = true; // Ignore the key
        //        }
        //    }
        //}
        //private void txbFilter_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txbFilter.Text))
        //    {
        //        _RefreshApplicationsList();
        //        return;
        //    }

        //    string filterType = combFilter.Text;
        //    string filterValue = txbFilter.Text;
        //    DataTable Application = null;

        //    switch (filterType)
        //    {
        //        case "L.D.L.AppID":
        //            if (int.TryParse(filterValue, out int LDLAppID))
        //            {
        //                Application = clsLocalDriving.FindByLDLAppID(LDLAppID);
        //            }
        //            break;

        //        case "National No":
        //            Application = clsLocalDriving.FindByNationalNo(filterValue);
        //            break;

        //        case "Full Name":
        //            Application = clsLocalDriving.FindByFullName(filterValue);
        //            break;

        //        case "Status":
        //            Application = clsLocalDriving.FindByStatus(filterValue);
        //            break;

        //    }

        //    if (Application != null && Application.Rows.Count > 0)
        //    {
        //        dataGridView1.DataSource = Application;
        //    }
        //    else
        //    {
        //        _RefreshApplicationsList(); // fallback if no match found
        //    }
        //}


        //private void TSMI_Cancel_Click(object sender, EventArgs e)
        //{
        //    int DrivingLicenseID = (int)dataGridView1.CurrentRow.Cells[0].Value;

        //    if (MessageBox.Show("Are yot sure you want to Cancel this application user ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {

        //        if (clsApplications._CancelApplication(DrivingLicenseID))
        //        {
        //            MessageBox.Show("Application Canceled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            _RefreshApplicationsList();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Application was not Canceled", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //        }

        //    }
        //}



        //private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        //{
        //    string Status = (string)dataGridView1.CurrentRow.Cells[6].Value;

        //    int passedTest = (int)dataGridView1.CurrentRow.Cells[5].Value;

        //    if (Status == "Completed")
        //    {
        //        TSMI_ShowDetails.Enabled = true;
        //        TSMI_Edit.Enabled = false;
        //        TSMI_Delete.Enabled = false;
        //        TSMI_Cancel.Enabled = false;
        //        TSMI_ScheduleTests.Enabled = false;
        //        TSMI_IssueDrivingLicense.Enabled = false;
        //        TSMI_ShowLicense.Enabled = true;
        //        TSMI_ShowPersonLicenseHistory.Enabled = true;

        //        return;
        //    }
        //    else if (Status == "Cancelled")
        //    {

        //        TSMI_ShowDetails.Enabled = false;
        //        TSMI_Edit.Enabled = false;
        //        TSMI_Delete.Enabled = false;
        //        TSMI_Cancel.Enabled = false;
        //        TSMI_ScheduleTests.Enabled = false;
        //        TSMI_IssueDrivingLicense.Enabled = false;
        //        TSMI_ShowLicense.Enabled = false;
        //        TSMI_ShowPersonLicenseHistory.Enabled = false;
        //        //phoneCallToolStripMenuItem1.Enabled = false;

        //        return;
        //    }

        //    TSMI_ShowDetails.Enabled = true;
        //    TSMI_Edit.Enabled = true;
        //    TSMI_Delete.Enabled = true;
        //    TSMI_Cancel.Enabled = true;
        //    TSMI_ScheduleTests.Enabled = true;
        //    TSMI_IssueDrivingLicense.Enabled = true;
        //    TSMI_ShowLicense.Enabled = true;
        //    TSMI_ShowPersonLicenseHistory.Enabled = true;
        //    //LDLAppIDphoneCallToolStripMenuItem1.Enabled = true;

        //    // Default: disable all test scheduling options
        //    ScheduleVisionTest.Enabled = false;
        //    ScheduleWrittenTest.Enabled = false;
        //    ScheduleStreetTest.Enabled = false;

        //    // Enable license options only if all tests are passed
        //    bool allTestsPassed = passedTest == 3;
        //    TSMI_IssueDrivingLicense.Enabled = allTestsPassed;
        //    TSMI_ShowLicense.Enabled = !allTestsPassed;
        //    TSMI_ScheduleTests.Enabled = !allTestsPassed;

        //    // Enable only the current required test
        //    switch (passedTest)
        //    {
        //        case 0:
        //            ScheduleVisionTest.Enabled = true;
        //            break;
        //        case 1:
        //            ScheduleWrittenTest.Enabled = true;
        //            break;
        //        case 2:
        //            ScheduleStreetTest.Enabled = true;
        //            break;
        //    }

        //}



        ////Tests
        //private void ScheduleVisionTest_Click(object sender, EventArgs e)
        //{
        //    CurrentTestType._TestType = 1;

        //    TestAppointment VisionTest = new TestAppointment((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    VisionTest.StartPosition = FormStartPosition.CenterScreen;
        //    VisionTest.ShowDialog();

        //    _RefreshApplicationsList();
        //}
        //private void ScheduleWrittenTest_Click(object sender, EventArgs e)
        //{
        //    CurrentTestType._TestType = 2;

        //    TestAppointment VisionTest = new TestAppointment((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    VisionTest.StartPosition = FormStartPosition.CenterScreen;
        //    VisionTest.ShowDialog();

        //    _RefreshApplicationsList();
        //}
        //private void ScheduleStreetTest_Click(object sender, EventArgs e)
        //{
        //    CurrentTestType._TestType = 3;

        //    TestAppointment VisionTest = new TestAppointment((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    VisionTest.StartPosition = FormStartPosition.CenterScreen;
        //    VisionTest.ShowDialog();

        //    _RefreshApplicationsList();
        //}


        ////License
        //private void TSMI_IssueDrivingLicense_Click(object sender, EventArgs e)
        //{
        //    //عملتها منفصلة اصدار الرخصة علشان اريد ان اخلي المقدم يدفع 
        //    IssueDrivingLicense IssueDrivingLicense = new IssueDrivingLicense((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    IssueDrivingLicense.StartPosition = FormStartPosition.CenterScreen;
        //    IssueDrivingLicense.ShowDialog();

        //    _RefreshApplicationsList();
        //}
        //private void TSMI_ShowLicense_Click(object sender, EventArgs e)
        //{
        //    int ApplicationID = clsLocalDriving._GetApplicationID((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    int LicenseID = clsIssueLicense.FindByApplicationID(ApplicationID).LicenseID;

        //    LicenseInfo IssueDrivingLicense = new LicenseInfo(LicenseID);
        //    IssueDrivingLicense.StartPosition = FormStartPosition.CenterScreen;
        //    IssueDrivingLicense.ShowDialog();

        //    _RefreshApplicationsList();
        //}
        //private void TSMI_ShowPersonLicenseHistory_Click(object sender, EventArgs e)
        //{
        //    int PersonID = clsApplications._GetApplicationPersonID_ByLocalLicenseID((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    LicenseHistore IssueDrivingLicense = new LicenseHistore(PersonID);
        //    IssueDrivingLicense.StartPosition = FormStartPosition.CenterScreen;
        //    IssueDrivingLicense.ShowDialog();

        //    _RefreshApplicationsList();
        //}



        //private void TSMI_Delete_Click(object sender, EventArgs e)
        //{
        //    //dataGridView1.CurrentRow.Cells[0].Value اول عنصر من الصف وهو ال اي دي
        //    //مهم جدا تحدف للذي ليس له ارتباط مع اي جدول لكي نحقق Refrential Integrity  
        //    int L_D_L_AppID = (int)dataGridView1.CurrentRow.Cells[0].Value;
        //    int PassedTests = (int)dataGridView1.CurrentRow.Cells[5].Value;
        //    string Status = dataGridView1.CurrentRow.Cells[5].Value.ToString();

        //    if (MessageBox.Show("Are yot sure you want ot delete this application ", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //    {

        //        if (!clsLocalDriving._IsLocalDrivingLicenseCompleted(L_D_L_AppID) && PassedTests == 0)
        //        {
        //            //بكره استخدم جوين لكل الجداول المرتبطه بهذا
        //            if (clsLocalDriving._DeleteLocalDrivingLicense(L_D_L_AppID))
        //            {
        //                MessageBox.Show("Application Deleted Successfully.", "Successfuly", MessageBoxButtons.OK, MessageBoxIcon.Information);

        //                _RefreshApplicationsList();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Application was not Deleted", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Application was not Deleted, because it has data linked to it.", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
        //        }
        //    }
        //}

        //private void TSMI_Edit_Click(object sender, EventArgs e)
        //{

        //}
        //private void TSMI_ShowDetails_Click(object sender, EventArgs e)
        //{
        //    int ApplicationID = clsLocalDriving._GetApplicationID((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    int PersonID = clsApplications._GetApplicationPersonID(ApplicationID);

        //    Person_Details PersonDetails = new Person_Details(PersonID);
        //    PersonDetails.StartPosition = FormStartPosition.CenterScreen;
        //    PersonDetails.ShowDialog();

        //    _RefreshApplicationsList();
        //}
    } 
}
