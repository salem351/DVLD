using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsTest
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        //composition
        public clsTestAppointment TestAppointmentInfo { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }
        public int CreatedByUserID { set; get; }

        public clsTest()

        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }
        public clsTest(int TestID, int TestAppointmentID,
            bool TestResult, string Notes, int CreatedByUserID)

        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID,
                this.TestResult, this.Notes, this.CreatedByUserID);


            return (this.TestID != -1);
        }
        private bool _UpdateTest()
        {
            //call DataAccess Layer 

            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID,
                this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if (clsTestData.GetTestInfoByID(TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))

                return new clsTest(TestID,
                        TestAppointmentID, TestResult,
                        Notes, CreatedByUserID);
            else
                return null;

        }
        public static clsTest FindLastTestPerPersonAndLicenseClass
            (int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass
                (PersonID, LicenseClassID, (int)TestTypeID, ref TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))

                return new clsTest(TestID,
                        TestAppointmentID, TestResult,
                        Notes, CreatedByUserID);
            else
                return null;

        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }


        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int TestID { get; set; }
        //public int TestAppointmentID { get; set; }
        //public bool TestResult { get; set; }
        //public string Notes { get; set; }
        //public int CreatedByUserID { get; set; }


        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsTests(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        //{
        //    this.TestID = TestID;
        //    this.TestAppointmentID = TestAppointmentID;
        //    this.TestResult = TestResult;
        //    this.Notes = Notes;
        //    this.CreatedByUserID = CreatedByUserID;

        //    Mode = enMode.UpdateMode;
        //}
        //public clsTests()
        //{
        //    this.TestID = -1;
        //    this.TestAppointmentID = -1;
        //    this.TestResult = false;
        //    this.Notes = "";
        //    this.CreatedByUserID = -1;

        //    Mode = enMode.AddMode;
        //}


        //public static clsTests Find(int TestID)
        //{
        //    int TestAppointmentID = -1, CreatedByUserID = -1;
        //    string Notes = "";
        //    bool TestResult = false;


        //    if (clsTestsDataAccessLayer.GetTestByID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUserID))

        //        return new clsTests(TestID, TestAppointmentID, TestResult, Notes, CreatedByUserID);

        //    else

        //        return null;
        //}

        //private bool _AddNewTest()
        //{
        //    //until now all the element are fill except the ID
        //    this.TestID = clsTestsDataAccessLayer.AddNewTest(this.TestAppointmentID, this.TestResult, this.Notes,this.CreatedByUserID);

        //    return (this.TestID != -1);
        //}

        //private bool _UpdateTest()
        //{
        //    return false;
        //}

        //public bool Save()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewTest())
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
        //            return _UpdateTest();


        //    }
        //    return false;
        //}





        //public static bool _IsApplicantPass(int TestAppointmentID)
        //{
        //    return clsTestsDataAccessLayer.IsApplicantPass(TestAppointmentID);
        //}
        //public static bool _IsApplicantFail(int TestAppointmentID)
        //{
        //    return clsTestsDataAccessLayer.IsApplicantFail(TestAppointmentID);
        //}






    }
}
