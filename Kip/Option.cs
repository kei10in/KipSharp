using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents an Option element defined in the Print Schema Specificaiton.
    /// </summary>
    public sealed class Option : IEquatable<Option>
    {
        /// <summary>
        /// Constructs with <see cref="Property"/>s and/or <see cref="ScoredProperty"/>s.
        /// </summary>
        public Option(params OptionChild[] elements)
            : this(null, null, elements)
        { }

        /// <summary>
        /// Constructs with the name and the children, <see cref="Property"/>s
        /// and/or <see cref="ScoredProperty"/>s.
        /// </summary>
        public Option(XName name, params OptionChild[] elements)
            : this(name, null, elements)
        { }

        /// <summary>
        /// Constructs with the name, the constrained value and children,
        /// <see cref="Property"/> and/or <see cref="ScoredProperty"/>.
        /// </summary>
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

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        private readonly ImmutableNamedElementCollection<ScoredProperty> _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollection();
        public IReadOnlyNamedElementCollection<ScoredProperty> ScoredProperties
        {
            get { return _scoredProperties; }
        }

        /// <summary>
        /// Set constrained value.
        /// </summary>
        /// <returns>A new Option with constrained value set.</returns>
        public Option SetConstrained(XName constrained)
        {
            return new Option(Name, constrained, _properties, _scoredProperties);
        }

        /// <summary>
        /// Add the specified element to the <see cref="Option"/>.
        /// </summary>
        /// <returns>A new Option with the element added.</returns>
        public Option Add(Property element)
        {
            return new Option(Name, Constrained, _properties.Add(element), _scoredProperties);
        }

        /// <summary>
        /// Add the specified element to the <see cref="Option"/>.
        /// </summary>
        /// <returns>A new Option with the element added.</returns>
        public Option Add(ScoredProperty element)
        {
            return new Option(Name, Constrained, _properties, _scoredProperties.Add(element));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Option);
        }

        public bool Equals(Option rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0 ^
                Constrained?.GetHashCode() ?? 0 ^
                _properties.GetHashCode() ^
                _scoredProperties.GetHashCode();
        }

        public static bool operator ==(Option v1, Option v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1.Constrained == v2.Constrained &&
                v1._properties == v2._properties &&
                v1._scoredProperties == v2._scoredProperties;
        }

        public static bool operator !=(Option v1, Option v2)
        {
            return !(v1 == v2);
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