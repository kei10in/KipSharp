using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a PrintCapabilities document defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Capabilities
    {
        /// <summary>
        /// Constructs with children: <see cref="Feature"/>,
        /// <see cref="ParameterDef"/> and/or <see cref="Property"/>.
        /// </summary>
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
            _declaredNamespaces = NamespaceManager.Default;
        }

        internal Capabilities(
            ImmutableNamedElementCollection<Feature> features,
            ImmutableNamedElementCollection<ParameterDef> parameters,
            ImmutableNamedElementCollection<Property> properties,
            NamespaceManager namespaceDeclarations)
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

        private readonly ImmutableNamedElementCollection<ParameterDef> _parameters
            = ImmutableNamedElementCollection.CreateParameterDefCollection();
        public IReadOnlyNamedElementCollection<ParameterDef> Parameters
        {
            get { return _parameters; }
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        private readonly NamespaceManager _declaredNamespaces;
        public IReadOnlyNamespaceManager DeclaredNamespaces
        {
            get { return _declaredNamespaces; }
        }

        public IEnumerable<Option> GetFeatureOptions(XName featureName)
        {
            var options = Features[featureName]?.Options();
            return options ?? Enumerable.Empty<Option>();
        }

        public IEnumerable<Option> GetFeatureOptions(XName featureName, params XName[] nestedFeatureNames)
        {
            var ft = Features[featureName];
            foreach (var nestedFeatureName in nestedFeatureNames)
            {
                ft = ft?.Features[nestedFeatureName];
            }
            var options = ft?.Options();
            return options ?? Enumerable.Empty<Option>();
        }

        /// <summary>
        /// Adds the specified element to the capabilities.
        /// </summary>
        /// <returns>A new capabilities with the element added</returns>
        public Capabilities Add(Feature element)
        {
            return new Capabilities(_features.Add(element), _parameters, _properties, _declaredNamespaces);
        }

        /// <summary>
        /// Adds the specified element to the capabilities.
        /// </summary>
        /// <returns>A new capabilities with the element added</returns>
        public Capabilities Add(ParameterDef element)
        {
            return new Capabilities(_features, _parameters.Add(element), _properties, _declaredNamespaces);
        }

        /// <summary>
        /// Adds the specified element to the capabilities.
        /// </summary>
        /// <returns>A new capabilities with the element added</returns>
        public Capabilities Add(Property element)
        {
            return new Capabilities(_features, _parameters, _properties.Add(element), _declaredNamespaces);
        }

        /// <summary>
        /// Adds the namespace declaration to the capabilities.
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns>A new capabilities with the declared namespace</returns>
        public Capabilities Add(NamespaceDeclaration declaration)
        {
            return new Capabilities(_features, _parameters, _properties, _declaredNamespaces.Add(declaration));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Capabilities);
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

        public static Capabilities Parse(string text)
        {
            using (var textReader = new System.IO.StringReader(text))
            {
                return Load(textReader);
            }
        }

        public static Capabilities Load(System.IO.Stream stream)
        {
            using (var reader = XmlReader.Create(stream))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        public static Capabilities Load(System.IO.TextReader input)
        {
            using (var reader = XmlReader.Create(input))
            {
                return PrintSchemaReader.ReadCapabilities(reader);
            }
        }

        public static Capabilities Load(XmlReader reader)
        {
            return PrintSchemaReader.ReadCapabilities(reader);
        }
    }

    /// <summary>
    /// Wrapper class that representing a child elements of the
    /// PrintCapabilities document.
    /// </summary>
    public sealed class CapabilitiesChild
    {
        private Element _holder;

        private CapabilitiesChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Feature> onFeature,
            Action<ParameterDef> onParameterDef,
            Action<Property> onProperty)
        {
            _holder.Apply(
                onFeature: onFeature,
                onParameterDef: onParameterDef,
                onProperty: onProperty);
        }

        public static implicit operator CapabilitiesChild(Feature element)
        {
            return new CapabilitiesChild(element);
        }

        public static implicit operator CapabilitiesChild(ParameterDef element)
        {
            return new CapabilitiesChild(element);
        }

        public static implicit operator CapabilitiesChild(Property element)
        {
            return new CapabilitiesChild(element);
        }
    }
}