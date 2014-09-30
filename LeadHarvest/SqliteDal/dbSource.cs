using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using System.Data.SQLite;

namespace LeadHarvest.SqliteDal
{
    class dbSource
    {
        public List<LeadHarvest.Entities.Source> FetchSources(SQLiteConnection dbConnection)
        {
            string query = "SELECT * FROM source;";
            SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
            SQLiteDataReader reader = cmd.ExecuteReader();
            
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
