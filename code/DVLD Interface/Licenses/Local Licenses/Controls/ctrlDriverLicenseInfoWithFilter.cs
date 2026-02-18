using DVLD_MyBusiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Application;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.UserControls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void PersonSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID); // Raise the event with the parameter
            }
        }
        //Event 
        //public event Action<int> OnLicenseIDSelected;
        //protected virtual void LicenseIDSelected(int LicenseID)
        //{
        //    Action<int> handler = OnLicenseIDSelected;
        //    if(handler != null)
        //    {
        //        handler(LicenseID);
        //    }
        //}




        //public int LicenseID { get; set; }
        //public int _IsActive { get; set; }
        //public DateTime _ExpirationDate { get; set; }
        //public string NationalNo { get; set; }
        //public string ClassName { get; set; }
        //public bool IsDetained { get; set; }


        //clsApplicationType _ApplicationType;

        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        private int _LicenseID = -1;
        public int LicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }
        public clsLicense SelectedLicenseInfo
        { get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; } }


        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLicenseID.Focus();
                return;

            }
            _LicenseID = int.Parse(txtLicenseID.Text);
            LoadLicenseInfo(_LicenseID);
        }



        //في حالة تلقائيا يناديه ويعطيه الرقم ويظهر المعلومات
        public void LoadLicenseInfo(int LicenseID)
        {


            txtLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.LoadInfo(LicenseID);
            _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            if (OnLicenseSelected != null && FilterEnabled)
                // Raise the event with a parameter
                OnLicenseSelected(_LicenseID);


        }


        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }
        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);


            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }
        }

        //private void ttx_LicenseID_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        //    //{
        //    //    e.Handled = true; // Ignore the key
        //    //}

        //}
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //LicenseID = Convert.ToInt32(ttx_LicenseID.Text);
        //    ////if (clsIssueLicense._ShowLicense(LicenseID).Rows.Count == 0)
        //    ////{
        //    ////    MessageBox.Show("No License with " + LicenseID + " LicenseID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    ////    return;
        //    ////}

        //    //driverLicenseInfo1.DriverLicenseInfoLoad(Convert.ToInt32(ttx_LicenseID.Text));

        //    //LicenseID = driverLicenseInfo1._LicenseID;
        //    //NationalNo = driverLicenseInfo1.NationalNo;
        //    //ClassName = driverLicenseInfo1.ClassName;
        //    //IsDetained = driverLicenseInfo1.IsDetained;

        //    //_IsActive = driverLicenseInfo1._IsActive;
        //    //_ExpirationDate = driverLicenseInfo1._ExpirationDate;



        //    //if (OnLicenseIDSelected != null)
        //    //{
        //    //    LicenseIDSelected(LicenseID);
        //    //}
        //}


        //public void ReleaseLicenseByReadyDetainID(int LicenseID)
        //{
        //    //driverLicenseInfo1.DriverLicenseInfoLoad(LicenseID);

        //    //groupBox1.Enabled = false;
        //    //ttx_LicenseID.Text = LicenseID.ToString();

        //    //LicenseID = driverLicenseInfo1._LicenseID;
        //    //NationalNo = driverLicenseInfo1.NationalNo;
        //    //ClassName = driverLicenseInfo1.ClassName;
        //    //IsDetained = driverLicenseInfo1.IsDetained;

        //    //_IsActive = driverLicenseInfo1._IsActive;
        //    //_ExpirationDate = driverLicenseInfo1._ExpirationDate;

        //}

    }
}
