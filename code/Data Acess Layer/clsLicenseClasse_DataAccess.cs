using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DVLD_MyDataAccess
{
    public class clsLicenseClassData
    {
        public static bool GetLicenseClassInfoByID(int LicenseClassID,
        ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
        ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ClassName = (string)reader["ClassName"];
                                ClassDescription = (string)reader["ClassDescription"];
                                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                                DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                                ClassFees = Convert.ToSingle(reader["ClassFees"]);

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


        public static bool GetLicenseClassInfoByClassName(string ClassName, ref int LicenseClassID,
            ref string ClassDescription, ref byte MinimumAllowedAge,
           ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClassName", ClassName);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                LicenseClassID = (int)reader["LicenseClassID"];
                                ClassDescription = (string)reader["ClassDescription"];
                                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                                DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                                ClassFees = Convert.ToSingle(reader["ClassFees"]);

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



        public static DataTable GetAllLicenseClasses()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM LicenseClasses order by ClassName";

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

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Insert Into LicenseClasses 
                                    (
                                     ClassName,ClassDescription,MinimumAllowedAge, 
                                     DefaultValidityLength,ClassFees)
                                                     Values ( 
                                     @ClassName,@ClassDescription,@MinimumAllowedAge, 
                                     @DefaultValidityLength,@ClassFees)
                                                     where LicenseClassID = @LicenseClassID;
                                                     SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ClassName", ClassName);
                        command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                        command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                        command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                        command.Parameters.AddWithValue("@ClassFees", ClassFees);


                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseClassID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return LicenseClassID;
        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName,
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"Update  LicenseClasses  
                            set ClassName = @ClassName,
                                ClassDescription = @ClassDescription,
                                MinimumAllowedAge = @MinimumAllowedAge,
                                DefaultValidityLength = @DefaultValidityLength,
                                ClassFees = @ClassFees
                                where LicenseClassID = @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@ClassName", ClassName);
                        command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                        command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                        command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                        command.Parameters.AddWithValue("@ClassFees", ClassFees);

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



        //public static bool GetLicenseClassesByID(int LicenseClassID, ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
        //                                          ref byte DefaultValidityLength, ref decimal ClassFees)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from LicenseClasses where LicenseClassID = @LicenseClassID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            ClassName = (string)reader["ClassName"];
        //            ClassDescription = (string)reader["ClassDescription"];
        //            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
        //            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
        //            ClassFees = (decimal)reader["ClassFees"];

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
        //public static bool GetLicenseClassesByClassName(ref int LicenseClassID, string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge,
        //                                        ref byte DefaultValidityLength, ref decimal ClassFees)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from LicenseClasses where ClassName = @ClassName";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@ClassName", ClassName);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            LicenseClassID = (int)reader["LicenseClassID"];
        //            ClassDescription = (string)reader["ClassDescription"];
        //            MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
        //            DefaultValidityLength = (byte)reader["DefaultValidityLength"];
        //            ClassFees = (decimal)reader["ClassFees"];

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




        ////public static int AddNewLocalDrivingLicense(int ApplicationID, int LicenseClassID)
        ////{

        ////    int LocalDrivingLicense = -1;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD INSERT INTO LocalDrivingLicenseApplications(ApplicationID, LicenseClassID) 
        ////                VALUES (@ApplicationID,@LicenseClassID);
        ////               SELECT SCOPE_IDENTITY();";

        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        ////    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);



        ////    try
        ////    {
        ////        connection.Open();
        ////        object Result = command.ExecuteScalar();

        ////        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        ////        {
        ////            LocalDrivingLicense = insertedID;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {

        ////        connection.Close();
        ////    }



        ////    return LocalDrivingLicense;
        ////}

        ////public static bool UpdateLocalDrivingLicense(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        ////{

        ////    //int RowsAffected = 0;
        ////    bool isUpdate = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD Update People 
        ////                   set ApplicationID = @ApplicationID, LicenseClassID = @LicenseClassID
        ////                   where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
        ////    command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
        ////    command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


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


        ////public static bool DeleteLocalDrivingLicense(int LocalDrivingLicenseApplicationID)
        ////{
        ////    //int RowsAffected = 0;
        ////    bool isDelete = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD Delete LocalDrivingLicenseApplications 
        ////                where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


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



        ////public static DataTable GetAllApplications()
        ////{
        ////    DataTable dt = new DataTable();

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD SELECT  * from LocalDrivingLicenseApplications_View";
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




        ////public static bool IsPersonExist(int PersonID)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select found = 1 from People where PersonID = @PersonID";
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
        ////public static bool IsPersonExistWithNationalNo(string NationalNo)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select found = 1 from People where NationalNo = @NationalNo";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@NationalNo", NationalNo);

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
        ////public static bool IsItUserByNationalNo(string NationalNo)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select found = 1 from UsersWithNationalNo where NationalNo = @NationalNo";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@NationalNo", NationalNo);

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





        ////public static bool IsPersonHasLinkedToOtherTables(int PersonID)
        ////{
        ////    bool IsExist = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD Select HasLink = 1 FROM People
        ////                            WHERE EXISTS(
        ////                             SELECT PersonID FROM Users 
        ////                              WHERE Users.PersonID = People.PersonID ) and PersonID = @PersonID";
        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@PersonID", PersonID);

        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();


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


    }
}
