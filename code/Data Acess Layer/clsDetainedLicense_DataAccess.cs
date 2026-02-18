 using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace DVLD_MyDataAccess
{
    public class clsDetainedLicenseData
    {

        public static bool GetDetainedLicenseInfoByID(int DetainID,
         ref int LicenseID, ref DateTime DetainDate,
         ref float FineFees, ref int CreatedByUserID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID", DetainID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                LicenseID = (int)reader["LicenseID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];

                                IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)

                                    ReleaseDate = DateTime.MaxValue;
                                else
                                    ReleaseDate = (DateTime)reader["ReleaseDate"];


                                if (reader["ReleasedByUserID"] == DBNull.Value)

                                    ReleasedByUserID = -1;
                                else
                                    ReleasedByUserID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)

                                    ReleaseApplicationID = -1;
                                else
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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


        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
         ref int DetainID, ref DateTime DetainDate,
         ref float FineFees, ref int CreatedByUserID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT top 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID order by DetainID desc";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                DetainID = (int)reader["DetainID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];

                                IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)

                                    ReleaseDate = DateTime.MaxValue;
                                else
                                    ReleaseDate = (DateTime)reader["ReleaseDate"];


                                if (reader["ReleasedByUserID"] == DBNull.Value)

                                    ReleasedByUserID = -1;
                                else
                                    ReleasedByUserID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)

                                    ReleaseApplicationID = -1;
                                else
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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

        public static DataTable GetAllDetainedLicenses()
        {

            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "select * from detainedLicenses_View order by IsReleased ,DetainID;";


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

        public static int AddNewDetainedLicense(
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"INSERT INTO dbo.DetainedLicenses
                               (LicenseID,
                               DetainDate,
                               FineFees,
                               CreatedByUserID,
                               IsReleased
                               )
                            VALUES
                               (@LicenseID,
                               @DetainDate, 
                               @FineFees, 
                               @CreatedByUserID,
                               0
                             );
                            
                            SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            DetainID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            return DetainID;

        }

        public static bool UpdateDetainedLicense(int DetainID,
            int LicenseID, DateTime DetainDate,
            float FineFees, int CreatedByUserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID,   
                              WHERE DetainID=@DetainID;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
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


        public static bool ReleaseDetainedLicense(int DetainID,
                 int ReleasedByUserID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleasedByUserID = @ReleasedByUserID,
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
                        command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                        command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

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

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;


            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"select IsDetained=1 
                            from detainedLicenses 
                            where 
                            LicenseID=@LicenseID 
                            and IsReleased=0;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            IsDetained = Convert.ToBoolean(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }


            return IsDetained;
        }

        ////Find for relesase
        //public static bool GetDetainByIDForRelease(int DetainID, ref int LicenseID, ref int CreatedByUserID, ref int ReleasedByUserID,
        //                                 ref int ReleaseApplicationID, ref DateTime DetainDate, ref DateTime ReleaseDate,
        //                                 ref byte IsReleased, ref decimal FineFees)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses where DetainID = @DetainID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DetainID", DetainID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            LicenseID = (int)reader["LicenseID"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            ReleasedByUserID = (int)reader["ReleasedByUserID"];
        //            ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
        //            DetainDate = (DateTime)reader["DetainDate"];
        //            ReleaseDate = (DateTime)reader["ReleaseDate"];
        //            IsReleased = Convert.ToByte(reader["IsReleased"]);
        //            FineFees = (decimal)reader["FineFees"];
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
        //public static bool GetDetainByLicenseIDForRelease(ref int DetainID, int LicenseID, ref int CreatedByUserID, ref int ReleasedByUserID,
        //                                 ref int ReleaseApplicationID, ref DateTime DetainDate, ref DateTime ReleaseDate,
        //                                 ref byte IsReleased, ref decimal FineFees)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses where LicenseID = @LicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            DetainID = (int)reader["DetainID"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            ReleasedByUserID = (int)reader["ReleasedByUserID"];
        //            ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
        //            DetainDate = (DateTime)reader["DetainDate"];
        //            ReleaseDate = (DateTime)reader["ReleaseDate"];
        //            IsReleased = Convert.ToByte(reader["IsReleased"]);
        //            FineFees = (decimal)reader["FineFees"];


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

        ////Find for Detain
        //public static bool GetDetainByIDForDetain(int DetainID, ref int LicenseID, ref DateTime DetainDate, ref decimal FineFees, ref int CreatedByUserID, ref byte IsReleased)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses where DetainID = @DetainID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DetainID", DetainID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            LicenseID = (int)reader["LicenseID"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];               
        //            DetainDate = (DateTime)reader["DetainDate"];
        //            IsReleased = Convert.ToByte(reader["IsReleased"]);
        //            FineFees = (decimal)reader["FineFees"];
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
        //public static bool GetDetainByLicenseIDForDetain(ref int DetainID, int LicenseID, ref DateTime DetainDate, ref decimal FineFees, ref int CreatedByUserID, ref byte IsReleased)
        //{
        //    bool isfound = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses where LicenseID = @LicenseID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);

        //    try
        //    {
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            isfound = true;

        //            DetainID = (int)reader["DetainID"];
        //            CreatedByUserID = (int)reader["CreatedByUserID"];
        //            DetainDate = (DateTime)reader["DetainDate"];
        //            IsReleased = Convert.ToByte(reader["IsReleased"]);
        //            FineFees = (decimal)reader["FineFees"];

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




        //public static int AddNewDetainAndGetID(int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID,  byte IsReleased)
        //{

        //    int DetainID = -1;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD INSERT INTO DetainedLicenses (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
        //                VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, @IsReleased);
        //               SELECT SCOPE_IDENTITY();";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);
        //    command.Parameters.AddWithValue("@DetainDate", DetainDate);
        //    command.Parameters.AddWithValue("@FineFees", FineFees);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@IsReleased", IsReleased);
        //    //command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
        //    //command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
        //    //command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
        //        {
        //            DetainID = insertedID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //        connection.Close();
        //    }



        //    return DetainID;
        //}

        //public static bool UpdateDetain(int DetainID, int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID,
        //                       byte IsReleased, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update DetainedLicenses 
        //                   set LicenseID = @LicenseID, DetainDate = @DetainDate, FineFees = @FineFees, CreatedByUserID = @CreatedByUserID, 
        //                       IsReleased = @IsReleased, ReleaseDate = @ReleaseDate, ReleasedByUserID = @ReleasedByUserID,
        //                       ReleaseApplicationID = @ReleaseApplicationID 
        //                   where DetainID = @DetainID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@DetainID", DetainID);
        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);
        //    command.Parameters.AddWithValue("@DetainDate", DetainDate);
        //    command.Parameters.AddWithValue("@FineFees", FineFees);
        //    command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
        //    command.Parameters.AddWithValue("@IsReleased", IsReleased);
        //    command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
        //    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
        //    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);


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


        //public static DataTable GetAllDetainsUsingView()
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses_View";
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



        //public static byte IsReleaseLicense(int LicenseID)
        //{

        //    //int RowsAffected = 0;
        //    byte IsRelease = 0;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select IsReleased from DetainedLicenses_View
        //                     where LicenseID = @LicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsRelease = Convert.ToByte(Result);
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
        //    return IsRelease;
        //}


        //public static bool IsDetainedLicense(int LicenseID)
        //{

        //    //int RowsAffected = 0;
        //    bool IsDetained = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select found = 1 from DetainedLicenses
        //                     where IsReleased = 0 and LicenseID = @LicenseID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@LicenseID", LicenseID);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsDetained = true;
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
        //    return IsDetained;
        //}

        //public static bool IsDetainedLicenseByDetainID(int DetainID)
        //{

        //    //int RowsAffected = 0;
        //    bool IsDetained = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Select found = 1 from DetainedLicenses
        //                     where IsReleased = 0 and DetainID = @DetainID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@DetainID", DetainID);



        //    try
        //    {
        //        connection.Open();
        //        object Result = command.ExecuteScalar();

        //        if (Result != null)
        //        {
        //            IsDetained = true;
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
        //    return IsDetained;
        //}



        //public static bool UpdateToRelease(int DetainID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        //{

        //    //int RowsAffected = 0;
        //    bool isUpdate = false;

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = @"use DVLD Update DetainedLicenses 
        //                   set IsReleased = 1, ReleaseDate = @ReleaseDate, ReleasedByUserID = @ReleasedByUserID,
        //                       ReleaseApplicationID = @ReleaseApplicationID 
        //                   where DetainID = @DetainID";

        //    SqlCommand command = new SqlCommand(query, connection);

        //    command.Parameters.AddWithValue("@DetainID", DetainID);
        //    command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
        //    command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
        //    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

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





        ////Filter
        //public static DataTable GetDetainedLicenseInfoByDetainID(int DetainID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses_View where DetainID = @DetainID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@DetainID", DetainID);


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
        //public static DataTable GetDetainedLicenseInfoByReleaseApplicationID(int ReleaseApplicationID)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses_View where ReleaseApplicationID = @ReleaseApplicationID";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);


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
        //public static DataTable GetDetainedLicenseInfoByNationalNo(string NationalNo)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses_View where NationalNo like @NationalNo";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@NationalNo", NationalNo +"%");


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
        //public static DataTable GetDetainedLicenseInfoByFullName(string FullName)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses_View where FullName like @FullName";
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
        //public static DataTable GetByIsReleased(byte IsReleased)
        //{
        //    DataTable dt = new DataTable();

        //    SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
        //    string query = "use DVLD Select * from DetainedLicenses_View where IsReleased = @IsReleased";
        //    SqlCommand command = new SqlCommand(query, connection);
        //    command.Parameters.AddWithValue("@IsReleased", IsReleased);



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
