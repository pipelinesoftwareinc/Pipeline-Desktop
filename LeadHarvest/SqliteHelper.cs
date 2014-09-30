using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace LeadHarvest
{
    class SqliteHelper
    {
        /// <summary>
        /// Add Escape Character Helper :-Ankur
        /// </summary>
        /// <param name="data">String to replace characters</param>
        /// <returns>string</returns>
        public static string Escape(string data)
        {
            data=data.Replace("'", "''");
            data=data.Replace("\\", "\\\\");
            return data;
        }
    }
}
