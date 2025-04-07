using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst.task_4
{
    class task4g
    {
      

            public static List<string> FetchAddressesFromDatabase()
            {
                List<string> addresses = new List<string>();

                try
                {
                    using (SqlConnection connection = CourierManagementSyst.Databasehelper.GetConnection())
                    {
                        string query = "SELECT Address FROM Users";

                        SqlCommand command = new SqlCommand(query, connection);
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string address = reader.GetString(0);
                            addresses.Add(address);
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching addresses: " + ex.Message);
                }

                return addresses;
            }

            public static int GetSimilarityScore(string a, string b)
            {
                a = a.ToLower();
                b = b.ToLower();

                int matchCount = 0;
                string[] aWords = a.Split(' ');
                string[] bWords = b.Split(' ');

                foreach (string wordA in aWords)
                {
                    foreach (string wordB in bWords)
                    {
                        if (wordA == wordB)
                        {
                            matchCount++;
                        }
                    }
                }
                return matchCount;
            }

            public static void FindSimilarAddresses(List<string> addresses)
            {
                int n = addresses.Count;
                string[,] similarPairs = new string[n, 2];
                Dictionary<string, bool> seen = new Dictionary<string, bool>();

                Console.WriteLine("\nSimilar Addresses Found:\n");

                bool found = false;

                for (int i = 0; i < n; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        int score = GetSimilarityScore(addresses[i], addresses[j]);

                        if (score >= 4)
                        {
                            string key = addresses[i] + "|" + addresses[j];
                            if (!seen.ContainsKey(key))
                            {
                                similarPairs[i, 0] = addresses[i];
                                similarPairs[i, 1] = addresses[j];
                                Console.WriteLine($"- {addresses[i]}  ↔  {addresses[j]}");
                                seen[key] = true;
                                found = true;
                            }
                        }
                    }
                }

                if (!found)
                {
                    Console.WriteLine("No similar address pairs found.");
                }
            }

            static void Main()
            {
                Console.WriteLine("Fetching addresses from database...");

                List<string> addresses = FetchAddressesFromDatabase();

                if (addresses.Count == 0)
                {
                    Console.WriteLine("No addresses found in the Users table.");
                    return;
                }

                Console.WriteLine("\nFetched Addresses:");
                foreach (var addr in addresses)
                {
                    Console.WriteLine("- " + addr);
                }

                FindSimilarAddresses(addresses);
            }
        }
    }



