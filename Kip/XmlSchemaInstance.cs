using System.Xml.Linq;

namespace Kip
{
    public static class XmlSchemaInstance
    {
        public static readonly XNamespace Url = "http://www.w3.org/2001/XMLSchema-instance";

        public static readonly XName Type = Url + "type";
    }
}
