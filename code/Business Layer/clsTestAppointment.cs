using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { set; get; }
        public clsTestType.enTestType TestTypeID { set; get; }
        public int LocalDrivingLicenseApplicationID { set; get; }
        public DateTime AppointmentDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public bool IsLocked { set; get; }
        public int RetakeTestApplicationID { set; get; }
        public clsApplication RetakeTestAppInfo { set; get; }
        public int TestID
        {
            get { return _GetTestID(); }

        }

        public clsTestAppointment()

        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;
            Mode = enMode.AddNew;

        }
        public clsTestAppointment(int TestAppointmentID, clsTestType.enTestType TestTypeID,
           int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
           int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)

        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID);
            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        {
            //call DataAccess Layer 

            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }
        private bool _UpdateTestAppointment()
        {
            //call DataAccess Layer 

            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = 1; int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int CreatedByUserID = -1; bool IsLocked = false; int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
            ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;

        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            int TestAppointmentID = -1;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int CreatedByUserID = -1; bool IsLocked = false; int RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID,
                ref TestAppointmentID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;

        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();

        }

        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }

            return false;
        }

        private int _GetTestID()
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }



        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int TestAppointmentID { get; set; }
        //public int TestTypeID { get; set; }
        //public int LocalDrivingLicenseApplicationID { get; set; }
        //public DateTime AppointmentDate { get; set; }
        //public decimal PaidFees { get; set; }
        //public int CreatedByUserID { get; set; }
        //public bool IsLocked { get; set; }
        //public int RetakeTestApplicationID { get; set; }


        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
        //                         decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        //{
        //    this.TestAppointmentID = TestAppointmentID;
        //    this.TestTypeID = TestTypeID;
        //    this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        //    this.AppointmentDate = AppointmentDate;
        //    this.PaidFees = PaidFees;
        //    this.CreatedByUserID = CreatedByUserID;
        //    this.IsLocked = IsLocked;
        //    this.RetakeTestApplicationID = RetakeTestApplicationID;

        //    Mode = enMode.UpdateMode;
        //}
        //public clsTestAppointment()
        //{
        //    this.TestAppointmentID = -1;
        //    this.TestTypeID = -1;
        //    this.LocalDrivingLicenseApplicationID = -1;
        //    this.AppointmentDate = DateTime.Now;
        //    this.PaidFees = 0;
        //    this.CreatedByUserID = -1;
        //    this.IsLocked = false;
        //    this.RetakeTestApplicationID = -1;

        //    Mode = enMode.AddMode;
        //}


        //public static clsTestAppointment Find(int TestAppointmentID)
        //{
        //    int RetakeTestApplicationID = -1, LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1, TestTypeID = -1;
        //    DateTime AppointmentDate = DateTime.Now;
        //    decimal PaidFees = 0;
        //    bool IsLocked = false;

        //    if (clsTestAppointmentsDataAccessLayer.GetTestAppointmentByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref  AppointmentDate,
        //                                      ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

        //        return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
        //                                      PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);

        //    else

        //        return null;
        //}
        //public static clsTestAppointment FindByL_D_D_AppID(int LocalDrivingLicenseApplicationID)
        //{
        //    int RetakeTestApplicationID = -1, TestAppointmentID = -1, CreatedByUserID = -1, TestTypeID = -1;
        //    DateTime AppointmentDate = DateTime.Now;
        //    decimal PaidFees = 0;
        //    bool IsLocked = false;

        //    if (clsTestAppointmentsDataAccessLayer.GetTestAppointmentByL_D_D_AppID(ref TestAppointmentID, ref TestTypeID, LocalDrivingLicenseApplicationID, ref AppointmentDate,
        //                                      ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

        //        return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate,
        //                                      PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);

        //    else

        //        return null;
        //}

        //private bool _AddNewTestAppointment()
        //{
        //    //until now all the element are fill except the ID
        //    this.TestAppointmentID = clsTestAppointmentsDataAccessLayer.AddNewTestAppointment(this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate,
        //                         this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);

        //    return (this.TestAppointmentID != -1);
        //}

        //private bool _UpdateTestAppointment()
        //{
        //    return clsTestAppointmentsDataAccessLayer.UpdateTestAppointment(this.TestAppointmentID, this.AppointmentDate);
        //}

        //public bool Save()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewTestAppointment())
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
        //            return _UpdateTestAppointment();


        //    }
        //    return false;
        //}


        //public static bool _DeleteTestAppointment(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.DeleteTestAppointment(TestAppointmentID);
        //}



        //public static DataTable _GetAllTestAppointments()
        //{
        //    return clsTestAppointmentsDataAccessLayer.GetAllTestAppointments();
        //}

        //public static DataTable _GetTestAppointmentByL_D_D_AppID(int L_D_L_AppID, int TestTypeID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.GetTestAppointmentByL_D_D_AppID(L_D_L_AppID, TestTypeID);
        //}



        //public static bool _IsApplicantHasTestAppointment(int L_D_L_AppID, int TestTypeID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.IsApplicantHasTestAppointment(L_D_L_AppID, TestTypeID);
        //}
        //public static bool _IsApplicantHasNewOneAppointment(int L_D_L_AppID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.IsApplicantHasNewOneAppointment(L_D_L_AppID);
        //}
        //public static bool _IsApplicantTakeTest(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.IsApplicantTakeTest(TestAppointmentID);
        //}



        //public static int _GetTestTypeID(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.GetTestTypeID(TestAppointmentID);
        //}
        //public static int _GetTrail(int L_D_L_AppID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.GetTrail(L_D_L_AppID);
        //}
        //public static int _GetRetakeTestApplicationID(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.GetRetakeTestApplicationID(TestAppointmentID);
        //}
        //public static DateTime _GetAppointmentDate(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.GetAppointmentDate(TestAppointmentID);
        //}




        //public static bool _IsRetakeTestApplication(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.IsRetakeTestApplication(TestAppointmentID);
        //}
        //public static bool _IsRetakeTestApplicationForEdit(int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.IsRetakeTestApplicationForEdit(TestAppointmentID);
        //}


        //public static bool _UpdateApplicationByRetake(int ApplicationID, int TestAppointmentID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.UpdateApplicationByRetake(ApplicationID, TestAppointmentID);
        //}


        // public static bool _UpdateTestApplicationTolock(int ApplicationID)
        //{
        //    return clsTestAppointmentsDataAccessLayer.UpdateTestApplicationTolock(ApplicationID);
        //}

        ////public static bool IsPersonExist(int ID)
        ////{
        ////    return clsPeopleDataAccess.IsPersonExist(ID);
        ////}
        ////public static bool IsPersonExistWithNationalNo(string NationalNo)
        ////{
        ////    return clsPeopleDataAccess.IsPersonExistWithNationalNo(NationalNo);
        ////}
        ////public static bool IsItUserByNationalNo(string NationalNo)
        ////{
        ////    return clsPeopleDataAccess.IsItUserByNationalNo(NationalNo);
        ////}



        ////public static bool IsPersonHasLinkedToOtherTables(int PersonID)
        ////{
        ////    return clsPeopleDataAccess.IsPersonHasLinkedToOtherTables(PersonID);
        ////}
    }
}
