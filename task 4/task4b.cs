using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{

    class task4b
    {

        static void Main()
        {
            Console.WriteLine("Fetching and Validating User Data from Database...\n");

            using (SqlConnection connection = Databasehelper.GetConnection())
            {
                connection.Open();

                string query = "SELECT Name, Address, ContactNumber FROM Users";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        string address = reader["Address"].ToString();
                        string phone = reader["ContactNumber"].ToString();

                        string formattedPhone = FormatPhoneNumber(phone);

                        Console.WriteLine($"Name: {name}");
                        Console.WriteLine($"Address: {address}");
                        Console.WriteLine($"Phone: {formattedPhone}");

                        Console.WriteLine($"Name Valid: {ValidateCustomerData(name, "name")}");
                        Console.WriteLine($"Address Valid: {ValidateCustomerData(address, "address")}");
                        Console.WriteLine($"Phone Valid: {ValidateCustomerData(phone, "phone")}");
                        Console.WriteLine("---------------------------------------");
                    }
                }

                connection.Close();
            }

            Console.WriteLine("\nValidation Completed. Press any key to exit....");
            Console.ReadKey();
        }

        static string FormatPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length == 10 && Regex.IsMatch(phoneNumber, @"^\d{10}$"))
            {
                return $"{phoneNumber.Substring(0, 3)}-{phoneNumber.Substring(3, 3)}-{phoneNumber.Substring(6, 4)}";
            }
            return phoneNumber;
        }

        static bool ValidateCustomerData(string data, string detail)
        {
            switch (detail.ToLower())
            {
                case "name":
                    return Regex.IsMatch(data, @"^[A-Z][a-z]+( [A-Z][a-z]+)*$");
                case "address":
                    return Regex.IsMatch(data, @"^[\w\s,.]+$");
                case "phone":
                    return Regex.IsMatch(data, @"^\d{10}$");
                default:
                    return false;
                    Console.ReadKey();
            }
        }
    }
}





   