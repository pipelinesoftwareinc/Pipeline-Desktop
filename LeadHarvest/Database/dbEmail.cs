using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using MySql.Data.MySqlClient;

namespace LeadHarvest.Database
{
    class dbEmail
    {
        public int CreateEmail(MySqlConnection dbConnection, Email email)
        {
            try
            {
                string query = String.Format("INSERT IGNORE INTO email(Address)VALUES('{0}');SELECT ID FROM email WHERE Address='{0}';", email.Address);
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }
        }
        public int CreateEmail_Opportunity(MySqlConnection dbConnection, Email email, Opportunity opp)
        {
            try
            {
            string query = String.Format("INSERT IGNORE INTO email_opportunity(EmailID,OpportunityID)VALUES({0},{1});", email.ID, opp.ID);
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }

        }
        public int CreateEmail_Organization(MySqlConnection dbConnection, Email email, Organization org)
        {
            try
            {
                string query = String.Format("INSERT IGNORE INTO email_organization(EmailID,OrganizationID)VALUES({0},{1});", email.ID, org.ID);
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }
        }    
    }
}
