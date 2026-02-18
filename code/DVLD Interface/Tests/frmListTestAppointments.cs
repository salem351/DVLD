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
using DVLD.Tests;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using The_DVLD_Project.Properties;

namespace DVLD
{
    public partial class frmListTestAppointments : Form
    {
        //  int _L_D_D_AppID;
        ////  clsLocalDriving _LocalDriving;
        //  int _TestType = -1; // local license
        //  int Trail = 0; // to send to test form
        //  int _ApplicationID;

        private DataTable _dtLicenseTestAppointments;
        private int _LocalDrivingLicenseApplicationID;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        public frmListTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            // _L_D_D_AppID = L_D_D_AppID;

            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestType = TestType;
        }


        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {

                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }
        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            _LoadTestTypeImageAndTitle();


            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicationID, _TestType);

            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();

            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }


        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            //---
            clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType(_TestType);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }

            //if person already passed the test s/he cannot retake it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestType);
            frm2.ShowDialog();
            frmListTestAppointments_Load(null, null);
            //---

        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;


            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType, TestAppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        //private void UpdateRecords()
        //{

        //   // DataTable dt = clsTestAppointment._GetTestAppointmentByL_D_D_AppID(_L_D_D_AppID, _TestType);
        //    //int count = dt.Rows.Count;
        //   // label_Records.Text = count.ToString();
        //   // Trail = count;
        //}
        //private void _RefreshList()
        //{
        //   //// dataGridView1.DataSource = clsTestAppointment._GetTestAppointmentByL_D_D_AppID(_L_D_D_AppID, _TestType);
        //    //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //}
        //private void Vision_Test_Load(object sender, EventArgs e)
        //{
        //    //_TestType = CurrentTestType._TestType;

        //    //if (_TestType == 1)
        //    //{
        //    // //   pictureBox1.BackgroundImage = Properties.Resources.Vision_512;
        //    // //   label1.Text = "Vison Test Appointments";

        //    //}
        //    //else if (_TestType == 2)
        //    //{
        //    // //   pictureBox1.BackgroundImage = Properties.Resources.Written_Test_512;
        //    //  //  label1.Text = "Written Test Appointments";
        //    //}
        //    //else
        //    //{
        //    //  //  pictureBox1.BackgroundImage = Properties.Resources.driving_test_512;
        //    //   // label1.Text = "Street Test Appointments";
        //    //}

        //    //clsApplicationInfo1.LoadDataInApplicationControl(_L_D_D_AppID);
        //    //_ApplicationID = clsApplicationInfo1.ApplicationID;
        //    ////dataGridView1.DataSource = clsTestAppointment._GetTestAppointmentByL_D_D_AppID(_L_D_D_AppID, _TestType);
        //    //dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //    //UpdateRecords();
        //    //_RefreshList();
        //}


        //private void _RefreshAll()
        //{
        //    //UpdateRecords();
        //    //_RefreshList();
        //}
        //private void OpenScheduleForm(string mode, int testAppointmentId)
        //{
        //    //Schedule_Test frm = new Schedule_Test(_L_D_D_AppID, mode, testAppointmentId);
        //    //frm.StartPosition = FormStartPosition.CenterScreen;
        //    //frm.ShowDialog();
        //    //_RefreshAll();
        //}
        //private void ShowError(string message)
        //{
        //    //MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //}

        //private void btnAppoinment_Click(object sender, EventArgs e)
        //{

        //   // if (!clsTestAppointment._IsApplicantHasTestAppointment(_L_D_D_AppID, _TestType))
        //   // {
        //    //    OpenScheduleForm("Add", -1);
        //    //    return;
        //   // }
        //    //else
        //    //{

        //       // int TestAppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;

        //        //if (clsTestAppointment._IsApplicantHasNewOneAppointment(_L_D_D_AppID))
        //        //{
        //        //    ShowError("Applicant already has an active appointment.");
        //        //    return;
        //        //}
        //        ////نجح
        //        //else if (clsTests._IsApplicantPass(TestAppointmentID))
        //        //{
        //        //    ShowError("Applicant has already passed the test.");
        //        //    return;
        //        //}
        //        //else if (clsTests._IsApplicantFail(TestAppointmentID))
        //        //{
        //        //    OpenScheduleForm("ReTake", TestAppointmentID);
        //        //}

        //    }
        //}

        //private void editToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    int TestAppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
        //    string mode;

        //    if (clsTestAppointment._IsApplicantTakeTest(TestAppointmentID))
        //    {
        //        mode = "TakeTest";
        //    }
        //     //اذا هو retake جديد
        //    else if (clsTestAppointment._IsRetakeTestApplicationForEdit(TestAppointmentID))
        //    {
        //        mode = "Edit_Retake";
        //    }
        //    else
        //    {
        //        //اذا هو edit or pass
        //        mode = "Edit";
        //    }
        //    OpenScheduleForm(mode, TestAppointmentID);

        //}

        //private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    int TestAppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;

        //    if (clsTestAppointment._IsApplicantTakeTest(TestAppointmentID))
        //    {
        //        ShowError("Test already taken. Cannot retake.");
        //        return;
        //    }

        //    TakeTest frm = new TakeTest(_L_D_D_AppID, TestAppointmentID, Trail, _ApplicationID);
        //    frm.StartPosition = FormStartPosition.CenterScreen;
        //    frm.ShowDialog();

        //    _RefreshAll();

        //}




        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        //private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{

        //}

    }
}
