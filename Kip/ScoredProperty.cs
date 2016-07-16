using System;
using System.Diagnostics;

using Kip.Helper;

namespace Kip
{
    /// <summary>
    /// Represents a <see cref="ScoredProeprty"/> element defined in the Print
    /// Schema Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ScoredProperty")]
    public sealed class ScoredProperty : IEquatable<ScoredProperty>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScoredProperty"/> class.
        /// </summary>
        /// <param name="name">Name of ScoredProperty element.</param>
        /// <param name="elements">Child Property and/or ScoredProperty.</param>
        public ScoredProperty(ScoredPropertyName name, params ScoredPropertyChild[] elements)
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoredProperty"/> class.
        /// </summary>
        /// <param name="name">Name of the ScoredProperty.</param>
        /// <param name="value">Value of the ScoredProperty.</param>
        /// <param name="elements">Child Property and/or ScoredProperty.</param>
        public ScoredProperty(ScoredPropertyName name, Value value, params ScoredPropertyChild[] elements)
            : this(name, elements)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoredProperty"/> class.
        /// </summary>
        /// <param name="name">Name of the ScoredProperty.</param>
        /// <param name="parameter">ParameterRef element of the ScoredProperty.</param>
        /// <param name="elements">Child Property and/or ScoredProperty.</param>
        public ScoredProperty(ScoredPropertyName name, ParameterRef parameter, params ScoredPropertyChild[] elements)
            : this(name, elements)
        {
            ParameterRef = parameter;
        }

        internal ScoredProperty(
            ScoredPropertyName name,
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

        public ScoredPropertyName Name
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

        public ValueOrParameterRef ValueOrParameterRef
        {
            get
            {
                if (Value != null) return new ValueOrParameterRef(Value);
                if (ParameterRef != null) return new ValueOrParameterRef(ParameterRef);
                else return null;
            }
        }

        private readonly ImmutableNamedElementCollection<ScoredProperty> _scoredProperties;

        public IReadOnlyNamedElementCollection<ScoredProperty> ScoredProperties
        {
            get { return _scoredProperties; }
        }

        public ValueOrParameterRef Get(ScoredPropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _scoredProperties.Get(name)?.ValueOrParameterRef;
        }

        private readonly ImmutableNamedElementCollection<Property> _properties;

        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Adds the specified element to the <see cref="ScoredProperty"/>.
        /// </summary>
        /// <param name="element">The ScoredProperty to add.</param>
        /// <returns>A new ScoredProperty with the element added.</returns>
        public ScoredProperty Add(ScoredProperty element)
        {
            return new ScoredProperty(
                Name,
                Value,
                ParameterRef,
                _scoredProperties.Add(element),
                _properties);
        }

        /// <summary>
        /// Adds the specified element to the <see cref="ScoredProperty"/>.
        /// </summary>
        /// <param name="property">The Property to add.</param>
        /// <returns>A new ScoredProperty with the element added.</returns>
        public ScoredProperty Add(Property property)
        {
            return new ScoredProperty(
                Name,
                Value,
                ParameterRef,
                _scoredProperties,
                _properties.Add(property));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ScoredProperty);
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
}
