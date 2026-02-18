using DVLD_MyDataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static DVLD_MyDataAccess.clsCountryDataAccess;

namespace Data_Access
{
    public class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
            ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];

                                if (reader["Notes"] == DBNull.Value)
                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
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

        public static DataTable GetAllLicenses()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Licenses";

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

        public static DataTable GetDriverLicenses(int DriverID)
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc";

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

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass,
             DateTime IssueDate, DateTime ExpirationDate, string Notes,
             float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"
                              INSERT INTO Licenses
                               (ApplicationID,
                                DriverID,
                                LicenseClass,
                                IssueDate,
                                ExpirationDate,
                                Notes,
                                PaidFees,
                                IsActive,IssueReason,
                                CreatedByUserID)
                         VALUES
                               (
                               @ApplicationID,
                               @DriverID,
                               @LicenseClass,
                               @IssueDate,
                               @ExpirationDate,
                               @Notes,
                               @PaidFees,
                               @IsActive,@IssueReason, 
                               @CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);

                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        if (Notes == "")
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", Notes);

                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);

                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }

            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
             DateTime IssueDate, DateTime ExpirationDate, string Notes,
             float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"UPDATE Licenses
                           SET ApplicationID=@ApplicationID, DriverID = @DriverID,
                              LicenseClass = @LicenseClass,
                              IssueDate = @IssueDate,
                              ExpirationDate = @ExpirationDate,
                              Notes = @Notes,
                              PaidFees = @PaidFees,
                              IsActive = @IsActive,IssueReason=@IssueReason,
                              CreatedByUserID = @CreatedByUserID
                         WHERE LicenseID=@LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        if (Notes == "")
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", Notes);

                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);
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

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"SELECT        Licenses.LicenseID
                            FROM Licenses INNER JOIN
                                                     Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE  
                             
                             Licenses.LicenseClass = @LicenseClass 
                              AND Drivers.PersonID = @PersonID
                              And IsActive=1;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClassID);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return LicenseID;
        }

        public static bool DeactivateLicense(int LicenseID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"UPDATE Licenses
                           SET 
                              IsActive = 0
                             
                         WHERE LicenseID=@LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


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


        //public static bool GetLicenseByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
        //                                         ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
        //                                                  ref decimal PaidFees, ref bool IsActive,
        //                                                   ref byte IssueReason, ref int CreatedByUserID)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Licenses where LicenseID = @LicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            ApplicationID = (int)reader["ApplicationID"];
        //            DriverID = (int)reader["DriverID"];
        //            LicenseClass = (int)reader["LicenseClass"];
        //            IssueDate = (DateTime)reader["IssueDate"];
        //            ExpirationDate = (DateTime)reader["ExpirationDate"];                 
        //            PaidFees = (decimal)reader["PaidFees"];
        //            IsActive = (bool)reader["IsActive"];
        //            IssueReason = (byte)reader["IssueReason"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];

        //            if (reader["Notes"] != DBNull.Value)
        //                Notes = (string)reader["Notes"].ToString();
        //            else
        //                Notes = "";

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
        //public static bool GetLicenseByApplicationID(ref int LicenseID, int ApplicationID, ref int DriverID, ref int LicenseClass,
        //                                         ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
        //                                                  ref decimal PaidFees, ref bool IsActive,
        //                                                   ref byte IssueReason, ref int CreatedByUserID)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Licenses where ApplicationID = @ApplicationID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            LicenseID = (int)reader["LicenseID"];
        //            DriverID = (int)reader["DriverID"];
        //            LicenseClass = (int)reader["LicenseClass"];
        //            IssueDate = (DateTime)reader["IssueDate"];
        //            ExpirationDate = (DateTime)reader["ExpirationDate"];
        //            PaidFees = (decimal)reader["PaidFees"];
        //            IsActive = (bool)reader["IsActive"];
        //            IssueReason = (byte)reader["IssueReason"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];

        //            if (reader["Notes"] != DBNull.Value)
        //                Notes = (string)reader["Notes"].ToString();
        //            else
        //                Notes = "";

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




        //public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
        //                                string Notes, decimal PaidFees, bool IsActive, byte IssueReason,  int CreatedByUserID)
        //{

        //    int LocalDrivingLicense = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD INSERT INTO Licenses(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate,
        //                     Notes, PaidFees, IsActive , IssueReason, CreatedByUserID) 
        //                VALUES (@ApplicationID,@DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees,
        //                        @IsActive, @IssueReason, @CreatedByUserID);
        //               SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@DriverID", DriverID);
        //    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
        //    command.Parameters.AddWithValue("@IssueDate", IssueDate);
        //    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);          
        //    command.Parameters.AddWithValue("@PaidFees", PaidFees);
        //    command.Parameters.AddWithValue("@IsActive", IsActive);
        //    command.Parameters.AddWithValue("@IssueReason", IssueReason);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


        //    if (Notes != "")
        //        command.Parameters.AddWithValue("@Notes", Notes);
        //    else
        //        command.Parameters.AddWithValue("@Notes", System.DBNull.Value);


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

        //public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
        //                                string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update Licenses 
        //                   set ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClass = @LicenseClass, 
        //                       IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, Notes = @Notes, PaidFees = @PaidFees, 
        //                       IsActive = @IsActive, IssueReason = @IssueReason, CreatedByUserID = @CreatedByUserID
        //                   where LicenseID = @LicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);


        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);
        //    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        //    command.Parameters.AddWithValue("@DriverID", DriverID);
        //    command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
        //    command.Parameters.AddWithValue("@IssueDate", IssueDate);
        //    command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
        //    command.Parameters.AddWithValue("@PaidFees", PaidFees);
        //    command.Parameters.AddWithValue("@IsActive", IsActive);
        //    command.Parameters.AddWithValue("@IssueReason", IssueReason);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


        //    if (Notes != "")
        //        command.Parameters.AddWithValue("@Notes", Notes);
        //    else
        //        command.Parameters.AddWithValue("@Notes", System.DBNull.Value);




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



        //public static DataTable GetAllLicenseToDriver(int DriverID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  * from Licenses
        //                     where DriverID = @DriverID";
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



        ////تجيب كل المعلومات التي في showinfo form
        //public static DataTable ShowLicense(int LicenseID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  * from LicenseInfo_View
        //                     where LicenseID = @LicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);

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


        //public static int GetApplicationID(int LicenseID)
        //{

        //    //int RowsAffected = 0;
        //    int ID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD select ApplicationID from Licenses
        //                     where LicenseID = @LicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            ID = (int)Result;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return ID;
        //}


        //public static bool UpdateToBeNotActive(int LicenseID)
        //{

        //    //int RowsAffected = 0;
        //    bool IsUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update Licenses 
        //                   set IsActive = 0
        //                   where LicenseID = @LicenseID";


        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);



        //    try
        //    {
        //        connection.Open();
        //        int RowsAffected = command.ExecuteNonQuery();

        //        if (RowsAffected > 0)
        //        {
        //            IsUpdate = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return IsUpdate;
        //}
        //public static bool UpdateToBeActive(int LicenseID)
        //{

        //    //int RowsAffected = 0;
        //    bool IsUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update Licenses 
        //                   set IsActive = 1
        //                   where LicenseID = @LicenseID";


        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);



        //    try
        //    {
        //        connection.Open();
        //        int RowsAffected = command.ExecuteNonQuery();

        //        if (RowsAffected > 0)
        //        {
        //            IsUpdate = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }

        //    return IsUpdate;
        //}






        //public static DataTable GetLocalLicense(int PersonID)
        //{
        //    DataTable dt = new DataTable();

        //    //اضهر كل الرخص ماعد الرخصة الدولة وهي رقم 6
        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive, ApplicationTypes.ApplicationTypeID, People.NationalNo
        //                     FROM  Licenses INNER JOIN Applications
        //                     ON Licenses.ApplicationID = Applications.ApplicationID INNER JOIN ApplicationTypes
        //                     ON Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID INNER JOIN LicenseClasses
        //                     ON Licenses.LicenseClass = LicenseClasses.LicenseClassID INNER JOIN People 
        //                     ON Applications.ApplicantPersonID = People.PersonID

        //                     where ApplicationTypes.ApplicationTypeID != 6 and  People.PersonID = @PersonID";
        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@PersonID", PersonID);

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
        //public static DataTable GetInternationalLicense(int ApplicantPersonID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //     string query = @"use DVLD Select IntLec.InternationalLicenseID, IntLec.ApplicationID, IntLec.InternationalLicenseID, IntLec.IssueDate
        //                      , IntLec.ExpirationDate, IntLec.IsActive from InternationalLicenses IntLec
        //                      where ApplicationID in (Select ApplicationID from Applications
        //                      where ApplicantPersonID = @ApplicantPersonID)";
        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);

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
