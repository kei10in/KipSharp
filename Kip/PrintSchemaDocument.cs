using System.Xml.Linq;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;

namespace Kip
{
    public class Capabilities
    {
        private FeatureContainer _features = new FeatureContainer();
        private ParameterDefContainer _parameters = new ParameterDefContainer();
        private PropertyContainer _properties = new PropertyContainer();

        public Capabilities(params AddableToCapabilities[] elements)
        {
            foreach (var e in elements)
            {
                e.AddTo(this);
            }
        }

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
            return _features.Get(name);
        }

        public void Add(ParameterDef parameter)
        {
            _parameters.Add(parameter);
        }

        public IEnumerable<ParameterDef> Parameters()
        {
            return _parameters;
        }

        public ParameterDef Parameter(XName name)
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

        public static Capabilities Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return ReadFrom(textReader);
            }
        }

        public static Capabilities ReadFrom(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        public static Capabilities ReadFrom(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }
    }

    public interface AddableToCapabilities
    {
        void AddTo(Capabilities capablities);
    }

    public class Ticket
    {
        private FeatureContainer _features = new FeatureContainer();
        private ParameterInitContainer _parameters = new ParameterInitContainer();
        private PropertyContainer _properties = new PropertyContainer();

        public Ticket(params AddableToTicket[] elements)
        {
            foreach (var e in elements)
            {
                e.AddTo(this);
            }
        }

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

    public interface AddableToTicket
    {
        void AddTo(Ticket ticket);
    }

    public class Feature : AddableToCapabilities, AddableToTicket, AddableToFeature
    {
        private PropertyContainer _properties = new PropertyContainer();
        private List<Option> _options = new List<Option>();
        private FeatureContainer _features = new FeatureContainer();

        public Feature(XName name)
        {
            Name = name;
        }

        public Feature(XName name, params AddableToFeature[] elements)
        {
            Name = name;
            foreach (var e in elements)
            {
                e.AddTo(this);
            }
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

        void AddableToCapabilities.AddTo(Capabilities capablities)
        {
            capablities.Add(this);
        }

        void AddableToTicket.AddTo(Ticket ticket)
        {
            ticket.Add(this);
        }

        void AddableToFeature.AddTo(Feature feature)
        {
            feature.Add(this);
        }
    }

    public interface AddableToFeature
    {
        void AddTo(Feature feature);
    }

    public class Option : AddableToFeature
    {
        private PropertyContainer _properties = new PropertyContainer();
        private ScoredPropertyContainer _scoredProperties = new ScoredPropertyContainer();

        public Option(params AddableToOption[] elements)
            : this(null, null, elements)
        {
        }

        public Option(XName name, params AddableToOption[] elements)
            : this(name, null, elements)
        {
        }

        public Option(XName name, XName constrained, params AddableToOption[] elements)
        {
            Name = name;
            Constrained = constrained;

            foreach (var e in elements)
            {
                e.AddTo(this);
            }
        }

        public XName Name
        {
            get;
        }

        public XName Constrained
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
            _scoredProperties.Add(scoredProperty);
        }

        public IEnumerable<ScoredProperty> ScoredProperties()
        {
            return _scoredProperties;
        }

        public ScoredProperty ScoredProperty(XName name)
        {
            return _scoredProperties.FirstOrDefault(x => x.Name == name);
        }

        void AddableToFeature.AddTo(Feature feature)
        {
            feature.Add(this);
        }
    }

    public interface AddableToOption
    {
        void AddTo(Option option);
    }

    public class ParameterDef : AddableToCapabilities
    {
        private List<Property> _properties = new List<Property>();

        public ParameterDef(XName name, params Property[] properties)
        {
            Name = name;

            foreach (var p in properties)
            {
                Add(p);
            }
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

        void AddableToCapabilities.AddTo(Capabilities capablities)
        {
            capablities.Add(this);
        }
    }

    public class ParameterInit : AddableToTicket
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

        void AddableToTicket.AddTo(Ticket ticket)
        {
            ticket.Add(this);
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
        : AddableToCapabilities
        , AddableToTicket
        , AddableToFeature
        , AddableToOption
        , AddableToScoredProperty
    {
        private IReadOnlyCollection<Property> _properties;

        public Property(XName name, params Property[] elements)
            : this(name, null, elements)
        {
        }

        public Property(XName name, Value value, params Property[] elements)
        {
            Name = name;
            Value = value;
            _properties = elements.ToList();
        }

        public XName Name
        {
            get;
        }

        public Value Value
        {
            get;
        }

        public IReadOnlyCollection<Property> SubProperties()
        {
            return _properties;
        }

        public Property SubProperty(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        void AddableToCapabilities.AddTo(Capabilities capablities)
        {
            capablities.Add(this);
        }

        void AddableToTicket.AddTo(Ticket ticket)
        {
            ticket.Add(this);
        }

        void AddableToFeature.AddTo(Feature feature)
        {
            feature.Add(this);
        }

        void AddableToOption.AddTo(Option option)
        {
            option.Add(this);
        }

        void AddableToScoredProperty.AddTo(ScoredPropertyChildren container)
        {
            container.Properties.Add(this);
        }
    }

    public class ScoredProperty : AddableToOption, AddableToScoredProperty
    {
        private IReadOnlyCollection<ScoredProperty> _scoredProperties;
        private IReadOnlyCollection<Property> _properties;

        public ScoredProperty(XName name, params AddableToScoredProperty[] elements)
        {
            Name = name;

            var container = new ScoredPropertyChildren();
            foreach (var e in elements)
            {
                e.AddTo(container);
            }

            _scoredProperties = container.ScoredProperties;
            _properties = container.Properties;
        }

        public ScoredProperty(XName name, Value value, params AddableToScoredProperty[] elements)
            : this(name, elements)
        {
            Value = value;
        }

        public ScoredProperty(XName name, ParameterRef parameter, params AddableToScoredProperty[] elements)
            : this(name, elements)
        {
            ParameterRef = parameter;
        }

        internal ScoredProperty(
            XName name,
            Value value,
            ParameterRef parameter,
            IEnumerable<ScoredProperty> scoredProperties,
            IEnumerable<Property> properties)
        {
            Name = name;
            Value = value;
            ParameterRef = parameter;
            _properties = new List<Property>(properties);
            _scoredProperties = new List<ScoredProperty>(scoredProperties);
        }

        public XName Name
        {
            get;
        }

        public Value Value
        {
            get;
        }

        public ParameterRef ParameterRef
        {
            get;
        }

        public IEnumerable<ScoredProperty> SubScoredProperties()
        {
            return _scoredProperties;
        }

        public ScoredProperty SubScoredProperty(XName name)
        {
            return _scoredProperties.FirstOrDefault(x => x.Name == name);
        }

        public IEnumerable<Property> SubProperties()
        {
            return _properties;
        }

        public Property SubProperty(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        void AddableToOption.AddTo(Option option)
        {
            option.Add(this);
        }

        void AddableToScoredProperty.AddTo(ScoredPropertyChildren container)
        {
            container.ScoredProperties.Add(this);
        }
    }

    public class ScoredPropertyChildren
    {
        public List<Property> Properties
        {
            get;
        } = new List<Property>();

        public List<ScoredProperty> ScoredProperties
        {
            get;
        } = new List<ScoredProperty>();
    }

    public interface AddableToScoredProperty
    {
        void AddTo(ScoredPropertyChildren container);
    }

    public sealed class Value
    {
        public static readonly Value Empty = new Value();

        object _value;

        private Value()
        {
            _value = null;
        }

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
                    return Xsd.Integer;
                }
                else if (type == typeof(float))
                {
                    return Xsd.Decimal;
                }
                else if (type == typeof(XName))
                {
                    return Xsd.QName;
                }
                else
                {
                    return Xsd.String;
                }
            }
        }

        public XElement AsXElement()
        {
            var type = ValueType;

            var element = new XElement(
                Psf.Value,
                new XAttribute(XNamespace.Xmlns + "psf", Psf.Url),
                new XAttribute(XNamespace.Xmlns + "xsi", Xsi.Url),
                new XAttribute(XNamespace.Xmlns + "xsd", Xsd.Url));
            element.Add(new XAttribute(Xsi.Type, type.ToQName(element)));

            if (type == Xsd.QName)
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

            if (type == Xsd.Integer) return v1.AsInt() == v2.AsInt();
            else if (type == Xsd.Decimal) return v1.AsFloat() == v2.AsFloat();
            else if (type == Xsd.QName) return v1.AsXName() == v2.AsXName();
            else return v1.AsString() == v2.AsString();
        }

        public static bool operator !=(Value v1, Value v2)
        {
            return !(v1 == v2);
        }
    }
}