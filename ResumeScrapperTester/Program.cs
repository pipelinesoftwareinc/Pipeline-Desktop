using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IndeedResume;

namespace ResumeScrapperTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var resumeScrapper = new ResumeScrapper();
                                                              
            resumeScrapper.Scrap("SharePoint Developer", "Jaipur, Rajasthan");
            //resumeScrapper.Scrap("\"SharePoint Developer\"");
        }
    }
}
