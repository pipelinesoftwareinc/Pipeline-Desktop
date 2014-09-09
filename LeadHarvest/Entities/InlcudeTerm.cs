using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadHarvest.Entities
{
    class InlcudeTerm
    {
        private int _ID;
        private int _UserID;
        private string _Term;

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }

        public string Term
        {
            get
            {
                return _Term;
            }
            set
            {
                _Term = value;
            }
        }
    }
}
