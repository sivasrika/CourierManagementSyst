using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{
    class task4c
    {
       
          // Capitalize first letter of each word
            public static string CapitalizeEachWord(string input)
            {
                TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                return textInfo.ToTitleCase(input.ToLower());
            }

            // Format Zip Code (5-digit or 9-digit with hyphen)
            public static string FormatZipCode(string zip)
            {
                zip = zip.Trim();
                if (zip.Length == 9)
                    return zip.Insert(5, "-");
                else if (zip.Length == 5)
                    return zip;
                else
                    return "Invalid Zip Code";
            }

            // Combine and format address
            public static string FormatAddress(string street, string city, string state, string zip, out string formattedZip)
            {
                formattedZip = FormatZipCode(zip);
                if (formattedZip == "Invalid Zip Code")
                    return "Invalid Zip Code";

                string formattedStreet = CapitalizeEachWord(street.Trim());
                string formattedCity = CapitalizeEachWord(city.Trim());
                string formattedState = CapitalizeEachWord(state.Trim());

                return $"{formattedStreet}, {formattedCity}, {formattedState}";
            }

            static void Main(string[] args)
            {
                Console.WriteLine("Enter the UserID to update the address:");
                int userId;
                while (!int.TryParse(Console.ReadLine(), out userId))
                {
                    Console.Write("Invalid input. Please enter a numeric UserID: ");
                }

                Console.WriteLine("\nEnter Address Details Below:\n");

                Console.Write("Street: ");
                string street = Console.ReadLine();

                Console.Write("City: ");
                string city = Console.ReadLine();

                Console.Write("State: ");
                string state = Console.ReadLine();

                Console.Write("Zip Code (5 or 9 digits): ");
                string zip = Console.ReadLine();

                string formattedZip;
                string formattedAddress = FormatAddress(street, city, state, zip, out formattedZip);

                if (formattedAddress == "Invalid Zip Code")
                {
                    Console.WriteLine("\n Error: Invalid Zip Code entered. Please use 5 or 9 digit zip codes.");
                    return;
                }

                // Display and update in DB
                Console.WriteLine($"\n Formatted Address: {formattedAddress}");
                Console.WriteLine($"Formatted ZipCode: {formattedZip}");

                using (SqlConnection conn = Databasehelper.GetConnection())
                {
                    conn.Open();
                    string updateQuery = "UPDATE Users SET Address = @Address, ZipCode = @ZipCode WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Address", formattedAddress);
                        cmd.Parameters.AddWithValue("@ZipCode", formattedZip);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            Console.WriteLine("\n Address and ZipCode updated successfully in the database.");
                        else
                            Console.WriteLine("\n Failed to update. Please check if the UserID exists.");
                    }
                }

                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }
    }











                    