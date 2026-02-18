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
using DVLD.License;
using static DVLD_MyBusiness.clsLicense;
using DVLD.Classes;
using DVLD.UserControls;
using The_DVLD_Project.Licenses.Local_Licenses;
using The_DVLD_Project.Licenses;

namespace DVLD.Damage_and_Lost
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        //clsApplicationType _ApplicationType;

        private int _NewLicenseID = -1;

        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        {
            //this will decide which application type to use accirding 
            // to user selection.

            if (rbDamagedLicense.Checked)

                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
        }
        private enIssueReason _GetIssueReason()
        {
            //this will decide which reason to issue a replacement for

            if (rbDamagedLicense.Checked)

                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;

            rbDamagedLicense.Checked = true;
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }
        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();

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

            //don't allow a replacement if is Active .
            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }
            //not active لان مانصير نعمل رخصة مفقوده وهي active ماشي داعي نشيك لانتهاء الرخصة لان مانريد الا فقط نشوف هل 

            btnIssueReplacement.Enabled = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure you want to Issue a Replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            clsLicense NewLicense =
               ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(),
               clsGlobal.CurrentUser.UserID);

            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Issue a replacemnet for this  License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = NewLicense.ApplicationID.ToString();
            _NewLicenseID = NewLicense.LicenseID;

            lblRreplacedLicenseID.Text = _NewLicenseID.ToString();
            MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;

        }


        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm =
                 new frmShowLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }
        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicenseHistory frm =
            new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnClose1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        //private void Replacement_for_Damaged_or_Lost_Licenses_Load(object sender, EventArgs e)
        //{
        //    btn_IssueReplacemnt.Enabled = false;
        //    radioButton_DamagedLicense.Checked = true;
        //    label_ApplicationDate.Text = DateTime.Now.ToString();          
        //    label8.Text = CurrentUserInfo.GetCurrentUser().UserName;

        //    if (radioButton_DamagedLicense.Checked)
        //    {
        //        _ApplicationType = clsApplicationType.FindByApplicationTypeID(4);
        //        label_ApplicationFees.Text = Convert.ToInt32(_ApplicationType.ApplicationFees).ToString();
        //    }
        //    else{
        //        _ApplicationType = clsApplicationType.FindByApplicationTypeID(3);
        //        label_ApplicationFees.Text = Convert.ToInt32(_ApplicationType.ApplicationFees).ToString();
        //    }

        //}

        //private void driver_License_Info_with_filter1_OnLicenseIDSelected(int obj)
        //{
        //    if (driver_License_Info_with_filter1._IsActive == 0)
        //    {

        //        MessageBox.Show("Selected License is not Active", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);


        //        linkLabel_LicenseHistory.Enabled = true;
        //        linkLabel_LicenseInfo.Enabled = true;

        //        label_LocalLince.Text = obj.ToString();

        //        return;
        //    }

        //    LicenseID = obj;

        //    btn_IssueReplacemnt.Enabled = true;
        //    linkLabel_LicenseHistory.Enabled = true;
        //    btn_IssueReplacemnt.Enabled = true;

        //    label_LocalLince.Text = obj.ToString();

        //}

        //private void btnClose1_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}




        //private void radioButton_DamagedLicense_CheckedChanged(object sender, EventArgs e)
        //{
        //    _ApplicationType = clsApplicationType.FindByApplicationTypeID(4);
        //    label_ApplicationFees.Text = Convert.ToInt32(_ApplicationType.ApplicationFees).ToString();

        //    label2.Text = "Replacement for Damaged License";
        //    this.Text = "Replacement for Damaged License";
        //}
        //private void radioButton_LostLicense_CheckedChanged(object sender, EventArgs e)
        //{
        //    _ApplicationType = clsApplicationType.FindByApplicationTypeID(3);
        //    label_ApplicationFees.Text = Convert.ToInt32(_ApplicationType.ApplicationFees).ToString();

        //    label2.Text = "Replacement for Lost License";
        //    this.Text = "Replacement for Lost License";
        //}





        //private void btn_IssueReplacemnt_Click(object sender, EventArgs e)
        //{
        //    if (driver_License_Info_with_filter1._IsActive == 1)
        //    {
        //        if (MessageBox.Show("Are you sure you want to Replacement the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        //        {
        //            //اولا نحفظ طلب جديد
        //            clsApplications Application = new clsApplications();

        //            Application.ApplicantPersonID = clsPerson.Find(driver_License_Info_with_filter1.NationalNo).PersonID;
        //            Application.ApplicationDate = DateTime.Now;
        //            Application.LastStatusDate = DateTime.Now;
        //            if (radioButton_DamagedLicense.Checked)
        //            {
        //                Application.ApplicationTypeID = 4;
        //            }
        //            else
        //            {
        //                Application.ApplicationTypeID = 3;
        //            }
        //            Application.ApplicationStatus = 3;
        //            Application.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;
        //            Application.PaidFees = clsApplicationType._GetApplicationTypesFee(Application.ApplicationTypeID);

        //            if (Application.SaveApplication())
        //            {

        //            }

        //            //الان احفظ الرخصة الجديد والغي القديمة
        //            int PersonID = clsApplications._GetApplicationPersonID(Application.ApplicationID);

        //            clsIssueLicense IssueLicense = new clsIssueLicense();

        //            IssueLicense.ApplicationID = Application.ApplicationID;
        //            IssueLicense.LicenseClass = clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).LicenseClassID;
        //            IssueLicense.DriverID = clsDrivers.FindByPersonID(PersonID).DriverID;
        //            IssueLicense.IssueDate = DateTime.Now;
        //            IssueLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).DefaultValidityLength);
        //            IssueLicense.Notes = "";
        //            IssueLicense.PaidFees = clsLicenseClasses.FindByClassName(driver_License_Info_with_filter1.ClassName).ClassFees;
        //            IssueLicense.IsActive = true;
        //            if (radioButton_DamagedLicense.Checked)
        //            {
        //                IssueLicense.IssueReason = 3;
        //            }
        //            else
        //            {
        //                IssueLicense.IssueReason = 4;
        //            }
        //            IssueLicense.CreatedByUserID = CurrentUserInfo.GetCurrentUser().UserID;

        //            if (IssueLicense.Save())
        //            {
        //                MessageBox.Show("License Replaced Successfully with ID = " + IssueLicense.LicenseID, "License Issued", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        //                linkLabel_LicenseInfo.Enabled = true;


        //                btn_IssueReplacemnt.Enabled = false;
        //                label_ILLicenseID1.Text = IssueLicense.LicenseID.ToString();
        //                label_L_R_LicenseID.Text = IssueLicense.ApplicationID.ToString();


        //                //الان الرخصة القديمة بنعمل لها NotActive
        //                if (clsIssueLicense._UpdateToBeNotActive(LicenseID))
        //                {

        //                }
        //            }



        //        }

        //    }
        //}

        //private void linkLabel_LicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    LicenseHistore frm = new LicenseHistore(clsApplications._GetApplicationPersonID(Convert.ToInt32(label_L_R_LicenseID.Text)));
        //    frm.ShowDialog();
        //}

        //private void linkLabel_LicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{

        //    LicenseInfo frm = new LicenseInfo(Convert.ToInt32(label_ILLicenseID1.Text));
        //    frm.ShowDialog();
        //}
    }
}
