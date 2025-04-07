using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{
    class task4a
    {
       

            // Simulated tracking storage
            static string[,] trackingData;
            static Dictionary<string, string> trackingMap = new Dictionary<string, string>();

            static void Main()
            {
                Console.WriteLine("========== Parcel Tracking System ==========");

                // Step 1: Load data using Databasehelper
                LoadTrackingData();

                // Step 2: User input
                Console.Write("Enter the Tracking Number: ");
                string userInput = Console.ReadLine().Trim().ToUpper();

                // Step 3: Simulate tracking
                TrackParcel(userInput);
            }

            static void LoadTrackingData()
            {
                List<string[]> tempList = new List<string[]>();

                using (SqlConnection conn = Databasehelper.GetConnection())
                {
                    string query = "SELECT TrackingNumber, Status FROM Courier";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string trackingNumber = reader["TrackingNumber"].ToString();
                        string status = reader["Status"].ToString();

                        tempList.Add(new string[] { trackingNumber, status });
                        trackingMap[trackingNumber] = status;
                    }
                    reader.Close();
                }

                trackingData = new string[tempList.Count, 2];
                for (int i = 0; i < tempList.Count; i++)
                {
                    trackingData[i, 0] = tempList[i][0];
                    trackingData[i, 1] = tempList[i][1];
                }
            }

            static void TrackParcel(string trackingNumber)
            {
                if (trackingMap.ContainsKey(trackingNumber))
                {
                    string status = trackingMap[trackingNumber];
                    Console.WriteLine($"Tracking Number: {trackingNumber}");
                    Console.WriteLine($"Current Status: {status}");

                    switch (status.ToLower())
                    {
                        case "in transit":
                            Console.WriteLine(" Your parcel is currently **In Transit**.");
                            break;
                        case "out for delivery":
                            Console.WriteLine(" Your parcel is **Out for Delivery**.");
                            break;
                        case "delivered":
                            Console.WriteLine(" Your parcel has been **Delivered**.");
                            break;
                        case "pending":
                            Console.WriteLine(" Parcel is still **Pending**.");
                            break;
                        case "processing":
                            Console.WriteLine(" Parcel is in **Processing**.");
                            break;
                        case "shipped":
                            Console.WriteLine(" Parcel has been **Shipped**.");
                            break;
                        default:
                            Console.WriteLine(" Status: " + status);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Tracking Number not found. Please check again.");
                }
            }
        }
    }


