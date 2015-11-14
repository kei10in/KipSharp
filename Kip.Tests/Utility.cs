using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kip.Tests
{
    internal static class Utility
    {
        private static readonly string PrintCapabilitiesHeader =
            @"<?xml version='1.0' encoding='UTF-8'?>
            <psf:PrintCapabilities
                version='1'
                xmlns:psf='http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework'
                xmlns:psk='http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords'
                xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                xmlns:xsd='http://www.w3.org/2001/XMLSchema'
                xmlns:exp='http://example.com/2015/printschemakeywords'>";
        private static readonly string PrintCapabilitiesFooter = "</psf:PrintCapabilities>";

        internal static string PrintCapabilitiesWith(string content)
        {
            return string.Join(
                Environment.NewLine,
                PrintCapabilitiesHeader,
                content,
                PrintCapabilitiesFooter);
        }
    }

    internal static class Exp
    {
        internal static readonly XNamespace Namespace = "http://example.com/2015/printschemakeywords";

        internal static readonly XName SomeFeature = Namespace + "SomeFeature";

        internal static readonly XName SomeOption = Namespace + "SomeOption";

        internal static readonly XName SomeParameter = Namespace + "SomeParameter";

        internal static readonly XName SomeProperty = Namespace + "SomeProperty";

        internal static readonly XName SomeScoredProperty = Namespace + "SomeScoredProperty";
    }
}
