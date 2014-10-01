using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using System.Data.SQLite;

namespace LeadHarvest.SqliteDal
{
    class dbPhone
    {
        public int CreatePhone(SQLiteConnection dbConnection, Phone phone)
        {
            string query = String.Format("INSERT OR IGNORE INTO phone(PhoneNUmber)VALUES('{0}');SELECT PhoneID FROM phone WHERE PhoneAddress='{0}';", phone.PhoneNumber);
            SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int CreatePhone_Opportunity(SQLiteConnection dbConnection, Phone phone, Opportunity opp)
        {
            string query = String.Format("INSERT OR IGNORE INTO phone_opportunity(PhoneID,OpportunityID)VALUES({0},{1});", phone.ID, opp.ID);
            SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int CreatePhone_Organization(SQLiteConnection dbConnection, Phone phone, Organization org)
        {
            string query = String.Format("INSERT OR IGNORE INTO phone_organization(PhoneID,OrganizationID)VALUES({0},{1});", phone.ID, org.ID);
            SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }    
    }
}
