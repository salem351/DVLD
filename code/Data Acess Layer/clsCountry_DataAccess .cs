using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//مهم جدا جدا: if you want to add new class library until ContactsDataAccessLayer ,, the NAMESPACE should be same name
//but the Connection name is different name and I have to write it above
namespace DVLD_MyDataAccess
{
    public class clsCountryDataAccess
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "use DVLD Select * from Countries where CountryID = @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", ID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                CountryName = (string)reader["CountryName"];

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

            }

            return isFound;
        }
        public static bool GetCountryInfoByName(string CountryName, ref int ID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "use DVLD SELECT * FROM Countries WHERE CountryName = @CountryName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", CountryName);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                ID = (int)reader["CountryID"];

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

            }

            return isFound;
        }




        //public static int AddNewContactAndGetID(string CountryName, string Code, string PhoneCode)
        //{

        //    int CountryID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"INSERT INTO Countries (CountryName,Code,PhoneCode) 
        //                     VALUES (@CountryName,@Code,@PhoneCode);
        //                     SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@CountryName", CountryName);


        //    if (Code != "")
        //        command.Parameters.AddWithValue("@Code", Code);
        //    else
        //        command.Parameters.AddWithValue("@Code", System.DBNull.Value);

        //    if (PhoneCode != "")
        //        command.Parameters.AddWithValue("@PhoneCode", PhoneCode);
        //    else
        //        command.Parameters.AddWithValue("@PhoneCode", System.DBNull.Value);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        //        {
        //            CountryID = insertedID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //        connection.Close();
        //    }



        //    return CountryID;
        //}


        //public static bool UpdateCountry(int ID, string CountryName, string Code, string PhoneCode)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"Update Countries 
        //                   set CountryName = @CountryName, Code = @Code, PhoneCode = @PhoneCode
        //                   where CountryID = @CountryID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@CountryID", ID);
        //    command.Parameters.AddWithValue("@CountryName", CountryName);

        //    if (Code != "")
        //        command.Parameters.AddWithValue("@Code", Code);
        //    else
        //        command.Parameters.AddWithValue("@Code", System.DBNull.Value);

        //    if (PhoneCode != "")
        //        command.Parameters.AddWithValue("@PhoneCode", PhoneCode);
        //    else
        //        command.Parameters.AddWithValue("@PhoneCode", System.DBNull.Value);


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


        //public static bool DeleteCountry(int ID)
        //{
        //    //int RowsAffected = 0;
        //    bool isDelete = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"Delete Countries 
        //                where CountryID = @CountryID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@CountryID", ID);


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


        public static DataTable GetAllCountry()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Countries order by CountryName";

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

            }

            return dt;
        }

    }
}
