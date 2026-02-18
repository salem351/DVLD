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
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID,
            ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime AppointmentDate, ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                TestTypeID = (int)reader["TestTypeID"];
                                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                IsLocked = (bool)reader["IsLocked"];

                                if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                    RetakeTestApplicationID = -1;
                                else
                                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];

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

        public static bool GetLastTestAppointment(
             int LocalDrivingLicenseApplicationID, int TestTypeID,
            ref int TestAppointmentID, ref DateTime AppointmentDate,
            ref float PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"SELECT       top 1 *
                                   FROM            TestAppointments
                                   WHERE        (TestTypeID = @TestTypeID) 
                                   AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID) 
                                   order by TestAppointmentID Desc";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                IsLocked = (bool)reader["IsLocked"];

                                if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                    RetakeTestApplicationID = -1;
                                else
                                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];


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

        public static DataTable GetAllTestAppointments()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select * from TestAppointments_View
                                  order by AppointmentDate Desc";

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
                // Console.WriteLine("Error: " + ex.Message);
            }

            return dt;
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"SELECT TestAppointmentID, AppointmentDate,PaidFees, IsLocked
                        FROM TestAppointments
                        WHERE  
                        (TestTypeID = @TestTypeID) 
                        AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                        order by TestAppointmentID desc;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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

        public static int AddNewTestAppointment(
             int TestTypeID, int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate, float PaidFees, int CreatedByUserID, int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Insert Into TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
                            Values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,0,@RetakeTestApplicationID);
                
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        if (RetakeTestApplicationID == -1)

                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);


                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestAppointmentID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return TestAppointmentID;
        }

        public static bool UpdateTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate, float PaidFees,
             int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"Update  TestAppointments  
                            set TestTypeID = @TestTypeID,
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                AppointmentDate = @AppointmentDate,
                                PaidFees = @PaidFees,
                                CreatedByUserID = @CreatedByUserID,
                                IsLocked=@IsLocked,
                                RetakeTestApplicationID=@RetakeTestApplicationID
                                where TestAppointmentID = @TestAppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@IsLocked", IsLocked);

                        if (RetakeTestApplicationID == -1)

                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);


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


        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select TestID from Tests where TestAppointmentID=@TestAppointmentID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return TestID;
        }


        //public static bool GetTestAppointmentByID(int TestAppointmentID,  ref int TestTypeID, ref int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate,
        //                                      ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD select * from TestAppointments
        //                     where TestAppointmentID = @TestAppointmentID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            IsLocked = (bool)reader["IsLocked"];
        //            AppointmentDate = (DateTime)reader["AppointmentDate"];
        //            PaidFees = (decimal)reader["PaidFees"];
        //            TestTypeID = (int)reader["TestTypeID"];
        //            LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];


        //            if (reader["RetakeTestApplicationID"] != DBNull.Value)
        //                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
        //            else
        //                RetakeTestApplicationID = -1;
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
        //public static bool GetTestAppointmentByL_D_D_AppID(ref int TestAppointmentID, ref int TestTypeID, int LocalDrivingLicenseApplicationID, ref DateTime AppointmentDate,
        //                                      ref decimal PaidFees, ref int CreatedByUserID, ref bool IsLocked, ref int RetakeTestApplicationID)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD select * from TestAppointments
        //                     where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            IsLocked = (bool)reader["IsLocked"];
        //            AppointmentDate = (DateTime)reader["AppointmentDate"];
        //            PaidFees = (decimal)reader["PaidFees"];
        //            TestTypeID = (int)reader["TestTypeID"];
        //            TestAppointmentID = (int)reader["TestAppointmentID"];


        //            if (reader["RetakeTestApplicationID"] != DBNull.Value)
        //                RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
        //            else
        //                RetakeTestApplicationID = -1;
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


        //public static int AddNewTestAppointment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
        //                                       decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        //{

        //    int LocalDrivingLicense = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD INSERT INTO TestAppointments(TestTypeID, LocalDrivingLicenseApplicationID,AppointmentDate,
        //                                           PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID) 
        //                VALUES (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,
        //                        @IsLocked, @RetakeTestApplicationID);
        //               SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
        //    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
        //    command.Parameters.AddWithValue("@PaidFees", PaidFees);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@IsLocked", IsLocked);
        //    //command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

        //    if (RetakeTestApplicationID != -1)
        //        command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
        //    else
        //        command.Parameters.AddWithValue("@RetakeTestApplicationID", System.DBNull.Value);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        //        {
        //            LocalDrivingLicense = insertedID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //        connection.Close();
        //    }



        //    return LocalDrivingLicense;
        //}

        //public static bool UpdateTestAppointment(int TestAppointmentID, DateTime AppointmentDate)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update TestAppointments  
        //                   set AppointmentDate = @AppointmentDate
        //                   where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
        //    command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);


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


        //public static bool DeleteTestAppointment(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    bool isDelete = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Delete TestAppointments 
        //                where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


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


        //public static bool IsApplicantHasTestAppointment(int LocalDrivingLicenseApplicationID, int TestTypeID)
        //{
        //    //int RowsAffected = 0;
        //    bool IsExist = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select found = 1 from TestAppointments 
        //                        where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
        //    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsExist = true;
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
        //    return IsExist;
        //}
        //public static bool IsApplicantHasNewOneAppointment(int LocalDrivingLicenseApplicationID)
        //{
        //    //int RowsAffected = 0;
        //    bool IsExist = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select found = 1 from TestAppointments 
        //                        where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and IsLocked = 0";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsExist = true;
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
        //    return IsExist;
        //}
        //public static bool IsApplicantTakeTest(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    bool IsExist = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select found = 1 from TestAppointments 
        //                        where TestAppointmentID = @TestAppointmentID and IsLocked = 1";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsExist = true;
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
        //    return IsExist;
        //}





        //public static int GetTestTypeID(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    int TestTypeID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select TestTypeID from TestAppointments 
        //                        where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            TestTypeID = (int)Result;
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
        //    return TestTypeID;
        //}
        //public static int GetTrail(int LocalDrivingLicenseApplicationID)
        //{
        //    //int RowsAffected = 0;
        //    int Trail = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select count (*) from TestAppointments 
        //                        where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            Trail = (int)Result;
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
        //    return Trail;
        //}
        //public static int GetRetakeTestApplicationID(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    int RetakeTestApplicationID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select RetakeTestApplicationID from TestAppointments 
        //                        where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            RetakeTestApplicationID = (int)Result;
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
        //    return RetakeTestApplicationID;
        //}
        //public static DateTime GetAppointmentDate(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    DateTime AppointmentDate = DateTime.Now;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select AppointmentDate from TestAppointments 
        //                        where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            AppointmentDate = (DateTime)Result;
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
        //    return AppointmentDate;
        //}








        //public static bool IsRetakeTestApplication(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    bool IsExist = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD select found = 1 from TestAppointments
        //                        where TestAppointmentID = @TestAppointmentID and RetakeTestApplicationID is not null";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsExist = true;
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
        //    return IsExist;
        //}
        //public static bool IsRetakeTestApplicationForEdit(int TestAppointmentID)
        //{
        //    //int RowsAffected = 0;
        //    bool IsExist = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD select found = 1 from TestAppointments
        //                        where TestAppointmentID = @TestAppointmentID and RetakeTestApplicationID is not null and IsLocked = 0 ";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsExist = true;
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
        //    return IsExist;
        //}


        //public static DataTable GetAllTestAppointments()
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  * from TestAppointments";
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


        //public static DataTable GetTestAppointmentByL_D_D_AppID(int LocalDrivingLicenseApplicationID, int TestTypeID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  TestAppointmentID, AppointmentDate, PaidFees, IsLocked   
        //                     from TestAppointments 
        //                     where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID and TestTypeID = @TestTypeID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
        //    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

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


        //public static bool UpdateApplicationByRetake(int RetakeTestApplicationID, int TestAppointmentID)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update TestAppointments  
        //                   set RetakeTestApplicationID = @RetakeTestApplicationID
        //                   where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);



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

        //public static bool UpdateTestApplicationTolock(int TestAppointmentID)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update TestAppointments  
        //                   set IsLocked = 1
        //                   where TestAppointmentID = @TestAppointmentID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);



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
