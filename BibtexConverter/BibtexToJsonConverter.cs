using BibtexLibrary.Parser;
using BibtexLibrary.Tokenizer;
using System.Web.Script.Serialization;

namespace BibtexConverter
{
    public class BibtexToJsonConverter
    {
        public BibtexToJsonConverter()
        {

        }


        public string Parse(string bibFilePath)
        {
            string input = "@book{halbwachs2004memoria,title ={ La memoria colectiva}, author ={ Halbwachs, Maurice}, volume ={ 6},year ={ 2004}, publisher ={ Prensas de la Universidad de Zaragoza}}";
            BibtexParser parser = new BibtexParser(new Tokenizer(new ExpressionDictionary(), input));

            var result = parser.Parse();

            return new JavaScriptSerializer().Serialize(result);
        }
    }
}
