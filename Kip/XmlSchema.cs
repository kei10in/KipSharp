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
        public static readonly XNamespace Url = "http://www.w3.org/2001/XMLSchema";

        public static readonly XName Integer = Url + "integer";
        public static readonly XName Decimal = Url + "decimal";
        public static readonly XName String = Url + "string";
        public static readonly XName QName = Url + "QName";
    }
}
