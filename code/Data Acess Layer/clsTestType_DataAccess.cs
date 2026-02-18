using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_MyDataAccess
{
    public class clsTestTypeDataAccess
    {

        public static bool GetTestTypeInfoByID(int TestTypeID,
            ref string TestTypeTitle, ref string TestDescription, ref float TestFees)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestTypeTitle = (string)reader["TestTypeTitle"];
                                TestDescription = (string)reader["TestTypeDescription"];
                                TestFees = Convert.ToSingle(reader["TestTypeFees"]);

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

        public static DataTable GetAllTestTypes()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM TestTypes order by TestTypeID";

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

        public static int AddNewTestType(string Title, string Description, float Fees)
        {
            int TestTypeID = -1;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeTitle,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@ApplicationFees)
                            where TestTypeID = @TestTypeID;
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestTypeID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return TestTypeID;
        }

        public static bool UpdateTestType(int TestTypeID, string Title, string Description, float Fees)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();


                    string query = @"Update  TestTypes  
                            set TestTypeTitle = @TestTypeTitle,
                                TestTypeDescription=@TestTypeDescription,
                                TestTypeFees = @TestTypeFees
                                where TestTypeID = @TestTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@TestTypeFees", Fees);

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








        //public static bool GetTestTypeByTestTypeID(int TestTypeID, ref string TestTypeTitle, ref string TestTypeDescription, ref decimal TestTypeFees)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from TestTypes where TestTypeID = @TestTypeID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            TestTypeTitle = (string)reader["TestTypeTitle"];
        //            TestTypeDescription = (string)reader["TestTypeDescription"];
        //            TestTypeFees = (decimal)reader["TestTypeFees"];

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





        ////public static bool GetUserInfoByUserName(ref int UserID, ref int PersonID, string UserName, ref string Password, ref byte IsActive)
        ////{
        ////    bool isfound = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from Users where UserName = @UserName";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@UserName", UserName);

        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.Read())
        ////        {
        ////            isfound = true;

        ////            UserID = (int)reader["UserID"];
        ////            PersonID = (int)reader["PersonID"];
        ////            Password = (string)reader["Password"];

        ////            if (reader["IsActive"] != DBNull.Value)
        ////            {
        ////                IsActive = Convert.ToByte(reader["IsActive"]);
        ////            }
        ////            else
        ////            {
        ////                // Handle the null case appropriately
        ////                IsActive = 0; // or any default value
        ////            }
        ////        }
        ////        else
        ////        {
        ////            isfound = false;

        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        isfound = false;

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return isfound;
        ////}
        ////public static bool GetUserInfoByPssword(ref int UserID, ref int PersonID, ref string UserName, string Password, ref byte IsActive)
        ////{
        ////    bool isfound = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = "use DVLD Select * from Users where Password = @Password";
        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@Password", Password);

        ////    try
        ////    {
        ////        connection.Open();
        ////        SqlDataReader reader = command.ExecuteReader();

        ////        if (reader.Read())
        ////        {
        ////            isfound = true;

        ////            UserID = (int)reader["UserID"];
        ////            PersonID = (int)reader["PersonID"];
        ////            UserName = (string)reader["UserName"];

        ////            if (reader["IsActive"] != DBNull.Value)
        ////            {
        ////                IsActive = Convert.ToByte(reader["IsActive"]);
        ////            }
        ////            else
        ////            {
        ////                // Handle the null case appropriately
        ////                IsActive = 0; // or any default value
        ////            }
        ////        }
        ////        else
        ////        {
        ////            isfound = false;

        ////        }

        ////        reader.Close();

        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        isfound = false;

        ////    }
        ////    finally
        ////    {
        ////        connection.Close();
        ////    }

        ////    return isfound;
        ////}


        ////public static int AddNewApplicationAndGetID(string ApplicationTypeTitle, decimal ApplicationFees)
        ////{

        ////    int UserID = -1;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees) 
        ////                VALUES (@ApplicationTypeTitle,@ApplicationFees);
        ////               SELECT SCOPE_IDENTITY();";

        ////    SqlCommand command = new SqlCommand(query, connection);

        ////    command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
        ////    command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

        ////    try
        ////    {
        ////        connection.Open();
        ////        object Result = command.ExecuteScalar();

        ////        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        ////        {
        ////            UserID = insertedID;
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {

        ////    }
        ////    finally
        ////    {

        ////        connection.Close();
        ////    }



        ////    return UserID;
        ////}

        //public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update TestTypes 
        //                   set TestTypeTitle = @TestTypeTitle, TestTypeDescription = @TestTypeDescription, TestTypeFees = @TestTypeFees
        //                   where TestTypeID = @TestTypeID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
        //    command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
        //    command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
        //    command.Parameters.AddWithValue("@TestTypeFees", TestTypeFees);


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


        ////public static bool DeleteUser(int UserID)
        ////{
        ////    //int RowsAffected = 0;
        ////    bool isDelete = false;

        ////    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        ////    string query = @"use DVLD Delete Users 
        ////                where UserID = @UserID";

        ////    SqlCommand command = new SqlCommand(query, connection);
        ////    command.Parameters.AddWithValue("@UserID", UserID);


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



        //public static DataTable GetALlTestTypes()
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from TestTypes";
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

    }
}
