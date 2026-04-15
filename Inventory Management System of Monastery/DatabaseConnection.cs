using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Inventory_Management_System_of_Monastery
{
    internal class DatabaseConnection
    {
        public static string connectionString = "server=localhost;uid=root;database=monastery_inventory;port=3306;password=mingyu0406;";

        public static string GetConnectionString()
        {
            return connectionString;
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}
