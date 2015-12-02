using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ScoredProeprty element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ScoredProperty
    {
        private readonly ImmutableNamedElementCollection<ScoredProperty> _scoredProperties;
        private readonly ImmutableNamedElementCollection<Property> _properties;

        public ScoredProperty(XName name, params ScoredPropertyChild[] elements)
        {
            Name = name;

            var scoredProperties = ImmutableNamedElementCollection.CreateScoredPropertyCollectionBuilder();
            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            foreach (var e in elements)
            {
                e.Apply(
                    onScoredProperty: x => scoredProperties.Add(x),
                    onProperty: x => properties.Add(x));
            }

            _scoredProperties = scoredProperties.ToImmutable();
            _properties = properties.ToImmutable();
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
            ImmutableNamedElementCollection<ScoredProperty> scoredProperties,
            ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            Value = value;
            ParameterRef = parameter;
            _scoredProperties = scoredProperties;
            _properties = properties;
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

        public override bool Equals(object obj)
        {
            return Equals(obj as ScoredProperty);
        }

        public bool Equals(ScoredProperty rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                Value?.GetHashCode() ?? 0 ^
                ParameterRef?.GetHashCode() ?? 0 ^
                _scoredProperties.GetHashCode() ^
                _properties.GetHashCode();
        }

        public static bool operator ==(ScoredProperty v1, ScoredProperty v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1.Value == v2.Value &&
                v1.ParameterRef == v2.ParameterRef &&
                v1._scoredProperties == v2._scoredProperties &&
                v1._properties == v2._properties;
        }

        public static bool operator !=(ScoredProperty v1, ScoredProperty v2)
        {
            return !(v1 == v2);
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
}