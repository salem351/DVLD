using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_MyDataAccess
{
    public class clsApplicationTypeDataAccess
    {
        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID,
          ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
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

        public static DataTable GetAllApplicationTypes()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM ApplicationTypes order by ApplicationTypeTitle";


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

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationTypeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@Title,@Fees)
                            
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);


                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ApplicationTypeID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return ApplicationTypeID;

        }

        public static bool UpdateApplicationType(int ApplicationTypeID, string Title, float Fees)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Update  ApplicationTypes  
                            set ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                                where ApplicationTypeID = @ApplicationTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@Title", Title);
                        command.Parameters.AddWithValue("@Fees", Fees);


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


        //public static bool GetUserInfoByUserName(ref int UserID, ref int PersonID, string UserName, ref string Password, ref byte IsActive)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Users where UserName = @UserName";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@UserName", UserName);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            UserID = (int)reader["UserID"];
        //            PersonID = (int)reader["PersonID"];
        //            Password = (string)reader["Password"];

        //            if (reader["IsActive"] != DBNull.Value)
        //            {
        //                IsActive = Convert.ToByte(reader["IsActive"]);
        //            }
        //            else
        //            {
        //                // Handle the null case appropriately
        //                IsActive = 0; // or any default value
        //            }
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
        //public static bool GetUserInfoByPssword(ref int UserID, ref int PersonID, ref string UserName, string Password, ref byte IsActive)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Users where Password = @Password";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@Password", Password);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            UserID = (int)reader["UserID"];
        //            PersonID = (int)reader["PersonID"];
        //            UserName = (string)reader["UserName"];

        //            if (reader["IsActive"] != DBNull.Value)
        //            {
        //                IsActive = Convert.ToByte(reader["IsActive"]);
        //            }
        //            else
        //            {
        //                // Handle the null case appropriately
        //                IsActive = 0; // or any default value
        //            }
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



        //public static bool DeleteUser(int UserID)
        //{
        //    //int RowsAffected = 0;
        //    bool isDelete = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Delete Users 
        //                where UserID = @UserID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@UserID", UserID);


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


        //public static decimal GetApplicationTypesFee(int ApplicationTypeID)
        //{
        //    decimal Fee = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select ApplicationFees from ApplicationTypes
        //                     where ApplicationTypeID = @ApplicationTypeID";
        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();


        //        if (Result != null)
        //        {
        //            Fee = (decimal)Result;
        //        }


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }


        //    return Fee;

        //}

    }
}
