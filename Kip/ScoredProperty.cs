using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
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