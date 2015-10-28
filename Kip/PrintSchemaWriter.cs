using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using psf = Kip.PrintSchemaFramework;
using psk = Kip.PrintSchemaKeywords;
using xsi = Kip.XmlSchemaInstance;
using xsd = Kip.XmlSchema;

namespace Kip
{
    public static class PrintSchemaWriter
    {
        public static void Write(XmlWriter writer, Capabilities pc)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("psf", psf.PrintCapabilities.LocalName, psf.Url.NamespaceName);
            writer.WriteAttributeString("version", "1");
            writer.WriteAttributeString("xmlns", "psf", null, psf.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "psk", null, psk.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsi", null, xsi.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsd", null, xsd.Url.NamespaceName);

            foreach (var f in pc.Features())
            {
                Write(writer, f);
            }

            foreach (var p in pc.Properties())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
            writer.Flush();
        }

        private static void Write(XmlWriter writer, Feature feature)
        {
            writer.WriteStartElement(psf.Feature.LocalName, psf.Url.NamespaceName);
            writer.WriteAttributeString("name", feature.Name.ToQName(writer));

            foreach (var o in feature.Options())
            {
                Write(writer, o);
            }

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Option option)
        {
            writer.WriteStartElement(psf.Option.LocalName, psf.Url.NamespaceName);
            if (option.Name != null)
                writer.WriteAttributeString("name", option.Name.ToQName(writer));

            // TODO: Property
            // TODO: ScoredProperty
            
            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Property property)
        {
            writer.WriteStartElement(psf.Property.LocalName, psf.Url.NamespaceName);
            writer.WriteAttributeString("name", property.Name.ToQName(writer));

            Write(writer, property.Value);

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, Value value)
        {
            writer.WriteStartElement(psf.Value.LocalName, psf.Url.NamespaceName);
            writer.WriteStartAttribute(xsi.Type.LocalName, xsi.Url.NamespaceName);
            writer.WriteValue(value.ValueType.ToQName(writer));
            writer.WriteEndAttribute();

            if (value.ValueType == xsd.Integer)
            {
                writer.WriteValue(value.AsInt().GetValueOrDefault());
            }
            else if (value.ValueType == xsd.Decimal)
            {
                writer.WriteValue(value.AsFloat().GetValueOrDefault());
            }
            else if (value.ValueType == xsd.QName)
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
