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

        internal static readonly FeatureName SomeFeature = new FeatureName(Namespace + "SomeFeature");
        internal static readonly FeatureName SomeFeature1 = new FeatureName(Namespace + "SomeFeature1");
        internal static readonly FeatureName SomeFeature2 = new FeatureName(Namespace + "SomeFeature2");
        internal static readonly FeatureName NestedFeature = new FeatureName(Namespace + "NestedFeature");
        internal static readonly FeatureName OtherFeature = new FeatureName(Namespace + "OtherFeature");

        internal static readonly XName SomeOption = Namespace + "SomeOption";
        internal static readonly XName SomeOption1 = Namespace + "SomeOption1";
        internal static readonly XName SomeOption2 = Namespace + "SomeOption2";

        internal static readonly ParameterName SomeParameter = new ParameterName(Namespace + "SomeParameter");
        internal static readonly ParameterName OtherParameter = new ParameterName(Namespace + "OtherParameter");

        internal static readonly PropertyName SomeProperty = new PropertyName(Namespace + "SomeProperty");
        internal static readonly PropertyName SomeProperty1 = new PropertyName(Namespace + "SomeProperty1");
        internal static readonly PropertyName SomeProperty2 = new PropertyName(Namespace + "SomeProperty2");
        internal static readonly PropertyName NestedProperty = new PropertyName(Namespace + "NestedProperty");
        internal static readonly PropertyName OtherProperty = new PropertyName(Namespace + "OtherProperty");

        internal static readonly ScoredPropertyName SomeScoredProperty = new ScoredPropertyName(Namespace + "SomeScoredProperty");
        internal static readonly ScoredPropertyName SomeScoredProperty1 = new ScoredPropertyName(Namespace + "SomeScoredProperty1");
        internal static readonly ScoredPropertyName SomeScoredProperty2 = new ScoredPropertyName(Namespace + "SomeScoredProperty2");
        internal static readonly ScoredPropertyName OtherScoredProperty = new ScoredPropertyName(Namespace + "OtherScoredProperty");

        internal static readonly XName SomeValue = Namespace + "SomeValue";
    }
}
