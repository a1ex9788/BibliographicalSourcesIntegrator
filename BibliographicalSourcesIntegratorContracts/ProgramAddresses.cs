using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class ProgramAddresses
    {
        public static string BibliographicalSourcesIntegratorWarehouseAddress 
        {
            get => "http://localhost:49845";
        }

        public static string DBLPWrapperAddress
        {
            get => "http://localhost:50065";
        }

        public static string GoogleScholarWrapperAddress
        {
            get => "http://localhost:50070";
        }

        public static string IEEEXploreWrapperAddress
        {
            get => "http://localhost:50069";
        }
    }
}
