using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class LoadRequest
    {
        public bool LoadFromDBLP { get; set; }

        public bool LoadFromIEEEXplore { get; set; }

        public bool LoadFromGoogleScholar { get; set; }


        public int InitialYear { get; set; }

        public int FinalYear { get; set; }


        public LoadRequest() { }
        
        public LoadRequest(bool loadFromDBLP, bool loadFromIEEEXplore, bool loadFromGoogleScholar, int initialYear, int finalYear)
        {
            LoadFromDBLP = loadFromDBLP;
            LoadFromIEEEXplore = loadFromIEEEXplore;
            LoadFromGoogleScholar = loadFromGoogleScholar;
            InitialYear = initialYear;
            FinalYear = finalYear;
        }
    }
}
