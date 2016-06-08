using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a PrintTicket document defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Ticket : IEquatable<Ticket>
    {
        /// <summary>
        /// Constructs with children: <see cref="Feature"/>,
        /// <see cref="ParameterInit"/> and/or <see cref="Property"/>.
        /// </summary>
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
            _declaredNamespaces = NamespaceDeclarationCollection.Default;
        }

        internal Ticket(
            ImmutableNamedElementCollection<Feature> features,
            ImmutableNamedElementCollection<ParameterInit> parameters,
            ImmutableNamedElementCollection<Property> properties,
            NamespaceDeclarationCollection namespaceDeclarations)
        {
            _features = features;
            _parameters = parameters;
            _properties = properties;
            _declaredNamespaces = namespaceDeclarations;
        }

        private readonly ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        public IReadOnlyNamedElementCollection<Feature> Features()
        {
            return _features;
        }

        public IReadOnlyList<Option> this[FeatureName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _features[name].Options();
            }
        }

        /// <summary>
        /// Set an option to the Feature specified by name.
        /// </summary>
        /// <param name="name">The name of Feature to set.</param>
        /// <param name="selection">An option to set to the Feature.</param>
        /// <returns>A new Ticket with the option set.</returns>
        public Ticket Set(FeatureName name, Option selection)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            var ft = _features.Contains(name)
                ? _features[name].Set(selection)
                : new Feature(name, selection);

            return new Ticket(_features.SetItem(ft), _parameters, _properties, _declaredNamespaces);
        }

        private readonly ImmutableNamedElementCollection<ParameterInit> _parameters
            = ImmutableNamedElementCollection.CreateParameterInitCollection();
        public IReadOnlyNamedElementCollection<ParameterInit> Parameters()
        {
            return _parameters;
        }

        public Value this[ParameterName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _parameters[name].Value;
            }
        }

        /// <summary>
        /// Set a value to the ParameterInit specified by name.
        /// </summary>
        /// <param name="name">The name of the ParameterInit to set.</param>
        /// <param name="value">A value to set to the ParameterInit.</param>
        /// <returns>A new Ticket with the value set.</returns>
        public Ticket Set(ParameterName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var pi = new ParameterInit(name, value);

            return new Ticket(_features, _parameters.SetItem(pi), _properties, _declaredNamespaces);
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        public IReadOnlyNamedElementCollection<Property> Properties()
        {
            return _properties;
        }

        public Value this[PropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _properties[name].Value;
            }
        }

        /// <summary>
        /// Set a value to the Property specified by name.
        /// </summary>
        /// <param name="name">The name of the Property to set.</param>
        /// <param name="value">A value to set to the Property.</param>
        /// <returns>A new Ticket with the value set.</returns>
        public Ticket Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var p = new Property(name, value);

            return new Ticket(_features, _parameters, _properties.SetItem(p), _declaredNamespaces);
        }

        private readonly NamespaceDeclarationCollection _declaredNamespaces;
        public IReadOnlyNamespaceDeclarationCollection DeclaredNamespaces
        {
            get { return _declaredNamespaces; }
        }

        /// <summary>
        /// Adds the specified element to the ticket.
        /// </summary>
        /// <returns>A new Ticket with element added.</returns>
        public Ticket Add(Feature element)
        {
            return new Ticket(
                _features.Add(element),
                _parameters,
                _properties,
                _declaredNamespaces);
        }

        /// <summary>
        /// Adds the specified element to the ticket.
        /// </summary>
        /// <returns>A new Ticket with element added.</returns>
        public Ticket Add(ParameterInit element)
        {
            return new Ticket(
                _features,
                _parameters.Add(element),
                _properties,
                _declaredNamespaces);
        }

        /// <summary>
        /// Adds the specified element to the ticket.
        /// </summary>
        /// <returns>A new Ticket with element added.</returns>
        public Ticket Add(Property element)
        {
            return new Ticket(
                _features,
                _parameters,
                _properties.Add(element),
                _declaredNamespaces);
        }

        public Ticket Add(NamespaceDeclaration declaration)
        {
            return new Ticket(
                _features,
                _parameters,
                _properties,
                _declaredNamespaces.Add(declaration));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Ticket);
        }

        public bool Equals(Ticket rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return _features.GetHashCode() ^
                _parameters.GetHashCode() ^
                _properties.GetHashCode() ^
                _declaredNamespaces.GetHashCode();
        }

        public static bool operator ==(Ticket v1, Ticket v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1._features == v2._features &&
                v1._parameters == v2._parameters &&
                v1._properties == v2._properties &&
                v1._declaredNamespaces == v2._declaredNamespaces;
        }

        public static bool operator !=(Ticket v1, Ticket v2)
        {
            return !(v1 == v2);
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