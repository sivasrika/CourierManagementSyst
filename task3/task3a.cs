using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task3
{
    
    
    class task3a
    {
    
        
            static void Main()
            {
                Console.Write("Enter Courier History ID: ");
                string historyId = Console.ReadLine();

                try
                {
                    using (SqlConnection conn = Databasehelper.GetConnection())
                    {
                        string query = "SELECT LocationUpdate, UpdatedTime FROM TrackingHistory WHERE HistoryID = @HistoryID ORDER BY UpdatedTime";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@HistoryID", historyId);

                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();

                        bool hasData = false;
                        Console.WriteLine("\nTracking Details:\n");

                        while (reader.Read())
                        {
                            hasData = true;
                            string location = reader["LocationUpdate"].ToString();
                            string time = reader["UpdatedTime"].ToString();

                            Console.WriteLine($"Location: {location} \t Time: {time}");
                        }

                        if (!hasData)
                        {
                            Console.WriteLine("No tracking data found for the given History ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while fetching tracking data:\n" + ex.Message);
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }








