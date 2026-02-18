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

namespace DVLD.UserControls
{
    public partial class ctrlDrivingLicenseApplicationInfo : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        private int _LocalDrivingLicenseApplicationID = -1;

        private int _LicenseID;

        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }

        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }

        private void _FillLocalDrivingLicenseApplicationInfo()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            //incase there is license enable the show link.
            llShowLicenceInfo.Enabled = (_LicenseID != -1);


            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = _LocalDrivingLicenseApplication.GetPassedTestCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID);

        }
        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
            lblLocalDrivingLicenseApplicationID.Text = "[????]";
            lblAppliedFor.Text = "[????]";


        }

        public void LoadApplicationInfoByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }
        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingLicenseApplicationInfo();
        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //frmShowLicenseInfo frm = new frmShowLicenseInfo(_LocalDrivingLicenseApplication.GetActiveLicenseID());
            //frm.ShowDialog();
            MessageBox.Show("This Form is under process!", "Soon!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }



        //public int ApplicationID { get; set; }
        //public string ClassName { get; set; }


        //public void LoadDataInApplicationControl(int L_D_D_AppID)
        //{
        //    _LocalDriving = clsLocalDriving.MyFindByLDLAppID(L_D_D_AppID);

        //    label_DLAPPID.Text = _LocalDriving.Rows[0]["LocalDrivingLicenseApplicationID"].ToString();
        //    label_ClasseApp.Text = _LocalDriving.Rows[0]["ClassName"].ToString();
        //    label_PassedTest.Text = _LocalDriving.Rows[0]["PassedTestCount"].ToString() + "/3";
        //    label_ID.Text = _LocalDriving.Rows[0]["ApplicationID"].ToString();

        //    ApplicationID = (int)_LocalDriving.Rows[0]["ApplicationID"];
        //    ClassName = _LocalDriving.Rows[0]["ClassName"].ToString();


        //    label_Status.Text = _LocalDriving.Rows[0]["Status"].ToString();
        //    label_Fees.Text = _LocalDriving.Rows[0]["PaidFees"].ToString();
        //    label_Fees.Text = _LocalDriving.Rows[0]["PaidFees"].ToString();
        //    label10.Text = _LocalDriving.Rows[0]["ApplicationTypeTitle"].ToString();
        //    label8.Text = _LocalDriving.Rows[0]["FullName"].ToString();
        //    label_Date.Text = Convert.ToDateTime(_LocalDriving.Rows[0]["ApplicationDate"]).ToString("yyyy-MM-dd");
        //    label_SatausDate.Text = Convert.ToDateTime(_LocalDriving.Rows[0]["LastStatusDate"]).ToString("yyyy-MM-dd");

        //    label_CreatedBy.Text = CurrentUserInfo.GetCurrentUser().UserName;
        //}

        //private void linkLabel_InfoPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    Person_Details frm = new Person_Details((int)_LocalDriving.Rows[0]["PersonID"]);
        //    frm.StartPosition = FormStartPosition.CenterScreen;
        //    frm.ShowDialog();
        //}


    }
}
