using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
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
    }
}