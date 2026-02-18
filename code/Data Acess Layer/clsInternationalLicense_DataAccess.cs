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
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID,
      ref int ApplicationID,
      ref int DriverID, ref int IssuedUsingLocalLicenseID,
      ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];


                                IsActive = (bool)reader["IsActive"];
                                CreatedByUserID = (int)reader["DriverID"];


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

        public static DataTable GetAllInternationalLicenses()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"
                                     SELECT    InternationalLicenseID, ApplicationID,DriverID,
		                                         IssuedUsingLocalLicenseID , IssueDate, 
                                                 ExpirationDate, IsActive
		                             from InternationalLicenses 
                                         order by IsActive, ExpirationDate desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //command.Parameters.AddWithValue("@DriverID", DriverID);


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
                // Console.WriteLine("Error: " + ex.Message);
            }

            return dt;

        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"
                                     SELECT    InternationalLicenseID, ApplicationID,
		                                         IssuedUsingLocalLicenseID , IssueDate, 
                                                 ExpirationDate, IsActive
		                             from InternationalLicenses where DriverID=@DriverID
                                         order by ExpirationDate desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);


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
                // Console.WriteLine("Error: " + ex.Message);
            }

            return dt;

        }


        public static int AddNewInternationalLicense(int ApplicationID,
             int DriverID, int IssuedUsingLocalLicenseID,
             DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalLicenseID = -1;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    //اذا مثلا اضفت رخصة جديده قفل الرخص اللي قبل 
                    string query = @"
                               Update InternationalLicenses 
                               set IsActive=0
                               where DriverID=@DriverID;

                             INSERT INTO InternationalLicenses
                               (
                                ApplicationID,
                                DriverID,
                                IssuedUsingLocalLicenseID,
                                IssueDate,
                                ExpirationDate,
                                IsActive,
                                CreatedByUserID)
                         VALUES
                               (@ApplicationID,
                                @DriverID,
                                @IssuedUsingLocalLicenseID,
                                @IssueDate,
                                @ExpirationDate,
                                @IsActive,
                                @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return InternationalLicenseID;

        }

        public static bool UpdateInternationalLicense(
              int InternationalLicenseID, int ApplicationID,
             int DriverID, int IssuedUsingLocalLicenseID,
             DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"UPDATE InternationalLicenses
                           SET 
                              ApplicationID=@ApplicationID,
                              DriverID = @DriverID,
                              IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                              IssueDate = @IssueDate,
                              ExpirationDate = @ExpirationDate,
                              IsActive = @IsActive,
                              CreatedByUserID = @CreatedByUserID
                         WHERE InternationalLicenseID=@InternationalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static int GetActiveInternationalLicenseIDByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    //كويري حلوه
                    string query = @"  
                            SELECT Top 1 InternationalLicenseID
                            FROM InternationalLicenses 
                            where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                            order by ExpirationDate Desc;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);


                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return InternationalLicenseID;
        }


        //public static bool GeInternationalLicenseByID(int InternationalLicenseID, ref int ApplicationID, ref int DriverID,
        //                     ref int  IssuedUsingLocalLicenseID, ref int CreatedByUserID, ref DateTime IssueDate, ref DateTime ExpirationDate,
        //                      ref bool IsActive)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select * from InternationalLicenses
        //                     where InternationalLicenseID = @InternationalLicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            ApplicationID = (int)reader["ApplicationID"];
        //            DriverID = (int)reader["DriverID"];
        //            IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            IssueDate = (DateTime)reader["IssueDate"];
        //            ExpirationDate = (DateTime)reader["ExpirationDate"];
        //            IsActive = (bool)reader["IsActive"];

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




        ////public static DataTable GetApplicationInfoByLDLAppID(int LDLAppID)
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from LocalDrivingLicenseApplications_View where LocalDrivingLicenseApplicationID like @LDLAppID";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@LDLAppID", LDLAppID + "%");


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}
        ////public static DataTable MyGetApplicationInfoByLDLAppID(int LDLAppID)
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from MyLDLApplications_View where LocalDrivingLicenseApplicationID like @LDLAppID";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@LDLAppID", LDLAppID + "%");


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}
        ////public static DataTable GetApplicationInfoByNationalNo(string NationalNo)
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from LocalDrivingLicenseApplications_View where NationalNo like @NationalNo";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@NationalNo", NationalNo);


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}
        ////public static DataTable GetApplicationInfoByFullName(string FullName)
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from LocalDrivingLicenseApplications_View where FullName like @FullName";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@FullName", FullName + "%");


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}
        ////public static DataTable GetApplicationInfoByStatus(string Status)
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from LocalDrivingLicenseApplications_View where Status like @Status";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@Status", Status + "%");


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}


        //public static int AddNewInternationalLicense(int ApplicationID, int DriverID,
        //                      int IssuedUsingLocalLicenseID, int CreatedByUserID, DateTime IssueDate, DateTime ExpirationDate,
        //                      bool IsActive)
        //{

        //    int InternationalLicenseID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD INSERT INTO InternationalLicenses(ApplicationID, DriverID, IssuedUsingLocalLicenseID, CreatedByUserID,
        //                     IssueDate, ExpirationDate, IsActive) 
        //                VALUES (@ApplicationID, @DriverID, @IssuedUsingLocalLicenseID, @CreatedByUserID,
        //                        @IssueDate, @ExpirationDate, @IsActive);
        //               SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@DriverID", DriverID);
        //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@IssueDate", IssueDate);
        //    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
        //    command.Parameters.AddWithValue("@IsActive", IsActive);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        //        {
        //            InternationalLicenseID = insertedID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //        connection.Close();
        //    }



        //    return InternationalLicenseID;
        //}

        //public static bool UpdateInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID,
        //                      int IssuedUsingLocalLicenseID, int CreatedByUserID, DateTime IssueDate, DateTime ExpirationDate,
        //                      bool IsActive)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update InternationalLicenses 
        //                   set ApplicationID = @ApplicationID, DriverID = @DriverID, IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID
        //                       CreatedByUserID = @CreatedByUserID, IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, IsActive = @IsActive
        //                   where InternationalLicenseID = @InternationalLicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@DriverID", DriverID);
        //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@IssueDate", IssueDate);
        //    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
        //    command.Parameters.AddWithValue("@IsActive", IsActive);

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


        ////public static bool IsLocalDrivingLicenseCancelled(int LocalDrivingLicenseApplicationID)
        ////{

        ////    //int RowsAffected = 0;
        ////    bool isUpdate = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD select found = 1 from LocalDrivingLicenseApplications_View
        ////                     where LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID and  Status = 'Cancelled'";

        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);



        ////    try
        ////    {
        ////        connection.Open();
        ////        int RowsAffected = command.ExecuteNonQuery();

        ////        if (RowsAffected > 0)
        ////        {
        ////            isUpdate = true;
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
        ////    return isUpdate;
        ////}
        ////public static bool IsLocalDrivingLicenseCompleted(int LocalDrivingLicenseApplicationID)
        ////{

        ////    //int RowsAffected = 0;
        ////    bool isUpdate = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD select found = 1 from LocalDrivingLicenseApplications_View
        ////                     where LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID and  Status = 'Completed'";

        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);



        ////    try
        ////    {
        ////        connection.Open();
        ////        int RowsAffected = command.ExecuteNonQuery();

        ////        if (RowsAffected > 0)
        ////        {
        ////            isUpdate = true;
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
        ////    return isUpdate;
        ////}



        //public static bool DeleteInternationalLicense(int InternationalLicenseID)
        //{
        //    //int RowsAffected = 0;
        //    bool isDelete = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Delete InternationalLicenses 
        //                where InternationalLicenseID = @InternationalLicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);


        //    try
        //    {
        //        connection.Open();
        //        int RowsAffected = command.ExecuteNonQuery();

        //        if (RowsAffected > 0)
        //        {
        //            isDelete = true;
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
        //    return isDelete;
        //}



        //public static bool IsInternationalLicenseExistByLocalLicense(int IssuedUsingLocalLicenseID)
        //{
        //    //int RowsAffected = 0;
        //    bool isExist = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select x = 1 from InternationalLicenses
        //                     where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);


        //    try
        //    {
        //        connection.Open();
        //        object result = command.ExecuteScalar();

        //        if (result != null)
        //        {
        //            isExist = true;
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
        //    return isExist;
        //}
        //public static int GetInternationalLicenseByLocalLicense(int IssuedUsingLocalLicenseID)
        //{
        //    //int RowsAffected = 0;
        //    int InternationalLicense = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select InternationalLicenseID from InternationalLicenses
        //                     where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);


        //    try
        //    {
        //        connection.Open();
        //        object result = command.ExecuteScalar();

        //        if (result != null)
        //        {
        //            InternationalLicense = Convert.ToInt32(result);
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
        //    return InternationalLicense;
        //}
        //public static int GetInternationalLicenseByApplicationID(int IssuedUsingLocalLicenseID)
        //{
        //    //int RowsAffected = 0;
        //    int InternationalLicense = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select ApplicationID from InternationalLicenses
        //                     where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);


        //    try
        //    {
        //        connection.Open();
        //        object result = command.ExecuteScalar();

        //        if (result != null)
        //        {
        //            InternationalLicense = Convert.ToInt32(result);
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
        //    return InternationalLicense;
        //}




        //public static DataTable GetAllInternationalLicense()
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  * from InternationalLicenses";
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


        ////public static DataTable GetApplicationInfoByStatus()
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD SELECT  * from LicenseClasses";
        ////    SqlCommand command = new SqlCommand(query, connection);


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}




        ////public static int GetApplicationID(int LocalDrivingLicenseApplicationID)
        ////{

        ////    //int RowsAffected = 0;
        ////    int ID = -1;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD select ApplicationID from LocalDrivingLicenseApplications
        ////                     where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);



        ////    try
        ////    {
        ////        connection.Open();
        ////        object Result = command.ExecuteScalar();

        ////        if (Result != null)
        ////        {
        ////            ID = (int)Result;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return ID;
        ////}




        ////public static DataTable FindFromFull(int LocalDrivingLicenseApplicationID)
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD Select * from LocalDrivingLicenseFullApplications_View
        ////                     where LocalDrivingLicenseApplicationID like @LocalDrivingLicenseApplicationID";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.HasRows)
        ////        {
        ////            dt.Load(reader);
        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return dt;
        ////}



        ////filter
        //public static DataTable GetByInt_License(int InternationalLicenseID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from InternationalLicenses where InternationalLicenseID = @InternationalLicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);



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
        //public static DataTable GetByApplicationID(int ApplicationID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from InternationalLicenses where ApplicationID = @ApplicationID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);



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
        //public static DataTable GetByDriverID(int DriverID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from InternationalLicenses where DriverID = @DriverID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DriverID", DriverID);



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
        //public static DataTable GetByL_License(int IssuedUsingLocalLicenseID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from InternationalLicenses where IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);



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

        //public static DataTable GetByIsActive(byte IsActive)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from InternationalLicenses where IsActive = @IsActive";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@IsActive", IsActive);



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
    }
}
