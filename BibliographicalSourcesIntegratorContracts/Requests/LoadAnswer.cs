using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class LoadAnswer
    {
        public int DBLPNumberOfResults { get; set; }
        public List<string> DBLPErrors { get; set; }

        public int IEEEXploreNumberOfResults { get; set; }
        public List<string> IEEEXploreErrors { get; set; }

        public int GoogleScholarNumberOfResults { get; set; }
        public List<string> GoogleScholarErrors { get; set; }


        public LoadAnswer()
        {
            DBLPErrors = new List<string>();
            IEEEXploreErrors = new List<string>();
            GoogleScholarErrors = new List<string>();
        }

        public LoadAnswer(int dBLPNumberOfResults, List<string> dBLPErrors, int iEEEXploreNumberOfResults, List<string> iEEEXploreErrors, int googleScholarNumberOfResults, List<string> googleScholarErrors)
        {
            DBLPNumberOfResults = dBLPNumberOfResults;
            DBLPErrors = dBLPErrors;
            IEEEXploreNumberOfResults = iEEEXploreNumberOfResults;
            IEEEXploreErrors = iEEEXploreErrors;
            GoogleScholarNumberOfResults = googleScholarNumberOfResults;
            GoogleScholarErrors = googleScholarErrors;
        }
    }
}
