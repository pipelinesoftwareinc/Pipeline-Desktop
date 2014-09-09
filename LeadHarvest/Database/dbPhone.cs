using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Entities;
using MySql.Data.MySqlClient;

namespace LeadHarvest.Database
{
    class dbPhone
    {
        public int CreatePhone(MySqlConnection dbConnection, Phone phone)
        {
            string query = String.Format("INSERT IGNORE INTO phone(PhoneNUmber)VALUES('{0}');SELECT PhoneID FROM phone WHERE PhoneAddress='{0}';", phone.PhoneNumber);
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int CreatePhone_Opportunity(MySqlConnection dbConnection, Phone phone, Opportunity opp)
        {
            string query = String.Format("INSERT IGNORE INTO phone_opportunity(PhoneID,OpportunityID)VALUES({0},{1});", phone.ID, opp.ID);
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        public int CreatePhone_Organization(MySqlConnection dbConnection, Phone phone, Organization org)
        {
            string query = String.Format("INSERT IGNORE INTO phone_organization(PhoneID,OrganizationID)VALUES({0},{1});", phone.ID, org.ID);
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }    
    }
}
