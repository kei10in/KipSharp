using System.Xml.Linq;

namespace Kip
{
    public static class Pskv11
    {
        public static readonly XNamespace Namespace = "http://schemas.microsoft.com/windows/2013/05/printing/printschemakeywordsv11";
        public static readonly string Prefix = "pskv11";
        public static readonly NamespaceDeclaration Declaration = new NamespaceDeclaration(Prefix, Namespace);

        public static readonly FeatureName JobPasscode = new FeatureName(Namespace + "JobPasscode");
        public static readonly ParameterName JobPasscodeString = new ParameterName(Namespace + "JobPasscodeString");
        public static readonly FeatureName JobImageFormat = new FeatureName(Namespace + "JobImageFormat");
        public static readonly PrintSchemaName Jpeg = new PrintSchemaName(Namespace + "Jpeg");
        public static readonly PrintSchemaName PNG = new PrintSchemaName(Namespace + "PNG");
    }
}
