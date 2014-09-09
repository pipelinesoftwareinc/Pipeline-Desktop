using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadHarvest.Database;
using LeadHarvest.Entities;
using LeadHarvest.Providers;
using MySql.Data.MySqlClient;

namespace LeadHarvest
{
    class program
    {
        static MySqlConnection _dbConnection = new Database.dbConnection().Open();
        static void Main(string[] args)
        {
            
            // Clear out database
            var query = "DELETE FROM `pipeline`.`email`;DELETE FROM `pipeline`.`email_opportunity`;DELETE FROM `pipeline`.`email_organization`;" +
                "DELETE FROM `pipeline`.`opportunity`;DELETE FROM `pipeline`.`organization`;" +
                "ALTER TABLE email AUTO_INCREMENT = 1;ALTER TABLE opportunity AUTO_INCREMENT = 1;ALTER TABLE organization AUTO_INCREMENT = 1";
            //MySqlCommand cmd = new MySqlCommand(query, _dbConnection);
            //cmd.ExecuteNonQuery();


            // FETCH TermS
            List<Search> lSearch = new dbSearch().FetchTerms(_dbConnection);
            
            // FETCH SOURCES
            List<Source> lSource = new dbSource().FetchSources(_dbConnection);
    
            //PROCESS SOURCES
            foreach (Search search in lSearch)
            {
                foreach (Source source in lSource)
                {
                    switch (source.Name.ToLower())
                    {
                        case "indeed":
                            indeed indeed = new indeed();
                            indeed.Source = source;
                            indeed.Search = search;
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

            Console.WriteLine("#### DONE ####");
            Console.ReadLine();


        }
    }
}
