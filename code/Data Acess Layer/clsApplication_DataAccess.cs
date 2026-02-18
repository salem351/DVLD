using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_MyDataAccess
{
    public class clsApplicationData
    {
        public static bool GetApplicationInfoByID(int ApplicationID,
               ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID,
               ref byte ApplicationStatus, ref DateTime LastStatusDate,
               ref float PaidFees, ref int CreatedByUserID)
        {

            bool isFound = false;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                ApplicationStatus = (byte)reader["ApplicationStatus"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];


                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }


            return isFound;
        }

        //0 Reference that mean no one use it
        public static DataTable GetAllApplications()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "select * from ApplicationsList_View order by ApplicationDate desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)

                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return dt;

        }

        public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)
        {

            //this function will return the new person id if succeeded and -1 if not.
            int ApplicationID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO Applications ( 
                            ApplicantPersonID,ApplicationDate,ApplicationTypeID,
                            ApplicationStatus,LastStatusDate,
                            PaidFees,CreatedByUserID)
                             VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,
                                      @ApplicationStatus,@LastStatusDate,
                                      @PaidFees,   @CreatedByUserID);
                             SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
                        command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
                        command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
                        command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
                        command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
                        command.Parameters.AddWithValue("PaidFees", @PaidFees);
                        command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ApplicationID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
            }

            return ApplicationID;
        }


        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Update  Applications  
                            set ApplicantPersonID = @ApplicantPersonID,
                                ApplicationDate = @ApplicationDate,
                                ApplicationTypeID = @ApplicationTypeID,
                                ApplicationStatus = @ApplicationStatus, 
                                LastStatusDate = @LastStatusDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID=@CreatedByUserID
                            where ApplicationID=@ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("ApplicantPersonID", @ApplicantPersonID);
                        command.Parameters.AddWithValue("ApplicationDate", @ApplicationDate);
                        command.Parameters.AddWithValue("ApplicationTypeID", @ApplicationTypeID);
                        command.Parameters.AddWithValue("ApplicationStatus", @ApplicationStatus);
                        command.Parameters.AddWithValue("LastStatusDate", @LastStatusDate);
                        command.Parameters.AddWithValue("PaidFees", @PaidFees);
                        command.Parameters.AddWithValue("CreatedByUserID", @CreatedByUserID);


                        rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            return (rowsAffected > 0);
        }

        public static bool DeleteApplication(int ApplicationID)
        {

            int rowsAffected = 0;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Delete Applications 
                                where ApplicationID = @ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



                        rowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }

            return (rowsAffected > 0);

        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT Found=1 FROM Applications WHERE ApplicationID = @ApplicationID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            isFound = reader.HasRows;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {

            //incase the ActiveApplication ID !=-1 return true.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        public static int GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT ActiveApplicationID=ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID and ApplicationTypeID=@ApplicationTypeID and ApplicationStatus=1";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                        object result = command.ExecuteScalar();


                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                        {
                            ActiveApplicationID = AppID;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return ActiveApplicationID;
            }

            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"SELECT ActiveApplicationID=Applications.ApplicationID  
                            From
                            Applications INNER JOIN
                            LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
                            WHERE ApplicantPersonID = @ApplicantPersonID 
                            and ApplicationTypeID=@ApplicationTypeID 
							and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                            and ApplicationStatus=1";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        object result = command.ExecuteScalar();


                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                        {
                            ActiveApplicationID = AppID;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return ActiveApplicationID;
            }

            return ActiveApplicationID;
        }

        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Update  Applications  
                            set 
                                ApplicationStatus = @NewStatus, 
                                LastStatusDate = @LastStatusDate
                            where ApplicationID=@ApplicationID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        command.Parameters.AddWithValue("LastStatusDate", DateTime.Now);

                        rowsAffected = command.ExecuteNonQuery();


                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            return (rowsAffected > 0);
        }



        //public static bool GetApplicationByID(int ApplicationID, ref int ApplicantPersonID, ref DateTime ApplicationDate, ref DateTime LastStatusDate,
        //                                      ref int ApplicationTypeID, ref byte ApplicationStatus, ref int CreatedByUserID, ref decimal PaidFees)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Applications where ApplicationID = @ApplicationID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            ApplicantPersonID = (int)reader["ApplicantPersonID"];
        //            ApplicationDate = (DateTime)reader["ApplicationDate"];
        //            LastStatusDate = (DateTime)reader["LastStatusDate"];
        //            ApplicationTypeID = (int)reader["ApplicationTypeID"];
        //            ApplicationStatus = (byte)reader["ApplicationStatus"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            PaidFees = (decimal)reader["PaidFees"];

        //        }
        //        else
        //        {
        //            isfound = false;

        //        }

        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        isfound = false;

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return isfound;
        //}


        //public static int AddNewApplication(int ApplicantPersonID, DateTime ApplicationDate, DateTime LastStatusDate,
        //                                            int ApplicationTypeID, byte ApplicationStatus, int CreatedByUserID, decimal PaidFees)
        //{

        //    int UserID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD INSERT INTO Applications (ApplicantPersonID, ApplicationDate, LastStatusDate, ApplicationTypeID, 
        //                                                        ApplicationStatus, CreatedByUserID, PaidFees) 
        //                VALUES (@ApplicantPersonID,@ApplicationDate, @LastStatusDate, @ApplicationTypeID, @ApplicationStatus, @CreatedByUserID,
        //                        @PaidFees);
        //               SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
        //    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
        //    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
        //    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
        //    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@PaidFees", PaidFees);

        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        //        {
        //            UserID = insertedID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //        connection.Close();
        //    }



        //    return UserID;
        //}

        //public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, DateTime LastStatusDate,
        //                                            int ApplicationTypeID, byte ApplicationStatus, int CreatedByUserID, decimal PaidFees)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update Applications 
        //                   set ApplicantPersonID = @ApplicantPersonID, ApplicationDate = @ApplicationDate, LastStatusDate = @LastStatusDate,
        //                       ApplicationTypeID = @ApplicationTypeID, ApplicationStatus = @ApplicationStatus, CreatedByUserID = @CreatedByUserID,
        //                       PaidFees = @PaidFees 
        //                   where ApplicationID = @ApplicationID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
        //    command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
        //    command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
        //    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
        //    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@PaidFees", PaidFees);


        //    try
        //    {
        //        connection.Open();
        //        int RowsAffected = command.ExecuteNonQuery();

        //        if (RowsAffected > 0)
        //        {
        //            isUpdate = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    //return (RowsAffected > 0);
        //    return isUpdate;
        //}


        ////public static bool DeleteApplication(int ApplicationID)
        ////{
        ////    //int RowsAffected = 0;
        ////    bool isDelete = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD Delete Applications 
        ////                where ApplicationID = @ApplicationID";

        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


        ////    try
        ////    {
        ////        connection.Open();
        ////        int RowsAffected = command.ExecuteNonQuery();

        ////        if (RowsAffected > 0)
        ////        {
        ////            isDelete = true;
        ////        }

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    //return (RowsAffected > 0);
        ////    return isDelete;
        ////}



        //public static DataTable GetAllApplications()
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Applications";
        //    SqlCommand command = new SqlCommand(query, connection);


        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.HasRows)
        //        {
        //            dt.Load(reader);
        //        }

        //        reader.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return dt;
        //}



        ////public static bool IsUserExist(int UserID)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select found = 1 from Users where UserID = @UserID";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@UserID", UserID);

        ////    try
        ////    {
        ////        connection.Open();
        ////        //object Result = command.ExecuteScalar();
        ////        SqlDataReader reader = command.ExecuteReader();


        ////        //if (Result != null)
        ////        //{
        ////        //    IsExist = true;
        ////        //}

        ////        IsExist = reader.HasRows;
        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }


        ////    return IsExist;

        ////}
        ////public static bool IsUserExistByPersonID(int PersonID)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select found = 1 from Users where PersonID = @PersonID";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@PersonID", PersonID);

        ////    try
        ////    {
        ////        connection.Open();
        ////        //object Result = command.ExecuteScalar();
        ////        SqlDataReader reader = command.ExecuteReader();


        ////        //if (Result != null)
        ////        //{
        ////        //    IsExist = true;
        ////        //}

        ////        IsExist = reader.HasRows;
        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }


        ////    return IsExist;

        ////}

        ////public static bool IsUserExistByUserNameAndPassword(string UserName, string Password)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select found = 1 from Users where Password = @Password AND UserName = @UserName";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@UserName", UserName);
        ////    command.Parameters.AddWithValue("@Password", Password);

        ////    try
        ////    {
        ////        connection.Open();
        ////        object Result = command.ExecuteScalar();
        ////        //SqlDataReader reader = command.ExecuteReader();


        ////        if (Result != null)
        ////        {
        ////            IsExist = true;
        ////        }

        ////        // IsExist = reader.HasRows;
        ////        // reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }


        ////    return IsExist;

        ////}
        ////public static bool IsUserActive(string UserName)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select IsActive = 1 from Users where UserName = @UserName and IsActive = 1 ";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@UserName", UserName);

        ////    try
        ////    {
        ////        connection.Open();
        ////        object Result = command.ExecuteScalar();
        ////        //SqlDataReader reader = command.ExecuteReader();


        ////        if (Result != null)
        ////        {
        ////            IsExist = true;
        ////        }

        ////        // IsExist = reader.HasRows;
        ////        // reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }


        ////    return IsExist;

        ////}



        //public static bool CancelApplication(int LocalDrivingLicenseApplicationID)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update LocalDrivingLicenseFullApplications_View 
        //                   set ApplicationStatus = 2,  LastStatusDate = GETDATE()
        //                   where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);



        //    try
        //    {
        //        connection.Open();
        //        int RowsAffected = command.ExecuteNonQuery();

        //        if (RowsAffected > 0)
        //        {
        //            isUpdate = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    //return (RowsAffected > 0);
        //    return isUpdate;
        //}

        //public static int GetApplicationPersonID(int ApplicationID)
        //{

        //    //int RowsAffected = 0;
        //    int PersonID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select ApplicantPersonID from Applications
        //                     where ApplicationID = @ApplicationID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            PersonID = Convert.ToInt32(Result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    //return (RowsAffected > 0);
        //    return PersonID;
        //}

        //public static int GetApplicationPersonID_ByLocalLicenseID(int LocalDrivingLicenseApplicationID)
        //{

        //    //int RowsAffected = 0;
        //    int PersonID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD select ApplicantPersonID from Applications
        //                     where ApplicationID in (Select ApplicationID from LocalDrivingLicenseApplications
        //                     where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            PersonID = Convert.ToInt32(Result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    //return (RowsAffected > 0);
        //    return PersonID;
        //}


        //public static bool CompleteApplication(int ApplicationID, int ApplicationStatus)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update Applications 
        //                   set ApplicationStatus = @ApplicationStatus,  LastStatusDate = GETDATE()
        //                   where ApplicationID = @ApplicationID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);



        //    try
        //    {
        //        connection.Open();
        //        int RowsAffected = command.ExecuteNonQuery();

        //        if (RowsAffected > 0)
        //        {
        //            isUpdate = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    //return (RowsAffected > 0);
        //    return isUpdate;
        //}

    }
}
