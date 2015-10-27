using System.Xml.Linq;

namespace Kip
{
    public static class PrintSchemaKeywords
    {
        public static readonly XNamespace Url = "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords";

        public static readonly XName PickOne = Url + "PickOne";
        public static readonly XName PickMany = Url + "PickMany";
        public static readonly XName DisplayUI = Url + "DisplayUI";
        public static readonly XName DisplayName = Url + "DisplayName";
    }
}
