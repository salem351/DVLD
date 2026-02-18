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
using System.Runtime.CompilerServices;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using cc;

namespace DVLD.Tests
{
    public partial class frmScheduleTest : Form
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private int _AppointmentID = -1;

        //public enum enMode { AddMode, UpdateMode }
        //private enMode _Mode;

        //clsTestAppointment _TestAppointment;
        //DataTable _LocalDriving;

        //int _L_D_D_AppID;
        //int _TestTypeID;
        //int _Trail = 0;
        //int _TestAppointmentID = 0;
        //string _Type;

        //Types - Add, ReTake, Edit, FailOrPass
        public frmScheduleTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID, int AppointmentID = -1)
        {
            //_L_D_D_AppID = L_D_D_AppID;
            //_TestAppointmentID = TestAppointmentID;
            //_Type = Type;

            InitializeComponent();

            //if (Type == "Add" || Type == "ReTake")
            //    _Mode = enMode.AddMode;
            //else
            //    _Mode = enMode.UpdateMode;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
            _AppointmentID = AppointmentID;


        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            ctrlScheduleTest1.TestTypeID = _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingLicenseApplicationID, _AppointmentID);


            // _TestTypeID = CurrentTestType._TestType;
            //// _Trail = clsTestAppointment._GetTrail(_L_D_D_AppID);
            //// decimal fees = (clsTestType.FindByTestTypeID(_TestTypeID).TestTypeFees);


            // if (_TestTypeID == 1)
            // {
            //    // pictureBox1.BackgroundImage = Properties.Resources.Vision_512;
            // }
            // else if (_TestTypeID == 2)
            // {
            //    // pictureBox1.BackgroundImage = Properties.Resources.Written_Test_512;
            // }
            // else
            // {
            //     //pictureBox1.BackgroundImage = Properties.Resources.driving_test_512;
            // }


            // if (_Type == "TakeTest")
            // {
            //     //if (clsTests._IsApplicantFail(_TestAppointmentID))
            //     //{
            //         label1.Text = "Schedule Retake Test";
            //     //}

            //     label13.Text = "Person already sat for the test, appointment locked";
            //     dateTimePicker1.Enabled = false;
            //    // lbl_TotalFees.Text = fees + lbl_RAppFees.Text;

            //     groupBox1.Enabled = true;
            //     btnSave.Enabled = false;
            // }
            // if (_Type == "Edit_Retake")
            // {
            //     label1.Text = "Schedule Retake Test";

            //     groupBox1.Enabled = true;

            //    // decimal fees_Retake = clsApplicationType._GetApplicationTypesFee(7);
            //     //lbl_RAppFees.Text = fees_Retake.ToString("0.00");  // or just .ToString()

            //    // decimal totalFees = fees + fees_Retake;
            //     //lbl_TotalFees.Text = totalFees.ToString("0.00");

            //     //label9.Text = clsTestAppointment._GetRetakeTestApplicationID(_TestAppointmentID).ToString();
            // }


            //// _LocalDriving = clsLocalDriving.MyFindByLDLAppID(_L_D_D_AppID);
            // label_DLAPPID.Text = _LocalDriving.Rows[0]["LocalDrivingLicenseApplicationID"].ToString();
            // label_ClasseApp.Text = _LocalDriving.Rows[0]["ClassName"].ToString();
            // label8.Text = _LocalDriving.Rows[0]["FullName"].ToString();

            // if(_Type == "Add")
            // {
            //     dateTimePicker1.Text = _LocalDriving.Rows[0]["LastStatusDate"].ToString();
            // }
            // else
            // {
            //    // dateTimePicker1.Text = clsTestAppointment._GetAppointmentDate(_TestAppointmentID).ToString("yyyy-MM-dd");

            // }

            //// label_Fees.Text = fees.ToString();
            //// lbl_TotalFees.Text = fees.ToString();
            // label4.Text = _Trail.ToString();


            // if (_Mode == enMode.AddMode)
            // {
            //     _TestAppointment = new clsTestAppointment();

            //     if (_Type == "ReTake")
            //     {
            //        // if (clsTests._IsApplicantFail(_TestAppointmentID))
            //         //{
            //             label1.Text = "Schedule Retake Test";

            //         //}
            //        // decimal fees_Retake = clsApplicationType._GetApplicationTypesFee(7);
            //         //lbl_RAppFees.Text = fees_Retake.ToString("0.00");  // or just .ToString()

            //         //decimal totalFees = fees + fees_Retake;
            //         //lbl_TotalFees.Text = totalFees.ToString("0.00");

            //         groupBox1.Enabled = true;
            //     }
            //     return;
            // }


            //// _TestAppointment = clsTestAppointment.FindByL_D_D_AppID(_L_D_D_AppID);

            // if (_TestAppointment == null)
            // {
            //     Console.WriteLine("This form will be close because No Person with " + _L_D_D_AppID);
            //     this.Close();

            //     return;
            // }


            // label4.Text = (_Trail - 1).ToString();

        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void btnSave_Click(object sender, EventArgs e)
        //{
        //   ////decimal fees = clsTestType.FindByTestTypeID(_TestTypeID).TestTypeFees;

        //   // if(_Mode == enMode.UpdateMode)
        //   // {
        //   //     _TestAppointment.TestAppointmentID = _TestAppointmentID;
        //   // }
           
        //   // //_TestAppointment.TestTypeID = CurrentTestType._TestType;
        //   // _TestAppointment.LocalDrivingLicenseApplicationID = _L_D_D_AppID;
        //   // _TestAppointment.AppointmentDate = dateTimePicker1.Value;
            
        //   // _TestAppointment.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;
        //   // _TestAppointment.IsLocked = false;

        //   // if (_Type == "ReTake")
        //   // {
        //   //      //_TestAppointment.RetakeTestApplicationID = clsLocalDriving._GetApplicationID(_L_D_D_AppID);
        //   //    // decimal fees_Retake = clsApplicationType._GetApplicationTypesFee(7);
        //   //     //_TestAppointment.PaidFees = fees + fees_Retake;
        //   // }
        //   // else
        //   // {
        //   //     //_TestAppointment.PaidFees = fees;
        //   // }
           


        //   // if (_TestAppointment.Save())

        //   //     MessageBox.Show("Data Saved Successfully.");
        //   // else
        //   //     MessageBox.Show("Error: Data Is not Saved Successfully.");
            


        //   // this.Close();
        //}

    }
}
