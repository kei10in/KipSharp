using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a PrintTicket document defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Ticket
    {
        private readonly ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        private readonly ImmutableNamedElementCollection<ParameterInit> _parameters
            = ImmutableNamedElementCollection.CreateParameterInitCollection();
        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public Ticket(params TicketChild[] elements)
        {
            var features = ImmutableNamedElementCollection.CreateFeatureCollectionBuilder();
            var parameters = ImmutableNamedElementCollection.CreateParameterInitCollectionBuilder();
            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();

            foreach (var e in elements)
            {
                e.Apply(
                    onFeature: x => features.Add(x),
                    onParameterInit: x => parameters.Add(x),
                    onProperty: x => properties.Add(x));
            }

            _features = features.ToImmutable();
            _parameters = parameters.ToImmutable();
            _properties = properties.ToImmutable();
        }

        internal Ticket(
            ImmutableNamedElementCollection<Feature> features,
            ImmutableNamedElementCollection<ParameterInit> parameters,
            ImmutableNamedElementCollection<Property> properties)
        {
            _features = features;
            _parameters = parameters;
            _properties = properties;
        }

        public Ticket Add(Feature feature)
        {
            return new Ticket(
                _features.Add(feature),
                _parameters,
                _properties);
        }

        public IEnumerable<Feature> Features()
        {
            return _features;
        }

        public Feature Feature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
        }

        public Ticket Add(ParameterInit parameter)
        {
            return new Ticket(
                _features,
                _parameters.Add(parameter),
                _properties);
        }

        public IEnumerable<ParameterInit> Parameters()
        {
            return _parameters;
        }

        public ParameterInit ParameterInit(XName name)
        {
            return _parameters.FirstOrDefault(x => x.Name == name);
        }

        public Ticket Add(Property property)
        {
            return new Ticket(
                _features,
                _parameters,
                _properties.Add(property));
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(p => p.Name == name);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public void Save(System.IO.Stream stream)
        {
            using (var writer = XmlWriter.Create(stream))
            {
                Save(writer);
            }
        }

        public void Save(System.IO.TextWriter textWriter)
        {
            using (var writer = XmlWriter.Create(textWriter))
            {
                Save(writer);
            }
        }

        public void Save(XmlWriter writer)
        {
            PrintSchemaWriter.Write(writer, this);
        }

        public static Ticket Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return Load(textReader);
            }
        }

        public static Ticket Load(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadTicket(reader);
            }
        }

        public static Ticket Load(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadTicket(reader);
            }
        }

        public static Ticket Load(XmlReader reader)
        {
            return PrintSchemaReader.ReadTicket(reader);
        }
    }

    /// <summary>
    /// Wrapper class that representing a child elements of the PrintTicket
    /// document.
    /// </summary>
    public sealed class TicketChild
    {
        private Element _holder;

        private TicketChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Feature> onFeature,
            Action<ParameterInit> onParameterInit,
            Action<Property> onProperty)
        {
            _holder.Apply(
                onFeature: onFeature,
                onParameterInit: onParameterInit,
                onProperty: onProperty);
        }

        public static implicit operator TicketChild(Feature element)
        {
            return new TicketChild(element);
        }

        public static implicit operator TicketChild(ParameterInit element)
        {
            return new TicketChild(element);
        }

        public static implicit operator TicketChild(Property element)
        {
            return new TicketChild(element);
        }
    }
}