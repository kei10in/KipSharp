using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kip
{
    public static class Xsd
    {
        public static readonly XNamespace Namespace = "http://www.w3.org/2001/XMLSchema";

        public static readonly XName Integer = Namespace + "integer";
        public static readonly XName Decimal = Namespace + "decimal";
        public static readonly XName String = Namespace + "string";
        public static readonly XName QName = Namespace + "QName";
    }
}
