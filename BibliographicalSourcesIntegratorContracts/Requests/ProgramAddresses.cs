namespace BibliographicalSourcesIntegratorContracts
{
    public class ProgramAddresses
    {
        public static string BibliographicalSourcesIntegratorWarehouseAddress
        {
            get => "http://localhost:50001";
        }

        public static string DBLPWrapperAddress
        {
            get => "http://localhost:50002";
        }

        public static string GoogleScholarWrapperAddress
        {
            get => "http://localhost:50003";
        }

        public static string IEEEXploreWrapperAddress
        {
            get => "http://localhost:50004";
        }

        public static string IEEEXploreAPIAddress
        {
            get => "https://ieeexploreapi.ieee.org";
        }
    }
}