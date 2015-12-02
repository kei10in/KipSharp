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
    }
}