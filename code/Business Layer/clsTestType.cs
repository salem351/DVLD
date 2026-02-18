using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsTestType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };

        public clsTestType.enTestType ID { set; get; }
        public string Title { set; get; }
        public string Description { set; get; }
        public float Fees { set; get; }

        public clsTestType()

        {
            this.ID = clsTestType.enTestType.VisionTest;
            this.Title = "";
            this.Description = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }
        public clsTestType(clsTestType.enTestType ID, string TestTypeTitle, string Description, float TestTypeFees)

        {
            this.ID = ID;
            this.Title = TestTypeTitle;
            this.Description = Description;

            this.Fees = TestTypeFees;
            Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            //call DataAccess Layer 

            this.ID = (clsTestType.enTestType)clsTestTypeDataAccess.AddNewTestType(this.Title, this.Description, this.Fees);

            return (this.Title != "");
        }

        private bool _UpdateTestType()
        {
            //call DataAccess Layer 

            return clsTestTypeDataAccess.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string Title = "", Description = ""; float Fees = 0;

            if (clsTestTypeDataAccess.GetTestTypeInfoByID((int)TestTypeID, ref Title, ref Description, ref Fees))

                return new clsTestType(TestTypeID, Title, Description, Fees);
            else
                return null;

        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeDataAccess.GetAllTestTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestType();

            }

            return false;
        }

        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int TestTypeID { get; set; }
        //public string TestTypeTitle { get; set; }
        //public string TestTypeDescription { get; set; }
        //public decimal TestTypeFees { get; set; } //Small Money


        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        //{
        //    this.TestTypeID = TestTypeID;
        //    this.TestTypeTitle = TestTypeTitle;
        //    this.TestTypeDescription = TestTypeDescription;
        //    this.TestTypeFees = TestTypeFees;

        //    Mode = enMode.UpdateMode;
        //}

        //public clsTestType()
        //{
        //    this.TestTypeID = -1;
        //    this.TestTypeTitle = "";
        //    this.TestTypeDescription = "";
        //    this.TestTypeFees = 0;

        //    Mode = enMode.AddMode;
        //}


        //public static clsTestType FindByTestTypeID(int TestTypeID)
        //{
        //    Decimal TestTypeFees = 0;
        //    string TestTypeTitle = "", TestTypeDescription = "";


        //    if (clsTestTypeDataAccess.GetTestTypeByTestTypeID(TestTypeID, ref TestTypeTitle, ref TestTypeDescription, ref TestTypeFees))

        //        return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);

        //    else

        //        return null;
        //}



        ////هذه ليست مفعله
        ////private bool _AddNewApplication()
        ////{
        ////    Decimal ApplicationFees = 0;
        ////    string ApplicationTypeTitle = "";


        ////    this.ApplicationTypeID = clsApplicationDataAccess.AddNewApplicationAndGetID(this.ApplicationTypeTitle, this.ApplicationFees);

        ////    return (this.ApplicationTypeID != -1);
        ////}


        //private bool _UpdateTestTypeID()
        //{
        //    return clsTestTypeDataAccess.UpdateTestType(this.TestTypeID, this.TestTypeTitle, this.TestTypeDescription, this.TestTypeFees);
        //}

        //public bool SaveTestType()
        //{

        //    switch (Mode)
        //    {
        //       // case enMode.AddMode:
        //           // if (_AddNewApplication())
        //           // {
        //                //until now all the element FULL
        //            //    Mode = enMode.AddMode;
        //             //   return true;
        //           // }
        //           // else
        //           // {
        //            //    return false;
        //           // }
        //        case enMode.UpdateMode:
        //            return _UpdateTestTypeID();


        //    }
        //    return false;
        //}


        ////public static bool DeleteUser(int UserID)
        ////{
        ////    return clsUsersDataAccess.DeleteUser(UserID);
        ////}


        //public static DataTable GetALlTestTypes()
        //{
        //    return clsTestTypeDataAccess.GetALlTestTypes();
        //}

        ////public static bool IsUserExist(int UserID)
        ////{
        ////    return clsUsersDataAccess.IsUserExist(UserID);
        ////}
        ////public static bool IsUserExistByPersonID(int PersonID)
        ////{
        ////    return clsUsersDataAccess.IsUserExistByPersonID(PersonID);
        ////}

        ////public static bool IsUserExistByUserNameAndPassword(string UserName, string Password)
        ////{
        ////    return clsUsersDataAccess.IsUserExistByUserNameAndPassword(UserName, Password);
        ////}
        ////public static bool IsUserActive(string UserName)
        ////{
        ////    return clsUsersDataAccess.IsUserActive(UserName);
        ////}

        ////public static bool IsUserHasLinkedToOtherTables(int UserID)
        ////{
        ////    return clsUsersDataAccess.IsUserHasLinkedToOtherTables(UserID);
        ////}

    }
}
