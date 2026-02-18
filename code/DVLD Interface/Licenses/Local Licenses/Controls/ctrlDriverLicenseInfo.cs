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
using System.IO;
using DVLD.Classes;
using The_DVLD_Project.Properties;

namespace DVLD.UserControls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID;
        private clsLicense _License;

        //int _ApplicationID = -1;

        //public int _LicenseID { get; set; }
        //public int _IsActive { get; set; }
        //public DateTime _ExpirationDate { get; set; }
        //public string NationalNo { get; set; }
        //public string ClassName { get; set; }
        //public bool IsDetained { get; set; }

        public ctrlDriverLicenseInfo()
        {
           
            InitializeComponent();
        }


        public int LicenseID
        {
            get { return _LicenseID; }
        }
        public clsLicense SelectedLicenseInfo
        { get { return _License; } }


        private void _LoadPersonImage()
        {
            if (_License.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _License.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        public void LoadInfo(int LicenseID)
        {
            _LicenseID = LicenseID;
            _License = clsLicense.Find(_LicenseID);
            if (_License == null)
            {
                MessageBox.Show("Could not find License ID = " + _LicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblLicenseID.Text = _License.LicenseID.ToString();
            lblIsActive.Text = _License.IsActive ? "Yes" : "No";
            lblIsDetained.Text = _License.IsDetained ? "Yes" : "No";
            lblClass.Text = _License.LicenseClassIfo.ClassName;
            lblFullName.Text = _License.DriverInfo.PersonInfo.FullName;
            lblNationalNo.Text = _License.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = _License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblDateOfBirth.Text = clsFormat.DateToShort(_License.DriverInfo.PersonInfo.DateOfBirth);

            lblDriverID.Text = _License.DriverID.ToString();
            lblIssueDate.Text = clsFormat.DateToShort(_License.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_License.ExpirationDate);
            lblIssueReason.Text = _License.IssueReasonText;
            lblNotes.Text = _License.Notes == "" ? "No Notes" : _License.Notes;
            _LoadPersonImage();



        }


    }

    //public void DriverLicenseInfoLoad(int LicenseID)
    //{
    //  //  DataTable SL = clsIssueLicense._ShowLicense(LicenseID);

    //    //label_Class.Text = SL.Rows[0]["ClassName"].ToString();
    //    //ClassName = SL.Rows[0]["ClassName"].ToString();

    //    //label_FullName.Text = SL.Rows[0]["FullName"].ToString();

    //    //label_LicenseID.Text = SL.Rows[0]["LicenseID"].ToString();

    //    //label_NationalNo.Text = SL.Rows[0]["NationalNo"].ToString();

    //    //NationalNo = SL.Rows[0]["NationalNo"].ToString();

    //    //if (Convert.ToInt32(SL.Rows[0]["Gendor"]) == 0)
    //    //      label_Gendor.Text = "Male";
    //    //else
    //    //    label_Gendor.Text = "Female";

    //    //label_IssueDate.Text = SL.Rows[0]["IssueDate"].ToString();

    //    //int IssueReason = Convert.ToInt32(SL.Rows[0]["IssueReason"]);
    //    //if (IssueReason == 1)
    //    //    label_IssueReason.Text = "First Time";
    //    //else if(IssueReason == 2)
    //    //    label_IssueReason.Text = "Renew";
    //    //else if (IssueReason == 3)
    //    //    label_IssueReason.Text = "Replacement for Damaged";
    //    //else 
    //    //    label_IssueReason.Text = "Replacement for Lost.";


    //    //if (SL.Rows[0]["Notes"].ToString() == "")
    //    //    label_Notes.Text = "No Notes";
    //    //else
    //    //    label_Notes.Text = SL.Rows[0]["Notes"].ToString();



    //    //_IsActive = Convert.ToInt32(SL.Rows[0]["IsActive"]);
    //    //if (_IsActive == 1)
    //    //    label_IsActive.Text = "Yes";
    //    //else
    //    //    label_IsActive.Text = "No";


    //    //label_DateOfBirth.Text = SL.Rows[0]["DateOfBirth"].ToString();

    //    //label_DriverID.Text = SL.Rows[0]["DriverID"].ToString();


    //    //_ExpirationDate = Convert.ToDateTime(SL.Rows[0]["ExpirationDate"]);
    //    //label_EpirationDate.Text = SL.Rows[0]["ExpirationDate"].ToString();

    //    //string path = SL.Rows[0]["ImagePath"].ToString();

    //    //if (File.Exists(path))
    //    //{
    //    //    pictureBox14.SizeMode = PictureBoxSizeMode.StretchImage;
    //    //    pictureBox14.Load(path);
    //    //}

    //    //_LicenseID = LicenseID;

    //    //IsDetained = clsDetainedLicense._IsDetainedLicense(LicenseID);

    //    //if (IsDetained) 
    //    //{
    //    //    labelIsDetained.Text = "Yes";
    //    //}
    //    //else
    //    //{
    //    //    labelIsDetained.Text = "No";
    //    //}
    //}


}
