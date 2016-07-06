using System;
using System.Diagnostics;

namespace Kip
{
    /// <summary>
    /// Represents a <see cref="ParameterDef"/> element defined in the
    /// PrintSchema Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ParameterDef")]
    public sealed class ParameterDef : IEquatable<ParameterDef>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterDef"/> class.
        /// </summary>
        /// <param name="name">Name of this element.</param>
        /// <param name="elements">Properties of this element.</param>
        public ParameterDef(ParameterName name, params Property[] elements)
        {
            Name = name;

            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            foreach (var e in elements)
            {
                properties.Add(e);
            }

            _properties = properties.ToImmutable();
        }

        internal ParameterDef(ParameterName name, ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            _properties = properties;
        }

        public ParameterName Name
        {
            get;
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Returns a property for a given name if exists in the element,
        /// otherwise throws exception.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The property with name if find, otherwise null.</returns>
        public Value this[PropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _properties[name].Value;
            }
        }

        /// <summary>
        /// Returns a property for a given name if exists in the element,
        /// otherwise returns null.
        /// </summary>
        /// <param name="name">The name to search for.</param>
        /// <returns>The property with name if find, otherwise null.</returns>
        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
        }

        /// <summary>
        /// Sets specified value to the specified the Property element by name.
        /// </summary>
        /// <param name="name">The name of the Property element.</param>
        /// <param name="value">The value to set to.</param>
        /// <returns>
        /// The new <see cref="ParameterDef"/> instance with new Property
        /// element.
        /// </returns>
        public ParameterDef Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var p = _properties.Get(name)?.Set(value)
                ?? new Property(name, value);
            return new ParameterDef(Name, _properties.SetItem(p));
        }

        /// <summary>
        /// Add the specified property to the <see cref="ParameterDef"/>.
        /// </summary>
        /// <param name="property">The Property element to add.</param>
        /// <returns>
        /// A new <see cref="ParameterDef"/> with the property added.
        /// </returns>
        public ParameterDef Add(Property property)
        {
            return new ParameterDef(Name, _properties);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ParameterDef);
        }

        public bool Equals(ParameterDef rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ _properties.GetHashCode();
        }

        public static bool operator ==(ParameterDef v1, ParameterDef v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1._properties == v2._properties;
        }

        public static bool operator !=(ParameterDef v1, ParameterDef v2)
        {
            return !(v1 == v2);
        }
    }
}
