﻿namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Article : Publication
    {
        public string InitialPage { get; set; }

        public string FinalPage { get; set; }

        public virtual Exemplar Exemplar { get; set; }

        public override bool Equals(object obj)
        {
            Article other = obj as Article;

            if (other == null)
            {
                return false;
            }

            bool a = InitialPage == null ? other.InitialPage == null : InitialPage.Equals(other.InitialPage);
            bool b = FinalPage == null ? other.FinalPage == null : FinalPage.Equals(other.FinalPage);
            bool c = Exemplar == null ? other.Exemplar == null : Exemplar.Equals(other.Exemplar);
            bool d = base.Equals(obj);

            return a && b && c && d;
        }
    }
}