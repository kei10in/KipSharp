using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using Kip.Helper;

namespace Kip
{
    /// <summary>
    /// Represents an Feature element defined in the Print Schema Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: Feature")]
    public sealed class Feature : IEquatable<Feature>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        public Feature(FeatureName name)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Feature"/> class.
        /// </summary>
        /// <param name="name">The name of the element.</param>
        /// <param name="elements">The options of the element.</param>
        public Feature(FeatureName name, params FeatureChild[] elements)
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
            FeatureName name,
            ImmutableNamedElementCollection<Property> properties,
            ImmutableList<Option> options,
            ImmutableNamedElementCollection<Feature> nestedFeature)
        {
            Name = name;
            _properties = properties;
            _options = options;
            _features = nestedFeature;
        }

        public FeatureName Name
        {
            get;
        }

        private readonly ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Gets the value of the Property specified name. Throws exception when
        /// the Property specified name is not found.
        /// </summary>
        /// <param name="name">The name of the Property.</param>
        /// <returns>The value of the Property.</returns>
        public Value this[PropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _properties[name].Value;
            }
        }

        /// <summary>
        /// Gets the value of the Property specified name. Returns null when the
        /// Property specified name is not found.
        /// </summary>
        /// <param name="name">The name of the Property.</param>
        /// <returns>The value of the Property.</returns>
        public Value Get(PropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _properties.Get(name)?.Value;
        }

        /// <summary>
        /// Sets a value to the Property specified name.
        /// </summary>
        /// <remarks>
        /// Returns new instance of Feature when specified Property found,
        /// otherwise return this.
        /// </remarks>
        /// <param name="name">The name of Property.</param>
        /// <param name="value">The new value of Property.</param>
        /// <returns>The new instance with Property set new value.</returns>
        public Feature Set(PropertyName name, Value value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var p = _properties.Get(name)?.Set(value)
                ?? new Property(name, value);

            return new Feature(Name, _properties.SetItem(p), _options, _features);
        }

        /// <summary>
        /// Gets a value of the nested Property specified name.
        /// </summary>
        /// <param name="name1">The name of Property.</param>
        /// <param name="name2">The name of nested Property.</param>
        /// <returns>The value of nested Property.</returns>
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
        /// Gets a value of the nested Property specified name.
        /// </summary>
        /// <param name="name1">The name of Property.</param>
        /// <param name="name2">The name of nested Property.</param>
        /// <returns>
        /// The value of nested Property. When property not found, returns null
        /// </returns>
        public Value Get(PropertyName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));
            return _properties.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Set a value to the nested Property specified by the name1 and name2.
        /// </summary>
        /// <param name="name1">The name of the Property.</param>
        /// <param name="name2">The name of the nested Property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The new instance with new Property.</returns>
        public Feature Set(PropertyName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            var p = _properties.Get(name1)?.Set(name2, value)
                ?? new Property(name1, new Property(name2, value));

            return new Feature(Name, _properties.SetItem(p), _options, _features);
        }

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

        /// <summary>
        /// Updates options via function specified.
        /// </summary>
        /// <param name="func">The delegate to update to <see cref="Option"/>.</param>
        /// <returns>
        /// The new instance with updated <see cref="Options"/> s.
        /// </returns>
        public Feature Update(Func<Option, Option> func)
        {
            var updated = _options.Select(func);
            return new Feature(Name, _properties, updated.ToImmutableList(), _features);
        }

        private readonly ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();

        public IReadOnlyNamedElementCollection<Feature> Features
        {
            get { return _features; }
        }

        /// <summary>
        /// Gets the nested <see cref="Feature"/>.
        /// </summary>
        /// <param name="name">The name of the nested <see cref="Feature"/>.</param>
        /// <returns>The Feature specified name.</returns>
        public Feature this[FeatureName name]
        {
            get { return _features[name]; }
        }

        /// <summary>
        /// Gets the nested <see cref="Feature"/>.
        /// </summary>
        /// <param name="name">The name of the nested <see cref="Feature"/>.</param>
        /// <returns>The Feature specified name.</returns>
        public Feature Get(FeatureName name)
        {
            return _features.Get(name);
        }

        /// <summary>
        /// Sets the <see cref="Option"/> to the nested <see cref="Feature"/>
        /// specified name.
        /// </summary>
        /// <param name="name">The name of the nested <see cref="Feature"/>.</param>
        /// <param name="selection">The <see cref="Option"/> to set.</param>
        /// <returns>The new instance contains new <see cref="Option"/>.</returns>
        public Feature Set(FeatureName name, Option selection)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (selection == null) throw new ArgumentNullException(nameof(selection));

            var ft = _features.Contains(name)
                ? _features[name].Set(selection)
                : new Feature(name, selection);

            return new Feature(Name, _properties, _options, _features.SetItem(ft));
        }

        /// <summary>
        /// Updates the all <see cref="Option"/> contained by nested <see
        /// cref="Feature"/> via the delegate.
        /// </summary>
        /// <param name="name">The name of the nested <see cref="Feature"/>.</param>
        /// <param name="func">Updater for options.</param>
        /// <returns>The new instance with updated Options.</returns>
        public Feature Update(FeatureName name, Func<Option, Option> func)
        {
            var ft = _features.Get(name)?.Update(func);
            if (ft == null) return this;

            return new Feature(Name, _properties, _options, _features.SetItem(ft));
        }

        /// <summary>
        /// The value of the Property of the nested Feature specified name.
        /// </summary>
        /// <param name="name1">The name of nested Feature.</param>
        /// <param name="name2">The name of Property.</param>
        /// <returns>The value of the Property of the nested Feature.</returns>
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
        /// <param name="name1">The name of nested.Feature containing Property.</param>
        /// <param name="name2">The name of Property of nested Feature.</param>
        /// <returns>The value of property.</returns>
        public Value Get(FeatureName name1, PropertyName name2)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            return _features.Get(name1)?.Get(name2);
        }

        /// <summary>
        /// Set a value to the Property of the nested Feature.
        /// </summary>
        /// <param name="name1">The name of nested Feature.</param>
        /// <param name="name2">The name of Property.</param>
        /// <param name="value">The value to set.</param>
        /// <returns>The new instance with updated value.</returns>
        public Feature Set(FeatureName name1, PropertyName name2, Value value)
        {
            if (name1 == null) throw new ArgumentNullException(nameof(name1));
            if (name2 == null) throw new ArgumentNullException(nameof(name2));

            var ft = _features.Get(name1)?.Set(name2, value)
                ?? new Feature(name1, new Property(name2, value));

            return new Feature(Name, _properties, _options, _features.SetItem(ft));
        }

        public Feature Add(Property property)
        {
            return new Feature(Name, _properties.Add(property), _options, _features);
        }

        public Feature Add(Option option)
        {
            return new Feature(Name, _properties, _options.Add(option), _features);
        }

        public Feature Add(Feature feature)
        {
            return new Feature(Name, _properties, _options, _features.Add(feature));
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Feature);
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
}
