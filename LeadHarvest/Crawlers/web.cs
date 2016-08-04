using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeadHarvest.Crawlers
{
    class web
    {
        private string _Url;
        private string _ResponseUri;
        private string _HTML;

        private List<string> _lSkip = new List<string>(new string[] { 
                "www.w3.org", "http://ssl", // standards
                "ajax.microsoft", "ajax.google.com", "schema.", // technology
                "verisign", "indeed.com", "Addthis.com",  // companys
                "//platform.", "twitter.com/widgets", "api.twitter.com", "twitter.com/share", "twitter.com/home?","twitter.com/intent/", "=http://twitter.com", "logo@2x.png", "2x.png"
            });
        private List<string> _lSocial = new List<string>(new string[] { 
                "twitter.com", "http://facebook.com/", "youtube.com", "plus.google.com/+", "http://www.linkedin.com/company" });
        public string Url
        {
            get
            {
                return _Url;
            }
            set
            {
                _Url = value;
            }
        }

        public string ResponseUri
        {
            get
            {
                return _ResponseUri;
            }
            set
            {
                _ResponseUri = value;
            }
        }
        public string HTML
        {
            get
            {
                return _HTML;
            }
         }

        public string GetHTML()
        {
            return GetHTML(_Url);
        }
        public string GetHTML(string url)
        {
            _Url = url;
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                myRequest.Method = "GET";
                WebResponse myResponse = myRequest.GetResponse();
                this._ResponseUri = myResponse.ResponseUri.ToString();
                StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8);
                _HTML = sr.ReadToEnd();
                return _HTML;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public List<string> GetSocialUrls()
        {
            List<string> list = GetList(@"((mailto\:|(news|(ht|f)tp(s?))\://){1}\S+)");

            var urls = (from s in list
                      from e in _lSocial
                      where s.Contains(e)
                      select s).ToList(); // only get social
            
            return urls;
        }
        public List<string> GetURLs()
        {
             return GetList(@"((mailto\:|(news|(ht|f)tp(s?))\://){1}\S+)");
        }

        public List<string> GetEmails()
        {
            return GetList(@"\b[A-Z0-9._-]+@[A-Z0-9][A-Z0-9.-]{0,61}[A-Z0-9]\.com");
        }
        public List<string> GetPhoneNumbers()
        {
            return GetList(@"^(?:\([2-9]\d{2}\)\ ?|[2-9]\d{2}(?:\-?|\ ?))[2-9]\d{2}[- ]?\d{4}$");
        }
        private List<string> GetList(string RegexPattern)
        {
            MatchCollection matches = Regex.Matches(_HTML, RegexPattern, RegexOptions.IgnoreCase);
            List<string> list = Regex.Matches(_HTML, RegexPattern, RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Select(match => match.Value)
                .Distinct()
                .ToList();

            // SKIP URLS
            foreach(var item in list.ToList())
            {
                foreach(string skip in _lSkip)
                {
                    if(item.Contains(skip))
                    {
                        list.Remove(item);
                        break;
                    }
                }
            }

            return list;     
        }
    }
}
