using System.Xml.Linq;

namespace Kip
{
    public static class PrintSchemaFramework
    {
        public static readonly XNamespace Url = "http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework";

        public static readonly XName PrintCapabilities = Url + "PrintCapabilities";
        public static readonly XName Feature = Url + "Feature";
        public static readonly XName Option = Url + "Option";
        public static readonly XName Property = Url + "Property";
        public static readonly XName ScoredProperty = Url + "ScoredProperty";
        public static readonly XName Value = Url + "Value";
    }
}
