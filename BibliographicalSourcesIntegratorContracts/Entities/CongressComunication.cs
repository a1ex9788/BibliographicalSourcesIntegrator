using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class CongressComunication : Publication
    {
        public string Congress { get; set; }

        public string Edition { get; set; }

        public string Place { get; set; }

        public string InitialPage { get; set; }

        public string FinalPage { get; set; }


        public override bool Equals(object obj)
        {
            CongressComunication other = obj as CongressComunication;

            if (other == null)
            {
                return false;
            }

            bool a = Congress == null ? other.Congress == null : Congress.Equals(other.Congress);
            bool b = Edition == null ? other.Edition == null : Edition.Equals(other.Edition);
            bool c = Place == null ? other.Place == null : Place.Equals(other.Place);
            bool d = InitialPage == null ? other.InitialPage == null : InitialPage.Equals(other.InitialPage);
            bool e = FinalPage == null ? other.FinalPage == null : FinalPage.Equals(other.FinalPage);
            bool f = base.Equals(obj);

            return a && b && c && d && e && f;
        }
    }
}
