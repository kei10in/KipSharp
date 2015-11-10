using System.Xml.Linq;

namespace Kip
{
    public static class Pskv11
    {
        public static readonly XNamespace Namespace = "http://schemas.microsoft.com/windows/2013/05/printing/printschemakeywordsv11";

        public static readonly XName JobPasscode = Namespace + "JobPasscode";
        public static readonly XName JobPasscodeString = Namespace + "JobPasscodeString";
        public static readonly XName JobImageFormat = Namespace + "JobImageFormat";
        public static readonly XName Jpeg = Namespace + "Jpeg";
        public static readonly XName PNG = Namespace + "PNG";
    }
}
