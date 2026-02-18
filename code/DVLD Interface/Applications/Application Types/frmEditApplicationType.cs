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

namespace DVLD.Applications
{
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        private clsApplicationType _ApplicationType;



        public frmEditApplicationType(int ApplicationTypeID)
        {
            _ApplicationTypeID = ApplicationTypeID;

            InitializeComponent();
        }

        private void UpdateApplication_Type_Load(object sender, EventArgs e)
        {
            lblApplicationTypeID.Text = _ApplicationTypeID.ToString();

            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if (_ApplicationType != null)
            {
                txtTitle.Text = _ApplicationType.Title;
                txtFees.Text = _ApplicationType.Fees.ToString();


            }



            //_Application = clsApplicationType.Find(_ApplicationID);

            //if (_Application == null)
            //{
            //    Console.WriteLine("This form will be close because No application with " + _ApplicationID);
            //    this.Close();

            //    return;
            //}

            //label_ID.Text = _ApplicationID.ToString();

            ////txt_Title.Text = _Application.ApplicationTypeTitle.ToString();
            ////txt_Fees.Text = _Application.ApplicationFees.ToString();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we don't continue because the form is not valid
                MessageBox.Show("Some filed are not valid!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            _ApplicationType.Title = txtTitle.Text.Trim();
            _ApplicationType.Fees = Convert.ToSingle(txtFees.Text.Trim());


            if (_ApplicationType.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);





            ////_Application.ApplicationTypeID = Convert.ToInt32(label_ID.Text);
            ////_Application.ApplicationTypeTitle = txt_Title.Text;
            ////_Application.ApplicationFees = Convert.ToDecimal(txt_Fees.Text);


            //if (_Application.Save())

            //    MessageBox.Show("Data Saved Successfully.");
            //else
            //    MessageBox.Show("Error: Data Is not Saved Successfully.");

            //_Mode = enMode.UpdateMode;

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
