using System.Xml.Linq;

namespace Kip
{
    public static class Xsi
    {
        public static readonly XNamespace Namespace = "http://www.w3.org/2001/XMLSchema-instance";

        public static readonly XName Type = Namespace + "type";
    }
}
