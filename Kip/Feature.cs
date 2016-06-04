using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents an Feature element defined in the Print Schema
    /// Specification.
    /// </summary>
    public sealed class Feature : IEquatable<Feature>
    {
        /// <summary>
        /// Constructs with the name.
        /// </summary>
        public Feature(XName name)
        {
            Name = name;
        }

        /// <summary>
        /// Constructs with the name and the children, <see cref="Option"/>,
        /// <see cref="Property"/> and/or <see cref="Feature"/>.
        /// </summary>
        public Feature(XName name, params FeatureChild[] elements)
        {
            Name = name;
            var o = new Option();
            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            var options = ImmutableList.CreateBuilder<Option>();
            var features = ImmutableNamedElementCollection.CreateFeatureCollectionBuilder();
            foreach (var e in elements)
            {
                e.Apply(
                    onProperty: x => properties.Add(x),
                    onOption: x => options.Add(x),
                    onFeature: x => features.Add(x));
            }
            _properties = properties.ToImmutable();
            _options = options.ToImmutable();
            _features = features.ToImmutable();
        }

        internal Feature(
            XName name,
            ImmutableNamedElementCollection<Property> properties,
            ImmutableList<Option> options,
            ImmutableNamedElementCollection<Feature> nestedFeature)
        {
            Name = name;
            _properties = properties;
            _options = options;
            _features = nestedFeature;
        }

        public XName Name
        {
            get;
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        private readonly ImmutableList<Option> _options
            = ImmutableList.Create<Option>();
        public IReadOnlyList<Option> Options()
        {
            return _options;
        }

        private readonly ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        public IReadOnlyNamedElementCollection<Feature> Features
        {
            get { return _features; }
        }

        /// <summary>
        /// Add the specified element to the <see cref="Feature"/>.
        /// </summary>
        /// <returns>A new Feature with the element added.</returns>
        public Feature Add(Property element)
        {
            return new Feature(Name, _properties.Add(element), _options, _features);
        }

        /// <summary>
        /// Add the specified element to the <see cref="Feature"/>.
        /// </summary>
        /// <returns>A new Feature with the element added.</returns>
        public Feature Add(Option option)
        {
            return new Feature(Name, _properties, _options.Add(option), _features);
        }

        /// <summary>
        /// Set the specified element to the <see cref="Feature"/>.
        /// </summary>
        /// <param name="option">The option to set to <see cref="Feature"/>.</param>       
        /// <returns>A new Feature with the element set.</returns>
        public Feature Set(Option option)
        {
            var options = new []{ option };
            return new Feature(Name, _properties, options.ToImmutableList(), _features);
        }

        /// <summary>
        /// Add the specified element to the <see cref="Feature"/>.
        /// </summary>
        /// <returns>A new Feature with the element added.</returns>
        public Feature Add(Feature feature)
        {
            return new Feature(Name, _properties, _options, _features.Add(feature));
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Feature);
        }

        public bool Equals(Feature rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                _options.GetHashCode() ^
                _properties.GetHashCode() ^
                _features.GetHashCode();
        }

        public static bool operator ==(Feature v1, Feature v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1._options.SequenceEqual(v2._options) &&
                v1._properties == v2._properties &&
                v1._features == v2._features;
        }

        public static bool operator !=(Feature v1, Feature v2)
        {
            return !(v1 == v2);
        }
    }

    public sealed class FeatureChild
    {
        private Element _holder;

        private FeatureChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Property> onProperty,
            Action<Option> onOption,
            Action<Feature> onFeature)
        {
            _holder.Apply(
                onProperty: onProperty,
                onOption: onOption,
                onFeature: onFeature);
        }

        public static implicit operator FeatureChild(Property element)
        {
            return new FeatureChild(element);
        }

        public static implicit operator FeatureChild(Option element)
        {
            return new FeatureChild(element);
        }

        public static implicit operator FeatureChild(Feature element)
        {
            return new FeatureChild(element);
        }
    }
}