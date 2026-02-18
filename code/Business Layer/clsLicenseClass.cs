 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsLicenseClass
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID { set; get; }
        public string ClassName { set; get; }
        public string ClassDescription { set; get; }
        public byte MinimumAllowedAge { set; get; }
        public byte DefaultValidityLength { set; get; }
        public float ClassFees { set; get; }

        public clsLicenseClass()

        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;

            Mode = enMode.AddNew;

        }
        public clsLicenseClass(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)

        {
            this.LicenseClassID = LicenseClassID;
            this.ClassName = ClassName;
            this.ClassDescription = ClassDescription;
            this.MinimumAllowedAge = MinimumAllowedAge;
            this.DefaultValidityLength = DefaultValidityLength;
            this.ClassFees = ClassFees;
            Mode = enMode.Update;
        }

        private bool _AddNewLicenseClass()
        {
            //call DataAccess Layer 

            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);


            return (this.LicenseClassID != -1);
        }

        private bool _UpdateLicenseClass()
        {
            //call DataAccess Layer 

            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string ClassName = ""; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID, ref ClassName, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;

        }
        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;

        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseClass())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicenseClass();

            }

            return false;
        }

        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int LicenseClassID { get; set; }
        //public string ClassName { get; set; }
        //public string ClassDescription { get; set; }
        //public byte MinimumAllowedAge { get; set; }
        //public byte DefaultValidityLength { get; set; }
        //public decimal ClassFees { get; set; }


        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsLicenseClasses(int LicenseClassID, string ClassName, string ClassDescription, byte MinimumAllowedAge, 
        //                          byte DefaultValidityLength, decimal ClassFees)
        //{
        //    this.LicenseClassID = LicenseClassID;
        //    this.ClassName = ClassName;
        //    this.ClassDescription = ClassDescription;
        //    this.MinimumAllowedAge = MinimumAllowedAge;
        //    this.DefaultValidityLength = DefaultValidityLength;
        //    this.ClassFees = ClassFees;

        //    Mode = enMode.UpdateMode;
        //}

        //public clsLicenseClasses()
        //{
        //    this.LicenseClassID = -1;
        //    this.ClassName = "";
        //    this.ClassDescription = "";
        //    this.MinimumAllowedAge = 0;
        //    this.DefaultValidityLength = 0;
        //    this.ClassFees = 0;


        //    Mode = enMode.AddMode;
        //}


        //public static clsLicenseClasses FindByLicenseClassID(int LicenseClassID)
        //{
        //    string ClassName = "", ClassDescription = "";
        //    byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
        //    decimal ClassFees = 0;

        //    if (clsLicenseClassesDataAccessLayer.GetLicenseClassesByID(LicenseClassID, ref ClassName, ref ClassDescription,
        //                                                  ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

        //        return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);

        //    else

        //        return null;
        //}
        //public static clsLicenseClasses FindByClassName(string ClassName)
        //{
        //    int LicenseClassID = -1; string ClassDescription = "";
        //    byte MinimumAllowedAge = 0, DefaultValidityLength = 0;
        //    decimal ClassFees = 0;

        //    if (clsLicenseClassesDataAccessLayer.GetLicenseClassesByClassName(ref LicenseClassID, ClassName, ref ClassDescription,
        //                                                  ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

        //        return new clsLicenseClasses(LicenseClassID, ClassName, ClassDescription, MinimumAllowedAge, DefaultValidityLength, ClassFees);

        //    else

        //        return null;
        //}



        ////private bool _AddNewLocalDrivingLicense()
        ////{
        ////    //until now all the element are fill except the ID
        ////    this.LocalDrivingLicenseApplicationID = clsLocalDrivingDataAccessLayer.AddNewLocalDrivingLicense(this.ApplicationID, this.LicenseClassID);

        ////    return (this.LocalDrivingLicenseApplicationID != -1);
        ////}


        ////private bool _UpdateLocalDrivingLicense()
        ////{
        ////    return clsLocalDrivingDataAccessLayer.UpdateLocalDrivingLicense(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        ////}

        ////public bool Save()
        ////{

        ////    switch (Mode)
        ////    {
        ////        case enMode.AddMode:
        ////            if (_AddNewLocalDrivingLicense())
        ////            {
        ////                //until now all the element FULL
        ////                Mode = enMode.AddMode;
        ////                return true;
        ////            }
        ////            else
        ////            {
        ////                return false;
        ////            }
        ////        case enMode.UpdateMode:
        ////            return _UpdateLocalDrivingLicense();


        ////    }
        ////    return false;
        ////}


        ////public static bool _DeleteLocalDrivingLicense(int LocalDrivingLicenseApplicationID)
        ////{
        ////    return clsLocalDrivingDataAccessLayer.DeleteLocalDrivingLicense(LocalDrivingLicenseApplicationID);
        ////}


        ////public static DataTable GetAllApplications()
        ////{
        ////    return clsLocalDrivingDataAccessLayer.GetAllApplications();
        ////}

    }
}
