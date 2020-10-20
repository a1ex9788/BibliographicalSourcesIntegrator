using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class ExtractRequest
    {
        public int InitialYear { get; set; }

        public int FinalYear { get; set; }

        public ExtractRequest(int initialYear, int finalYear)
        {
            InitialYear = initialYear;
            FinalYear = finalYear;
        }
    }
}
