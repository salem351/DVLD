using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyBusiness;
using DVLD_MyDataAccess;

namespace Business_Layer
{
    public class clsDriver
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public clsPerson PersonInfo;

        public int DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { get; }

        public clsDriver()

        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;
            Mode = enMode.AddNew;

        }

        public clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)

        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedDate = CreatedDate;
            this.PersonInfo = clsPerson.Find(PersonID);

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            //call DataAccess Layer 

            this.DriverID = clsDriverData.AddNewDriver(PersonID, CreatedByUserID);


            return (this.DriverID != -1);
        }

        private bool _UpdateDriver()
        {
            //call DataAccess Layer 

            return clsDriverData.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID);
        }

        public static clsDriver FindByDriverID(int DriverID)
        {

            int PersonID = -1; int CreatedByUserID = -1; DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))

                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;

        }

        public static clsDriver FindByPersonID(int PersonID)
        {

            int DriverID = -1; int CreatedByUserID = -1; DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))

                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;

        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }

        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }

        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }



        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int DriverID { get; set; }
        //public int PersonID { get; set; }
        //public int CreatedByUserID { get; set; }
        //public DateTime CreatedDate { get; set; }

        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsDrivers(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        //{
        //    this.DriverID = DriverID;
        //    this.PersonID = PersonID;
        //    this.CreatedByUserID = CreatedByUserID;
        //    this.CreatedDate = CreatedDate;


        //    Mode = enMode.UpdateMode;
        //}
        //public clsDrivers()
        //{
        //    this.DriverID = -1;
        //    this.PersonID = -1;
        //    this.CreatedByUserID = -1;         
        //    this.CreatedDate = DateTime.Now;         

        //    Mode = enMode.AddMode;
        //}

        //public static clsDrivers Find(int DriverID)
        //{
        //    int PersonID = -1, CreatedByUserID = -1;
        //    DateTime CreatedDate = DateTime.Now;

        //    if (clsDriversDataAccessLayer.GetDriver(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))

        //        return new clsDrivers(PersonID, DriverID, CreatedByUserID, CreatedDate);

        //    else

        //        return null;
        //}


        //public static clsDrivers FindByPersonID(int PersonID)
        //{
        //    int DriverID = -1, CreatedByUserID = -1;
        //    DateTime CreatedDate = DateTime.Now;

        //    if (clsDriversDataAccessLayer.GetDriverByByPersonID(ref DriverID, PersonID, ref CreatedByUserID, ref CreatedDate))

        //        return new clsDrivers(DriverID, PersonID, CreatedByUserID, CreatedDate);

        //    else

        //        return null;
        //}




        //private bool _AddNewDriver()
        //{
        //    //until now all the element are fill except the ID
        //    this.DriverID = clsDriversDataAccessLayer.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);

        //    return (this.DriverID != -1);
        //}
        //private bool _UpdateDriver()
        //{
        //    return clsDriversDataAccessLayer.UpdateDriver(this.DriverID, this.PersonID, this.CreatedByUserID, this.CreatedDate);
        //}
        //public bool Save()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewDriver())
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
        //            return _UpdateDriver();


        //    }
        //    return false;
        //}



        //public static DataTable _GetAllDriverss()
        //{
        //    return clsDriversDataAccessLayer.GetAllDriverss();
        //}






        ////Filter
        //public static DataTable FindByDriverID(int DriverID)
        //{
        //    return clsDriversDataAccessLayer.GetDriverByDriverID(DriverID);
        //}
        //public static DataTable FindByPersonID_Filter(int PersonID)
        //{
        //    return clsDriversDataAccessLayer.GetDriverByPersonID(PersonID);
        //}
        //public static DataTable FindByNationalNo(string NationalNo)
        //{
        //    return clsDriversDataAccessLayer.GetDriverByNationalNo(NationalNo);
        //}
        //public static DataTable FindByFullName(string FullName)
        //{
        //    return clsDriversDataAccessLayer.GetDriverByFullName(FullName);
        //}




    }
}
