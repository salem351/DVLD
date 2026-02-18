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
using DVLD.Internaional_Licesne;
using static System.Net.Mime.MediaTypeNames;
using DVLD.License;
using DVLD.Classes;
using DVLD.UserControls;
using The_DVLD_Project.Licenses.Local_Licenses;
using The_DVLD_Project.Licenses;

namespace DVLD.Local_License.Renew
{
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        private int _NewLicenseID = -1;

        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();


            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;

            lblExpirationDate.Text = "???";
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)

            {
                return;
            }

            int DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassIfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassIfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text) + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            txtNotes.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;


            //check the license is not Expired.
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not yet expired, it will expire on: " + clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate)
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            //check the license is not active.
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }



            btnRenewLicense.Enabled = true;

        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense =
                ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text.Trim(),
                clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Renew the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Renewed Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);


            btnRenewLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;


        }

        private void frmRenewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm =
               new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm =
           new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }




        //private void RenewLocalDrivingLicense_Load(object sender, EventArgs e)
        //{
        //    btnRenew.Enabled = false;

        //    label_ApplicationDate.Text = DateTime.Now.ToString();
        //    label_IssueDate.Text = DateTime.Now.ToString();
        //    label8.Text = CurrentUserInfo.GetCurrentUser().UserName;
        //    _ApplicationType = clsApplicationType.FindByApplicationTypeID(2);
        //    label_ApplicationFees.Text =  Convert.ToInt32(_ApplicationType.ApplicationFees).ToString();

        //}

        //private void driver_License_Info_with_filter1_OnLicenseIDSelected(int obj)
        //{
        //    if (DateTime.Now < driver_License_Info_with_filter1._ExpirationDate)
        //    {

        //        MessageBox.Show("Selected License is not yet expired, it will expire on: " +  driver_License_Info_with_filter1._ExpirationDate, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        linkLabel_LicenseHistory.Enabled = true;

        //        return;
        //    }



        //    LicenseID = obj;
        //    int LicesneFees = Convert.ToInt32(clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).ClassFees);

        //    label_LocalLince.Text = obj.ToString();
        //    label_Expirationdate.Text = DateTime.Now.AddYears(10).ToString();        
        //    label_LicesneFees.Text = LicesneFees.ToString();
        //    label_TotalFees.Text = Convert.ToInt32((LicesneFees + clsApplicationType.FindByApplicationTypeID(2).ApplicationFees)).ToString();

        //    linkLabel_LicenseHistory.Enabled = true;
        //    btnRenew.Enabled = true;

        //    label_Expirationdate.Text = DateTime.Now.AddYears(10).ToString();


        //}

        //private void btnClose1_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}




        //private void btnRenew_Click(object sender, EventArgs e)
        //{
        //    if (driver_License_Info_with_filter1._ExpirationDate < DateTime.Now)
        //    {
        //        if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            //اولا نحفظ طلب جديد
        //            clsApplications Application = new clsApplications();

        //            Application.ApplicantPersonID = clsPerson.Find(driver_License_Info_with_filter1.NationalNo).PersonID;
        //            Application.ApplicationDate = DateTime.Now;
        //            Application.LastStatusDate = DateTime.Now;
        //            Application.ApplicationTypeID = 2;
        //            Application.ApplicationStatus = 3;
        //            Application.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;
        //            Application.PaidFees = clsApplicationType._GetApplicationTypesFee(Application.ApplicationTypeID);

        //            if (Application.SaveApplication())
        //            {

        //            }


        //            int PersonID = clsApplications._GetApplicationPersonID(Application.ApplicationID);

        //            clsIssueLicense IssueLicense = new clsIssueLicense();

        //            IssueLicense.ApplicationID = Application.ApplicationID;
        //            IssueLicense.LicenseClass = clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).LicenseClassID;
        //            IssueLicense.DriverID = clsDrivers.FindByPersonID(PersonID).DriverID;
        //            IssueLicense.IssueDate = DateTime.Now;
        //            IssueLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).DefaultValidityLength);
        //            IssueLicense.Notes = textBox_Notes.Text;
        //            IssueLicense.PaidFees = clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).ClassFees;
        //            IssueLicense.IsActive = true;
        //            IssueLicense.IssueReason = 2;
        //            IssueLicense.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;

        //            if (IssueLicense.Save())
        //            {
        //                MessageBox.Show("Renew license Issued Successfully with ID = " + IssueLicense.LicenseID, "License Issued", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //                linkLabel_LicenseInfo.Enabled = true;


        //                btnRenew.Enabled = false;
        //                label_ILLicenseID1.Text = IssueLicense.LicenseID.ToString();
        //                label_R_LicenseID.Text = IssueLicense.ApplicationID.ToString();


        //                //الان الرخصة القديمة بنعمل لها NotActive
        //                if (clsIssueLicense._UpdateToBeNotActive(LicenseID))
        //                {

        //                }
        //            }



        //        }

        //    }
        //    else
        //    {

        //    }
        //}



        //private void linkLabel_LicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{

        //    LicenseHistore frm = new LicenseHistore(clsApplications._GetApplicationPersonID(Convert.ToInt32(label_R_LicenseID.Text)));
        //    frm.ShowDialog();
        //}
        //private void linkLabel_LicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{

        //    LicenseInfo frm = new LicenseInfo(Convert.ToInt32(label_ILLicenseID1.Text));
        //    frm.ShowDialog();
        //}

        //private void driver_License_Info_with_filter1_Load(object sender, EventArgs e)
        //{

        //}
    }
}
