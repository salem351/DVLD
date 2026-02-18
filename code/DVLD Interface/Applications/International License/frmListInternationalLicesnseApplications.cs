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
using DVLD.Application;
using static System.Net.Mime.MediaTypeNames;
using DVLD.License;
using DVLD.UserControls;
using DVLD.Main_Forms;
using Business_Layer;
using The_DVLD_Project.Licenses;

namespace DVLD.Internaional_Licesne
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        private DataTable _dtInternationalLicenseApplications;

        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtInternationalLicenseApplications = clsInternationalLicense.GetAllInternationalLicenses();
            cbFilterBy.SelectedIndex = 0;

            dgvInternationalLicenses.DataSource = _dtInternationalLicenseApplications;
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();

            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 160;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;

            }
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
            //refresh
            frmListInternationalLicenseApplications_Load(null, null);
        }
        private void PersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicenses.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenses.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }



        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }

            else

            {

                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsReleased.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                    //_dtDetainedLicenses.DefaultView.RowFilter = "";
                    //lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }

            else

            {

                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsReleased.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;
                    //_dtDetainedLicenses.DefaultView.RowFilter = "";
                    //lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();

                }
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    {
                        FilterColumn = "ApplicationID";
                        break;
                    };

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;

                case "Is Active":
                    FilterColumn = "IsActive";
                    break;


                default:
                    FilterColumn = "None";
                    break;
            }


            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtInternationalLicenseApplications.DefaultView.RowFilter = "";
                lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
                return;
            }



            _dtInternationalLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());

            lblInternationalLicensesRecords.Text = _dtInternationalLicenseApplications.Rows.Count.ToString();
        }
        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow numbers only becasue all fiters are numbers.
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }



        //private void _RefreshApplicationsList()
        //{
        //    dataGridView1.DataSource = clsInternationalLicense._GetAllInternationalLicense();
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //}

        //private void ListInternationalLicenseApplications_Load(object sender, EventArgs e)
        //{
        //    DataTable dt = clsInternationalLicense._GetAllInternationalLicense();

        //    dataGridView1.DataSource = clsInternationalLicense._GetAllInternationalLicense();
        //    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        //    label_Records.Text = dt.Rows.Count.ToString();

        //    _RefreshApplicationsList();
        //}

        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();

        //}

        //private void _AddApplication()
        //{
        //    New_International__License_Application International__License = new New_International__License_Application();
        //    International__License.StartPosition = FormStartPosition.CenterScreen;
        //    International__License.ShowDialog();

        //}
        //private void btnApply_Click(object sender, EventArgs e)
        //{
        //    _AddApplication();
        //    _RefreshApplicationsList();
        //}



        //private void TSMI_ShowPersonLicenseHistory_Click(object sender, EventArgs e)
        //{
        //    LicenseHistore frm = new LicenseHistore(clsApplications._GetApplicationPersonID((int)dataGridView1.CurrentRow.Cells[1].Value));
        //    frm.ShowDialog();

        //    _RefreshApplicationsList();
        //}
        //private void TSMI_ShowLicense_Click(object sender, EventArgs e)
        //{

        //    Internnational_Driver_Info frm = new Internnational_Driver_Info((int)dataGridView1.CurrentRow.Cells[0].Value);
        //    frm.ShowDialog();

        //    _RefreshApplicationsList();
        //}
        //private void TSMI_ShowDetails_Click(object sender, EventArgs e)
        //{
        //    int PersonID = clsApplications._GetApplicationPersonID((int)dataGridView1.CurrentRow.Cells[1].Value);

        //    Person_Details PersonDetails = new Person_Details(PersonID);
        //    PersonDetails.StartPosition = FormStartPosition.CenterScreen;
        //    PersonDetails.ShowDialog();

        //    _RefreshApplicationsList();
        //}


        ////Filter
        //private void txbFilter_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (combFilter.Text == "Int.License" || combFilter.Text == "Application ID" || combFilter.Text == "Driver ID" || combFilter.Text == "L.License")
        //    {

        //        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        //        {
        //            e.Handled = true; // Ignore the key
        //        }
        //    }

        //}
        //private void combFilter_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    _RefreshApplicationsList();

        //    if ((combFilter.SelectedItem.ToString() != "None") && (combFilter.SelectedItem.ToString() != "IsActive"))
        //    {
        //        txbFilter.Text = "";

        //        txbFilter.Visible = true;
        //        txbFilter.Focus();

        //        combIsActive.Visible = false;
        //        //MessageBox.Show(combFilter.Text);           
        //    }
        //    else if ((combFilter.SelectedItem.ToString() == "IsActive"))
        //    {
        //        txbFilter.Visible = false;
        //        combIsActive.Visible = true;
        //        combIsActive.Focus();
        //    }
        //    else
        //    {
        //        combIsActive.Visible = false;
        //        txbFilter.Visible = false;
        //    }
        //}
        //private void txbFilter_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrWhiteSpace(txbFilter.Text) && combFilter.Text == "Int.License")
        //    {
        //        int Int_License;
        //        if (int.TryParse(txbFilter.Text, out Int_License))
        //        {
        //            DataTable License = clsInternationalLicense._GetByInt_License(Int_License);

        //            if (License != null)
        //            {
        //                dataGridView1.DataSource = License;
        //            }
        //            else
        //            {
        //                dataGridView1.DataSource = null;

        //            }
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(txbFilter.Text) && combFilter.Text == "Application ID")
        //    {
        //        int ApplicationID;
        //        if (int.TryParse(txbFilter.Text, out ApplicationID))
        //        {
        //            DataTable License = clsInternationalLicense._GetByApplicationID(ApplicationID);

        //            if (License != null)
        //            {
        //                dataGridView1.DataSource = License;
        //            }
        //            else
        //            {
        //                dataGridView1.DataSource = null;

        //            }
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(txbFilter.Text) && combFilter.Text == "Driver ID")
        //    {
        //        int DriverID;
        //        if (int.TryParse(txbFilter.Text, out DriverID))
        //        {
        //            DataTable License = clsInternationalLicense._GetByDriverID(DriverID);

        //            if (License != null)
        //            {
        //                dataGridView1.DataSource = License;
        //            }
        //            else
        //            {
        //                dataGridView1.DataSource = null;

        //            }
        //        }
        //    }
        //    if (!string.IsNullOrWhiteSpace(txbFilter.Text) && combFilter.Text == "L.License")
        //    {
        //        int L_License;
        //        if (int.TryParse(txbFilter.Text, out L_License))
        //        {
        //            DataTable License = clsInternationalLicense._GetByL_License(L_License);

        //            if (License != null)
        //            {
        //                dataGridView1.DataSource = License;
        //            }
        //            else
        //            {
        //                dataGridView1.DataSource = null;

        //            }
        //        }
        //    }      

        //    //اذا هو فاضي اعمل رفريش
        //    if (string.IsNullOrWhiteSpace(txbFilter.Text))
        //    {
        //        _RefreshApplicationsList();
        //    }
        //}
        //private void combIsActive_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (combFilter.Text == "IsActive")
        //    {
        //        DataTable Licenses = null;



        //        if (combIsActive.Text == "All")
        //        {
        //            Licenses = clsInternationalLicense._GetAllInternationalLicense();
        //        }
        //        else if (combIsActive.Text == "Yes")
        //        {
        //            Licenses = clsInternationalLicense.GetByIsActive(1);
        //        }
        //        else if (combIsActive.Text == "No")
        //        {
        //            Licenses = clsInternationalLicense.GetByIsActive(0);
        //        }

        //        dataGridView1.DataSource = Licenses ?? null;
        //    }
        //}
    }
}
