using DVLD_MyBusiness;
using cc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using The_DVLD_Project.Tests.Controls;
using DVLD.Classes;

namespace DVLD.Tests
{
    public partial class frmTakeTest : Form
    {

        private int _AppointmentID;
        private clsTestType.enTestType _TestType;

        private int _TestID = -1;
        private clsTest _Test;

        //  public enum enMode { AddMode, UpdateMode }
        //  private enMode _Mode;



        //  int _L_D_D_AppID;
        //  int _TestAppointmentID;
        //  int _Trail;

        ////  clsApplications _Applications;
        //  clsTestAppointment _TestAppointment;
        ////  clsTests _Test;
        //  DataTable _LocalDriving;
        //  int _ApplicationID;


        public frmTakeTest(int AppointmentID, clsTestType.enTestType TestType)
        {
            //_ApplicationID = ApplicationID;
            //_TestAppointmentID = TestAppointmentID;
            //_L_D_D_AppID = L_D_D_AppID;
            //_Trail = Trail;

            InitializeComponent();


            _AppointmentID = AppointmentID;
            _TestType = TestType;

            //if (L_D_D_AppID != -1)
            //    _Mode = enMode.AddMode;
            //else
            //    _Mode = enMode.UpdateMode;

        }

        //private void TakeTest_Load(object sender, EventArgs e)
        //{
        //  ////  decimal fees = (clsTestType.FindByTestTypeID(CurrentTestType._TestType).TestTypeFees);


        //  // // _LocalDriving = clsLocalDriving.MyFindByLDLAppID(_L_D_D_AppID);

        //  //  int _TestTypeID = CurrentTestType._TestType;

        //  //  if (_TestTypeID == 1)
        //  //  {
        //  //   //   pictureBox1.BackgroundImage = Properties.Resources.Vision_512;
        //  //  }
        //  //  else if (_TestTypeID == 2)
        //  //  {
        //  //   //  pictureBox1.BackgroundImage = Properties.Resources.Written_Test_512;
        //  //  }
        //  //  else
        //  //  {
        //  //     // pictureBox1.BackgroundImage = Properties.Resources.driving_test_512;
        //  //  }


        //  //  label_DLAPPID.Text = _LocalDriving.Rows[0]["LocalDrivingLicenseApplicationID"].ToString();
        //  //  label_ClasseApp.Text = _LocalDriving.Rows[0]["ClassName"].ToString();
        //  //  label8.Text = _LocalDriving.Rows[0]["FullName"].ToString();
        //  //  label_Date.Text = (_LocalDriving.Rows[0]["ApplicationDate"]).ToString();

        //  //  //label_Date.Text = clsTestAppointment._GetAppointmentDate(_TestAppointmentID).ToString("yyyy-MM-dd");
        //  //  //label_Fees.Text = fees.ToString();

        //  //  label4.Text = _Trail.ToString();

        //  //  if (_Mode == enMode.AddMode)
        //  //  {
        //  //     // _Test = new clsTests();
        //  //      return;
        //  //  }


        //}

        //private void btnSave_Click(object sender, EventArgs e)
        //{

        //   // //_Test.TestAppointmentID = _TestAppointmentID;

        //   // if (rdio_Pass.Checked)
        //   // {
        //   //    // _Test.TestResult = true;

        //   // }
        //   // else{
        //   //     //_Test.TestResult = false;

        //   // }


        //   //// _Test.Notes = txt_Notes.Text;
        //   //// _Test.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;


        //   // //بمجرد عملت الامتحان خلاص قفل هذا الموعد
        //   // //if (clsTestAppointment._UpdateTestApplicationTolock(_TestAppointmentID))
        //   //// {

        //   //// }

        //   // if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        //   // {
        //   //     //if (_Test.Save())

        //   //         MessageBox.Show("Data Saved Successfully.");
        //   //     //else
        //   //         MessageBox.Show("Error: Data Is not Saved Successfully.");

        //   // }


        //   // this.Close();
        //}



        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestTypeID = _TestType;
            ctrlScheduledTest1.LoadInfo(_AppointmentID);

            if (ctrlScheduledTest1.TestAppointmentID == -1)
                btnSave.Enabled = false;
            else
                btnSave.Enabled = true;


            int _TestID = ctrlScheduledTest1.TestID;
            if (_TestID != -1)
            {
                _Test = clsTest.Find(_TestID);

                if (_Test.TestResult)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;
                txtNotes.Text = _Test.Notes;

                lblUserMessage.Visible = true;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
            }

            else
                _Test = new clsTest();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to save? After that you cannot change the Pass/Fail results after you save?.",
                      "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No
             )
            {
                return;
            }

            _Test.TestAppointmentID = _AppointmentID;
            _Test.TestResult = rbPass.Checked;
            _Test.Notes = txtNotes.Text.Trim();
            _Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_Test.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }


    }
}
