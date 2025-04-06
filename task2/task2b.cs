using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CourierManagementSyst.task2
{
    class task2b
    {

        static void Main(string[] args)
        {



            Console.Write("Enter Tracking Number: ");
            string trackingNumber = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(trackingNumber))
            {
                Console.WriteLine("Tracking number cannot be empty.");
                return;
            }

            string status = "";

            while (status != "Delivered")
            {
                status = GetCourierStatus(trackingNumber);

                if (string.IsNullOrEmpty(status))
                {
                    Console.WriteLine(" Tracking number not found. Please check and try again.");
                    break;
                }

                Console.WriteLine($" Current Status: {status}");

                if (status == "Delivered")
                {
                    Console.WriteLine(" Courier has reached the destination.");
                    break;
                }

                Console.WriteLine(" Tracking... waiting for next update.\n");
                Thread.Sleep(3000); // 3 seconds delay
            }

            Console.WriteLine("\n Tracking complete. Press any key to exit.");
            Console.ReadKey();
        }

        static string GetCourierStatus(string trackingNumber)
        {
            string status = "";

            using (SqlConnection conn = Databasehelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Status FROM Courier WHERE TrackingNumber = @TrackingNumber";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TrackingNumber", trackingNumber);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                status = reader["Status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" Error fetching status: " + ex.Message);
                }
            }

            return status;
        }
    }  
}
 



