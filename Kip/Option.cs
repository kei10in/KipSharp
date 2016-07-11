using System;
using System.Xml.Linq;
using Kip.Helper;

namespace Kip
{
    /// <summary>
    /// Represents an Option element defined in the Print Schema Specification.
    /// </summary>
    public sealed class Option : IEquatable<Option>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        /// <param name="elements">Children of the Option.</param>
        public Option(params OptionChild[] elements)
            : this(null, null, elements)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        /// <param name="name">Name of the Option.</param>
        /// <param name="elements">Children of the Option.</param>
        public Option(XName name, params OptionChild[] elements)
            : this(name, null, elements)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Option"/> class.
        /// </summary>
        /// <param name="name">Name of the Option.</param>
        /// <param name="constrained">Constraint value.</param>
        /// <param name="elements">Children of the Option.</param>
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

        /// <summary>
        /// Set constrained value.
        /// </summary>
        /// <param name="constrained">Constrained value to set.</param>
        /// <returns>A new Option with constrained value set.</returns>
        public Option SetConstrained(XName constrained)
        {
            return new Option(Name, constrained, _properties, _scoredProperties);
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Gets the value of the Property specified name. If no property found
        /// throws exception.
        /// </summary>
        /// <param name="name">The name of Property to get the value.</param>
        /// <returns>Value of the specified Property.</returns>
        public Value this[PropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _properties[name].Value;
            }
        }

        /// <summary>
        /// Gets the value of the Property specified name. If no property found
        /// returns null.
        /// </summary>
        /// <param name="name">The name of Property to get the value.</param>
        /// <returns>
        /// Value of the specified Property if found, otherwise null.
        /// </returns>
        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
        }

        /// <summary>
        /// Sets the value to the Property specified name.
        /// </summary>
        /// <param name="name">The name of the Property to set value.</param>
        /// <param name="value">The new value of the Property.</param>
        /// <returns>The new instance with new Property value.</returns>
        public Option Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var p = _properties.Get(name)?.Set(value) ?? new Property(name, value);
            return new Option(Name, Constrained, _properties.SetItem(p), _scoredProperties);
        }

        /// <summary>
        /// Removes the Property specified name.
        /// </summary>
        /// <param name="name">The name of the Property to remove.</param>
        /// <returns>The new instance not contained the Property.</returns>
        public Option Remove(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return new Option(Name, Constrained, _properties.Remove(name), _scoredProperties);
        }

        /// <summary>
        /// Gets the value of the nested Property specified name. Throws
        /// exception when specified Property not found.
        /// </summary>
        /// <param name="name1">The name of the Property.</param>
        /// <param name="name2">The name of the nested Property.</param>
        /// <returns>The value of nested Property.</returns>
        public Value this[PropertyName name1, PropertyName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));
                return _properties[name1][name2];
            }
        }

        /// <summary>
        /// Gets the value of the nested Property specified name.
        /// </summary>
        /// <param name="name1">The name of the Property.</param>
        /// <param name="name2">The name of the nested Property.</param>
        /// <returns>
        /// The value of nested Property. If nested Property not found, returns null.
        /// </returns>
        public Value Get(PropertyName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            return _properties.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Sets the value to the Property specified name.
        /// </summary>
        /// <param name="name1">The name of Property.</param>
        /// <param name="name2">The name of nested Property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>
        /// The new instance of <see cref="Property"/> with new nested Property set.
        /// </returns>
        public Option Set(PropertyName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            var p = _properties.Get(name1)?.Set(name2, value)
                ?? new Property(name1, new Property(name2, value));
            return new Option(Name, Constrained, _properties.SetItem(p), _scoredProperties);
        }

        private readonly ImmutableNamedElementCollection<ScoredProperty> _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollection();

        public IReadOnlyNamedElementCollection<ScoredProperty> ScoredProperties
        {
            get { return _scoredProperties; }
        }

        /// <summary>
        /// Gets the value of <see cref="ScoredProperty"/> specified name. If
        /// specified ScoredProperty not found, throws exception.
        /// </summary>
        /// <param name="name">The name of <see cref="ScoredProperty"/>.</param>
        /// <returns>The value of specified ScoredProperty.</returns>
        public ValueOrParameterRef this[ScoredPropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _scoredProperties[name].ValueOrParameterRef;
            }
        }

        /// <summary>
        /// Gets the value of <see cref="ScoredProperty"/> specified name.
        /// </summary>
        /// <param name="name">The name of <see cref="ScoredProperty"/>.</param>
        /// <returns>
        /// The value of specified ScoredProperty if found, otherwise return null.
        /// </returns>
        public ValueOrParameterRef Get(ScoredPropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _scoredProperties.Get(name)?.ValueOrParameterRef;
        }

        /// <summary>
        /// Gets the value of nested <see cref="ScoredProperty"/> specified name.
        /// </summary>
        /// <param name="name1">The name of <see cref="ScoredProperty"/>.</param>
        /// <param name="name2">The name of nested <see cref="ScoredProperty"/>.</param>
        /// <returns>The value of specified ScoredProperty.</returns>
        public ValueOrParameterRef this[ScoredPropertyName name1, ScoredPropertyName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));
                return _scoredProperties[name1][name2];
            }
        }

        /// <summary>
        /// Gets the value of nested <see cref="ScoredProperty"/> specified name.
        /// </summary>
        /// <param name="name1">The name of <see cref="ScoredProperty"/>.</param>
        /// <param name="name2">The name of nested <see cref="ScoredProperty"/>.</param>
        /// <returns>The value of specified ScoredProperty.</returns>
        public ValueOrParameterRef Get(ScoredPropertyName name1, ScoredPropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            return _scoredProperties.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Add the specified element to the <see cref="Option"/>.
        /// </summary>
        /// <param name="element">The <see cref="Option"/> to add.</param>
        /// <returns>A new Option with the element added.</returns>
        public Option Add(Property element)
        {
            return new Option(Name, Constrained, _properties.Add(element), _scoredProperties);
        }

        /// <summary>
        /// Add the specified element to the <see cref="Option"/>.
        /// </summary>
        /// <param name="element">The <see cref="ScoredProperty"/> to add.</param>
        /// <returns>A new Option with the element added.</returns>
        public Option Add(ScoredProperty element)
        {
            return new Option(Name, Constrained, _properties, _scoredProperties.Add(element));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Option);
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
}
