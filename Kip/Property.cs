using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a Property element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class Property
    {
        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public Property(XName name, params Property[] elements)
            : this(name, null, elements)
        {
        }

        public Property(XName name, Value value, params Property[] elements)
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
            XName name,
            Value value,
            ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            Value = value;
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

        public IReadOnlyCollection<Property> NestedProperties()
        {
            return _properties;
        }

        public Property NestedProperty(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Property);
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
    }
}