using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task3
{
    class task3b
    {
    
            static void Main()
            {
                try
                {
                    Console.Write("Enter New Order Pickup Address: ");
                    string newPickupAddress = Console.ReadLine();

                    using (SqlConnection conn = Databasehelper.GetConnection())
                    {
                        string query = @"
                        SELECT CourierID, SenderName, SenderAddress 
                        FROM Courier
                        WHERE Status IN ('In Transit', 'Shipped','Processing','Pending')
                        AND SenderAddress LIKE @pickupAddress;";

                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@pickupAddress", "%" + newPickupAddress + "%");

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            bool found = false;
                            Console.WriteLine("\nNearby Couriers Found:\n");

                            while (reader.Read())
                            {
                                found = true;
                                int courierId = reader["CourierID"] != DBNull.Value ? Convert.ToInt32(reader["CourierID"]) : 0;
                                string senderName = reader["SenderName"]?.ToString();
                                string senderAddress = reader["SenderAddress"]?.ToString();

                                Console.WriteLine($"ID: {courierId}");
                                Console.WriteLine($"Sender: {senderName}");
                                Console.WriteLine($"Address: {senderAddress}");
                                Console.WriteLine("-----------------------------");
                            }

                            if (!found)
                            {
                                Console.WriteLine("No available couriers found for that location.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }





