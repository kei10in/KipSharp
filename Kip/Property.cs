using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a Property element defined in the Print Schema
    /// Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: Property")]
    public sealed class Property : IEquatable<Property>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="name">Name of this element.</param>
        /// <param name="elements">Nested Properties.</param>
        public Property(PropertyName name, params Property[] elements)
            : this(name, null, elements)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="name">Name of this element.</param>
        /// <param name="value">Value element contained by this element</param>
        /// <param name="elements">Nested Properties</param>
        public Property(PropertyName name, Value value, params Property[] elements)
        {
            Name = name;
            Value = value;

            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            foreach (var e in elements)
            {
                properties.Add(e);
            }

            _properties = properties.ToImmutable();
        }

        internal Property(
            PropertyName name,
            Value value,
            ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            Value = value;
            _properties = properties;
        }

        public PropertyName Name
        {
            get;
        }

        public Value Value
        {
            get;
        }

        /// <summary>
        /// Updates value.
        /// </summary>
        /// <param name="value">New value.</param>
        /// <returns>New instance of Property.</returns>
        public Property Set(Value value)
        {
            return new Property(Name, value, _properties);
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        public Value this[PropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _properties[name].Value;
            }
        }

        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
        }

        public Property Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var p = _properties.Get(name) ?? new Property(name);
            return new Property(Name, value, _properties.SetItem(p));
        }

        /// <summary>
        /// Adds the specified element to the <see cref="Property"/>.
        /// </summary>
        /// <param name="property">The property to add to this instance.</param>
        /// <returns>A new Property with the element added.</returns>
        public Property Add(Property property)
        {
            return new Property(Name, Value, _properties.Add(property));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Property);
        }

        public bool Equals(Property rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                Value?.GetHashCode() ?? 0 ^
                _properties.GetHashCode();
        }

        public static bool operator ==(Property v1, Property v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1.Value == v2.Value &&
                v1._properties == v2._properties;
        }

        public static bool operator !=(Property v1, Property v2)
        {
            return !(v1 == v2);
        }

        public override string ToString()
        {
            var result = new List<string>() { "Property" };
            result.Add($"name=\"{Name}\"");
            if (Value != null)
                result.Add($"value={Value}");
            if (_properties.Any())
                result.Add($"nested properties={_properties.Count}");
            return string.Join(" ", result);
        }
    }
}