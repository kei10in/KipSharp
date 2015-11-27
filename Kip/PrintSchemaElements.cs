using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a PrintCapabilities document defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Capabilities
    {
        private ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        private ImmutableNamedElementCollection<ParameterDef> _parameters
            = ImmutableNamedElementCollection.CreateParameterDefCollection();
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public Capabilities(params CapabilitiesChild[] elements)
        {
            foreach (var e in elements)
            {
                e.Apply(
                    onFeature: f =>
                    {
                        _features = _features.Add(f);
                    },
                    onParameterDef: pd =>
                    {
                        _parameters = _parameters.Add(pd);
                    },
                    onProperty: p =>
                    {
                        _properties = _properties.Add(p);
                    });
            }
        }

        internal Capabilities(
            ImmutableNamedElementCollection<Feature> features,
            ImmutableNamedElementCollection<ParameterDef> parameters,
            ImmutableNamedElementCollection<Property> properties)
        {
            _features = features;
            _parameters = parameters;
            _properties = properties;
        }

        public IEnumerable<Option> GetFeatureOptions(XName featureName)
        {
            var options = Feature(featureName)?.Options();
            return options ?? Enumerable.Empty<Option>();
        }

        public IEnumerable<Option> GetFeatureOptions(XName featureName, params XName[] nestedFeatureNames)
        {
            var ft = Feature(featureName);
            foreach (var nestedFeatureName in nestedFeatureNames)
            {
                ft = ft?.NestedFeature(nestedFeatureName);
            }
            var options = ft?.Options();
            return options ?? Enumerable.Empty<Option>();
        }

        public Capabilities Add(Feature feature)
        {
            return new Capabilities(_features.Add(feature), _parameters, _properties);
        }

        public IEnumerable<Feature> Features()
        {
            return _features;
        }

        public Feature Feature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }

        public Capabilities Add(ParameterDef parameter)
        {
            return new Capabilities(_features, _parameters.Add(parameter), _properties);
        }

        public IEnumerable<ParameterDef> Parameters()
        {
            return _parameters;
        }

        public ParameterDef Parameter(XName name)
        {
            return _parameters.FirstOrDefault(x => x.Name == name);
        }

        public Capabilities Add(Property property)
        {
            return new Capabilities(_features, _parameters, _properties.Add(property));
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(p => p.Name == name);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public void Save(System.IO.Stream stream)
        {
            using (var writer = XmlWriter.Create(stream))
            {
                Save(writer);
            }
        }

        public void Save(System.IO.TextWriter textWriter)
        {
            using (var writer = XmlWriter.Create(textWriter))
            {
                Save(writer);
            }
        }

        public void Save(XmlWriter writer)
        {
            PrintSchemaWriter.Write(writer, this);
        }

        public static Capabilities Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return Load(textReader);
            }
        }

        public static Capabilities Load(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        public static Capabilities Load(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        public static Capabilities Load(XmlReader reader)
        {
            return PrintSchemaReader.ReadCapabilities(reader);
        }
    }

    /// <summary>
    /// Wrapper class that representing a child elements of the
    /// PrintCapabilities document.
    /// </summary>
    public sealed class CapabilitiesChild
    {
        private Element _holder;

        private CapabilitiesChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Feature> onFeature,
            Action<ParameterDef> onParameterDef,
            Action<Property> onProperty)
        {
            _holder.Apply(
                onFeature: onFeature,
                onParameterDef: onParameterDef,
                onProperty: onProperty);
        }

        public static implicit operator CapabilitiesChild(Feature element)
        {
            return new CapabilitiesChild(element);
        }

        public static implicit operator CapabilitiesChild(ParameterDef element)
        {
            return new CapabilitiesChild(element);
        }

        public static implicit operator CapabilitiesChild(Property element)
        {
            return new CapabilitiesChild(element);
        }
    }

    /// <summary>
    /// Represents a PrintTicket document defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Ticket
    {
        private ImmutableNamedElementCollection<Feature> _features = ImmutableNamedElementCollection.CreateFeatureCollection();
        private ImmutableNamedElementCollection<ParameterInit> _parameters = ImmutableNamedElementCollection.CreateParameterInitCollection();
        private ImmutableNamedElementCollection<Property> _properties = ImmutableNamedElementCollection.CreatePropertyCollection();

        public Ticket(params TicketChild[] elements)
        {
            foreach (var e in elements)
            {
                e.Apply(
                    onFeature: x =>
                    {
                        _features = _features.Add(x);
                    },
                    onParameterInit: x =>
                    {
                        _parameters = _parameters.Add(x);
                    },
                    onProperty: x =>
                    {
                        _properties = _properties.Add(x);
                    });
            }
        }

        internal Ticket(
            ImmutableNamedElementCollection<Feature> features,
            ImmutableNamedElementCollection<ParameterInit> parameters,
            ImmutableNamedElementCollection<Property> properties)
        {
            _features = features;
            _parameters = parameters;
            _properties = properties;
        }

        public Ticket Add(Feature feature)
        {
            return new Ticket(
                _features.Add(feature),
                _parameters,
                _properties);
        }

        public IEnumerable<Feature> Features()
        {
            return _features;
        }

        public Feature Feature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }

        public Ticket Add(ParameterInit parameter)
        {
            return new Ticket(
                _features,
                _parameters.Add(parameter),
                _properties);
        }

        public IEnumerable<ParameterInit> Parameters()
        {
            return _parameters;
        }

        public ParameterInit ParameterInit(XName name)
        {
            return _parameters.FirstOrDefault(x => x.Name == name);
        }

        public Ticket Add(Property property)
        {
            return new Ticket(
                _features,
                _parameters,
                _properties.Add(property));
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(p => p.Name == name);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public void Save(System.IO.Stream stream)
        {
            using (var writer = XmlWriter.Create(stream))
            {
                Save(writer);
            }
        }

        public void Save(System.IO.TextWriter textWriter)
        {
            using (var writer = XmlWriter.Create(textWriter))
            {
                Save(writer);
            }
        }

        public void Save(XmlWriter writer)
        {
            PrintSchemaWriter.Write(writer, this);
        }

        public static Ticket Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return Load(textReader);
            }
        }

        public static Ticket Load(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadTicket(reader);
            }
        }

        public static Ticket Load(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadTicket(reader);
            }
        }

        public static Ticket Load(XmlReader reader)
        {
            return PrintSchemaReader.ReadTicket(reader);
        }
    }

    /// <summary>
    /// Wrapper class that representing a child elements of the PrintTicket
    /// document.
    /// </summary>
    public sealed class TicketChild
    {
        private Element _holder;

        private TicketChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Feature> onFeature,
            Action<ParameterInit> onParameterInit,
            Action<Property> onProperty)
        {
            _holder.Apply(
                onFeature: onFeature,
                onParameterInit: onParameterInit,
                onProperty: onProperty);
        }

        public static implicit operator TicketChild(Feature element)
        {
            return new TicketChild(element);
        }

        public static implicit operator TicketChild(ParameterInit element)
        {
            return new TicketChild(element);
        }

        public static implicit operator TicketChild(Property element)
        {
            return new TicketChild(element);
        }
    }

    /// <summary>
    /// Represents an Feature element defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Feature
    {
        private ImmutableNamedElementCollection<Property> _properties = ImmutableNamedElementCollection.CreatePropertyCollection();
        private ImmutableList<Option> _options = ImmutableList.Create<Option>();
        private ImmutableNamedElementCollection<Feature> _features = ImmutableNamedElementCollection.CreateFeatureCollection();

        public Feature(XName name)
        {
            Name = name;
        }

        public Feature(XName name, params FeatureChild[] elements)
        {
            Name = name;
            foreach (var e in elements)
            {
                e.Apply(
                    onProperty: x =>
                    {
                        _properties = _properties.Add(x);
                    },
                    onOption: x =>
                    {
                        _options = _options.Add(x);
                    },
                    onFeature: x =>
                    {
                        _features = _features.Add(x);
                    });
            }
        }

        internal Feature(
            XName name,
            ImmutableNamedElementCollection<Property> properties,
            ImmutableList<Option> options,
            ImmutableNamedElementCollection<Feature> nestedFeature)
        {
            Name = name;
            _properties = properties;
            _options = options;
            _features = nestedFeature;
        }

        public XName Name
        {
            get;
        }

        public Feature Add(Property property)
        {
            return new Feature(Name, _properties.Add(property), _options, _features);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        public Feature Add(Option option)
        {
            return new Feature(Name, _properties, _options.Add(option), _features);
        }

        public IEnumerable<Option> Options()
        {
            return _options;
        }

        public Feature Add(Feature feature)
        {
            return new Feature(Name, _properties, _options, _features.Add(feature));
        }

        public IEnumerable<Feature> NestedFeatures()
        {
            return _features;
        }

        public Feature NestedFeature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }
    }

    public sealed class FeatureChild
    {
        private Element _holder;

        private FeatureChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Property> onProperty,
            Action<Option> onOption,
            Action<Feature> onFeature)
        {
            _holder.Apply(
                onProperty: onProperty,
                onOption: onOption,
                onFeature: onFeature);
        }

        public static implicit operator FeatureChild(Property element)
        {
            return new FeatureChild(element);
        }

        public static implicit operator FeatureChild(Option element)
        {
            return new FeatureChild(element);
        }

        public static implicit operator FeatureChild(Feature element)
        {
            return new FeatureChild(element);
        }
    }

    /// <summary>
    /// Represents an Option element defined in the Print Schema Specificaiton.
    /// </summary>
    public sealed class Option
    {
        private ImmutableNamedElementCollection<Property> _properties = ImmutableNamedElementCollection.CreatePropertyCollection();
        private ImmutableNamedElementCollection<ScoredProperty> _scoredProperties = ImmutableNamedElementCollection.CreateScoredPropertyCollection();

        public Option(params OptionChild[] elements)
            : this(null, null, elements)
        {
        }

        public Option(XName name, params OptionChild[] elements)
            : this(name, null, elements)
        {
        }

        public Option(XName name, XName constrained, params OptionChild[] elements)
        {
            Name = name;
            Constrained = constrained;

            foreach (var e in elements)
            {
                e.Apply(
                    onProperty: x =>
                    {
                        _properties = _properties.Add(x);
                    },
                    onScoredProperty: x =>
                    {
                        _scoredProperties = _scoredProperties.Add(x);
                    });
            }
        }

        internal Option(
            XName name,
            XName constrained,
            ImmutableNamedElementCollection<Property> properties,
            ImmutableNamedElementCollection<ScoredProperty> scoredPropertis)
        {
            Name = name;
            Constrained = constrained;
            _properties = properties;
            _scoredProperties = scoredPropertis;
        }

        public XName Name
        {
            get;
        }

        public XName Constrained
        {
            get;
        }

        public Option SetConstrained(XName constrained)
        {
            return new Option(Name, constrained, _properties, _scoredProperties);
        }

        public Option Add(Property property)
        {
            return new Option(Name, Constrained, _properties.Add(property), _scoredProperties);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        public Option Add(ScoredProperty scoredProperty)
        {
            return new Option(Name, Constrained, _properties, _scoredProperties.Add(scoredProperty));
        }

        public IEnumerable<ScoredProperty> ScoredProperties()
        {
            return _scoredProperties;
        }

        public ScoredProperty ScoredProperty(XName name)
        {
            return _scoredProperties.FirstOrDefault(x => x.Name == name);
        }
    }

    public sealed class OptionChild
    {
        private Element _holder;

        private OptionChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty)
        {
            _holder.Apply(
                onProperty: onProperty,
                onScoredProperty: onScoredProperty);
        }

        public static implicit operator OptionChild(Property element)
        {
            return new OptionChild(element);
        }

        public static implicit operator OptionChild(ScoredProperty element)
        {
            return new OptionChild(element);
        }
    }

    /// <summary>
    /// Represents a ParameterDef element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterDef
    {
        private ImmutableNamedElementCollection<Property> _properties = ImmutableNamedElementCollection.CreatePropertyCollection();

        public ParameterDef(XName name, params Property[] properties)
        {
            Name = name;

            foreach (var p in properties)
            {
                Add(p);
            }
        }
        internal ParameterDef(XName name, ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            _properties = properties;
        }

        public XName Name
        {
            get;
        }

        public ParameterDef Add(Property property)
        {
            return new ParameterDef(Name, _properties);
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

    /// <summary>
    /// Represents a ParameterInit element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterInit
    {
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
            get;
        }

        public ParameterInit SetValue(Value value)
        {
            return new ParameterInit(Name, value);
        }
    }

    /// <summary>
    /// Represents a ParameterRef element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterRef
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

    /// <summary>
    /// Represents a Property element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class Property
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
            _properties = NamedElementCollection.CreatePropertyCollection(elements);
        }

        public XName Name
        {
            get;
        }

        public Value Value
        {
            get;
        }

        public IReadOnlyCollection<Property> NestedProperties()
        {
            return _properties;
        }

        public Property NestedProperty(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }
    }

    /// <summary>
    /// Represents a ScoredProeprty element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ScoredProperty
    {
        private IReadOnlyCollection<ScoredProperty> _scoredProperties;
        private IReadOnlyCollection<Property> _properties;

        public ScoredProperty(XName name, params ScoredPropertyChild[] elements)
        {
            Name = name;

            var scoredProperties = NamedElementCollection.CreateScoredPropertyCollection();
            var properties = NamedElementCollection.CreatePropertyCollection();
            foreach (var e in elements)
            {
                e.Apply(
                    onScoredProperty: x => scoredProperties.Add(x),
                    onProperty: x => properties.Add(x));
            }

            _scoredProperties = scoredProperties;
            _properties = properties;
        }

        public ScoredProperty(XName name, Value value, params ScoredPropertyChild[] elements)
            : this(name, elements)
        {
            Value = value;
        }

        public ScoredProperty(XName name, ParameterRef parameter, params ScoredPropertyChild[] elements)
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
            _properties = NamedElementCollection.CreatePropertyCollection(properties);
            _scoredProperties = NamedElementCollection.CreateScoredPropertyCollection(scoredProperties);
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

        public IEnumerable<ScoredProperty> NestedScoredProperties()
        {
            return _scoredProperties;
        }

        public ScoredProperty NestedScoredProperty(XName name)
        {
            return _scoredProperties.FirstOrDefault(x => x.Name == name);
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

    public sealed class ScoredPropertyChild
    {
        private Element _holder;

        private ScoredPropertyChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty)
        {
            _holder.Apply(
                onProperty: onProperty,
                onScoredProperty: onScoredProperty);
        }

        public static implicit operator ScoredPropertyChild(Property element)
        {
            return new ScoredPropertyChild(element);
        }

        public static implicit operator ScoredPropertyChild(ScoredProperty element)
        {
            return new ScoredPropertyChild(element);
        }
    }

    /// <summary>
    /// Represents a Value element defined in the Print Schema Specificaiton.
    /// </summary>
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

        public override string ToString()
        {
            return _value.ToString();
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