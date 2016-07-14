using System;
using System.Collections.Generic;
using System.Xml;
using Kip.Helper;

namespace Kip
{
    /// <summary>
    /// Represents a PrintTicket document defined in the Print Schema Specification.
    /// </summary>
    public sealed class Ticket : IEquatable<Ticket>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ticket"/> class.
        /// </summary>
        /// <param name="elements">The child elements.</param>
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

        public IReadOnlyList<Option> this[FeatureName name1, FeatureName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));
                return _features[name1][name2].Options();
            }
        }

        /// <summary>
        /// Gets the Options containing the Feature specified name.
        /// </summary>
        /// <param name="name">The name of the Feature.</param>
        /// <returns>The Options contained by the Feature specified name.</returns>
        public IReadOnlyList<Option> Get(FeatureName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _features.Get(name)?.Options();
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

            var sel = ToPrintTicketOption(selection);
            var ft = _features.Contains(name)
                ? _features[name].Set(sel)
                : new Feature(name, sel);

            return new Ticket(_features.SetItem(ft), _parameters, _properties, _declaredNamespaces);
        }

        /// <summary>
        /// Gets the Options containing the nested Feature specified name.
        /// </summary>
        /// <param name="name1">The name of the parent Feature.</param>
        /// <param name="name2">The name of the nested Feature.</param>
        /// <returns>The Options contained by the Feature specified name.</returns>
        public IReadOnlyList<Option> Get(FeatureName name1, FeatureName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            return _features.Get(name1)?.Get(name2)?.Options();
        }

        /// <summary>
        /// Set an option to the Feature specified by name.
        /// </summary>
        /// <param name="name1">The name of Feature containing the nested Feature.</param>
        /// <param name="name2">The name of the nested Feature to set.</param>
        /// <param name="selection">An option to set to the Feature.</param>
        /// <returns>A new Ticket with the option set.</returns>
        public Ticket Set(FeatureName name1, FeatureName name2, Option selection)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            var sel = ToPrintTicketOption(selection);
            var ft = _features.Contains(name1)
                ? _features[name1].Set(sel)
                : new Feature(name1, new Feature(name2, sel));

            return new Ticket(_features.SetItem(ft), _parameters, _properties, _declaredNamespaces);
        }

        private Option ToPrintTicketOption(Option option)
        {
            return option.Remove(Psk.DisplayName)
                .Remove(Psk.DisplayUI)
                .Remove(Psf.IdentityOption)
                .SetConstrained(null);
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
        /// Gets the value of the <see cref="ParameterInit"/> specified name.
        /// </summary>
        /// <param name="name">The name of the <see cref="ParameterInit"/>.</param>
        /// <returns>
        /// The value of the <see cref="ParameterInit"/> specified name.
        /// </returns>
        public Value Get(ParameterName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _parameters.Get(name)?.Value;
        }

        /// <summary>
        /// Set a value to the <see cref="ParameterInit"/> specified by name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ParameterInit"/> to set.
        /// </param>
        /// <param name="value">A value to set to the <see cref="ParameterInit"/>.</param>
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
        /// Gets the value of the Property specified name.
        /// </summary>
        /// <param name="name">The name of the Property.</param>
        /// <returns>The value of the Property specified name.</returns>
        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
        }

        /// <summary>
        /// Sets a value to the Property specified by name.
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

        public Value this[PropertyName name1, PropertyName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));

                return _properties[name1][name2];
            }
        }

        /// <summary>
        /// Gets the value of the nested Property specified name.
        /// </summary>
        /// <param name="name1">The name of the parent Property.</param>
        /// <param name="name2">The name of the nested Property.</param>
        /// <returns>The value of the nested Property specified name.</returns>
        public Value Get(PropertyName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            return _properties.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Set a value to the Property specified by name.
        /// </summary>
        /// <param name="name1">
        /// The name of the Property containing the nested Property.
        /// </param>
        /// <param name="name2">The name of the nested Property to set.</param>
        /// <param name="value">A value to set to the Property.</param>
        /// <returns>A new Ticket with the value set.</returns>
        public Ticket Set(PropertyName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (value == null) throw new ArgumentNullException(nameof(value));

            var p = _properties.Get(name1)?.Set(name2, value)
                ?? new Property(name1, new Property(name2, value));

            return new Ticket(_features, _parameters, _properties.SetItem(p), _declaredNamespaces);
        }

        private readonly NamespaceDeclarationCollection _declaredNamespaces;

        public IReadOnlyNamespaceDeclarationCollection DeclaredNamespaces
        {
            get { return _declaredNamespaces; }
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
            return this.Equals(obj as Ticket);
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

        /// <summary>
        /// Outputs this Ticket to the specified <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">The stream to output this PrintTicket document.</param>
        public void Save(System.IO.Stream stream)
        {
            using (var writer = XmlWriter.Create(stream))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Serialize this Ticket to a <see cref="System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="textWriter">
        /// The TextWrite that the Ticket will be written to.
        /// </param>
        public void Save(System.IO.TextWriter textWriter)
        {
            using (var writer = XmlWriter.Create(textWriter))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Serialize this Ticket to an <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> that the Ticket will be written to.
        /// </param>
        public void Save(XmlWriter writer)
        {
            PrintSchemaWriter.Write(writer, this);
        }

        /// <summary>
        /// Create a new Ticket from a string.
        /// </summary>
        /// <param name="text">The string that contains PrintTicket document.</param>
        /// <returns>
        /// A <see cref="Ticket"/> populated from the string that contains
        /// PrintTicket document.
        /// </returns>
        public static Ticket Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new Ticket instance from the data contained in the
        /// specified stream.
        /// </summary>
        /// <param name="stream">The stream that contains the XML data.</param>
        /// <returns>
        /// A <see cref="Ticket"/> populated from the data contained in the stream.
        /// </returns>
        public static Ticket Load(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadTicket(reader);
            }
        }

        /// <summary>
        /// Creates a new Ticket from a <see cref="System.IO.TextReader"/>.
        /// </summary>
        /// <param name="input">
        /// The <see cref="System.IO.TextReader"/> that contains the PrintTicket document.
        /// </param>
        /// <returns>
        /// A <see cref="Ticket"/> containing the content of the specified <see cref="System.IO.TextReader"/>.
        /// </returns>
        public static Ticket Load(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadTicket(reader);
            }
        }

        /// <summary>
        /// Creates a new Ticket from the <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">
        /// The XmlReader that contains the PrintTicket document.
        /// </param>
        /// <returns>
        /// A <see cref="Ticket"/> containing the content of the specified <see cref="XmlReader"/>.
        /// </returns>
        public static Ticket Load(XmlReader reader)
        {
            return PrintSchemaReader.ReadTicket(reader);
        }
    }
}
