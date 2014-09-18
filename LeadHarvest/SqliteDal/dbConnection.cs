using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace LeadHarvest.SqliteDal
{
    class dbConnection
    {
       // static MySqlConnection _connection=new MySqlConnection();
        static SQLiteConnection _connection=new SQLiteConnection();

        public SQLiteConnection Open()
        {
            return Open("pipeLine.db");
        }
        public SQLiteConnection Open(string Database)
        {
            try
            {
                _connection.ConnectionString=_connection.ConnectionString=String.Format("Data Source={0};version=3;Compress=True;", Database);

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
