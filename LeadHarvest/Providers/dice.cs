//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.IO;
//using System.Xml.Linq;
//using System.Text.RegularExpressions;

//namespace LeadHarvest.Providers
//{
//    class dice
//    {
//        public void fetch(string title)
//        {
//            TextWriter contact_file = File.CreateText("C:\\Users\\Steven\\Dropbox\\Code\\dice_" + title + ".csv"); 
//            List<string> lJobs = new List<string>();

//            try
//            {
//                for(int i=0; i<9999; i++) 
//                {
//                    string search_url = "http://seeker.dice.com/jobsearch/servlet/JobSearch?Ntx=mode+matchall&FRMT=0&QUICK=1&ZC_COUNTRY=0&SORTSPEC=0&N=0&Hf=0&Ntk=JobSearchRanking&op=300&LOCATION_OPTION=2&TAXTERM=0&RADIUS=64.37376&DAYSBACK=99&FREE_TEXT=" + title + "&TRAVEL=0&NU_PER_PAGE=99999";
//                    string search_results = _helper.GetHTML(search_url);
//                    List<string> results_links = _helper.ExtractURLs(search_results);

//                    foreach (string link in results_links)
//                    {
//                        Boolean has_email = false;

//                        // <a href="/jobsearch/servlet/JobSearch?op=302&amp;dockey=xml/8/8/881290d54e36531cc530a6f9f41a387e@endecaindex&amp;source=19&amp;FREE_TEXT=Project+Manager+sharepoint&amp;rating=99">

//                        if (link.Contains("jobview"))
//                        {
//                            string job_url = _helper.StripAfter(link, ".aspx");
//                            string jobd_details = _helper.GetHTML(job_url);

//                            //EMAIL
//                            List<string> job_emails = _helper.ExtractEmails(jobd_details);
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
//            Console.ReadLine();
//        }
//    }
//}

        
        
        
        
        
        
        
        
        
        
