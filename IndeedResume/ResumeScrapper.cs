using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Web;
using System.IO;

namespace IndeedResume
{
    

    public class ResumeScrapper
    {

        public void Scrap(string KeyWord,string Location)
        {
            string url = @"http://www.indeed.com/resumes?q={0}&l={1}&radius=100";
            GlobusHttpHelper ghh = new GlobusHttpHelper();

            KeyWord = HttpUtility.UrlEncode(KeyWord);
            Location = HttpUtility.UrlEncode(Location);
            string scrapeUrl = string.Format(url, KeyWord, Location);
            var html = ghh.getHtmlfromUrl(new Uri(scrapeUrl));

            HtmlDocument hd = new HtmlDocument();
            hd.LoadHtml(html);

            var hnc = hd.DocumentNode.SelectNodes("//a[@class='confirm-nav refinement-link']");

            var lstCompanies = new List<string>();
            foreach(var hn in hnc)
            {
                var link = hn.GetAttributeValue("href", "");
                if(link.Contains("cmpid"))
                {
                    var Name = hn.GetAttributeValue("Title", "");
                    lstCompanies.Add(Name);
                }
            }
        }

        public void Scrap(string KeyWord)
        {
            var Locations = File.ReadAllLines("States.csv");

            foreach(var loc in Locations)
            {                
                Scrap(KeyWord, loc);
            }
        }
    }
}
