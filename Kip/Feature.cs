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
    public sealed class Feature
    {
        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        private readonly ImmutableList<Option> _options
            = ImmutableList.Create<Option>();
        private readonly ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();

        public Feature(XName name)
        {
            Name = name;
        }

        public Feature(XName name, params FeatureChild[] elements)
        {
            Name = name;

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

        public Feature Add(Property property)
        {
            return new Feature(Name, _properties.Add(property), _options, _features);
        }

        public IEnumerable<Property> Properties()
        {
            return _properties;
        }

        public Property Property(XName name)
        {
            return _properties.FirstOrDefault(x => x.Name == name);
        }

        public Feature Add(Option option)
        {
            return new Feature(Name, _properties, _options.Add(option), _features);
        }

        public IEnumerable<Option> Options()
        {
            return _options;
        }

        public Feature Add(Feature feature)
        {
            return new Feature(Name, _properties, _options, _features.Add(feature));
        }

        public IEnumerable<Feature> NestedFeatures()
        {
            return _features;
        }

        public Feature NestedFeature(XName name)
        {
            return _features.FirstOrDefault(x => x.Name == name);
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