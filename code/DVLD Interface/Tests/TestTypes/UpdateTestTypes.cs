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

namespace DVLD.Applications
{
    public partial class frmEditTestType : Form
    {

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsTestType _TestType;


        public frmEditTestType(clsTestType.enTestType TestTypeID)
        {

            InitializeComponent();

            _TestTypeID = TestTypeID;

        }

        private void UpdateTestTypes_Load(object sender, EventArgs e)
        {

            _TestType = clsTestType.Find(_TestTypeID);

            if (_TestType != null)
            {


                lblTestTypeID.Text = ((int)_TestTypeID).ToString();
                txtTitle.Text = _TestType.Title;
                txtDescription.Text = _TestType.Description;
                txtFees.Text = _TestType.Fees.ToString();
            }

            else

            {
                MessageBox.Show("Could not find Test Type with id = " + _TestTypeID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

            }
            ////_TestType = clsTestType.Find(_TestID);

            ////if (_TestType == null)
            ////{
            //    //Console.WriteLine("This form will be close because No Test with " + _TestID);
            //    //this.Close();

            //    //return;
            ////}

            ////label_ID.Text = _TestID.ToString();

            ////txt_Title.Text = _TestType.TestTypeTitle.ToString();
            ////txt_Descrptions.Text = _TestType.TestTypeDescription.ToString();
            ////txt_Fees.Text = _TestType.TestTypeFees.ToString();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we don't continue because the form is not valid
                MessageBox.Show("Some filed are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _TestType.Title = txtTitle.Text.Trim();
            _TestType.Description = txtDescription.Text.Trim();
            _TestType.Fees = Convert.ToSingle(txtFees.Text.Trim());


            if (_TestType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


            ////_TestType.TestTypeID = Convert.ToInt32(label_ID.Text);
            ////_TestType.TestTypeTitle = txt_Title.Text;
            ////_TestType.TestTypeDescription = txt_Descrptions.Text;
            ////_TestType.TestTypeFees= Convert.ToDecimal(txt_Fees.Text);


            // //if (_TestType.SaveTestType())

            //     MessageBox.Show("Data Saved Successfully.");
            // //else
            //     MessageBox.Show("Error: Data Is not Saved Successfully.");

            // _Mode = enMode.UpdateMode;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Title cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            };
        }
        private void txtDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDescription, "Description cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtDescription, null);
            };
        }
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            };


            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            };
        }
    }
}
