using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_MyBusiness;
using DVLD.Application;
using DVLD.Applications;
using DVLD.Apply;
using DVLD.Damage_and_Lost;
using DVLD.Detain_Licenses;
using DVLD.Internaional_Licesne;
using DVLD.Local_License.Renew;
using DVLD.Main_Forms;
using DVLD.Classes;
using The_DVLD_Project.Tests.TestTypes;

namespace DVLD
{
    public partial class frmMain : Form
    {
        frmLogin _frmLogin;

        public frmMain(frmLogin frm)
        {
            InitializeComponent();
           _frmLogin = frm;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListPeople ManageForm = new frmListPeople();
            ManageForm.StartPosition = FormStartPosition.CenterScreen;
            ManageForm.ShowDialog();
        }

        //هنا تكمن فائدة ارسال الفورم حق التسجيل الى هنا
        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // مسح المستخدم الحالي
            clsGlobal.CurrentUser = null;

            // إظهار فورم تسجيل الدخول
            _frmLogin.Show();

            // إغلاق الفورم الرئيسي
            this.Close();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListUsers ManagementUsers1 = new frmListUsers();
            ManagementUsers1.StartPosition = FormStartPosition.CenterScreen;
            ManagementUsers1.ShowDialog();
        }

        private void currentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ShowUserDetails ShowUserDetails1 = new ShowUserDetails(CurrentUserInfo.GetCurrentUser().UserID);
            //ShowUserDetails1.StartPosition = FormStartPosition.CenterScreen;
            //ShowUserDetails1.ShowDialog();
        }
        private void cToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //chnagePassword chnagePassword1 = new chnagePassword(CurrentUserInfo.GetCurrentUser().UserID);
            //chnagePassword1.StartPosition = FormStartPosition.CenterScreen;
            //chnagePassword1.ShowDialog();
        }

        private void manageApplicationsTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListApplicationTypes ApplicationTypes = new frmListApplicationTypes();
            ApplicationTypes.StartPosition = FormStartPosition.CenterScreen;
            ApplicationTypes.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes ManageTestType = new frmListTestTypes();
            ManageTestType.StartPosition = FormStartPosition.CenterScreen;
            ManageTestType.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicesnseApplication NewLocal = new frmAddUpdateLocalDrivingLicesnseApplication();
            NewLocal.StartPosition = FormStartPosition.CenterScreen;
            NewLocal.ShowDialog();
        }

        private void localLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicesnseApplications frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers _Drivers = new frmListDrivers();
            _Drivers.StartPosition = FormStartPosition.CenterScreen;
            _Drivers.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmNewInternationalLicenseApplication International__License = new frmNewInternationalLicenseApplication();
            International__License.StartPosition = FormStartPosition.CenterScreen;
            International__License.ShowDialog();
        }

        private void internationalLicenseApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApplications International__License = new frmListInternationalLicenseApplications();
            International__License.StartPosition = FormStartPosition.CenterScreen;
            International__License.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicenseApplication RenewLocalDrivingLicense = new frmRenewLocalDrivingLicenseApplication();
            RenewLocalDrivingLicense.StartPosition = FormStartPosition.CenterScreen;
            RenewLocalDrivingLicense.ShowDialog();
        }

        private void replacmentForLostDamagedLicesneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplaceLostOrDamagedLicenseApplication ReplacementforDamagedorLostLicenses = new frmReplaceLostOrDamagedLicenseApplication();
            ReplacementforDamagedorLostLicenses.StartPosition = FormStartPosition.CenterScreen;
            ReplacementforDamagedorLostLicenses.ShowDialog();

        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmDetainLicenseApplication DetainLicense = new frmDetainLicenseApplication();
            DetainLicense.StartPosition = FormStartPosition.CenterScreen;
            DetainLicense.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication ReleaseDetainLicense = new frmReleaseDetainedLicenseApplication();
            ReleaseDetainLicense.StartPosition = FormStartPosition.CenterScreen;
            ReleaseDetainLicense.ShowDialog();
        }

        private void manageDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses ManageDetainedLicenses = new frmListDetainedLicenses();
            ManageDetainedLicenses.StartPosition = FormStartPosition.CenterScreen;
            ManageDetainedLicenses.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication ReleaseDetainLicense = new frmReleaseDetainedLicenseApplication();
            ReleaseDetainLicense.StartPosition = FormStartPosition.CenterScreen;
            ReleaseDetainLicense.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicesnseApplications frm = new frmListLocalDrivingLicesnseApplications();
            frm.ShowDialog();
        }
    }
}
