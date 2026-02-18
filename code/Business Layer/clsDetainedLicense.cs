using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsDetainedLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float FineFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByUserID { set; get; }
        public clsUser ReleasedByUserInfo { set; get; }
        public int ReleaseApplicationID { set; get; }

        public clsDetainedLicense()

        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByUserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByUserID = 0;
            this.ReleaseApplicationID = -1;



            Mode = enMode.AddNew;

        }
        public clsDetainedLicense(int DetainID,
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID,
            bool IsReleased, DateTime ReleaseDate,
            int ReleasedByUserID, int ReleaseApplicationID)

        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(this.CreatedByUserID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByUserID = ReleasedByUserID;
            this.ReleaseApplicationID = ReleaseApplicationID;
            this.ReleasedByUserInfo = clsUser.FindByPersonID(this.ReleasedByUserID);
            Mode = enMode.Update;
        }

        private bool _AddNewDetainedLicense()
        {
            //call DataAccess Layer 

            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(
                this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);

            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            //call DataAccess Layer 

            return clsDetainedLicenseData.UpdateDetainedLicense(
                this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID);
        }

        public static clsDetainedLicense Find(int DetainID)
        {
            int LicenseID = -1; DateTime DetainDate = DateTime.Now;
            float FineFees = 0; int CreatedByUserID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID,
            ref LicenseID, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))

                return new clsDetainedLicense(DetainID,
                     LicenseID, DetainDate,
                     FineFees, CreatedByUserID,
                     IsReleased, ReleaseDate,
                     ReleasedByUserID, ReleaseApplicationID);
            else
                return null;

        }
        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            int DetainID = -1; DateTime DetainDate = DateTime.Now;
            float FineFees = 0; int CreatedByUserID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID,
            ref DetainID, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))

                return new clsDetainedLicense(DetainID,
                     LicenseID, DetainDate,
                     FineFees, CreatedByUserID,
                     IsReleased, ReleaseDate,
                     ReleasedByUserID, ReleaseApplicationID);
            else
                return null;

        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();

        }


        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID,
                   ReleasedByUserID, ReleaseApplicationID);
        }

        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int DetainID { get; set; }
        //public int LicenseID { get; set; }
        //public DateTime DetainDate { get; set; }
        //public decimal FineFees { get; set; }
        //public int CreatedByUserID { get; set; }
        //public byte IsReleased { get; set; }
        //public DateTime ReleaseDate { get; set; }
        //public int ReleasedByUserID { get; set; }
        //public int ReleaseApplicationID { get; set; }



        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID,
        //                       byte IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        //{
        //    this.DetainID = DetainID;
        //    this.LicenseID = LicenseID;
        //    this.DetainDate = DetainDate;
        //    this.FineFees = FineFees;
        //    this.CreatedByUserID = CreatedByUserID;
        //    this.IsReleased = IsReleased;
        //    this.ReleaseDate = ReleaseDate;
        //    this.ReleasedByUserID = ReleasedByUserID;
        //    this.ReleaseApplicationID = ReleaseApplicationID;

        //    Mode = enMode.UpdateMode;
        //}
        //private clsDetainedLicense(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID, byte IsReleased)
        //{
        //    this.DetainID = DetainID;
        //    this.LicenseID = LicenseID;
        //    this.DetainDate = DetainDate;
        //    this.FineFees = FineFees;
        //    this.CreatedByUserID = CreatedByUserID;
        //    this.IsReleased = IsReleased;

        //    Mode = enMode.UpdateMode;
        //}


        //public clsDetainedLicense()
        //{
        //    this.DetainID = -1;
        //    this.LicenseID = -1;
        //    this.DetainDate = DateTime.Now;
        //    this.FineFees = 0;
        //    this.CreatedByUserID = -1;
        //    this.IsReleased = 0;           

        //    Mode = enMode.AddMode;
        //}

        ////Find For Release
        //public static clsDetainedLicense ReleaseFind(int DetainID)
        //{
        //    int LicenseID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
        //    DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
        //    byte IsReleased = 0;
        //    decimal FineFees = 0;

        //    if (clsDetainedLicenseDataAccessLayer.GetDetainByIDForRelease(DetainID, ref LicenseID, ref CreatedByUserID, ref ReleasedByUserID,
        //                                                         ref ReleaseApplicationID, ref DetainDate, ref ReleaseDate, ref IsReleased, ref FineFees))

        //        return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees,
        //                                      CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);

        //    else

        //        return null;
        //}
        //public static clsDetainedLicense ReleaseFindBtLicenseID(int LicenseID)
        //{
        //    int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
        //    DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
        //    byte IsReleased = 0;
        //    decimal FineFees = 0;

        //    if (clsDetainedLicenseDataAccessLayer.GetDetainByLicenseIDForRelease(ref DetainID, LicenseID, ref CreatedByUserID, ref ReleasedByUserID,
        //                                                         ref ReleaseApplicationID, ref DetainDate, ref ReleaseDate, ref IsReleased, ref FineFees))

        //        return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees,
        //                                      CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);

        //    else

        //        return null;
        //}

        ////Find For Detain
        //public static clsDetainedLicense DetainFind(int DetainID)
        //{
        //    int LicenseID = -1, CreatedByUserID = -1;
        //    DateTime DetainDate = DateTime.Now;
        //    byte IsReleased = 0;
        //    decimal FineFees = 0;

        //    if (clsDetainedLicenseDataAccessLayer.GetDetainByIDForDetain(DetainID, ref LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased))

        //        return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased);

        //    else

        //        return null;
        //}
        //public static clsDetainedLicense DetainFindByLicenseID(int LicenseID)
        //{
        //    int DetainID = -1, CreatedByUserID = -1;
        //    DateTime DetainDate = DateTime.Now;
        //    byte IsReleased = 0;
        //    decimal FineFees = 0;

        //    if (clsDetainedLicenseDataAccessLayer.GetDetainByLicenseIDForDetain(ref DetainID, LicenseID, ref DetainDate, ref FineFees, ref CreatedByUserID, ref IsReleased))

        //        return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased);

        //    else

        //        return null;
        //}





        ////هذه ليست مفعله
        //private bool _AddNewDetain()
        //{

        //    this.DetainID = clsDetainedLicenseDataAccessLayer.AddNewDetainAndGetID(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID,
        //                       this.IsReleased);

        //    return (this.DetainID != -1);
        //}


        //private bool _UpdateDetain()
        //{
        //    return clsDetainedLicenseDataAccessLayer.UpdateDetain(this.DetainID, this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByUserID,
        //                       this.IsReleased, this.ReleaseDate, this.ReleasedByUserID, this.ReleaseApplicationID);
        //}

        //public bool SaveDetain()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewDetain())
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
        //            return _UpdateDetain();


        //    }
        //    return false;
        //}


        ////public static bool DeleteApplication(int ApplicationID)
        ////{
        ////    return clsApplicationsDataAccessLayer.DeleteApplication(ApplicationID);
        ////}


        //public static DataTable GetAllDetainsUsingView()
        //{
        //    return clsDetainedLicenseDataAccessLayer.GetAllDetainsUsingView();
        //}


        //public static byte _IsReleaseLicense(int LicenseID)
        //{
        //    return clsDetainedLicenseDataAccessLayer.IsReleaseLicense(LicenseID);
        //}
        //public static bool _IsDetainedLicense(int LicenseID)
        //{
        //    return clsDetainedLicenseDataAccessLayer.IsDetainedLicense(LicenseID);
        //}
        //public static bool _IsDetainedLicenseByDetainID(int DetainID)
        //{
        //    return clsDetainedLicenseDataAccessLayer.IsDetainedLicenseByDetainID(DetainID);
        //}
        //public static bool _UpdateToRelease(int DetainID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        //{
        //    return clsDetainedLicenseDataAccessLayer.UpdateToRelease( DetainID, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);
        //}





        ////Using For Filter
        //public static DataTable FindByDetainID(int DetainID)
        //{
        //    return clsDetainedLicenseDataAccessLayer.GetDetainedLicenseInfoByDetainID(DetainID);
        //}
        //public static DataTable MyFindByReleaseApplicationID(int ReleaseApplicationID)
        //{
        //    return clsDetainedLicenseDataAccessLayer.GetDetainedLicenseInfoByReleaseApplicationID(ReleaseApplicationID);
        //}
        //public static DataTable FindByNationalNo(string NationalNo)
        //{
        //    return clsDetainedLicenseDataAccessLayer.GetDetainedLicenseInfoByNationalNo(NationalNo);
        //}
        //public static DataTable FindByFullName(string FullName)
        //{
        //    return clsDetainedLicenseDataAccessLayer.GetDetainedLicenseInfoByFullName(FullName);
        //}

        //public static DataTable _GetByIsReleased(byte IsReleased)
        //{
        //    return clsDetainedLicenseDataAccessLayer.GetByIsReleased(IsReleased);
        //}


    }
}
