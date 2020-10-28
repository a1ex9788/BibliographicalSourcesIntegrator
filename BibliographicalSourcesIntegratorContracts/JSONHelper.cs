using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BibliographicalSourcesIntegratorContracts
{
    public static class JSONHelper
    {
        public static string Serialize<T>(T objectToJSON)
        {
            return new JavaScriptSerializer().Serialize(objectToJSON);
        }

        public static T Deserialize<T>(string jsonToString)
        {
            return new JavaScriptSerializer().Deserialize<T>(jsonToString);
        }
    }
}
