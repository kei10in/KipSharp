using System.Xml.Linq;
using psf = Kip.PrintSchemaFramework;
using psk = Kip.PrintSchemaKeywords;
using xsi = Kip.XmlSchemaInstance;
using xsd = Kip.XmlSchema;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace Kip
{
    public class PrintSchemaCapabilities
    {
        List<PrintSchemaProperty> _properties;

        public PrintSchemaCapabilities()
        {
            _properties = new List<PrintSchemaProperty>();
        }

        public void Add(PrintSchemaProperty property)
        {
            _properties.Add(property);
        }

        public PrintSchemaProperty Property(XName name)
        {
            return _properties.FirstOrDefault(p => p.Name == name);
        }

        public IEnumerable<PrintSchemaProperty> Properties()
        {
            return _properties;
        }

        public void WriteTo(XmlWriter writer)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("psf", psf.PrintCapabilities.LocalName, psf.Url.NamespaceName);
            writer.WriteAttributeString("version", "1");
            writer.WriteAttributeString("xmlns", "psf", null, psf.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "psk", null, psk.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsi", null, xsi.Url.NamespaceName);
            writer.WriteAttributeString("xmlns", "xsd", null, xsd.Url.NamespaceName);

            writer.WriteEndElement();
            writer.Flush();
        }

        public static PrintSchemaCapabilities ReadFrom(XmlReader reader)
        {
            reader.Read();
            reader.ReadStartElement(psf.PrintCapabilities.LocalName, psf.Url.NamespaceName);

            var pc = new PrintSchemaCapabilities();

            if (reader.NodeType != XmlNodeType.None)
                reader.ReadEndElement();

            return pc;
        }
    }

    public class PrintSchemaFeature
    {
    }

    public class PrintSchemaOption
    {
    }

    public class PrintSchemaProperty
    {
        public PrintSchemaProperty(XName name, PrintSchemaValue value)
        {
            Name = name;
            Value = value;
        }

        public XName Name
        {
            get;
        }

        public PrintSchemaValue Value
        {
            get;
        }
    }

    public class PrintSchemaScoredProperty
    {

    }

    public sealed class PrintSchemaValue
    {
        object _value;

        public PrintSchemaValue(int? value)
        {
            _value = value.GetValueOrDefault();
        }

        public PrintSchemaValue(float? value)
        {
            _value = value.GetValueOrDefault();
        }

        public PrintSchemaValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                _value = string.Empty;
            else
                _value = value;
        }

        public PrintSchemaValue(XName value)
        {
            _value = value;
        }

        public XName ValueType
        {
            get
            {
                var type = _value.GetType();
                if (type == typeof(int))
                {
                    return xsd.Integer;
                }
                else if (type == typeof(float))
                {
                    return xsd.Decimal;
                }
                else if (type == typeof(XName))
                {
                    return xsd.QName;
                }
                else
                {
                    return xsd.String;
                }
            }
        }

        public XElement AsXElement()
        {
            var type = ValueType;

            var element = new XElement(
                psf.Value,
                new XAttribute(XNamespace.Xmlns + "psf", psf.Url),
                new XAttribute(XNamespace.Xmlns + "xsi", xsi.Url),
                new XAttribute(XNamespace.Xmlns + "xsd", xsd.Url));
            element.Add(new XAttribute(xsi.Type, type.ToQName(element)));

            if (type == xsd.QName)
            {
                var value = _value as XName;
                element.Add(value.ToQName(element));
            }
            else
            {
                element.Add(_value);
            }

            return element;
        }

        public int? AsInt()
        {
            return _value as int?;
        }

        public float? AsFloat()
        {
            return _value as float?;
        }

        public XName AsXName()
        {
            return _value as XName;
        }

        public string AsString()
        {
            return _value as string;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PrintSchemaValue);
        }

        public bool Equals(PrintSchemaValue value)
        {
            return this == value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static implicit operator PrintSchemaValue(int value)
        {
            return new PrintSchemaValue(value);
        }

        public static implicit operator PrintSchemaValue(float value)
        {
            return new PrintSchemaValue(value);
        }

        public static implicit operator PrintSchemaValue(XName value)
        {
            return new PrintSchemaValue(value);
        }

        public static implicit operator PrintSchemaValue(string value)
        {
            return new PrintSchemaValue(value);
        }

        public static bool operator ==(PrintSchemaValue v1, PrintSchemaValue v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            if (v1.ValueType != v2.ValueType) return false;

            var type = v1.ValueType;

            if (type == xsd.Integer) return v1.AsInt() == v2.AsInt();
            else if (type == xsd.Decimal) return v1.AsFloat() == v2.AsFloat();
            else if (type == xsd.QName) return v1.AsXName() == v2.AsXName();
            else return v1.AsString() == v2.AsString();
        }

        public static bool operator !=(PrintSchemaValue v1, PrintSchemaValue v2)
        {
            return !(v1 == v2);
        }
    }
}