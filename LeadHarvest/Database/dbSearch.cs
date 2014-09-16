using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using MySql.Data.MySqlClient;

namespace LeadHarvest.Database
{
    class dbSearch
    {
        public List<Search> FetchTerms(MySqlConnection dbConnection)
        {
            string query = "SELECT * FROM Search WHERE UserID =1;";
            MySqlCommand cmd=new MySqlCommand(query, dbConnection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<Search> Searches = new List<Search>();

            while (reader.Read())
            {
                var search = new Search();

                search.ID = Convert.ToInt32(reader["ID"]);
                search.Term = reader["Term"].ToString();

                Searches.Add(search);
            }
            //close Data Reader
            reader.Close();

            return Searches;
        }
    }
}
