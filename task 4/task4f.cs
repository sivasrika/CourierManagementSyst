using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{
    class task4f
    {
        static Random rand = new Random();

        static string GenerateSecurePassword(int length = 10)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()-_=+";

            string allChars = upper + lower + digits + special;

            StringBuilder password = new StringBuilder();
            password.Append(upper[rand.Next(upper.Length)]);
            password.Append(lower[rand.Next(lower.Length)]);
            password.Append(digits[rand.Next(digits.Length)]);
            password.Append(special[rand.Next(special.Length)]);

            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[rand.Next(allChars.Length)]);
            }

            // Shuffle the password
            char[] array = password.ToString().ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }

        static void UpdatePasswords()
        {
            using (SqlConnection conn = Databasehelper.GetConnection())
            {
                conn.Open();

                for (int userId = 100; userId <= 109; userId++)
                {
                    string newPassword = GenerateSecurePassword();

                    string query = "UPDATE Users SET Password = @Password WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", newPassword);
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Password updated for UserID {userId}: {newPassword}");
                        }
                        else
                        {
                            Console.WriteLine($"UserID {userId} not found.");
                        }
                    }
                }

                conn.Close();
            }
        }

        static void Main()
        {
            Console.WriteLine("Updating passwords for UserID 100–109...\n");
            UpdatePasswords();
            Console.WriteLine("\n All passwords updated successfully!");
        }
    }
}

    

