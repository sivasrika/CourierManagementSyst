using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierManagementSyst
{
    public static class Databasehelper
    {
        private static readonly string connectionString = "Server=(localdb)\\mssqllocaldb; Database= CourierManagament ; Integrated Security = True;";
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
        
        
    }
}
