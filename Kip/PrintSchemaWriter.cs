using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    public static class PrintSchemaWriter
    {
        private class NamespaceResolver
        {
            private Dictionary<XNamespace, string> cache = new Dictionary<XNamespace, string>();
            private int n = 0;

            internal NamespaceResolver(IReadOnlyNamespaceDeclarationCollection namespaces)
            {
                NamespaceDeclarations = new NamespaceDeclarationCollection(namespaces);
            }

            internal NamespaceDeclarationCollection NamespaceDeclarations { get; }

            internal void Add(XNamespace uri)
            {
                if (uri == null) throw new ArgumentNullException(nameof(uri));
                if (cache.ContainsKey(uri)) return;

                var prefix = string.Format("ns{0:0000}", n);
                n += 1;
                cache[uri] = prefix;
            }
        }

        public static void Write(XmlWriter writer, Capabilities pc)
        {
            var declarations = NamespaceDeclarations(pc);

            writer.WriteStartDocument();
            var prefix = declarations.LookupPrefix(Psf.PrintCapabilities.NamespaceName);
            writer.WriteStartElement(prefix, Psf.PrintCapabilities.LocalName, Psf.PrintCapabilities.NamespaceName);
            writer.WriteAttributeString("version", "1");

            foreach (var decl in declarations)
            {
                writer.WriteAttributeString("xmlns", decl.Prefix, null, decl.Uri.NamespaceName);
            }

            foreach (var f in pc.Features)
            {
                Write(writer, f);
            }

            foreach (var p in pc.Properties)
            {
                Write(writer, p);
            }

            foreach (var p in pc.Parameters)
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
            writer.Flush();
        }

        public static void Write(XmlWriter writer, Ticket pt)
        {
            var declarations = NamespaceDeclarations(pt);

            writer.WriteStartDocument();
            var prefix = declarations.LookupPrefix(Psf.PrintTicket.NamespaceName);
            writer.WriteStartElement(prefix, Psf.PrintTicket.LocalName, Psf.PrintTicket.NamespaceName);
            writer.WriteAttributeString("version", "1");

            foreach (var decl in declarations)
            {
                writer.WriteAttributeString("xmlns", decl.Prefix, null, decl.Uri.NamespaceName);
            }

            foreach (var f in pt.Features())
            {
                Write(writer, f);
            }

            foreach (var p in pt.Properties())
            {
                Write(writer, p);
            }

            foreach (var p in pt.Parameters())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
            writer.Flush();
        }

        private static NamespaceDeclarationCollection NamespaceDeclarations(Capabilities pc)
        {
            return NamespaceDeclarations(NamespaceCollector.Collect(pc), pc.DeclaredNamespaces);
        }

        private static NamespaceDeclarationCollection NamespaceDeclarations(Ticket pt)
        {
            return NamespaceDeclarations(NamespaceCollector.Collect(pt), pt.DeclaredNamespaces);
        }

        private static NamespaceDeclarationCollection NamespaceDeclarations(
            IEnumerable<XNamespace> appeared, IReadOnlyNamespaceDeclarationCollection declared)
        {
            int n = 0;
            var result = new List<NamespaceDeclaration>();

            appeared = Enumerable.Concat(
                new List<XNamespace> { Psf.Namespace, Psk.Namespace, Xsd.Namespace, Xsi.Namespace },
                appeared);

            foreach (var uri in appeared)
            {
                var prefix = declared.LookupPrefix(uri);
                if (prefix == null)
                {
                    prefix = string.Format("ns{0:0000}", n);
                    n += 1;
                }
                result.Add(new NamespaceDeclaration(prefix, uri));
            }

            return new NamespaceDeclarationCollection(result);
        }

        private static void Write(XmlWriter writer, Feature feature)
        {
            writer.WriteStartElement(Psf.Feature.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteAttributeString("name", feature.Name.ToQName(writer));

            foreach (var o in feature.Options())
            {
                Write(writer, o);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Option option)
        {
            writer.WriteStartElement(Psf.Option.LocalName, Psf.Namespace.NamespaceName);
            if (option.Name != null)
                writer.WriteAttributeString("name", option.Name.ToQName(writer));

            foreach (var p in option.Properties)
            {
                Write(writer, p);
            }

            foreach (var sp in option.ScoredProperties)
            {
                Write(writer, sp);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ParameterDef element)
        {
            writer.WriteStartElement(Psf.ParameterDef.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            foreach (var p in element.Properties)
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ParameterInit element)
        {
            writer.WriteStartElement(Psf.ParameterDef.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            Write(writer, element.Value);

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ParameterRef element)
        {
            if (element == null) return;

            writer.WriteStartElement(Psf.ParameterRef.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));
            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Property element)
        {
            writer.WriteStartElement(Psf.Property.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            Write(writer, element.Value);

            foreach (var p in element.Properties)
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ScoredProperty element)
        {
            writer.WriteStartElement(Psf.ScoredProperty.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            Write(writer, element.Value);
            Write(writer, element.ParameterRef);

            foreach (var p in element.Properties)
            {
                Write(writer, p);
            }

            foreach (var sp in element.ScoredProperties)
            {
                Write(writer, sp);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Value value)
        {
            if (value == null) return;

            writer.WriteStartElement(Psf.Value.LocalName, Psf.Namespace.NamespaceName);
            writer.WriteStartAttribute(Xsi.Type.LocalName, Xsi.Namespace.NamespaceName);
            writer.WriteValue(value.ValueType.ToQName(writer));
            writer.WriteEndAttribute();

            if (value.ValueType == Xsd.Integer)
            {
                writer.WriteValue(value.AsInt().GetValueOrDefault());
            }
            else if (value.ValueType == Xsd.Decimal)
            {
                writer.WriteValue(value.AsFloat().GetValueOrDefault());
            }
            else if (value.ValueType == Xsd.QName)
            {
                writer.WriteValue(value.AsXName().ToQName(writer));
            }
            else
            {
                writer.WriteValue(value.AsString());
            }

            writer.WriteEndElement();
        }
    }


    internal static class NamespaceCollector
    {
        internal static HashSet<XNamespace> Collect(Capabilities pc)
        {
            var result = new HashSet<XNamespace>();
            Collect(result, pc);
            return result;
        }
        internal static HashSet<XNamespace> Collect(Ticket pt)
        {
            var result = new HashSet<XNamespace>();
            Collect(result, pt);
            return result;
        }

        private static void Collect(HashSet<XNamespace> result, Capabilities pc)
        {
            foreach (var f in pc.Features)
            {
                Collect(result, f);
            }

            foreach (var p in pc.Properties)
            {
                Collect(result, p);
            }

            foreach (var p in pc.Parameters)
            {
                Collect(result, p);
            }
        }

        private static void Collect(HashSet<XNamespace> result, Ticket pc)
        {
            foreach (var f in pc.Features())
            {
                Collect(result, f);
            }

            foreach (var p in pc.Properties())
            {
                Collect(result, p);
            }

            foreach (var p in pc.Parameters())
            {
                Collect(result, p);
            }
        }

        private static void Collect(HashSet<XNamespace> result, Feature feature)
        {
            result.AddXNamespaceOfXName(feature.Name);

            foreach (var o in feature.Options())
            {
                Collect(result, o);
            }
        }

        private static void Collect(HashSet<XNamespace> result, Option option)
        {
            result.AddXNamespaceOfXName(option.Name);
            result.AddXNamespaceOfXName(option.Constrained);

            foreach (var p in option.Properties)
            {
                Collect(result, p);
            }

            foreach (var sp in option.ScoredProperties)
            {
                Collect(result, sp);
            }
        }

        private static void Collect(HashSet<XNamespace> result, ParameterDef element)
        {
            result.AddXNamespaceOfXName(element.Name);

            foreach (var p in element.Properties)
            {
                Collect(result, p);
            }
        }

        private static void Collect(HashSet<XNamespace> result, ParameterInit element)
        {
            result.AddXNamespaceOfXName(element.Name);

            Collect(result, element.Value);
        }

        private static void Collect(HashSet<XNamespace> result, ParameterRef element)
        {
            if (element == null) return;

            result.AddXNamespaceOfXName(element.Name);
        }

        private static void Collect(HashSet<XNamespace> result, Property element)
        {
            result.AddXNamespaceOfXName(element.Name);

            Collect(result, element.Value);

            foreach (var p in element.Properties)
            {
                Collect(result, p);
            }
        }

        private static void Collect(HashSet<XNamespace> result, ScoredProperty element)
        {
            result.AddXNamespaceOfXName(element.Name);

            Collect(result, element.Value);
            Collect(result, element.ParameterRef);

            foreach (var p in element.Properties)
            {
                Collect(result, p);
            }

            foreach (var sp in element.ScoredProperties)
            {
                Collect(result, sp);
            }
        }

        private static void Collect(HashSet<XNamespace> result, Value value)
        {
            if (value == null) return;

            var valueType = value.ValueType;
            result.AddXNamespaceOfXName(valueType);

            if (value.ValueType == Xsd.QName)
            {
                result.AddXNamespaceOfXName(value.AsXName());
            }
        }

        private static void AddXNamespaceOfXName(this HashSet<XNamespace> set, XName name)
        {
            if (name == null) return;
            if (name.Namespace == null) return;
            set.Add(name.Namespace);
        }
    }
}
