using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using System.Data.SQLite;

namespace LeadHarvest.SqliteDal
{
    class dbSearch
    {
        public List<Search> FetchTerms(SQLiteConnection dbConnection)
        {
            string query = "SELECT * FROM Search";
            SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
            SQLiteDataReader reader = cmd.ExecuteReader();

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
