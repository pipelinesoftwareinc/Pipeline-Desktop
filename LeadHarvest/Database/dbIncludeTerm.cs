using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using MySql.Data.MySqlClient;

namespace LeadHarvest.Database
{
    class dbIncludeTerm
    {
        public List<InlcudeTerm> FetchLeads(MySqlConnection dbConnection)
        {
            string query = "SELECT * FROM Include WHERE UserID =1;";
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            MySqlDataReader reader = cmd.ExecuteReader();

            List<InlcudeTerm> Leads = new List<InlcudeTerm>();

            while (reader.Read())
            {
                var lead = new InlcudeTerm();

                lead.ID = Convert.ToInt32(reader["TermID"]);
                lead.UserID = Convert.ToInt32(reader["UserID"]);
                lead.Term = reader["TermValue"].ToString();

                Leads.Add(lead);
            }
            //close Data Reader
            reader.Close();

            return Leads;
        }
    }
}
