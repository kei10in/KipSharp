using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ParameterDef element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterDef
    {
        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public ParameterDef(XName name, params Property[] elements)
        {
            Name = name;

            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            foreach (var e in elements)
            {
                properties.Add(e);
            }
            _properties = properties.ToImmutable();
        }
        internal ParameterDef(XName name, ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            _properties = properties;
        }

        public XName Name
        {
            get;
        }

        public ParameterDef Add(Property property)
        {
            return new ParameterDef(Name, _properties);
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