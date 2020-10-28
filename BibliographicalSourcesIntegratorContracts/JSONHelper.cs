using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BibliographicalSourcesIntegratorContracts
{
    public static class JSONHelper<T>
    {
        public static string Serialize(T objectToJSON)
        {
            return new JavaScriptSerializer().Serialize(objectToJSON);
        }

        public static T Deserialize(string jsonToString)
        {
            return new JavaScriptSerializer().Deserialize<T>(jsonToString);
        }
    }
}
