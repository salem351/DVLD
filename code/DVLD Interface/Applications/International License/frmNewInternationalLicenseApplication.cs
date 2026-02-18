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
using DVLD.License;
using DVLD.UserControls;
using static System.Net.Mime.MediaTypeNames;
using DVLD.Classes;
using The_DVLD_Project.Licenses;

namespace DVLD.Application
{
    public partial class frmNewInternationalLicenseApplication : Form
    {

        private int _InternationalLicenseID = -1;


        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();       
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));//add one year.
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;

            lblLocalLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)

            {
                return;
            }



            //check the license class, person could not issue international license without having
            //normal license of class 3.

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check if person already have an active international license
            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if (ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternationalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                btnIssueLicense.Enabled = false;
                return;
            }

            btnIssueLicense.Enabled = true;

        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsInternationalLicense InternationalLicense = new clsInternationalLicense();
            //those are the information for the base application, because it inhirts from application, they are part of the sub class.

            InternationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense.ApplicationDate = DateTime.Now;
            InternationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            InternationalLicense.LastStatusDate = DateTime.Now;
            InternationalLicense.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            InternationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense.IssueDate = DateTime.Now;
            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            //من نوع رخصة دولية application  داخله نعمل على حفظ ال
            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;

        }


        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm =
             new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm =
            new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void frmNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();

        }



        //private void New_International__License_Application_Load(object sender, EventArgs e)
        //{
        //    btnIssue.Enabled = false;

        //    label_ApplicationDate.Text = DateTime.Now.ToString();
        //    label_IssueDate.Text = DateTime.Now.ToString();
        //    label8.Text = CurrentUserInfo.GetCurrentUser().UserName;
        //    _ApplicationType = clsApplicationType.FindByApplicationTypeID(6);
        //    label_Fees.Text = _ApplicationType.ApplicationFees.ToString();


        //}

        //private void btnClose1_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}


        ////Event when I click to the Search button
        //private void driver_License_Info_with_filter1_OnLicenseIDSelected(int obj)
        //{
        //    if (clsInternationalLicense._IsInternationalLicenseExistByLocalLicense(Convert.ToInt32(driver_License_Info_with_filter1.LicenseID)))
        //    {
        //        InternationalLicenseID = clsInternationalLicense._GetInternationalLicenseByLocalLicense(Convert.ToInt32(driver_License_Info_with_filter1.LicenseID));

        //        MessageBox.Show("Person already have an active international license ID = " + InternationalLicenseID, "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //        linkLabel_LicenseHistory.Enabled = true;
        //        linkLabel_LicenseInfo.Enabled = true;

        //        return;
        //    }


        //    btnIssue.Enabled = true;

        //    LicenseID = obj;

        //    label_LocalLince.Text = obj.ToString();

        //    linkLabel_LicenseHistory.Enabled = true;

        //    label_Expirationdate.Text = DateTime.Now.AddYears(10).ToString();


        //}

        //private void btnIssue_Click(object sender, EventArgs e)
        //{

        //    if (driver_License_Info_with_filter1._IsActive == 1 && driver_License_Info_with_filter1._ExpirationDate > DateTime.Now)
        //    {
        //        if (MessageBox.Show("Are you sure you want to Issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            //اولا نحفظ طلب جديد
        //            clsApplications Application = new clsApplications();

        //            Application.ApplicantPersonID = clsPerson.Find(driver_License_Info_with_filter1.NationalNo).PersonID;
        //            Application.ApplicationDate = DateTime.Now;
        //            Application.LastStatusDate = DateTime.Now;
        //            Application.ApplicationTypeID = 6;
        //            Application.ApplicationStatus = 3;
        //            Application.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;
        //            Application.PaidFees = clsApplicationType._GetApplicationTypesFee(Application.ApplicationTypeID);

        //            if (Application.SaveApplication())
        //            {

        //            }


        //            int PersonID = clsApplications._GetApplicationPersonID(Application.ApplicationID);

        //            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

        //            InternationalLicense.ApplicationID = Application.ApplicationID;
        //            InternationalLicense.DriverID = clsDrivers.FindByPersonID(PersonID).DriverID;
        //            InternationalLicense.IssuedUsingLocalLicenseID = LicenseID;
        //            InternationalLicense.IssueDate = DateTime.Now;
        //            InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
        //            InternationalLicense.IsActive = driver_License_Info_with_filter1._IsActive == 1;
        //            InternationalLicense.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;

        //            if (InternationalLicense.Save())
        //            {
        //                MessageBox.Show("International license Issued Successfully with ID = " + InternationalLicense.InternationalLicenseID, "License Issued", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //                linkLabel_LicenseInfo.Enabled = true;
        //                InternationalLicenseID = InternationalLicense.InternationalLicenseID;
        //                btnIssue.Enabled = false;
        //                label_ILLicenseID1.Text = InternationalLicenseID.ToString();
        //                label_ID.Text = InternationalLicense.ApplicationID.ToString();
        //            }



        //        }

        //    }
        //    else
        //    {

        //    }
        //}





        //private void linkLabel_LicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    InternationalLicenseID = clsInternationalLicense._GetInternationalLicenseByLocalLicense(Convert.ToInt32(driver_License_Info_with_filter1.LicenseID));

        //    Internnational_Driver_Info frm = new Internnational_Driver_Info(InternationalLicenseID);
        //    frm.ShowDialog();
        //}
        //private void linkLabel_LicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    ApplicationID = clsInternationalLicense._GetInternationalLicenseByApplicationID(Convert.ToInt32(driver_License_Info_with_filter1.LicenseID));

        //    LicenseHistore frm = new LicenseHistore(clsApplications._GetApplicationPersonID(ApplicationID));
        //    frm.ShowDialog();
        //}


    }
}
