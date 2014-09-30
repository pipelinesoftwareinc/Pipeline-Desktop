//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.IO;
//using System.Xml.Linq;
//using System.Text.RegularExpressions;

//namespace LeadHarvest.Providers
//{
//    class monster
//    {
//        public void fetch(string title)
//        {
//            string path = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
//            if (Environment.OSVersion.Version.Major >= 6) { path = Directory.GetParent(path).ToString(); }

//            TextWriter contact_file = File.CreateText(path + "\\monster_" + title + ".csv");
//            List<string> lJobs = new List<string>();

//            try
//            {
//                for(int i=0; i<9999; i++) 
//                {
//                    string search_url = "http://jobsearch.monster.com/search/?q=" + title + "&pg=" + i + "&sort=sal";
//                    string search_results = _helper.GetHTML(search_url);

//                    List<string> results_links = _helper.ExtractURLs(search_results);

//                    foreach (string link in results_links)
//                    {
//                        Boolean has_email = false;
//                        if (link.Contains("jobview"))
//                        {
//                            string job_url = _helper.StripAfter(link, ".aspx");
//                            string jobd_details = _helper.GetHTML(job_url);

//                            //lJobs.Add(job_url);

//                            //// strings
//                            //string url = "http://jobview.monster.com/";
//                            //string pattern = "-Job-";
//                            //string ext = ".aspx";

//                            //// Length of
//                            //int lJ = job_url.Length;
//                            //int lU = url.Length;
//                            //int lP = pattern.Length;
//                            //int lE = ext.Length;
//                            //int lI = 10;

//                            ////Index of
//                            //int iU = 1;
//                            //int iP = job_url.IndexOf(pattern); // -Job-
//                            //int iE = job_url.IndexOf(ext); // .aspx
//                            //int iL = iP + lP; // location
//                            //int iI = lJ - (lI + lE); // job id

//                            ////Length of
//                            //int lT = iP - lU;
//                            //int lL = iI - (iP + lP);

//                            //string title = job_url.Substring(lU, lT).Replace("-", " ");
//                            //string location = job_url.Substring(iL, lL).Replace("-", " ");
//                            //string job_id = job_url.Substring(iI + 1, lI - 1);

//                            //lJobs.Add(job_id);
//                            //lJobs.Add(title);
//                            //lJobs.Add(location);

//                            //EMAIL
//                            //string[] job_emails = _helper.ExtractEmails(jobd_details);
//                            //foreach (var value in job_emails)
//                            //{
//                            //    has_email = true;
//                            //    if(!lJobs.Contains(value))
//                            //        lJobs.Add(value);
//                            //}

//                            if (has_email)
//                            {
//                                string lContactCSV = string.Join("|", lJobs.ToArray());
//                                contact_file.WriteLine(lContactCSV);
//                                contact_file.Flush();
//                                Console.WriteLine(lContactCSV);
//                                Console.WriteLine(Environment.NewLine);
//                            }
//                            lJobs.Clear();
//                        }
//                    }

//                }
//            }
//            catch(Exception appEX)
//            {
//                contact_file.Close();
//                Console.WriteLine(appEX.Message);
//            }
//            contact_file.Close();

//        }
//    }
//}
