using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst
{
    class task1d
   
  
        {
        static void AssignCourierToShipment(int courierId)
        {
            // Use the connection from your DatabaseHelper class.
            using (SqlConnection conn = Databasehelper.GetConnection())
            {
                try
                {
                    conn.Open();

                    // Retrieve all employee IDs.
                    string employeeQuery = "SELECT EmployeeID FROM Employee";
                    SqlCommand cmdEmployees = new SqlCommand(employeeQuery, conn);
                    SqlDataReader empReader = cmdEmployees.ExecuteReader();
                    List<int> employeeIds = new List<int>();

                    while (empReader.Read())
                    {
                        employeeIds.Add(Convert.ToInt32(empReader["EmployeeID"]));
                    }
                    empReader.Close();

                    if (employeeIds.Count == 0)
                    {
                        Console.WriteLine("No employees available.");
                        return;
                    }

                    // Determine the employee with the least number of assigned shipments.
                    int selectedEmployeeId = employeeIds[0];
                    int minLoad = int.MaxValue;

                    foreach (int empId in employeeIds)
                    {
                        string countQuery = "SELECT COUNT(*) FROM Courier WHERE EmployeeID = @EmpID";
                        SqlCommand countCmd = new SqlCommand(countQuery, conn);
                        countCmd.Parameters.AddWithValue("@EmpID", empId);

                        int load = (int)countCmd.ExecuteScalar();
                        if (load < minLoad)
                        {
                            minLoad = load;
                            selectedEmployeeId = empId;
                        }
                    }

                    // Update the Courier record to assign the selected employee.
                    string updateQuery = "UPDATE Courier SET EmployeeID = @EmpID WHERE CourierID = @CourierID";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@EmpID", selectedEmployeeId);
                    updateCmd.Parameters.AddWithValue("@CourierID", courierId);

                    int rowsAffected = updateCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        Console.WriteLine($"Courier {courierId} assigned to employee {selectedEmployeeId} (current load: {minLoad} shipments).");
                    else
                        Console.WriteLine("Assignment failed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        static void Main(string[] args)
        {
            Console.Write("Enter CourierID to assign: ");
            int courierId;
            if (int.TryParse(Console.ReadLine(), out courierId))
            {
                AssignCourierToShipment(courierId);
            }
            else
            {
                Console.WriteLine("Invalid Courier ID.");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}