using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using MySql.Data.MySqlClient;

namespace LeadHarvest.Database
{
    class dbSource
    {
        public List<LeadHarvest.Entities.Source> FetchSources(MySqlConnection dbConnection)
        {
            string query = "SELECT * FROM source;";
            MySqlCommand cmd=new MySqlCommand(query, dbConnection);
            MySqlDataReader reader = cmd.ExecuteReader();
            
            List<Source> Sources = new List<Source>();

            while (reader.Read())
            {
                var source = new Source();

                source.ID = Convert.ToInt32(reader["ID"]);
                source.Name = reader["Name"].ToString();
                
                Sources.Add(source);
            }
            //close Data Reader
            reader.Close();

            return Sources;
        }
    }
}
