using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace LeadHarvest.SqliteDal
{
    class dbOpportunity
    {
        public int Create(SQLiteConnection dbConnection, Opportunity oppertunity)
        {
            try 
            { 
                string query = String.Format("INSERT OR IGNORE INTO opportunity" +
                    "(OrganizationID, SourceID, SearchID, Title, Snippet, DatePosted, City, State, ResponseUri, SourceKey, Created)" +
                    "VALUES({1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}');" +
                    "SELECT ID FROM opportunity WHERE SourceKey='{10}';",
                    oppertunity.ID,
                    oppertunity.OrganizationID,
                    oppertunity.SourceID,
                    oppertunity.SearchID,
                     oppertunity.Title.Replace("'","''"),
                     oppertunity.Snippet.Replace("'", "''"), 
                    oppertunity.DatePosted.ToString("yyyy-MM-dd HH:mm:ss"),
                     oppertunity.City.Replace("'", "''"), 
                    oppertunity.State, 
                    oppertunity.ResponseUri,
                    oppertunity.SourceKey,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { 
                return 0; 
            }
        }

        public List<string> FetchSourceKey(SQLiteConnection dbConnection, int SourceID)
        {
            List<string> results = new List<string>();
            try
            {
                string query = String.Format("SELECT SourceKey FROM opportunity WHERE SourceID={0};", SourceID);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(reader[0].ToString());
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                return null;
            }
        }    
    }
}
