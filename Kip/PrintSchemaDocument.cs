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
        private List<ParameterDef> _parameters = new List<ParameterDef>();
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

        public void Add(ParameterDef parameter)
        {
            _parameters.Add(parameter);
        }

        public IEnumerable<ParameterDef> Parameters()
        {
            return _parameters;
        }

        public ParameterDef ParameterDef(XName name)
        {
            return _parameters.FirstOrDefault(x => x.Name == name);
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
        private List<Feature> _features = new List<Feature>();
        private List<ParameterInit> _parameters = new List<ParameterInit>();
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

        public void Add(ParameterInit parameter)
        {
            _parameters.Add(parameter);
        }

        public IEnumerable<ParameterInit> Parameters()
        {
            return _parameters;
        }

        public ParameterInit ParameterInit(XName name)
        {
            return _parameters.FirstOrDefault(x => x.Name == name);
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
    }

    public class Feature
    {
        private List<Property> _properties = new List<Property>();
        private List<Option> _options = new List<Option>();
        private List<Feature> _features = new List<Feature>();

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

        public void Add(Feature feature)
        {
            _features.Add(feature);
        }

        public IEnumerable<Feature> SubFeatures()
        {
            return _features;
        }

        public Feature SubFeature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }
    }

    public sealed class Constraint
    {
        internal Constraint(XName value)
        {
            Value = value;
        }

        public XName Value
        {
            get;
        }

        public override bool Equals(object rhs)
        {
            return Equals(rhs as Constraint);
        }

        public bool Equals(Constraint rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Constraint lhs, Constraint rhs)
        {
            if (ReferenceEquals(lhs, rhs)) return true;
            if ((object)lhs == null || (object)rhs == null) return false;

            return lhs.Value == rhs.Value;
        }

        public static bool operator !=(Constraint lhs, Constraint rhs)
        {
            return !(lhs == rhs);
        }
    }

    public static class Constraints
    {
        public static readonly Constraint None = new Constraint(psk.None);
        public static readonly Constraint PrintTicket = new Constraint(psk.PrintTicketSettings);
        public static readonly Constraint Admin = new Constraint(psk.AdminSettings);
        public static readonly Constraint Device = new Constraint(psk.DeviceSettings);
    }


    public class Option
    {
        private List<Property> _properties = new List<Property>();
        private List<ScoredProperty> _scoredProperty = new List<ScoredProperty>();

        public Option()
        {
        }

        public Option(XName name)
        {
            Name = name;
        }

        public Option(XName name, Constraint constrained)
        {
            Name = name;
            Constrained = constrained;
        }

        public XName Name
        {
            get;
        }

        public Constraint Constrained
        {
            get; set;
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

        public void Add(ScoredProperty scoredProperty)
        {
            _scoredProperty.Add(scoredProperty);
        }

        public IEnumerable<ScoredProperty> ScoredProperties()
        {
            return _scoredProperty;
        }

        public ScoredProperty ScoredProperty(XName name)
        {
            return _scoredProperty.FirstOrDefault(x => x.Name == name);
        }
    }

    public class ParameterDef
    {
        private List<Property> _properties = new List<Property>();

        public ParameterDef(XName name)
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
    }

    public class ParameterInit
    {
        public ParameterInit(XName name)
            : this(name, null)
        {
        }

        public ParameterInit(XName name, Value value)
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
            get; set;
        }
    }

    public class ParameterRef
    {
        public ParameterRef(XName name)
        {
            Name = name;
        }

        public XName Name
        {
            get;
        }
    }

    public class Property
    {
        private List<Property> _properties = new List<Property>();

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

        public void Add(Property property)
        {
            _properties.Add(property);
        }

        public IEnumerable<Property> SubProperties()
        {
            return _properties;
        }

        public Property SubProperty(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }
    }

    public class ScoredProperty
    {
        private List<ScoredProperty> _scoredProperties = new List<ScoredProperty>();
        private List<Property> _properties = new List<Property>();

        public ScoredProperty(XName name)
        {
            Name = name;
        }

        public XName Name
        {
            get;
        }

        public Value Value
        {
            get; set;
        }

        public ParameterRef ParmaeterRef
        {
            get; set;
        }

        public void Add(ScoredProperty scoredProperty)
        {
            _scoredProperties.Add(scoredProperty);
        }

        public IEnumerable<ScoredProperty> SubScoredProperty()
        {
            return _scoredProperties;
        }

        public ScoredProperty SubScoredProperty(XName name)
        {
            return _scoredProperties.FirstOrDefault(x => x.Name == name);
        }

        public void Add(Property property)
        {
            _properties.Add(property);
        }

        public IEnumerable<Property> SubProperty()
        {
            return _properties;
        }

        public Property SubProperty(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }
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