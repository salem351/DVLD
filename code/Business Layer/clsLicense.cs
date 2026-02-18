using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer;
using Data_Access;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        //من هذه تقدر توصل للشخص 
        public clsDriver DriverInfo;
        public int LicenseID { set; get; }
        public int ApplicationID { set; get; }
        public int DriverID { set; get; }
        public int LicenseClass { set; get; }
        public clsLicenseClass LicenseClassIfo;
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public clsDetainedLicense DetainedInfo { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); }
        }

        public clsLicense()

        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }
        public clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes,
            float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)

        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.LicenseClassIfo = clsLicenseClass.Find(this.LicenseClass);
            this.DetainedInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            //call DataAccess Layer 

            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);


            return (this.LicenseID != -1);
        }
        private bool _UpdateLicense()
        {
            //call DataAccess Layer 

            return clsLicenseData.UpdateLicense(this.ApplicationID, this.LicenseID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1; int DriverID = -1; int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0; bool IsActive = true; int CreatedByUserID = 1;
            byte IssueReason = 1;
            if (clsLicenseData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes,
            ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))

                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
                                     IssueDate, ExpirationDate, Notes,
                                     PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;

        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {

            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public Boolean IsLicenseExpired()
        {

            return (this.ExpirationDate < DateTime.Now);

        }

        public bool DeactivateCurrentLicense()
        {
            return (clsLicenseData.DeactivateLicense(this.LicenseID));
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }


        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if (!detainedLicense.Save())
            {

                return -1;
            }

            return detainedLicense.DetainID;

        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, ref int ApplicationID)
        {
            //لان فك الرخصة لها طلب جديد
            //First Create Applicaiton 
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;                         
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense).Fees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;


            return this.DetainedInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);

        }
        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {
            //لان تجديد الرخصة لها طلب جديد

            //First Create Applicaiton 
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }


            //انسخ محتويات الرخصة القديمه للجديده وقفل الرخصة القديمة
            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassIfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassIfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;


            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;
        }
        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {

            //لان تبديل الرخصة لها طلب جديد

            //First Create Applicaiton 
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;

            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            //انسخ محتويات الرخصة القديمه للجديده وقفل الرخصة القديمة
            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;



            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return NewLicense;
        }





        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int LicenseID { get; set; }
        //public int ApplicationID { get; set; }
        //public int DriverID { get; set; }
        //public int LicenseClass { get; set; }
        //public DateTime IssueDate { get; set; }
        //public DateTime ExpirationDate { get; set; }
        //public string Notes { get; set; }
        //public decimal PaidFees { get; set; }
        //public bool IsActive { get; set; }
        //public byte IssueReason { get; set; }
        //public int CreatedByUserID { get; set; }


        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsIssueLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate
        //                        , string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        //{
        //    this.LicenseID = LicenseID;
        //    this.ApplicationID = ApplicationID;
        //    this.DriverID = DriverID;
        //    this.LicenseClass = LicenseClass;
        //    this.IssueDate = IssueDate;
        //    this.ExpirationDate = ExpirationDate;
        //    this.Notes = Notes;
        //    this.PaidFees = PaidFees;
        //    this.IsActive = IsActive;
        //    this.IssueReason = IssueReason;
        //    this.CreatedByUserID = CreatedByUserID;

        //    Mode = enMode.UpdateMode;
        //}

        //public clsIssueLicense()
        //{
        //    this.LicenseID = -1;
        //    this.ApplicationID = -1;
        //    this.DriverID = -1;
        //    this.LicenseClass = -1;
        //    this.IssueDate = DateTime.Now;
        //    this.ExpirationDate = DateTime.Now;
        //    this.Notes = "";
        //    this.PaidFees = 0;
        //    this.IsActive = false;
        //    this.IssueReason = 1;
        //    this.CreatedByUserID = -1;

        //    Mode = enMode.AddMode;
        //}


        //public static clsIssueLicense FindByLicenseID(int LicenseID)
        //{
        //    int ApplicationID = -1, DriverID = -1, LicenseClass = -1;
        //    DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        //    string Notes = "";
        //    decimal PaidFees = 0;
        //    bool IsActive = false;
        //    byte IssueReason = 1;
        //    int CreatedByUserID = 0;

        //    if (clsIssueLicenseDataAccessLayer.GetLicenseByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
        //                                                  ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive,
        //                                                   ref IssueReason, ref CreatedByUserID))

        //        return new clsIssueLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
        //                                                  IssueDate, ExpirationDate, Notes,  PaidFees,  IsActive,
        //                                                  IssueReason, CreatedByUserID);

        //    else

        //        return null;
        //}
        //public static clsIssueLicense FindByApplicationID(int ApplicationID)
        //{
        //    int LicenseID = -1, DriverID = -1, LicenseClass = -1;
        //    DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        //    string Notes = "";
        //    decimal PaidFees = 0;
        //    bool IsActive = false;
        //    byte IssueReason = 1;
        //    int CreatedByUserID = 0;

        //    if (clsIssueLicenseDataAccessLayer.GetLicenseByApplicationID(ref LicenseID, ApplicationID, ref DriverID, ref LicenseClass,
        //                                                  ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive,
        //                                                   ref IssueReason, ref CreatedByUserID))

        //        return new clsIssueLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
        //                                                  IssueDate, ExpirationDate, Notes, PaidFees, IsActive,
        //                                                  IssueReason, CreatedByUserID);

        //    else

        //        return null;
        //}




        //private bool _AddNewLicense()
        //{
        //    //until now all the element are fill except the ID
        //    this.LicenseID = clsIssueLicenseDataAccessLayer.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
        //                            this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason,
        //                            this.CreatedByUserID);

        //    return (this.LicenseID != -1);
        //}
        //private bool _UpdateLicense()
        //{
        //    return clsIssueLicenseDataAccessLayer.UpdateLicense(this.LicenseID,this.ApplicationID, this.DriverID, this.LicenseClass,
        //                            this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, this.IssueReason,
        //                            this.CreatedByUserID);
        //}
        //public bool Save()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewLicense())
        //            {
        //                //until now all the element FULL
        //                Mode = enMode.AddMode;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        case enMode.UpdateMode:
        //            return _UpdateLicense();


        //    }
        //    return false;
        //}


        //public static DataTable _GetAllLicenseToDriver(int DriverID)
        //{
        //    return clsIssueLicenseDataAccessLayer.GetAllLicenseToDriver(DriverID);
        //}




        //public static DataTable _ShowLicense(int LicenseID)
        //{
        //    return clsIssueLicenseDataAccessLayer.ShowLicense(LicenseID);
        //}

        //public static int _GetApplicationID(int LicenseID)
        //{
        //    return clsIssueLicenseDataAccessLayer.GetApplicationID(LicenseID);
        //}

        //public static bool _UpdateToBeNotActive(int LicenseID)
        //{
        //    return clsIssueLicenseDataAccessLayer.UpdateToBeNotActive(LicenseID);
        //}
        //public static bool _UpdateToBeActive(int LicenseID)
        //{
        //    return clsIssueLicenseDataAccessLayer.UpdateToBeActive(LicenseID);
        //}


        //public static DataTable _GetLocalLicense(int PersonID)
        //{
        //    return clsIssueLicenseDataAccessLayer.GetLocalLicense(PersonID);
        //}
        //public static DataTable _GetInternationalLicense(int PersonID)
        //{
        //    return clsIssueLicenseDataAccessLayer.GetInternationalLicense(PersonID);
        //}

    }
}
