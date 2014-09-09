using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LeadHarvest.Entities;

namespace LeadHarvest.Database
{
    class dbOrganization
    {
        private helper _helper = new helper();
        public List<Organization> FetchOrganziations(MySqlConnection dbConnection)
        {
            List<Organization> Orgs = new List<Organization>();

            try
            {
                string query = "SELECT * FROM organization;";
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                MySqlDataReader reader = cmd.ExecuteReader();



                while (reader.Read())
                {
                    var org = new Organization();

                    org.ID = Convert.ToInt32(reader["OrganizationID"]);
                    org.Name = reader["OrganizationName"].ToString();

                    Orgs.Add(org);
                }
                //close Data Reader
                reader.Close();

                return Orgs;
            }
            catch(Exception ex)
            { return Orgs; }
        }
        public Organization FetchOrganziation(MySqlConnection dbConnection, String Name)
        {
            var org = new Organization();
            try
            {
                string query = String.Format("SELECT * FROM organization WHERE Name='{0}';", Name);
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Depth == 0)
                { return null; }

                reader.Read();
              
                org.ID = Convert.ToInt32(reader["OrganizationID"]);
                org.Name = reader["Name"].ToString();
                org.EmailDomain = reader["EmailDomain"].ToString();
                org.Description = reader["Description"].ToString();
                reader.Close();

                return org;
            }
            catch(Exception ex)
            { return org; }
        }
        public int Create(MySqlConnection dbConnection, string name)
        {
            try
            {
                string query = String.Format(@"INSERT IGNORE INTO organization(Name)VALUES('{0}');SELECT ID FROM organization WHERE Name='{0}';", name);
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }
        }

        public void Update(MySqlConnection dbConnection, Organization org)
        {
            try            {
            string query = String.Format(@"UPDATE organization SET 
                Name='{0}',EmailDomain='{1}',Description='{2}',LinkedIn='{3}',Facebook='{4}',Twitter='{5}',GooglePlus='{6}'
                WHERE ID={7};",
                org.Name, org.EmailDomain, MySqlHelper.EscapeString(org.Description), org.LinkedIn, org.Facebook, org.Twitter, org.GooglePlus, org.ID);
            MySqlCommand cmd = new MySqlCommand(query, dbConnection);
            cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
        }        
        
        public void UpdateEmailDomain(MySqlConnection dbConnection, Organization org)
        {
            try
            {
                string query = String.Format("UPDATE organization SET EmailDomain = '{0}' WHERE ID = {1};",
                    org.EmailDomain, org.ID);
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
        }

        public void UpdateSocial(MySqlConnection dbConnection, Organization org)
        {
            try
            {
                string query = String.Format(@"UPDATE organization SET 
                LinkedIn='{0}',Facebook='{1}',Twitter='{2}',GooglePlus='{3}' 
                WHERE ID={4};",
                    org.LinkedIn, org.Facebook, org.Twitter, org.GooglePlus, org.ID);
                MySqlCommand cmd = new MySqlCommand(query, dbConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
        }      
    }
}
