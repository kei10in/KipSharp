using System.Xml.Linq;

namespace Kip
{
    public static class Xsd
    {
        public static readonly XNamespace Namespace = "http://www.w3.org/2001/XMLSchema";
        public static readonly string Prefix = "xsd";
        public static readonly NamespaceDeclaration Declaration = new NamespaceDeclaration(Prefix, Namespace);

        public static readonly XName Integer = Namespace + "integer";
        public static readonly XName Decimal = Namespace + "decimal";
        public static readonly XName String = Namespace + "string";
        public static readonly XName QName = Namespace + "QName";
    }
}
