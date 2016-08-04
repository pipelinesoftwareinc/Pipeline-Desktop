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
    class dbOrganization
    {
        private helper _helper = new helper();
        public List<Organization> FetchOrganziations(SQLiteConnection dbConnection)
        {
            List<Organization> Orgs = new List<Organization>();

            try
            {
                string query = "SELECT * FROM organization;";
                SQLiteCommand cmd = new SQLiteCommand(query, dbConnection);
                SQLiteDataReader reader = cmd.ExecuteReader();

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
        public Organization FetchOrganziation(SQLiteConnection dbConnection, String Name)
        {
            var org = new Organization();
            try
            {
                string query = String.Format("SELECT * FROM organization WHERE Name='{0}';", Name);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
                SQLiteDataReader reader = cmd.ExecuteReader();

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
        public int Create(SQLiteConnection dbConnection, string name)
        {
            try
            {
                string query = String.Format(@"INSERT OR IGNORE INTO organization(Name, Modified)VALUES('{0}','{1}');SELECT ID FROM organization WHERE Name='{0}';", name, DateTime.Now);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex) { return 0; }
        }

        public void Update(SQLiteConnection dbConnection, Organization org)
        {
            try            {
            string query = String.Format(@"UPDATE organization SET 
                Name='{1}',EmailDomain='{2}',Description='{3}',LinkedIn='{4}',Facebook='{5}',Twitter='{6}',GooglePlus='{7}', Modified={8}
                WHERE ID={0};",
                org.ID, org.Name, org.EmailDomain, org.Description.Replace("'","''"), org.LinkedIn, org.Facebook, org.Twitter, org.GooglePlus, DateTime.Now);
            SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
            cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw ex; }
        }        
        
        public void UpdateEmailDomain(SQLiteConnection dbConnection, Organization org)
        {
            try
            {
                string query = String.Format("UPDATE organization SET EmailDomain = '{0}' WHERE ID = {1};",
                    org.EmailDomain, org.ID);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw ex; }
        }

        public void UpdateSocial(SQLiteConnection dbConnection, Organization org)
        {
            try
            {
                string query = String.Format(@"UPDATE organization SET 
                LinkedIn='{0}',Facebook='{1}',Twitter='{2}',GooglePlus='{3}' 
                WHERE ID={4};",
                    org.LinkedIn, org.Facebook, org.Twitter, org.GooglePlus, org.ID);
                SQLiteCommand cmd=new SQLiteCommand(query, dbConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { throw ex; }
        }      
    }
}
