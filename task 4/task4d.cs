using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{
    class task4d
    {
       
            static void Main(string[] args)
            {
                Console.Write("Enter Tracking Number: ");
                string trackingNumber = Console.ReadLine();

                try
                {
                    Order order = OrderService.GetOrderDetails(trackingNumber);

                    if (order != null)
                    {
                        string email = EmailGenerator.GenerateEmail(order);
                        Console.WriteLine("\n--- Order Confirmation Email ---\n");
                        Console.WriteLine(email);
                    }
                    else
                    {
                        Console.WriteLine("Order not found. Please check the tracking number.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        public class Order
        {
            public int CourierID { get; set; }
            public string CustomerName { get; set; }
            public string ReceiverAddress { get; set; }
            public DateTime DeliveryDate { get; set; }
            public string TrackingNumber { get; set; }
        }

        public static class OrderService
        {
        public static Order GetOrderDetails(string trackingNumber)
        {
            Order order = null;

            using (SqlConnection conn = new SqlConnection(Databasehelper.GetConnection().ConnectionString))
            {
                conn.Open();
                string query = @"
            SELECT c.CourierID, u.Name, c.ReceiverAddress, c.DeliveryDate, c.TrackingNumber
            FROM Courier c
            JOIN Users u ON c.SenderID = u.UserID
            WHERE c.TrackingNumber = @TrackingNumber";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TrackingNumber", trackingNumber);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            order = new Order
                            {
                                CourierID = reader.GetInt32(0),
                                CustomerName = reader.GetString(1),
                                ReceiverAddress = reader.GetString(2),
                                DeliveryDate = reader.GetDateTime(3),
                                TrackingNumber = reader.GetString(4)
                            };
                        }
                    }
                }
            }

            return order;
        }

    }

    public static class EmailGenerator
        {
            public static string GenerateEmail(Order order)
            {
                return $@"
Subject: Order Confirmation - Tracking No: {order.TrackingNumber}

Dear {order.CustomerName},

Thank you for choosing our courier service. Here are your delivery details:

Order Number      : {order.CourierID}
Delivery Address  : {order.ReceiverAddress}
Expected Delivery : {order.DeliveryDate:dddd, dd MMMM yyyy}

We appreciate your business.

Warm regards,  
Courier Management Team
";
            }
        }
    }



