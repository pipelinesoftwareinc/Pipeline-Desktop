using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadHarvest.Entities
{
    class Opportunity
    {
        private int _ID;
        private int _OrganizationID;
        private string _Snippet;
        private DateTime _DatePosted;
        private int _SourceID;
        private int _SearchID;
        private string _Title;
        private string _City;
        private string _State;
        private string _ResponseUri;
        private string _SourceUri;
        private string _SourceKey;
        private string _Compensation;
        private string _JobType;

        public int OrganizationID
        {
            get
            {
                return _OrganizationID;
            }
            set
            {
                _OrganizationID = value;
            }
        }

        public string Snippet
        {
            get
            {
                return _Snippet;
            }
            set
            {
                _Snippet = value;
            }
        }

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

        public DateTime DatePosted
        {
            get
            {
                return _DatePosted;
            }
            set
            {
                _DatePosted = value;
            }
        }

        public int SourceID
        {
            get
            {
                return _SourceID;
            }
            set
            {
                _SourceID = value;
            }
        }

        public int SearchID
        {
            get
            {
                return _SearchID;
            }
            set
            {
                _SearchID = value;
            }
        }

        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        public string City
        {
            get
            {
                return _City;
            }
            set
            {
                _City = value;
            }
        }

        public string State
        {
            get
            {
                return _State;
            }
            set
            {
                _State = value;
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

        public string SourceUri
        {
            get
            {
                return _SourceUri;
            }
            set
            {
                _SourceUri = value;
            }
        }

        public string SourceKey
        {
            get
            {
                return _SourceKey;
            }
            set
            {
                _SourceKey = value;
            }
        }

        public string Compensation
        {
            get
            {
                return _Compensation;
            }
            set
            {
                _Compensation = value;
            }
        }

        public string JobType
        {
            get
            {
                return _JobType;
            }
            set
            {
                _JobType = value;
            }
        }
      
    }
}
