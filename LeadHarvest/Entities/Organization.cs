using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadHarvest.Entities
{
    class Organization
    {
        private int _ID;
        private string _Name;
        private string _EmailDomain;
        private string _Description;
        private string _LinkedIn;
        private string _Facebook;
        private string _Twitter;
        private string _GooglePlus;

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
        public String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        public String EmailDomain
        {
            get
            {
                return _EmailDomain;
            }
            set
            {
                _EmailDomain = value;
            }
        }
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        public String LinkedIn
        {
            get
            {
                return _LinkedIn;
            }
            set
            {
                _LinkedIn = value;
            }
        }

        public String Facebook
        {
            get
            {
                return _Facebook;
            }
            set
            {
                _Facebook = value;
            }
        }

        public String Twitter
        {
            get
            {
                return _Twitter;
            }
            set
            {
                _Twitter = value;
            }
        }

        public String GooglePlus
        {
            get
            {
                return _GooglePlus;
            }
            set
            {
                _GooglePlus = value;
            }
        }
    }
}
