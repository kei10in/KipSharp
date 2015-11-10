using System;
using System.Collections.Generic;
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
        private FeatureCollection _features = new FeatureCollection();
        private ParameterDefCollection _parameters = new ParameterDefCollection();
        private PropertyCollection _properties = new PropertyCollection();

        public Capabilities(params CapabilitiesChild[] elements)
        {
            foreach (var e in elements)
            {
                e.Apply(
                    onFeature: f => _features.Add(f),
                    onParameterDef: pd => _parameters.Add(pd),
                    onProperty: p => _properties.Add(p));
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
        private ElementHolder _holder;

        private CapabilitiesChild(ElementHolder holder) { _holder = holder; }

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
            return new CapabilitiesChild(new FeatureHolder { Element = element });
        }

        public static implicit operator CapabilitiesChild(ParameterDef element)
        {
            return new CapabilitiesChild(new ParameterDefHolder { Element = element });
        }

        public static implicit operator CapabilitiesChild(Property element)
        {
            return new CapabilitiesChild(new PropertyHolder { Element = element });
        }
    }

    /// <summary>
    /// Represents a PrintTicket document defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Ticket
    {
        private FeatureCollection _features = new FeatureCollection();
        private ParameterInitCollection _parameters = new ParameterInitCollection();
        private PropertyCollection _properties = new PropertyCollection();

        public Ticket(params TicketChild[] elements)
        {
            foreach (var e in elements)
            {
                e.Apply(
                    onFeature: x => _features.Add(x),
                    onParameterInit: x => _parameters.Add(x),
                    onProperty: x => _properties.Add(x));
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
        private ElementHolder _holder;

        private TicketChild(ElementHolder holder) { _holder = holder; }

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
            return new TicketChild(new FeatureHolder { Element = element });
        }

        public static implicit operator TicketChild(ParameterInit element)
        {
            return new TicketChild(new ParameterInitHolder { Element = element });
        }

        public static implicit operator TicketChild(Property element)
        {
            return new TicketChild(new PropertyHolder { Element = element });
        }
    }

    /// <summary>
    /// Represents an Feature element defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Feature
    {
        private PropertyCollection _properties = new PropertyCollection();
        private List<Option> _options = new List<Option>();
        private FeatureCollection _features = new FeatureCollection();

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
                    onProperty: x => _properties.Add(x),
                    onOption: x => _options.Add(x),
                    onFeature: x => _features.Add(x));
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
    }

    public sealed class FeatureChild
    {
        private ElementHolder _holder;

        private FeatureChild(ElementHolder holder) { _holder = holder; }

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
            return new FeatureChild(new PropertyHolder { Element = element });
        }

        public static implicit operator FeatureChild(Option element)
        {
            return new FeatureChild(new OptionHolder { Element = element });
        }

        public static implicit operator FeatureChild(Feature element)
        {
            return new FeatureChild(new FeatureHolder { Element = element });
        }
    }

    /// <summary>
    /// Represents an Option element defined in the Print Schema Specificaiton.
    /// </summary>
    public sealed class Option
    {
        private PropertyCollection _properties = new PropertyCollection();
        private ScoredPropertyCollection _scoredProperties = new ScoredPropertyCollection();

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
                    onProperty: x => _properties.Add(x),
                    onScoredProperty: x => _scoredProperties.Add(x));
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
    }

    public sealed class OptionChild
    {
        private ElementHolder _holder;

        private OptionChild(ElementHolder holder) { _holder = holder; }

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
            return new OptionChild(new PropertyHolder { Element = element });
        }

        public static implicit operator OptionChild(ScoredProperty element)
        {
            return new OptionChild(new ScoredPropertyHolder { Element = element });
        }
    }

    /// <summary>
    /// Represents a ParameterDef element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterDef
    {
        private PropertyCollection _properties = new PropertyCollection();

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
    }

    /// <summary>
    /// Represents a ParameterInit element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterInit
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
            _properties = new PropertyCollection(elements);
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

            var scoredProperties = new ScoredPropertyCollection();
            var properties = new PropertyCollection();
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
            _properties = new PropertyCollection(properties);
            _scoredProperties = new ScoredPropertyCollection(scoredProperties);
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
    }

    public sealed class ScoredPropertyChild
    {
        private ElementHolder _holder;

        private ScoredPropertyChild(ElementHolder holder) { _holder = holder; }

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
            return new ScoredPropertyChild(new PropertyHolder { Element = element });
        }

        public static implicit operator ScoredPropertyChild(ScoredProperty element)
        {
            return new ScoredPropertyChild(new ScoredPropertyHolder { Element = element });
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

    internal interface ElementHolder
    {
        void Apply(
            Action<Feature> onFeature = null,
            Action<Option> onOption = null,
            Action<ParameterDef> onParameterDef = null,
            Action<ParameterInit> onParameterInit = null,
            Action<ParameterRef> onParameterRef = null,
            Action<Property> onProperty = null,
            Action<ScoredProperty> onScoredProperty = null,
            Action<Value> onValue = null);
    }

    internal class FeatureHolder : ElementHolder
    {
        public Feature Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onFeature?.Invoke(Element);
        }
    }

    internal class OptionHolder : ElementHolder
    {
        public Option Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onOption?.Invoke(Element);
        }
    }

    internal class ParameterDefHolder : ElementHolder
    {
        public ParameterDef Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onParameterDef?.Invoke(Element);
        }
    }

    internal class ParameterInitHolder : ElementHolder
    {
        public ParameterInit Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onParameterInit?.Invoke(Element);
        }
    }

    internal class ParameterRefHolder : ElementHolder
    {
        public ParameterRef Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onParameterRef?.Invoke(Element);
        }
    }

    internal class PropertyHolder : ElementHolder
    {
        public Property Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onProperty?.Invoke(Element);
        }
    }

    internal class ScoredPropertyHolder : ElementHolder
    {
        public ScoredProperty Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onScoredProperty?.Invoke(Element);
        }
    }

    internal class ValueHolder : ElementHolder
    {
        public Value Element { get; set; }

        void ElementHolder.Apply(
            Action<Feature> onFeature,
            Action<Option> onOption,
            Action<ParameterDef> onParameterDef,
            Action<ParameterInit> onParameterInit,
            Action<ParameterRef> onParameterRef,
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty,
            Action<Value> onValue)
        {
            onValue?.Invoke(Element);
        }
    }
}