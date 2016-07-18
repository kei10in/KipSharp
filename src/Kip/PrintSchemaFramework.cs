using System.Xml.Linq;

namespace Kip
{
    public static class Psf
    {
        public static readonly XNamespace Namespace = "http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework";
        public static readonly string Prefix = "psf";
        public static readonly NamespaceDeclaration Declaration = new NamespaceDeclaration(Prefix, Namespace);

        // Tag names
        public static readonly PrintSchemaName PrintCapabilities = new PrintSchemaName(Namespace + "PrintCapabilities");
        public static readonly PrintSchemaName PrintTicket = new PrintSchemaName(Namespace + "PrintTicket");
        public static readonly PrintSchemaName Feature = new PrintSchemaName(Namespace + "Feature");
        public static readonly PrintSchemaName Option = new PrintSchemaName(Namespace + "Option");
        public static readonly PrintSchemaName ParameterDef = new PrintSchemaName(Namespace + "ParameterDef");
        public static readonly PrintSchemaName ParameterInit = new PrintSchemaName(Namespace + "ParameterInit");
        public static readonly PrintSchemaName ParameterRef = new PrintSchemaName(Namespace + "ParameterRef");
        public static readonly PrintSchemaName Property = new PrintSchemaName(Namespace + "Property");
        public static readonly PrintSchemaName ScoredProperty = new PrintSchemaName(Namespace + "ScoredProperty");
        public static readonly PrintSchemaName Value = new PrintSchemaName(Namespace + "Value");

        // Property for Feature
        public static readonly PropertyName SelectionType = new PropertyName(Namespace + "SelectionType");

        // Property for Option
        public static readonly PropertyName IdentityOption = new PropertyName(Namespace + "IdentityOpiton");

        // Property for Parameter
        public static readonly PropertyName DataType = new PropertyName(Namespace + "DataType");
        public static readonly PropertyName DefaultValue = new PropertyName(Namespace + "DefaultValue");
        public static readonly PropertyName MaxLength = new PropertyName(Namespace + "MaxLength");
        public static readonly PropertyName MinLength = new PropertyName(Namespace + "MinLength");
        public static readonly PropertyName MaxValue = new PropertyName(Namespace + "MaxValue");
        public static readonly PropertyName MinValue = new PropertyName(Namespace + "MinValue");
        public static readonly PropertyName Multiple = new PropertyName(Namespace + "Multiple");
        public static readonly PropertyName Mandatory = new PropertyName(Namespace + "Mandatory");
        public static readonly PropertyName UnitType = new PropertyName(Namespace + "UnitType");
    }
}
