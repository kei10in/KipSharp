using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Kip
{
    public static class PrintSchemaWriter
    {
        public static void Write(XmlWriter writer, Capabilities pc)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("psf", Psf.PrintCapabilities.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("version", "1");
            writer.WriteAttributeString("xmlns", "psf", null, Psf.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "psk", null, Psk.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsi", null, Xsi.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsd", null, Xsd.Url.NamespaceName);

            foreach (var f in pc.Features())
            {
                Write(writer, f);
            }

            foreach (var p in pc.Properties())
            {
                Write(writer, p);
            }

            foreach (var p in pc.Parameters())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
            writer.Flush();
        }

        public static void Write(XmlWriter writer, Ticket pc)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("psf", Psf.PrintTicket.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("version", "1");
            writer.WriteAttributeString("xmlns", "psf", null, Psf.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "psk", null, Psk.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsi", null, Xsi.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsd", null, Xsd.Url.NamespaceName);

            foreach (var f in pc.Features())
            {
                Write(writer, f);
            }

            foreach (var p in pc.Properties())
            {
                Write(writer, p);
            }

            foreach (var p in pc.Parameters())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
            writer.Flush();
        }

        private static void Write(XmlWriter writer, Feature feature)
        {
            writer.WriteStartElement(Psf.Feature.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("name", feature.Name.ToQName(writer));

            foreach (var o in feature.Options())
            {
                Write(writer, o);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Option option)
        {
            writer.WriteStartElement(Psf.Option.LocalName, Psf.Url.NamespaceName);
            if (option.Name != null)
                writer.WriteAttributeString("name", option.Name.ToQName(writer));

            foreach (var p in option.Properties())
            {
                Write(writer, p);
            }

            foreach (var sp in option.ScoredProperties())
            {
                Write(writer, sp);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ParameterDef element)
        {
            writer.WriteStartElement(Psf.ParameterDef.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            foreach (var p in element.Properties())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ParameterInit element)
        {
            writer.WriteStartElement(Psf.ParameterDef.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            Write(writer, element.Value);

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ParameterRef element)
        {
            if (element == null) return;

            writer.WriteStartElement(Psf.ParameterRef.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));
            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Property element)
        {
            writer.WriteStartElement(Psf.Property.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            Write(writer, element.Value);

            foreach (var p in element.SubProperties())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, ScoredProperty element)
        {
            writer.WriteStartElement(Psf.ScoredProperty.LocalName, Psf.Url.NamespaceName);
            writer.WriteAttributeString("name", element.Name.ToQName(writer));

            Write(writer, element.Value);
            Write(writer, element.ParameterRef);

            foreach (var p in element.SubProperties())
            {
                Write(writer, p);
            }

            foreach (var sp in element.SubScoredProperties())
            {
                Write(writer, sp);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Value value)
        {
            if (value == null) return;

            writer.WriteStartElement(Psf.Value.LocalName, Psf.Url.NamespaceName);
            writer.WriteStartAttribute(Xsi.Type.LocalName, Xsi.Url.NamespaceName);
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
}
