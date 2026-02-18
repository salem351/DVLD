using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using DVLD_MyDataAccess;

//مهم جدا جدا: if you want to add new class library until ContactsBusinessLayer ,,the NAMESPACE should be same name
namespace DVLD_MyBusiness
{
    public class clsPerson
    {
        public enum enMode { AddMode, UpdateMode }
        private enMode Mode;

        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        private string _ImagePath;
        public string ImagePath
        {
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }
        public int NationalityCountryID { get; set; }
        public clsCountry CountryInfo;


        //for this class only and Id is auto number and I can not but it from outside the class
        private clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, short Gendor,
             string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.ImagePath = ImagePath;
            this.NationalityCountryID = NationalityCountryID;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);

            Mode = enMode.UpdateMode;
        }

        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 1;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.ImagePath = "";
            this.NationalityCountryID = -1;

            Mode = enMode.AddMode;

        }


        public static clsPerson Find(int PersonID)
        {

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", NationalNo = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = 0;

            bool IsFound = clsPersonData.GetPersonInfoByID
                                (
                                    PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref NationalNo, ref DateOfBirth,
                                    ref Gendor, ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }
        //public static clsPeople Find(string FirstName)
        //{
        //    int ID = -1, CountryID = -1; byte Gendor = 0;
        //    string NationalNo = "", SecondName = "", ThirdName = "", LastName = "", Address = "",
        //          Phone = "", Email = "", ImagePath = "";
        //    DateTime DateOfBirth = DateTime.Now;


        //    if (clsPeopleDataAccess.GetPersonInfoByFirstName(FirstName, ref NationalNo, ref ID, ref SecondName, ref ThirdName, ref LastName,
        //                                              ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref ImagePath, ref CountryID))

        //        return new clsPeople(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
        //                            DateOfBirth, Gendor, Address, Phone, Email, ImagePath, CountryID);

        //    else

        //        return null;
        //}
        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID = -1, NationalityCountryID = -1;
            short Gendor = 0;

            bool IsFound = clsPersonData.GetPersonInfoByNationalNo
                                (
                                    NationalNo, ref PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref DateOfBirth,
                                    ref Gendor, ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath
                                );

            if (IsFound)

                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            else
                return null;
        }



        private bool _AddNewPerson()
        {
            //until now all the element are fill except the ID
            this.PersonID = clsPersonData.AddNewPerson(
                 this.FirstName, this.SecondName, this.ThirdName,
                 this.LastName, this.NationalNo,
                 this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email,
                 this.NationalityCountryID, this.ImagePath);

            return (this.PersonID != -1);
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(
                this.PersonID, this.FirstName, this.SecondName, this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gendor,
                this.Address, this.Phone, this.Email,
                  this.NationalityCountryID, this.ImagePath);
        }

        public bool Save()
        {

            switch (Mode)
            {
                case enMode.AddMode:
                    if (_AddNewPerson())
                    {
                        //until now all the element FULL
                        Mode = enMode.UpdateMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdatePerson();


            }
            return false;
        }


        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }


        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }



        public static bool IsPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }
        public static bool IsPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }



        //public static bool IsPersonHasLinkedToOtherTables(int PersonID)
        //{
        //    return clsPeopleDataAccess.IsPersonHasLinkedToOtherTables(PersonID);
        //}
    }
}
