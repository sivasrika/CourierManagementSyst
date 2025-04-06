using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task2
{
    class task2a
    {



        static void Main(string[] args)
        {
            Console.Write("Enter Customer ID to view orders: ");
            string customerId = Console.ReadLine();

            List<string> orders = GetOrdersForCustomer(customerId);

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders found for this customer.");
            }
            else
            {
                Console.WriteLine($"\nOrders for Customer ID: {customerId}");
                for (int i = 0; i < orders.Count; i++)
                {
                    Console.WriteLine($"Order {i + 1}: {orders[i]}");
                }
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        public static List<string> GetOrdersForCustomer(string customerId)
        {
            List<string> orderList = new List<string>();

            using (SqlConnection conn = Databasehelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CourierID, TrackingNumber, Status, DeliveryDate FROM Courier WHERE SenderID = @CustomerID OR ReceiverID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string orderInfo = $"CourierID: {reader["CourierID"]}, Tracking#: {reader["TrackingNumber"]}, Status: {reader["Status"]}, Delivery Date: {Convert.ToDateTime(reader["DeliveryDate"]).ToShortDateString()}";
                        orderList.Add(orderInfo);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error retrieving orders: " + ex.Message);
                }
            }

            return orderList;
        }
    }
}
