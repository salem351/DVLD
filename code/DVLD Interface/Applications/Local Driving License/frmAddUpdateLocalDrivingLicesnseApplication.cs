using DVLD.Classes;
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
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Apply
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        // public enum enMode { AddMode, UpdateMode }
        // private enMode _Mode;

        // int _LocalDrivingLicenseApplicationID = -1;
        // byte ApplicationStatus = 1;
        // DateTime LastStatusDate = DateTime.Now;

        //// clsLocalDriving _LocalDriving;
        //// clsApplications _Application;
        //// clsApplicationType _ApplicationType = clsApplicationType.FindByApplicationTypeID(1);

        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            _Mode = enMode.Update;
        }
        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }


        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            //this will initialize the reset the Default values
            _FillLicenseClassesInComoboBox();


            if (_Mode == enMode.AddNew)
            {

                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                cbLicenseClass.SelectedIndex = 2;
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;


            }

        }
        private void _LoadData()
        {

            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationDate);
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString();
            lblCreatedByUser.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication.CreatedByUserID).UserName;

        }

        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);


        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }

        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;

        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();

        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {

            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }


            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];

            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //هذه مالها داعي
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }


            int LicenseClassID = clsLicenseClass.Find(cbLicenseClass.Text).LicenseClassID;
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicenseClass.Focus();
                return;
            }


            //check if user already have issued license of the same driving  class.
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID; ;
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(lblFees.Text);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;


            if (_LocalDrivingLicenseApplication.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }



        //private void _FillTheLicenseClasses()
        //{
        //    DataTable dtLicenseClasses = clsLocalDriving.GetALlLicenseClasses();

        //    foreach (DataRow Row in dtLicenseClasses.Rows)
        //    {
        //        comboBox1.Items.Add(Row["ClassName"]);
        //    }
        //}
        //private void _Load()
        //{
        //    _FillTheLicenseClasses();
        //    comboBox1.SelectedIndex = 2;
        //    labl_ApplicationFees.Text = ((int)_ApplicationType.ApplicationFees).ToString();
        //    lab_Date.Text = DateTime.Now.ToString();
        //    lab_CreatedBy.Text = CurrentUserInfo.GetCurrentUser().UserName;

        //    if (_Mode == enMode.AddMode)
        //    {
        //        label2.Text = "Add New Local Driving License Application";
        //        _LocalDriving = new clsLocalDriving();
        //        _Application = new clsApplications();
        //        ApplicationStatus = 1;
        //        LastStatusDate = DateTime.Now;
        //        return;
        //    }

        //    //label2.Text = "Udate New Local Driving License Application";

        //    //_LocalDriving = clsLocalDriving.Find(_LocalDrivingLicenseApplicationID);

        //    //if (_LocalDriving == null)
        //    //{
        //    //    Console.WriteLine("This form will be close because No User with " + _LocalDrivingLicenseApplicationID);
        //    //    this.Close();

        //    //    return;
        //    //}

        //    //_Aplication = clsApplications.FindByApplicationID(_LocalDriving.ApplicationID);
        //    //clsPersonInfoWithFilter2.loadInformation(_Aplication.ApplicantPersonID);

        //    ////الشق الثاني
        //    //comboBox1.SelectedIndex = 
        //    //txt_UserName.Text = _User.UserName.ToString();
        //    //txt_Password.Text = _User.Password.ToString();
        //    //txt_Confirm.Text = _User.Password.ToString();

        //    //if (_User.IsActive == 1)
        //    //{
        //    //    chk_ISActive.Checked = true;
        //    //}
        //    //else
        //    //{
        //    //    chk_ISActive.Checked = false;
        //    //}

        //}
        //private void New_Local_Load(object sender, EventArgs e)
        //{
        //    _Load();
        //}





        //private void btn_Next1_Click(object sender, EventArgs e)
        //{
        //    tabControl.SelectedTab = tabPage_ApplicationInfo;
        //}

        //private void btnClose1_Click(object sender, EventArgs e)
        //{
        //    this.Close();

        //}

        //private void btnSave1_Click(object sender, EventArgs e)
        //{



        //    if (tabControl.SelectedTab == tabPage_ApplicationInfo)
        //    {
        //        DataTable dt = clsLocalDriving.FindByNationalNo(clsPersonInfoWithFilter2.NationalNo);

        //        foreach (DataRow row in dt.Rows)
        //        {
        //            if (comboBox1.Text == row["ClassName"].ToString() && row["Status"].ToString() == "New"
        //                || comboBox1.Text == row["ClassName"].ToString() && row["Status"].ToString() == "Completed")
        //            {
        //                MessageBox.Show("Choose another License class, the selected person already has an active application " +
        //                                "for the selected class with ID = " + clsPersonInfoWithFilter2.PersonID,
        //                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                return;
        //            }
        //        }


        //        //Load Application
        //        _Application.ApplicantPersonID = clsPersonInfoWithFilter2.PersonID;
        //        _Application.ApplicationDate = Convert.ToDateTime(lab_Date.Text);
        //        _Application.ApplicationTypeID = _ApplicationType.ApplicationTypeID;
        //        _Application.ApplicationStatus = ApplicationStatus;
        //        _Application.LastStatusDate = LastStatusDate;
        //        _Application.PaidFees = _ApplicationType.ApplicationFees;
        //        _Application.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;


        //        // Save Application
        //        if (_Application.SaveApplication())
        //        {
        //           // MessageBox.Show("Application data saved successfully.");

        //            // Load Local Driving
        //            _LocalDriving.ApplicationID = _Application.ApplicationID;
        //            _LocalDriving.LicenseClassID = clsLicenseClasses.FindByClassName(comboBox1.Text).LicenseClassID;

        //            // Save Local Driving
        //            if (_LocalDriving.Save())
        //            {
        //                MessageBox.Show("Local driving license data saved successfully.");
        //                _Mode = enMode.UpdateMode;
        //                label_ID.Text = _LocalDriving.LocalDrivingLicenseApplicationID.ToString();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Error: Local driving license data was not saved successfully.");
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Error: Application data was not saved successfully.");
        //        }


        //    }
        //    else
        //    {

        //        MessageBox.Show("the selected Person Already have an active application " +
        //                           "for the selected class with id = " + clsPersonInfoWithFilter2.PersonID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //}
    }
}
