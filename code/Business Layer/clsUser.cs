using System;
using System.Data;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using DVLD_MyDataAccess;

//مهم جدا جدا: if you want to add new class library until ContactsBusinessLayer ,,the NAMESPACE should be same name
namespace DVLD_MyBusiness
{
    public class clsUser
    {
        public enum enMode { AddMode, UpdateMode }
        private enMode Mode;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        //composition
        public clsPerson PersonInfo;
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }



        //for this class only and Id is auto number and I can not but it from outside the class
        public clsUser()

        {
            this.UserID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;
            Mode = enMode.AddMode;
        }

        private clsUser(int UserID, int PersonID, string Username, string Password,
            bool IsActive)

        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = Username;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.UpdateMode;
        }


        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            bool IsFound = clsUsersDataAccess.GetUserInfoByUserID
                                (UserID, ref PersonID, ref UserName, ref Password, ref IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            bool IsFound = clsUsersDataAccess.GetUserInfoByPersonID
                                (PersonID, ref UserID, ref UserName, ref Password, ref IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }
        public static clsUser FindByUsernameAndPassword(string UserName, string Password)
        {
            int UserID = -1;
            int PersonID = -1;

            bool IsActive = false;

            bool IsFound = clsUsersDataAccess.GetUserInfoByUsernameAndPassword
                                (UserName, Password, ref UserID, ref PersonID, ref IsActive);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID, UserName, Password, IsActive);
            else
                return null;
        }




        private bool _AddNewUser()
        {
            //until now all the element are fill except the ID
            //call DataAccess Layer 

            this.UserID = clsUsersDataAccess.AddNewUser(this.PersonID, this.UserName,
                this.Password, this.IsActive);

            return (this.UserID != -1);
        }


        private bool _UpdateUser()
        {
            return clsUsersDataAccess.UpdateUser(this.UserID, this.PersonID, this.UserName,
                        this.Password, this.IsActive);
        }

        public bool SaveUsers()
        {

            switch (Mode)
            {
                case enMode.AddMode:
                    if (_AddNewUser())
                    {
                        //until now all the element FULL
                        Mode = enMode.AddMode;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.UpdateMode:
                    return _UpdateUser();


            }
            return false;
        }


        public static bool DeleteUser(int UserID)
        {
            return clsUsersDataAccess.DeleteUser(UserID);
        }


        public static DataTable GetAllUsers()
        {
            return clsUsersDataAccess.GetAllUsers();
        }

        public static bool IsUserExist(int UserID)
        {
            return clsUsersDataAccess.IsUserExist(UserID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUsersDataAccess.IsUserExist(UserName);
        }


        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUsersDataAccess.IsUserExistForPersonID(PersonID);
        }

        //public static bool IsUserExistByUserNameAndPassword(string UserName, string Password)
        //{
        //    return clsUsersDataAccess.IsUserExistByUserNameAndPassword(UserName, Password);
        //}
        //public static bool IsUserActive(string UserName)
        //{
        //    return clsUsersDataAccess.IsUserActive(UserName);
        //}



        ////Filter
        //public static DataTable GetByUserID(int UserID)
        //{
        //    return clsUsersDataAccess.GetByUserID(UserID);
        //}
        //public static DataTable GetByFullName(string FullName)
        //{
        //    return clsUsersDataAccess.GetByFullName(FullName);
        //}
        //public static DataTable GetByUserName(string UserName)
        //{
        //    return clsUsersDataAccess.GetByUserName(UserName);
        //}
        //public static DataTable GetByIsActive(byte IsActive)
        //{
        //    return clsUsersDataAccess.GetByIsActive(IsActive);
        //}
        //public static DataTable GetByPersonID(int PersonID)
        //{
        //    return clsUsersDataAccess.GetByPersonID(PersonID);
        //}



        //public static bool IsUserHasLinkedToOtherTables(int UserID)
        //{
        //    return clsUsersDataAccess.IsUserHasLinkedToOtherTables(UserID);
        //}
    }
}
