﻿using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LeadHarvest;
//using LeadHarvest.Database;
using LeadHarvest.SqliteDal;
using LeadHarvest.Entities;
using LeadHarvest.Crawlers;
using System.Data.SQLite;

namespace LeadHarvest.Providers
{
    class indeed
    {
        public Source Source;
        public Search Search;

        private web web = new web();
        public void fetch(SQLiteConnection _dbConnection)
        {

            dbOpportunity dbOpp = new dbOpportunity();
            List<String> keys = dbOpp.FetchSourceKey(_dbConnection,Source.ID);

            string url;

            // USE THE API TO SEARCH
            url = "http://api.indeed.com/ads/apisearch?publisher=7755817834685213&v=2&q=" + Search.Term;
            XDocument results = XDocument.Load(url);
            var totalresults = Convert.ToInt32(results.Root.Element("totalresults").Value);

           Console.WriteLine("Total Results:" + totalresults);

            // THERE ARE LOTS OF RESULTS NEED TO PAGE OUT
            var pages = totalresults / 10;
            for (int i = 0; i <= pages; i += 10)
                {

                Console.WriteLine("######################################################");
                Console.WriteLine("Processing page:" + i + " of " + pages + " total pages");
                Console.WriteLine("######################################################");

                // FETCH PAGE
                url = "http://api.indeed.com/ads/apisearch?publisher=7755817834685213&v=2&start=" + i + "&q=" + Search.Term  ;
                XDocument joblisting = XDocument.Load(url);

                // FETCH EACH RESULT 
                foreach (XElement result in joblisting.Descendants("result"))
                {
                    if (!keys.Contains(result.Element("jobkey").Value))
                    {
                        try {

                            // ####################
                            // PROCESS INDEED PAGE
                            // ####################

                            Organization org = new Organization();
                            dbOrganization dbOrg = new dbOrganization();
                            org.Name = result.Element("company").Value.Replace("'", "");
                            org.ID = dbOrg.Create(_dbConnection, org.Name);
                            Console.WriteLine(org.ID + "|" + org.Name);

                            Opportunity opp = new Opportunity();
                            opp.SourceID = Source.ID;
                            opp.SearchID = Search.ID;
                            opp.OrganizationID = org.ID;
                            opp.Title = result.Element("jobtitle").Value;
                            opp.Snippet = result.Element("snippet").Value;
                            opp.DatePosted = Convert.ToDateTime(result.Element("date").Value);
                            opp.City = result.Element("city").Value;
                            opp.State = result.Element("state").Value;
                            opp.SourceKey = result.Element("jobkey").Value;
                            opp.SourceUri = "http://www.indeed.com/rc/clk?jk=" + opp.SourceKey; // Indeed page
                            opp.Compensation = "$110/hr";

                            // NEEDED TO GET THE RESPONSE URI
                            web.GetHTML(opp.SourceUri); // company page
                            opp.ResponseUri = web.ResponseUri;

                            // CREATE OPPORTUNITY
                            opp.ID = dbOpp.Create(_dbConnection, opp);
                            Console.WriteLine("  >>" + opp.ID + "|" + opp.Title + "|" + opp.City + "|" + opp.State);

                            // CAPTURE EMAIL AND SOCIAL
                            List<string> emails = web.GetEmails();
                            List<string> social = web.GetSocialUrls();
                            
                            // ####################
                            // PROCESS EXTERNAL PAGE
                            // ####################
                            web.GetHTML(result.Element("url").Value);

                            // EXTRACT ORG DESCRIPTION
                            List<string> descrip = new List<string>();
                            string text = "<div class=\"cmp_description\">\n                ([^<]+)...\n";
                            foreach (Match match in Regex.Matches(web.HTML, text))
                                descrip.Add(match.Groups[1].Value);
                            if (descrip.Count > 0)
                            {
                                org.Description = descrip[0].ToString().Replace("'", "\'");
                                dbOrg.Update(_dbConnection, org);
                            }

                            // PROCESS EMAIL ON HTML PAGE   (Moved email from here to close to the url call)
                          
                            dbEmail dbEmail = new dbEmail();
                          
                            foreach (var value in emails)
                            {
                                Email email = new Email();
                                email.Address = value;

                                // CREATE EMAIL
                                email.ID = dbEmail.CreateEmail(_dbConnection, email);
                                dbEmail.CreateEmail_Opportunity(_dbConnection, email, opp);
                                dbEmail.CreateEmail_Organization(_dbConnection, email, org);

                                // STORE EMAIL HOST WITH ORG REC
                                var host = new MailAddress(value).Host;
                                org.EmailDomain = new MailAddress(email.Address).Host;
                                dbOrg.UpdateEmailDomain(_dbConnection, org);
                            }

                            // ####################
                            // PROCESS REDIRECT HTML
                            // ####################

                          

                            // PROCESS ALL EMAIL ON HTML PAGE 
                            
                            #region Emails
                            foreach (var value in emails)
                            {
                                Email email = new Email();
                                email.Address = value;

                                // CREATE EMAIL
                                email.ID = dbEmail.CreateEmail(_dbConnection, email);
                                dbEmail.CreateEmail_Opportunity(_dbConnection, email, opp);
                                dbEmail.CreateEmail_Organization(_dbConnection, email, org);

                                // STORE EMAIL HOST WITH ORG REC
                                var host = new MailAddress(value).Host;
                                org.EmailDomain = new MailAddress(email.Address).Host;
                                dbOrg.UpdateEmailDomain(_dbConnection, org);
                            }
                            #endregion

                            // SOCIAL 
                            #region Social
                          
                            if (social.Count > 0)
                            {
                                org.LinkedIn = social.Find(item => item.Contains("linkedin"));
                                org.Facebook = social.Find(item => item.Contains("facebook"));
                                org.Twitter = social.Find(item => item.Contains("twitter"));
                                org.GooglePlus = social.Find(item => item.Contains("plus.google"));

                                if (!string.IsNullOrEmpty(org.LinkedIn))
                                    org.LinkedIn = org.LinkedIn.Replace("\"", "");
                                if (!string.IsNullOrEmpty(org.Facebook))
                                    org.Facebook = org.Facebook.Replace("\"", "");
                                if (!string.IsNullOrEmpty(org.Twitter))
                                    org.Twitter = org.Twitter.Replace("\"", "");
                                if (!string.IsNullOrEmpty(org.GooglePlus))
                                    org.GooglePlus = org.GooglePlus.Replace("\"", "");

                                dbOrg.UpdateSocial(_dbConnection, org);
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("ERROR " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Skipping " + result.Element("jobkey").Value);
                    }
                }      
            }
        }
    }
}