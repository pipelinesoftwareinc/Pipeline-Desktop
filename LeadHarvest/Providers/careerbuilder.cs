//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.IO;
//using System.Xml.Linq;
//using System.Text.RegularExpressions;

//namespace LeadHarvest.Providers
//{
//    class careerbuilder
//    {
//        helper _helper = new helper();
//        public void fetch(string title)
//        {
//            TextWriter contact_file = File.CreateText("C:\\Users\\Steven\\Dropbox\\Code\\careerbuilder_" + title + ".csv");
//            List<string> lJobs = new List<string>();

//            try
//            {
//                for (int i = 0; i < 9999; i++)
//                {
//                    string search_url = @"http://www.careerbuilder.com/JobSeeker/Jobs/JobResults.aspx?" +
//                        "excrit=QID%3dA6659244663569%3bst%3da%3buse%3dALL%3brawWords%3d" + title + "%3bCID%3dUS%3bSID%3d%3f%3bTID%3d0%3bENR%3dNO%3bDTP%3dDRNS%3bYDI%3dYES%3bIND%3dALL%3bPDQ%3dAll%3bPDQ%3dAll%3bPAYL%3d0%3bPAYH%3dgt120%3bPOY%3dNO%3bETD%3dALL%3bRE%3dALL%3bMGT%3dDC%3bSUP%3dDC%3bFRE%3d30%3bCHL%3dAL%3bQS%3dsid_unknown%3bSS%3dNO%3bTITL%3d0%3bJQT%3dRAD%3bJDV%3dFalse%3bHost%3dUS%3bMaxLowExp%3d-1%3bRecsPerPage%3d25&" +
//                        "pg=" + i + "&IPath=JRKV";
//                    string search_results = _helper.GetHTML(search_url);
//                    List<string> results_links = _helper.ExtractURLs(search_results);


//                    foreach (string link in results_links)
//                    {
//                        Boolean has_email = false;

//                        // <a href="/jobsearch/servlet/JobSearch?op=302&amp;dockey=xml/8/8/881290d54e36531cc530a6f9f41a387e@endecaindex&amp;source=19&amp;FREE_TEXT=Project+Manager+sharepoint&amp;rating=99">

//                        if (link.Contains("http://www.careerbuilder.com/JobSeeker/Jobs/JobDetails.aspx?job_did="))
//                        {
//                            string job_url = _helper.StripAfter(link, ".aspx");
//                            string x = "http://www.careerbuilder.com/JobSeeker/Jobs/JobDetails.aspx?job_did=";
//                            string y = "JHV82L74100YYVL7Y14";
//                            var a = x.Length;
//                            var b = y.Length;
//                            var c = a + b;
//                            var d = link.Substring(0, c);
//                            string details = _helper.GetHTML(d);

                           
//                            //EMAIL
//                            //string[] job_emails = _helper.ExtractEmails(details);
//                            //foreach (var value in job_emails)
//                            //{
//                            //    has_email = true;
//                            //    if (!lJobs.Contains(value))
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
//            catch (Exception appEX)
//            {
//                contact_file.Close();
//                Console.WriteLine(appEX.Message);
//            }
//            contact_file.Close();
//        }
//    }
//}
