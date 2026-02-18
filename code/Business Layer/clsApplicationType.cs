using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

namespace DVLD_MyBusiness
{
    public class clsApplicationType //in beginning change internal nae to public
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;


        public int ID { set; get; }
        public string Title { set; get; }
        public float Fees { set; get; }

        public clsApplicationType()

        {
            this.ID = -1;
            this.Title = "";
            this.Fees = 0;
            Mode = enMode.AddNew;

        }
        public clsApplicationType(int ID, string ApplicationTypeTitel, float ApplicationTypeFees)

        {
            this.ID = ID;
            this.Title = ApplicationTypeTitel;
            this.Fees = ApplicationTypeFees;
            Mode = enMode.Update;
        }

        private bool _AddNewApplicationType()
        {
            //call DataAccess Layer 

            this.ID = clsApplicationTypeDataAccess.AddNewApplicationType(this.Title, this.Fees);


            return (this.ID != -1);
        }
        private bool _UpdateApplicationType()
        {
            //call DataAccess Layer 

            return clsApplicationTypeDataAccess.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }

        public static clsApplicationType Find(int ID)
        {
            string Title = ""; float Fees = 0;

            if (clsApplicationTypeDataAccess.GetApplicationTypeInfoByID((int)ID, ref Title, ref Fees))

                return new clsApplicationType(ID, Title, Fees);
            else
                return null;

        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypeDataAccess.GetAllApplicationTypes();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplicationType())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplicationType();

            }

            return false;
        }



        //public enum enMode { AddMode, UpdateMode }
        //private enMode Mode;

        //public int ApplicationTypeID { get; set; }
        //public string ApplicationTypeTitle { get; set; }
        //public decimal ApplicationFees { get; set; } //Small Money


        ////for this class only and Id is auto number and I can not but it from outside the class
        //private clsApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, decimal ApplicationFees)
        //{
        //    this.ApplicationTypeID = ApplicationTypeID;
        //    this.ApplicationTypeTitle = ApplicationTypeTitle;
        //    this.ApplicationFees = ApplicationFees;

        //    Mode = enMode.UpdateMode;
        //}

        //public clsApplicationType()
        //{
        //    this.ApplicationTypeID = -1;
        //    this.ApplicationTypeTitle = "";
        //    this.ApplicationFees = 0;

        //    Mode = enMode.AddMode;

        //}


        //public static clsApplicationType FindByApplicationTypeID(int ApplicationTypeID)
        //{
        //    Decimal ApplicationFees = 0;
        //    string ApplicationTypeTitle = "";


        //    if (clsApplicationTypeDataAccess.GetApplicationTypeByTypeID(ApplicationTypeID, ref ApplicationTypeTitle, ref ApplicationFees))

        //        return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationFees);

        //    else

        //        return null;
        //}



        ////هذه ليست مفعله
        //private bool _AddNewApplicationType()
        //{
        //    Decimal ApplicationFees = 0;
        //    string ApplicationTypeTitle = "";


        //    this.ApplicationTypeID = clsApplicationTypeDataAccess.AddNewApplicationTypeAndGetID(this.ApplicationTypeTitle, this.ApplicationFees);

        //    return (this.ApplicationTypeID != -1);
        //}


        //private bool _UpdateApplicationType()
        //{
        //    return clsApplicationTypeDataAccess.UpdateApplicationTypes(this.ApplicationTypeID, this.ApplicationTypeTitle, this.ApplicationFees);
        //}

        //public bool SaveApplicationType()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewApplicationType())
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
        //            return _UpdateApplicationType();


        //    }
        //    return false;
        //}



        //public static DataTable GetALlApplicationTypes()
        //{
        //    return clsApplicationTypeDataAccess.GetAllApplicationTypes();
        //}


        //public static decimal _GetApplicationTypesFee(int ApplicationTypeID)
        //{
        //    return clsApplicationTypeDataAccess.GetApplicationTypesFee(ApplicationTypeID);
        //}
    }
}
