﻿namespace BibliographicalSourcesIntegratorContracts.Entities
{
    public class Book : Publication
    {
        public string Editorial { get; set; }

        public override bool Equals(object obj)
        {
            Book other = obj as Book;

            if (other == null)
            {
                return false;
            }

            bool a = Editorial == null ? other.Editorial == null : Editorial.Equals(other.Editorial);
            bool b = base.Equals(obj);

            return a && b;
        }
    }
}