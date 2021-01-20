namespace BibliographicalSourcesIntegratorContracts
{
    public class ExtractRequest
    {
        public int InitialYear { get; set; }

        public int FinalYear { get; set; }

        public ExtractRequest()
        {
        }

        public ExtractRequest(int initialYear, int finalYear)
        {
            InitialYear = initialYear;
            FinalYear = finalYear;
        }
    }
}