using System;
using System.Xml;
using Kip.Helper;

namespace Kip
{
    /// <summary>
    /// Represents a PrintCapabilities document defined in the Print Schema Specification.
    /// </summary>
    public sealed class Capabilities : IEquatable<Capabilities>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        /// <param name="elements">The child elements.</param>
        public Capabilities(params CapabilitiesChild[] elements)
        {
            var features = ImmutableNamedElementCollection.CreateFeatureCollectionBuilder();
            var parameters = ImmutableNamedElementCollection.CreateParameterDefCollectionBuilder();
            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();

            foreach (var e in elements)
            {
                e.Apply(
                    onFeature: x => features.Add(x),
                    onParameterDef: x => parameters.Add(x),
                    onProperty: x => properties.Add(x));
            }

            _features = features.ToImmutable();
            _parameters = parameters.ToImmutable();
            _properties = properties.ToImmutable();
            _declaredNamespaces = NamespaceDeclarationCollection.Default;
        }

        internal Capabilities(
            ImmutableNamedElementCollection<Feature> features,
            ImmutableNamedElementCollection<ParameterDef> parameters,
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

        public IReadOnlyNamedElementCollection<Feature> Features
        {
            get { return _features; }
        }

        public Feature this[FeatureName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _features[name];
            }
        }

        /// <summary>
        /// Gets the Feature specified name.
        /// </summary>
        /// <param name="name">The name of the Feature.</param>
        /// <returns>The Feature specified name.</returns>
        public Feature Get(FeatureName name)
        {
            return _features.Get(name);
        }

