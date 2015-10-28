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
    public class Capabilities
    {
        private List<Feature> _features = new List<Feature>();
        private List<Property> _properties = new List<Property>();

        public void Add(Feature feature)
        {
            _features.Add(feature);
        }

        public IEnumerable<Feature> Features()
        {
            return _features;
        }

        public Feature Feature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }

        public void Add(Property property)
        {
            _properties.Add(property);
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(p => p.Name == name);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public void WriteTo(XmlWriter writer)
        {
            PrintSchemaWriter.Write(writer, this);
        }

        public static Capabilities ReadFrom(XmlReader reader)
        {
            return PrintSchemaReader.Read(reader);
        }
    }

    public class Ticket
    {
    }

    public class Feature
    {
        private List<Option> _options = new List<Option>();
        private List<Property> _properties = new List<Property>();

        public Feature(XName name)
        {
            Name = name;
        }

        public XName Name
        {
            get;
        }

        public void Add(Property property)
        {
            _properties.Add(property);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        public void Add(Option option)
        {
            _options.Add(option);
        }

        public IEnumerable<Option> Options()
        {
            return _options;
        }
    }

    public class Option
    {
        public Option()
        {
        }

        public Option(XName name)
        {
            Name = name;
        }

        XName Name
        {
            get;
        }
    }

    public class ParameterDef
    {
    }

    public class ParameterInit
    {
    }

    public class ParameterRef
    {
    }

    public class Property
    {
        public Property(XName name, Value value = null)
        {
            Name = name;
            Value = value;
        }

        public XName Name
        {
            get;
        }

        public Value Value
        {
            get;
            set;
        }
    }

    public class ScoredProperty
    {

    }

    public sealed class Value
    {
        object _value;

        public Value(int? value)
        {
            _value = value.GetValueOrDefault();
        }

        public Value(float? value)
        {
            _value = value.GetValueOrDefault();
        }

        public Value(string value)
        {
            if (string.IsNullOrEmpty(value))
                _value = string.Empty;
            else
                _value = value;
        }

        public Value(XName value)
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
            return Equals(obj as Value);
        }

        public bool Equals(Value value)
        {
            return this == value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static implicit operator Value(int value)
        {
            return new Value(value);
        }

        public static implicit operator Value(float value)
        {
            return new Value(value);
        }

        public static implicit operator Value(XName value)
        {
            return new Value(value);
        }

        public static implicit operator Value(string value)
        {
            return new Value(value);
        }

        public static bool operator ==(Value v1, Value v2)
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

        public static bool operator !=(Value v1, Value v2)
        {
            return !(v1 == v2);
        }
    }
}