using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class LoadRequest
    {
        public bool loadFromDBLP { get; set; }

        public bool loadFromIEEEXplore { get; set; }

        public bool loadFromGoogleScholar { get; set; }


        public int InitialYear { get; set; }

        public int FinalYear { get; set; }


        public LoadRequest() { }
        
        public LoadRequest(bool loadFromDBLP, bool loadFromIEEEXplore, bool loadFromGoogleScholar, int initialYear, int finalYear)
        {
            this.loadFromDBLP = loadFromDBLP;
            this.loadFromIEEEXplore = loadFromIEEEXplore;
            this.loadFromGoogleScholar = loadFromGoogleScholar;
            InitialYear = initialYear;
            FinalYear = finalYear;
        }
    }
}