        public Feature this[FeatureName name1, FeatureName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));
                return _features[name1].Features[name2];
            }
        }

        /// <summary>
        /// Gets the nested Feature specified name.
        /// </summary>
        /// <param name="name1">The name of parent Feature.</param>
        /// <param name="name2">The name of nested Feature.</param>
        /// <returns>The nested Feature specified name.</returns>
        public Feature Get(FeatureName name1, FeatureName name2)
        {
            return _features.Get(name1)?.Features?.Get(name2);
        }

        /// <summary>
        /// Sets the value to the Property of the Feature specified name.
        /// </summary>
        /// <param name="name1">The name of Feature.</param>
        /// <param name="name2">The name of Property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The new instance containing new value.</returns>
        public Capabilities Set(FeatureName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            var ft = _features.Get(name1)?.Set(name2, value) ?? new Feature(name1, new Property(name2, value));

            return new Capabilities(_features.SetItem(ft), _parameters, _properties, _declaredNamespaces);
        }

        /// <summary>
        /// Update Options contained by the Feature specified name via specified delegate.
        /// </summary>
        /// <param name="name">The name of the Feature.</param>
        /// <param name="func">Option updating delegate.</param>
        /// <returns>The new instance containing updated options</returns>
        public Capabilities Update(FeatureName name, Func<Option, Option> func)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var ft = _features.Get(name);
            if (ft == null) return this;

            return new Capabilities(_features.SetItem(ft.Update(func)), _parameters, _properties, _declaredNamespaces);
        }

        /// <summary>
        /// Sets the value to the Property of nested Feature.
        /// </summary>
        /// <param name="name1">The name of the parent Feature.</param>
        /// <param name="name2">The name of the nested Feature.</param>
        /// <param name="name3">The name of the Property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The new instance with new value.</returns>
        public Capabilities Set(FeatureName name1, FeatureName name2, PropertyName name3, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            if (name3 == null) throw new ArgumentNullException(nameof(name3));

            var ft = _features.Get(name1)?.Set(name2, name3, value)
                ?? new Feature(name1, new Feature(name2, new Property(name3, value)));

            return new Capabilities(_features.SetItem(ft), _parameters, _properties, _declaredNamespaces);
        }

        /// <summary>
        /// Update the Options contained by nested Feature specified name.
        /// </summary>
        /// <param name="name1">The name of the parent Feature.</param>
        /// <param name="name2">The name of the nested Feature.</param>
        /// <param name="func">Updating delegate.</param>
        /// <returns>The new instance containing updated Options.</returns>
        public Capabilities Update(FeatureName name1, FeatureName name2, Func<Option, Option> func)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            var ft = _features.Get(name1);
            if (ft == null) return this;

            return new Capabilities(_features.SetItem(ft.Update(name2, func)), _parameters, _properties, _declaredNamespaces);
        }

        private readonly ImmutableNamedElementCollection<ParameterDef> _parameters
            = ImmutableNamedElementCollection.CreateParameterDefCollection();

        public IReadOnlyNamedElementCollection<ParameterDef> Parameters
        {
            get { return _parameters; }
        }

        public ParameterDef this[ParameterName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _parameters[name];
            }
        }

        /// <summary>
        /// Gets the <see cref="ParameterDef"/> specified name.
        /// </summary>
        /// <param name="name">The name of the <see cref="ParameterDef"/>.</param>
        /// <returns>The <see cref="ParameterDef"/> specified name.</returns>
        public ParameterDef Get(ParameterName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _parameters.Get(name);
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

        /// <summary>
        /// Gets the value of the Property specified name.
        /// </summary>
        /// <param name="name">The name of the Property.</param>
        /// <returns>The value of the Property.</returns>
        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
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
        /// <param name="name1">The name of parent Property.</param>
        /// <param name="name2">The name of nested Property.</param>
        /// <returns>The value of the nested Property.</returns>
        public Value Get(PropertyName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            return _properties.Get(name1)?.Get(name1);
        }

        private readonly NamespaceDeclarationCollection _declaredNamespaces;

        public IReadOnlyNamespaceDeclarationCollection DeclaredNamespaces
        {
            get { return _declaredNamespaces; }
        }

        public Capabilities Add(Feature element)
        {
            return new Capabilities(_features.Add(element), _parameters, _properties, _declaredNamespaces);
        }

        public Capabilities Add(ParameterDef element)
        {
            return new Capabilities(_features, _parameters.Add(element), _properties, _declaredNamespaces);
        }

        public Capabilities Add(Property element)
        {
            return new Capabilities(_features, _parameters, _properties.Add(element), _declaredNamespaces);
        }

        public Capabilities Add(NamespaceDeclaration declaration)
        {
            return new Capabilities(_features, _parameters, _properties, _declaredNamespaces.Add(declaration));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Capabilities);
        }

        public bool Equals(Capabilities rhs)
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

        public static bool operator ==(Capabilities v1, Capabilities v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1._features == v2._features &&
                v1._parameters == v2._parameters &&
                v1._properties == v2._properties &&
                v1._declaredNamespaces == v2._declaredNamespaces;
        }

        public static bool operator !=(Capabilities v1, Capabilities v2)
        {
            return !(v1 == v2);
        }

        /// <summary>
        /// Outputs this Capabilities to the specified <see cref="System.IO.Stream"/>.
        /// </summary>
        /// <param name="stream">
        /// The stream to output this PrintCapabilities document.
        /// </param>
        public void Save(System.IO.Stream stream)
        {
            using (var writer = XmlWriter.Create(stream))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Serialize this Capabilities to a <see cref="System.IO.TextWriter"/>.
        /// </summary>
        /// <param name="textWriter">
        /// The TextWrite that the Capabilities will be written to.
        /// </param>
        public void Save(System.IO.TextWriter textWriter)
        {
            using (var writer = XmlWriter.Create(textWriter))
            {
                Save(writer);
            }
        }

        /// <summary>
        /// Serialize this Capabilities to an <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="XmlWriter"/> that the Capabilities will be written to.
        /// </param>
        public void Save(XmlWriter writer)
        {
            PrintSchemaWriter.Write(writer, this);
        }

        /// <summary>
        /// Create a new Capabilities from a string.
        /// </summary>
        /// <param name="text">The string that contains PrintCapabilities document.</param>
        /// <returns>
        /// A <see cref="Capabilities"/> populated from the string that contains
        /// PrintCapabilities document.
        /// </returns>
        public static Capabilities Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return Load(textReader);
            }
        }

        /// <summary>
        /// Creates a new Capabilities instance from the data contained in the
        /// specified stream.
        /// </summary>
        /// <param name="stream">The stream that contains the XML data.</param>
        /// <returns>
        /// A <see cref="Capabilities"/> populated from the data contained in the stream.
        /// </returns>
        public static Capabilities Load(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        /// <summary>
        /// Creates a new Capabilities from a <see cref="System.IO.TextReader"/>.
        /// </summary>
        /// <param name="input">
        /// The <see cref="System.IO.TextReader"/> that contains the
        /// PrintCapabilities document.
        /// </param>
        /// <returns>
        /// A <see cref="Capabilities"/> containing the content of the specified
        /// <see cref="System.IO.TextReader"/>.
        /// </returns>
        public static Capabilities Load(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        /// <summary>
        /// Creates a new Capabilities from the <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">
        /// The XmlReader that contains the PrintCapabilities document.
        /// </param>
        /// <returns>
        /// A <see cref="Capabilities"/> containing the content of the specified
        /// <see cref="XmlReader"/>.
        /// </returns>
        public static Capabilities Load(XmlReader reader)
        {
            return PrintSchemaReader.ReadCapabilities(reader);
        }
    }
}
