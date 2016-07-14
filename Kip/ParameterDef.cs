using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ParameterDef element defined in the Print Schema
    /// Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ParameterDef")]
    public sealed class ParameterDef : IEquatable<ParameterDef>
    {

        #region Constructors

        /// <summary>
        /// Constructs with name and the <see cref="Property"/>s.
        /// </summary>
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

        #endregion

        public ParameterName Name
        {
            get;
        }

        #region Properties

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

        public ParameterDef Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            var p = _properties.Get(name)?.Set(value)
                ?? new Property(name, value);
            return new ParameterDef(Name, _properties.SetItem(p));
        }

        #endregion

        /// <summary>
        /// Add the specified property to the <see cref="ParameterDef"/>.
        /// </summary>
        /// <returns>A new ParameterDef with the property added.</returns>
        public ParameterDef Add(Property property)
        {
            return new ParameterDef(Name, _properties);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParameterDef);
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