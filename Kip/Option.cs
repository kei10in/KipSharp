using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents an Option element defined in the Print Schema Specificaiton.
    /// </summary>
    public sealed class Option
    {
        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        private readonly ImmutableNamedElementCollection<ScoredProperty> _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollection();

        public Option(params OptionChild[] elements)
            : this(null, null, elements)
        { }

        public Option(XName name, params OptionChild[] elements)
            : this(name, null, elements)
        { }

        public Option(XName name, XName constrained, params OptionChild[] elements)
        {
            Name = name;
            Constrained = constrained;

            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            var scoredProperties = ImmutableNamedElementCollection.CreateScoredPropertyCollectionBuilder();

            foreach (var e in elements)
            {
                e.Apply(
                    onProperty: x => properties.Add(x),
                    onScoredProperty: x => scoredProperties.Add(x));
            }

            _properties = properties.ToImmutable();
            _scoredProperties = scoredProperties.ToImmutable();
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
}