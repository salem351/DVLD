using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_MyDataAccess;

//مهم جدا جدا: if you want to add new class library until ContactsDataAccessLayer ,, the NAMESPACE should be same name
//but the Connection name is different name and I have to write it above
namespace DVLD_MyBusiness
{
    public class clsCountry
    {

        public int ID { get; set; }
        public string CountryName { get; set; }

        private clsCountry(int ID, string CountryName)
        {
            this.ID = ID;
            this.CountryName = CountryName;

        }

        public clsCountry()
        {
            this.ID = -1;
            this.CountryName = "";

        }


        public static clsCountry Find(int ID)
        {
            string CountryName = " ";

            if (clsCountryDataAccess.GetCountryInfoByID(ID, ref CountryName))

                return new clsCountry(ID, CountryName);

            else

                return null;
        }

        public static clsCountry Find(string CountryName)
        {
            int ID = -1;

            if (clsCountryDataAccess.GetCountryInfoByName(CountryName, ref ID))

                return new clsCountry(ID, CountryName);

            else

                return null;
        }



        //private bool _AddNewCountry()
        //{
        //    //until now all the element are fill except the ID
        //    this.ID = clsCountryDataAccess.AddNewContactAndGetID(this.CountryName, this.Code, this.PhoneCode);

        //    return (this.ID != -1);
        //}

        //private bool _UpdateCountry()
        //{
        //    return clsCountryDataAccess.UpdateCountry(this.ID, this.CountryName, this.Code, this.PhoneCode);
        //}

        //public bool Save()
        //{

        //    switch (Mode)
        //    {
        //        case enMode.AddMode:
        //            if (_AddNewCountry())
        //            {
        //                //until now all the element FULL
        //                Mode = enMode.UpdateMode;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        case enMode.UpdateMode:
        //            return _UpdateCountry();


        //    }
        //    return false;
        //}


        //public static bool _DeleteCountry(int ID)
        //{
        //    return clsCountryDataAccess.DeleteCountry(ID);
        //}


        public static DataTable GetAllCountry()
        {
            return clsCountryDataAccess.GetAllCountry();
        }


    }
}
