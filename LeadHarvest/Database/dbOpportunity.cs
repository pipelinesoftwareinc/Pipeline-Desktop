using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LeadHarvest.Entities;

namespace LeadHarvest.Database
{
    class dbOpportunity
    {
        public int Create(MySqlConnection dbConnection, Opportunity oppertunity)
        {
            try 
            { 
                string query = String.Format("INSERT IGNORE INTO opportunity" +
                    "(ID, OrganizationID, SourceID, SearchID, Title, Snippet, DatePosted, City, State, ResponseUri, SourceKey, Created, Modified)" +
                    "VALUES({0},{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}');" +
                    "SELECT ID FROM opportunity WHERE SourceKey='{9}';",
                    oppertunity.ID,
                    oppertunity.OrganizationID,
                    oppertunity.SourceID,
                    oppertunity.SearchID,
                    MySqlHelper.EscapeString(oppertunity.Title),
                    MySqlHelper.EscapeString(oppertunity.Snippet), 
                    oppertunity.DatePosted.ToString("yyyy-MM-dd HH:mm:ss"),
                    MySqlHelper.EscapeString(oppertunity.City), 
                    oppertunity.State, 
                    oppertunity.ResponseUri,
                    oppertunity.SourceKey,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { 
                return 0; 
            }
        }

        public List<string> FetchSourceKey(MySqlConnection dbConnection, int SourceID)
        {
            List<string> results = new List<string>();
            try
            {
                string query = String.Format("SELECT SourceKey FROM opportunity WHERE SourceID={0};", SourceID);
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                using (MySqlDataReader reader = cmd.ExecuteReader())
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
