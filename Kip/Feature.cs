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


        #region Properties

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

        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
        }

        /// <summary>
        /// Set a value to the Property specified by the name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Feature Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var p = _properties.Get(name)?.Set(value)
                ?? new Property(name, value);

            return new Feature(Name, _properties.SetItem(p), _options, _features);
        }

        #region Nested properties

        public Value this[PropertyName name1, PropertyName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));
                return _properties[name1][name2];
            }
        }

        public Value Get(PropertyName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            return _properties.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Set a value to the nested Property specified by the name1 and
        /// name2.
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Feature Set(PropertyName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            var p = _properties.Get(name1)?.Set(name2, value)
                ?? new Property(name1, new Property(name2, value));

            return new Feature(Name, _properties.SetItem(p), _options, _features);
        }

        #endregion

        #endregion

        #region Options

        private readonly ImmutableList<Option> _options
            = ImmutableList.Create<Option>();
        public IReadOnlyList<Option> Options()
        {
            return _options;
        }

        /// <summary>
        /// Set the specified element to the <see cref="Feature"/>.
        /// </summary>
        /// <param name="option">The option to set to <see cref="Feature"/>.</param>       
        /// <returns>A new Feature with the element set.</returns>
        public Feature Set(Option option)
        {
            var options = new[] { option };
            return new Feature(Name, _properties, options.ToImmutableList(), _features);
        }

        public Feature Update(Func<Option, Option> func)
        {
            var updated = _options.Select(func);
            return new Feature(Name, _properties, updated.ToImmutableList(), _features);
        }

        #endregion

        #region Nested feature

        #region Options of the nested feature

        private readonly ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        public IReadOnlyNamedElementCollection<Feature> Features
        {
            get { return _features; }
        }

        public IReadOnlyList<Option> this[FeatureName name]
        {
            get { return _features[name].Options(); }
        }

        public IReadOnlyList<Option> Get(FeatureName name)
        {
            return _features.Get(name)?.Options();
        }

        public Feature Set(FeatureName name, Option selection)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            var ft = _features.Contains(name)
                ? _features[name].Set(selection)
                : new Feature(name, selection);

            return new Feature(Name, _properties, _options, _features.SetItem(ft));
        }

        public Feature Update(FeatureName name, Func<Option, Option> func)
        {
            var ft = _features.Get(name)?.Update(func);
            if (ft == null) return this;

            return new Feature(Name, _properties, _options, _features.SetItem(ft));
        }

        #endregion

        #region Properties of the nested feature

        public Value this[FeatureName name1, PropertyName name2]
        {
            get
            {
                if (name1 == null) throw new ArgumentNullException(nameof(name1));
                if (name2 == null) throw new ArgumentNullException(nameof(name2));

                return _features[name1][name2];
            }
        }

        /// <summary>
        /// Get a value of the Property of the nested Feature.
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <returns></returns>
        public Value Get(FeatureName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            return _features.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Set a value to the Property of the nested Feature.
        /// </summary>
        /// <param name="name1"></param>
        /// <param name="name2"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Feature Set(FeatureName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            var ft = _features.Get(name1)?.Set(name2, value)
                ?? new Feature(name1, new Property(name2, value));

            return new Feature(Name, _properties, _options, _features.SetItem(ft));
        }

        #endregion

        #endregion

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