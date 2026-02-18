 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Classes;
using DVLD.Properties;
using DVLD_MyBusiness;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Runtime.ConstrainedExecution;
using The_DVLD_Project.Properties;

namespace DVLD
{
    public partial class frmAddUpdatePerson : Form
    {
        // Declare a delegate
        public delegate void DataBackEventHandler(object sender, int PersonID);
        // Declare an event using the delegate
        public event DataBackEventHandler DataBack;



        public enum enMode { AddMode, UpdateMode }
        public enum enGendor { Male = 0, Female = 1 };

        private enMode _Mode;

        string destinationPath;
        clsPerson _Person;
        int _PersonID = -1;

        //Add mode calling 
        public frmAddUpdatePerson()
        {
            InitializeComponent();

            _Mode = enMode.AddMode;
        }
        //Update mode calling 
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            _Mode = enMode.UpdateMode;
        }


        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountry();

            foreach (DataRow Row in dtCountries.Rows)
            {
                cbCountry.Items.Add(Row["CountryName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            //this will initialize the reset the default values
            _FillCountriesInComboBox();

            if (_Mode == enMode.AddMode)
            {
                lblTitle.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person";
            }

            //set default image for the person.
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;


            //hide/show the remove linke incase there is no image for the person.
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);


            //we set the max date to 18 years from today, and set the default value the same.
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            //should not allow adding age more than 100 years
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);


            //this will set default country to jordan.
            cbCountry.SelectedIndex = cbCountry.FindString("Yemen");


            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";


        }

        private void _LoadData()
        {

            _Person = clsPerson.Find(_PersonID);

            if (_Person == null)
            {
                Console.WriteLine("This form will be close because No Person with " + _PersonID);
                this.Close();

                return;
            }

            lblTitle.Text = "Edit Person ID = " + _PersonID;
            lblPersonID.Text = _PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;

            txtNationalNo.Text = _Person.NationalNo;
            //علشان ما تغيره,, واذا عملته صح بيعملك مشاكل في التعديل لان بيعتبره موجود
            txtNationalNo.Enabled = false;

            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;


            txtEmail.Text = _Person.Email;
            txtAddress.Text = _Person.Address;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            txtPhone.Text = _Person.Phone;
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName);


            //load person image incase it was set.
            if (_Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;

                //الان اللود يعمل على تقفيل الصورة فإذا جيت تعمل لها حدف يعطيك خطأ
                //pbPersonImage.Load(_Person.ImagePath);
            }


            //hide/show the remove link incase there is no image for the person.
            llRemoveImage.Visible = (_Person.ImagePath != "");
        }
        private void AddEditPersonInfo_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if (_Mode == enMode.UpdateMode)
                _LoadData();
        }






        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.


            //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
            //1- اذا فيه صورة وانت ماعدلت عليها اذا بينزل لانهم نفس الباث       
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                //Update Mode
                //2- اذا ماشي صورة بينزل
                //واذا فيه صورة بنحدفها ثم تحت نضيف صورة او فقط عملت حدف للصورة هنا
                if (_Person.ImagePath != "")
                {
                    //first we delete the old image from the folder in case there is any.

                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {
                        // We could not delete the file.
                        //log it later   
                    }
                }

                //Add Mode
                //3- اذا ماشي صورة بينزل 
                //واذا اضفت صورة بيضيفها
                if (pbPersonImage.ImageLocation != null)
                {
                    //then we copy the new image to the image folder after we rename it
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

            }
            return true;
        }

        //private void SavePhotoInFolderWithGuidID()
        //{
        //    //Copy the image to folder
        //    string sourceFile = pbPersonImage.ImageLocation; // full path to original image
        //    string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(sourceFile);
        //    string destinationFolder = @"D:\شهادات كورسات محمد ابو هدهود\كورس 19\DVLD-People-Image";
        //    destinationPath = Path.Combine(destinationFolder, newFileName);
        //    _Person.ImagePath = destinationPath;
        //    // Ensure folder exists (optional safety)
        //    Directory.CreateDirectory(destinationFolder);

        //    // Copy the file
        //    File.Copy(sourceFile, destinationPath, overwrite: true);
        //}
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we don't continue because the form is not valid
                MessageBox.Show("Some filed are not valid!, put the mouse over the red icon(s) to see the error", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }


            if (!_HandlePersonImage())
                return;


            int NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;


            _Person.NationalNo = txtNationalNo.Text;
            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;

            if (rbMale.Checked)
                _Person.Gendor = (short)enGendor.Male;
            else
                _Person.Gendor = (short)enGendor.Female;
            _Person.Email = txtEmail.Text;
            _Person.Phone = txtPhone.Text;
            _Person.Address = txtAddress.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.NationalityCountryID = NationalityCountryID;

            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";


            //////// مهم جدا هذا حلي انا فخور جدا 
            ////////Just enter If already photo that Contain GUID
            //if (_Mode == enMode.UpdateMode && DeletePhoto)
            //{
            //    if (_Person.ImagePath != "")
            //    {
            //        File.Delete(_Person.ImagePath);
            //        _Person.ImagePath = null;
            //    }

            //    //inside this function I store new path which is include GUID Name to store ((_Person.ImagePath)) because then If I want to delet it it will be easy
            //    SavePhotoInFolderWithGuidID();
            //    pbPersonImage.ImageLocation = _Person.ImagePath;
            //}

            //else if (_Mode == enMode.AddMode && pbPersonImage.ImageLocation != null)
            //{
            //    //inside this function I store new path which is include GUID Name to store ((_Person.ImagePath)) because then If I want to delet it it will be easy
            //    SavePhotoInFolderWithGuidID();
            //    pbPersonImage.ImageLocation = _Person.ImagePath;
            //}
            //else
            //{
            //    if(pbPersonImage.ImageLocation != null)
            //        _Person.ImagePath = pbPersonImage.ImageLocation;
            //    else
            //        _Person.ImagePath = "";
            //}

            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                //change form mode to update.
                _Mode = enMode.UpdateMode;
                lblTitle.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.ImageLocation = null;



            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }
        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
                // ...
            }
        }




        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


       

        private void rdb_Female_CheckedChanged(object sender, EventArgs e)
        {
            //change the default image to female incase there is no image set.
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }
        private void rdb_Male_CheckedChanged(object sender, EventArgs e)
        {
            //change the default image to male incase there is no image set.
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }



        //هذا لهم كلهم
        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox)sender);

            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate the email incase it's empty.
            if (txtEmail.Text.Trim() == "")
                return;

            //validate email format
            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            };

        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }


            //Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.IsPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");

            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

        
    }
}
