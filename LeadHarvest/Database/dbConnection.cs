using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace LeadHarvest.Database
{
    class dbConnection
    {
        static MySqlConnection _connection = new MySqlConnection();

        public MySqlConnection Open()
        {
            try
            {
                // NEED TO MOVE THESE TO ENV VARS
                var server = "localhost";
                var database = "pipeline";
                var uid = "pipeline";
                var password = "P@ssw0rd1";
                
                _connection.ConnectionString = String.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3}",
                    server, database, uid, password);

                _connection.Open();
                return _connection;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public bool Close()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
