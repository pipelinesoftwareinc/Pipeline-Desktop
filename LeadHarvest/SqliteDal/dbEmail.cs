using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace LeadHarvest.SqliteDal
{
    class dbEmail
    {
        public int CreateEmail(SQLiteConnection dbConnection, Email email)
        {
            try
            {
                string query = String.Format("INSERT OR IGNORE INTO email(Address)VALUES('{0}');SELECT ID FROM email WHERE Address='{0}';", email.Address);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }
        }
        public int CreateEmail_Opportunity(SQLiteConnection dbConnection, Email email, Opportunity opp)
        {
            try
            {
            string query = String.Format("INSERT OR IGNORE INTO email_opportunity(EmailID,OpportunityID)VALUES({0},{1});", email.ID, opp.ID);
            SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }

        }
        public int CreateEmail_Organization(SQLiteConnection dbConnection, Email email, Organization org)
        {
            try
            {
                string query = String.Format("INSERT OR IGNORE INTO email_organization(EmailID,OrganizationID)VALUES({0},{1});", email.ID, org.ID);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }
        }    
    }
}
