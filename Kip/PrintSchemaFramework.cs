using System.Xml.Linq;

namespace Kip
{
    public static class Psf
    {
        public static readonly XNamespace Namespace = "http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework";
        public static readonly string Prefix = "psf";
        public static readonly NamespaceDeclaration Declaration = new NamespaceDeclaration(Prefix, Namespace);

        public static readonly XName PrintCapabilities = Namespace + "PrintCapabilities";
        public static readonly XName PrintTicket = Namespace + "PrintTicket";
        public static readonly XName Feature = Namespace + "Feature";
        public static readonly XName Option = Namespace + "Option";
        public static readonly XName ParameterDef = Namespace + "ParameterDef";
        public static readonly XName ParameterInit = Namespace + "ParameterInit";
        public static readonly XName ParameterRef = Namespace + "ParameterRef";
        public static readonly XName Property = Namespace + "Property";
        public static readonly XName ScoredProperty = Namespace + "ScoredProperty";
        public static readonly XName Value = Namespace + "Value";

        public static readonly XName SelectionType = Namespace + "SelectionType";
        public static readonly XName IdentityOption = Namespace + "IdentityOpiton";

        public static readonly XName DataType = Namespace + "DataType";
        public static readonly XName DefaultValue = Namespace + "DefaultValue";
        public static readonly XName MaxLength = Namespace + "MaxLength";
        public static readonly XName MinLength = Namespace + "MinLength";
        public static readonly XName MaxValue = Namespace + "MaxValue";
        public static readonly XName MinValue = Namespace + "MinValue";
        public static readonly XName Mandatory = Namespace + "Mandatory";
        public static readonly XName UnitType = Namespace + "UnitType";

    }
}
