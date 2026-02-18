using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_MyDataAccess.clsCountryDataAccess;

namespace DVLD_MyDataAccess
{
    public class clsDriverData
    {

        public static bool GetDriverInfoByDriverID(int DriverID,
    ref int PersonID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];


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

        public static bool GetDriverInfoByPersonID(int PersonID, ref int DriverID,
            ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                DriverID = (int)reader["DriverID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];

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

        public static DataTable GetAllDrivers()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Drivers_View order by FullName";

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

        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"Insert Into Drivers (PersonID,CreatedByUserID,CreatedDate)
                            Values (@PersonID,@CreatedByUserID,@CreatedDate);
                          
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            DriverID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return DriverID;

        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    //we dont update the createddate for the driver.
                    string query = @"Update  Drivers  
                            set PersonID = @PersonID,
                                CreatedByUserID = @CreatedByUserID
                                where DriverID = @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
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


        //public static bool GetDriverByByPersonID(ref int DriverID, int PersonID, ref int CreatedByUserID,
        //                                         ref DateTime CreatedDate)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Drivers where PersonID = @PersonID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@PersonID", PersonID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            DriverID = (int)reader["DriverID"];              
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            CreatedDate = (DateTime)reader["CreatedDate"];

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
        //public static bool GetDriver(int DriverID, ref int PersonID, ref int CreatedByUserID,
        //                                          ref DateTime CreatedDate)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from Drivers where DriverID = @DriverID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DriverID", DriverID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            PersonID = (int)reader["PersonID"];                   
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            CreatedDate = (DateTime)reader["CreatedDate"];

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


        //public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        //{

        //    int DriverID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD INSERT INTO Drivers(PersonID, CreatedByUserID, CreatedDate) 
        //                VALUES (@PersonID,@CreatedByUserID, @CreatedDate);
        //               SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@PersonID", PersonID);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@CreatedDate", CreatedDate);




        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        //        {
        //            DriverID = insertedID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //        connection.Close();
        //    }



        //    return DriverID;
        //}

        //public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update Drivers 
        //                   set PersonID = @PersonID, CreatedByUserID = @CreatedByUserID, CreatedDate = @CreatedDate
        //                   where DriverID = @DriverID";

        //    SqlCommand command = new SqlCommand(query, connection);


        //    command.Parameters.AddWithValue("@DriverID", DriverID);
        //    command.Parameters.AddWithValue("@PersonID", PersonID);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@CreatedDate", CreatedDate);



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


        //public static DataTable GetAllDriverss()
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  Drivers.DriverID, People.PersonID, People.NationalNo, People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName AS FullName, Drivers.CreatedDate, Licenses.IsActive
        //                     FROM People INNER JOIN Drivers 
        //                     ON People.PersonID = Drivers.PersonID INNER JOIN Licenses
        //                     ON Drivers.DriverID = Licenses.DriverID";
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





        ////Filter
        //public static DataTable GetDriverByDriverID(int DriverID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  Drivers.DriverID, People.PersonID, People.NationalNo,
        //                     People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName AS FullName,
        //                     Drivers.CreatedDate, Licenses.IsActive
        //                     FROM People INNER JOIN Drivers
        //                     ON People.PersonID = Drivers.PersonID INNER JOIN Licenses
        //                     ON Drivers.DriverID = Licenses.DriverID

        //                     where Drivers.DriverID like @DriverID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DriverID", DriverID + "%");


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
        //public static DataTable GetDriverByPersonID(int PersonID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  Drivers.DriverID, People.PersonID, People.NationalNo,
        //                     People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName AS FullName,
        //                     Drivers.CreatedDate, Licenses.IsActive
        //                     FROM People INNER JOIN Drivers
        //                     ON People.PersonID = Drivers.PersonID INNER JOIN Licenses
        //                     ON Drivers.DriverID = Licenses.DriverID

        //                     where People.PersonID like @PersonID";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@PersonID", PersonID + "%");


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
        //public static DataTable GetDriverByNationalNo(string NationalNo)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  Drivers.DriverID, People.PersonID, People.NationalNo,
        //                     People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName AS FullName,
        //                     Drivers.CreatedDate, Licenses.IsActive
        //                     FROM People INNER JOIN Drivers
        //                     ON People.PersonID = Drivers.PersonID INNER JOIN Licenses
        //                     ON Drivers.DriverID = Licenses.DriverID

        //                     where People.NationalNo like @NationalNo";

        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@NationalNo", NationalNo);


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
        //public static DataTable GetDriverByFullName(string FullName)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD SELECT  Drivers.DriverID, People.PersonID, People.NationalNo,
        //                     People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName AS FullName,
        //                     Drivers.CreatedDate, Licenses.IsActive
        //                     FROM People INNER JOIN Drivers
        //                     ON People.PersonID = Drivers.PersonID INNER JOIN Licenses
        //                     ON Drivers.DriverID = Licenses.DriverID

        //                     where People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName like @FullName"; 


        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@FullName", FullName + "%");


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
