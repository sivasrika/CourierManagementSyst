using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace CourierManagementSyst
{
    class task1b
    {
        static void Main()
        {
            Console.Write("Enter Tracking Number: ");
            string trackingNumber = Console.ReadLine();

            using (SqlConnection conn = Databasehelper.GetConnection())
            {
                string query = "SELECT Status FROM Courier WHERE TrackingNumber = @TrackingNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrackingNumber", trackingNumber);

                conn.Open();
                var statusObj = cmd.ExecuteScalar();
                conn.Close();

                if (statusObj != null)
                {
                    string status = statusObj.ToString();

                    if (status == "Delivered")
                        Console.WriteLine("Your parcel has been delivered ✅");
                    else if (status == "Processing")
                        Console.WriteLine("Your parcel is still being processed 🕒");
                    else if (status == "Cancelled")
                        Console.WriteLine("Your parcel was cancelled ❌");
                    else
                        Console.WriteLine($"Status: {status}");
                }
                else
                {
                    Console.WriteLine("Tracking number not found.");
                }
            }
        }
    }
}
    

