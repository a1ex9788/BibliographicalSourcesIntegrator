using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliographicalSourcesIntegratorContracts
{
    public class LoadAnswer
    {
        public string DBLPAnswer { get; set; }

        public string IEEXploreAnswer { get; set; }

        public string GoogleScholarAnswer { get; set; }


        public LoadAnswer() { }

        public LoadAnswer(string dBLPAnswer, string iEEXploreAnswer, string googleScholarAnswer)
        {
            DBLPAnswer = dBLPAnswer;
            IEEXploreAnswer = iEEXploreAnswer;
            GoogleScholarAnswer = googleScholarAnswer;
        }
    }
}
