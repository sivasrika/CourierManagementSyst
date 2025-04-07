using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{
   
   
        public class task4e
        {
        public static void Main(string[] args)
        {
            CalculateShippingCost();
        }

        public static void CalculateShippingCost()
            {
                Console.WriteLine("Enter Source Address:");
                string source = Console.ReadLine();

                Console.WriteLine("Enter Destination Address:");
                string destination = Console.ReadLine();

                Console.WriteLine("Enter Parcel Weight (in kg):");
                if (!double.TryParse(Console.ReadLine(), out double weight))
                {
                    Console.WriteLine("Invalid weight input.");
                    return;
                }

                // Estimate distance between the two locations
                double distance = EstimateDistance(source, destination);
                double costPerKmPerKg = 0.05; // Example rate
                double totalCost = distance * weight * costPerKmPerKg;

                Console.WriteLine($"\nEstimated Distance: {distance} km");
                Console.WriteLine($"Calculated Shipping Cost: ${Math.Round(totalCost, 2)}");

                // Optional: if you ever want to log to database, the connection is here
                using (SqlConnection conn = Databasehelper.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Database connection failed: " + ex.Message);
                    }
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }

            private static double EstimateDistance(string from, string to)
            {
                from = from.ToLower();
                to = to.ToLower();

                if (from.Contains("new york") && to.Contains("washington"))
                    return 360;
                else if (from.Contains("london") && to.Contains("london"))
                    return 10;
                else if (from.Contains("new york") && to.Contains("london"))
                    return 5570;
                else if (from.Contains("sydney") && to.Contains("paris"))
                    return 17000;
                else if (from.Contains("dubai") && to.Contains("singapore"))
                    return 5800;
                else if (from.Contains("washington") && to.Contains("springfield"))
                    return 1340;
                else
                    return 1000; // Default fallback
            }
        }
    }



