using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task1
{
    class task1a
    {
        static void Main()
        {
            Console.Write("Enter Tracking Number: ");
            string trackingNumber = Console.ReadLine();

            using (SqlConnection conn = Databasehelper.GetConnection())
            {
                string query = "SELECT Weight FROM Courier WHERE TrackingNumber = @TrackingNumber";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@TrackingNumber", trackingNumber);

                conn.Open();
                var weightObj = cmd.ExecuteScalar();
                conn.Close();

                if (weightObj != null)
                {
                    double weight = Convert.ToDouble(weightObj);
                    string category;

                    if (weight <= 1.0)
                        category = "Light";
                    else if (weight > 1.0 && weight <= 3.0)
                        category = "Medium";
                    else
                        category = "Heavy";

                    Console.WriteLine($"The parcel with tracking number {trackingNumber} is categorized as: {category}");
                }
                else
                {
                    Console.WriteLine("Tracking number not found.");
                }
            }
        }
    }
}
    

