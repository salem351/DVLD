using DVLD.Classes;
using DVLD_MyBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DVLD.License
{
    public partial class frmIssueDriverLicenseFirstTime : Form
    {

        private int _LocalDrivingLicenseApplicationID;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        // public enum enMode { AddMode, UpdateMode }
        // private enMode _Mode;

        //// clsIssueLicense _IssueLicense;
        // int _L_D_D_AppID = -1;
        //// clsDrivers _Drivers;

        public frmIssueDriverLicenseFirstTime(int LocalDrivingLicenseApplicationID)
        {
          //  _L_D_D_AppID = LDLAppID;

            InitializeComponent();


            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            //if (LDLAppID != -1)
            //    _Mode = enMode.AddMode;
            //else
            //    _Mode = enMode.UpdateMode; 
        }

        //private void IssueDrivingLicense_Load(object sender, EventArgs e)
        //{
        //   /////////////////////// clsApplicationInfo1.LoadDataInApplicationControl(_L_D_D_AppID);



        //   // if (_Mode == enMode.AddMode)
        //   // {
        //   // //    _IssueLicense = new clsIssueLicense();
        //   //  //   _Drivers = new clsDrivers();
        //   //     return;
        //   // }

        //   //// _IssueLicense = clsIssueLicense.FindByApplicationID(clsApplicationInfo1.ApplicationID);
        //}

        //private void btnIssue_Click(object sender, EventArgs e)
        //{
        //    //احفظ انه من السواقين لعشان نحص على الاي دي
        //    //_Drivers.PersonID = (int)clsLocalDriving.FindFromFull(_L_D_D_AppID).Rows[0]["ApplicantPersonID"];
        //   // _Drivers.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;
        //   // _Drivers.CreatedDate = DateTime.Now;
        //   // if (_Drivers.Save())
        //   //{

        //  //  }


        //    //احفظ الرخصة الجديده
        //   // clsLicenseClasses lc = clsLicenseClasses.FindByClassName(clsApplicationInfo1.ClassName);
        //   ///////////////// int _ApplicationID = clsApplicationInfo1.ApplicationID;

        //    //_IssueLicense.ApplicationID = _ApplicationID;
        //    //_IssueLicense.DriverID = _Drivers.DriverID;
        //    //_IssueLicense.LicenseClass = lc.LicenseClassID;
        //    //_IssueLicense.IssueDate = DateTime.Now;
        //    //_IssueLicense.ExpirationDate = DateTime.Now.AddYears(lc.DefaultValidityLength);
        //    //_IssueLicense.Notes = textBox1.Text;
        //    //_IssueLicense.PaidFees = lc.ClassFees;
        //    //_IssueLicense.IsActive = true;
        //    //_IssueLicense.IssueReason = 1;
        //    //_IssueLicense.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;

        //   // if (_IssueLicense.Save())
        //       // MessageBox.Show("License Issue Successfully with License ID = " + _IssueLicense.LicenseID, "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    //else
        //     //   MessageBox.Show("Error: Data Is not Saved Successfully.");



        //  //  _Mode = enMode.UpdateMode;



        //    //الان عملنا له رخصة عاد فقط نعمل للطلب انه مكتمل
        //    //if (clsApplications._CompleteApplication(_ApplicationID, 3))
        //   // {

        //  //  }

        //}


        private void IssueDrivingLicense_Load_1(object sender, EventArgs e)
        {

            txtNotes.Focus();
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {

                MessageBox.Show("No Application with ID=" + _LocalDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //نتاكد انه نجح في كل الامتحانات للحماية قد اليوزر يجي لهذا الفورم من مكان في الكود وهو عاده ما كمل الاختبارات
            if (!_LocalDrivingLicenseApplication.PassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            //نتاكد انه ما عنده رخصة من نفس النوع لان هنا اصدار رخصة لاول مره
            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            //ما بندخل هنا الا بعد ما نتاكد انه نجح في كل الامتحانات وماعنده رخصة من نفس النوع بالفعل قد شيكنا في الفنكشن فوق تبع التحميل وهذا يرجع رقم الرخصة وداخلها اذا مش سائق يضيفه ويصدر الرخصة
            int LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirstTime(txtNotes.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ",
                 "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
