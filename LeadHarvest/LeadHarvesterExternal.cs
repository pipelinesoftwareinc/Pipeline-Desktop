using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Database;
using LeadHarvest.Entities;
using LeadHarvest.Providers;
using MySql.Data.MySqlClient;
using System.Data;

namespace LeadHarvest
{
    public class LeadHarvesterExternal
    {
        static MySqlConnection _dbConnection=new Database.dbConnection().Open();
        public void HarvestLead()
        {            
            // FETCH TermS
            List<Search> lSearch=new dbSearch().FetchTerms(_dbConnection);

            // FETCH SOURCES
            List<Source> lSource=new dbSource().FetchSources(_dbConnection);

            #region Process
            //PROCESS SOURCES
            foreach(Search search in lSearch)
            {
                foreach(Source source in lSource)
                {
                    switch(source.Name.ToLower())
                    {
                        case "indeed":
                            indeed indeed=new indeed();
                            indeed.Source=source;
                            indeed.Search=search;
                            indeed.fetch(_dbConnection);
                            break;
                        case "careerbuilder":
                            //careerbuilder careerbuilder = new careerbuilder();
                            //careerbuilder.fetch(Term.Term);
                            break;
                        case "monster":
                            //monster monster = new monster();
                            //monster.fetch(Term.Term);
                            break;
                    }
                }
            }
            #endregion           

        }
    }
}
