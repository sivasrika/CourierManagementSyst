using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst
{
    class task1c
    


      
        {
            static void Main()
            {
                Console.WriteLine(" Courier Management Login");
                Console.Write("Enter Email: ");
                string email = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                bool isLoggedIn = false;

                using (SqlConnection conn = Databasehelper.GetConnection())
                {
                    conn.Open();

                    // 1. Check Users table (Customers)
                    string userQuery = "SELECT Name FROM Users WHERE Email = @Email AND Password = @Password";
                    SqlCommand userCmd = new SqlCommand(userQuery, conn);
                    userCmd.Parameters.AddWithValue("@Email", email);
                    userCmd.Parameters.AddWithValue("@Password", password);

                    var userResult = userCmd.ExecuteScalar();

                    if (userResult != null)
                    {
                        Console.WriteLine($"\n Welcome Customer: {userResult}!");
                        isLoggedIn = true;
                    }
                    else
                    {
                        // 2. Check Employee table
                        string empQuery = "SELECT Name FROM Employee WHERE Email = @Email";
                        SqlCommand empCmd = new SqlCommand(empQuery, conn);
                        empCmd.Parameters.AddWithValue("@Email", email);

                        var empResult = empCmd.ExecuteScalar();

                        if (empResult != null)
                        {
                            Console.WriteLine($"\n Welcome Employee: {empResult}!");
                            isLoggedIn = true;
                        }
                    }

                    conn.Close();
                }

                if (!isLoggedIn)
                {
                    Console.WriteLine("\nInvalid email or password. Please try again.");
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }


