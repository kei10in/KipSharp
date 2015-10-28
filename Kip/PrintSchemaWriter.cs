﻿using System;
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
        public static void Write(XmlWriter writer, PrintSchemaCapabilities pc)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("psf", psf.PrintCapabilities.LocalName, psf.Url.NamespaceName);
            writer.WriteAttributeString("version", "1");
            writer.WriteAttributeString("xmlns", "psf", null, psf.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "psk", null, psk.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsi", null, xsi.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsd", null, xsd.Url.NamespaceName);

            foreach (var p in pc.Properties())
            {
                Write(writer, p);
            }

            writer.WriteEndElement();
            writer.Flush();
        }

        private static void Write(XmlWriter writer, PrintSchemaProperty property)
        {
            writer.WriteStartElement(psf.Property.LocalName, psf.Url.NamespaceName);
            writer.WriteAttributeString("name", property.Name.ToQName(writer));

            Write(writer, property.Value);

            writer.WriteEndElement();
        }

        private static void Write(XmlWriter writer, PrintSchemaValue value)
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