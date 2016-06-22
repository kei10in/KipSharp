using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ScoredProeprty element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ScoredProperty")]
    public sealed class ScoredProperty : IEquatable<ScoredProperty>
    {

        #region Constructors

        /// <summary>
        /// Constructs with the name and the children:
        /// <see cref="ScoredProperty"/>s and/or <see cref="Property"/>s.
        /// </summary>
        public ScoredProperty(ScoredPropertyName name, params ScoredPropertyChild[] elements)
        {
            Name = name;

            var scoredProperties = ImmutableNamedElementCollection.CreateScoredPropertyCollectionBuilder();
            var properties = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
            foreach (var e in elements)
            {
                e.Apply(
                    onScoredProperty: x => scoredProperties.Add(x),
                    onProperty: x => properties.Add(x));
            }

            _scoredProperties = scoredProperties.ToImmutable();
            _properties = properties.ToImmutable();
        }

        /// <summary>
        /// Constructs with the name, the <see cref="Value"/> and the children:
        /// <see cref="ScoredProperty"/>s and/or <see cref="Property"/>s.
        /// </summary>
        public ScoredProperty(ScoredPropertyName name, Value value, params ScoredPropertyChild[] elements)
            : this(name, elements)
        {
            Value = value;
        }

        /// <summary>
        /// Constructs with the name, the <see cref="ParameterRef"/> and the
        /// children: <see cref="ScoredProperty"/>s and/or <see cref="Property"/>.
        /// <see cref="Property"/>s.
        /// </summary>
        public ScoredProperty(ScoredPropertyName name, ParameterRef parameter, params ScoredPropertyChild[] elements)
            : this(name, elements)
        {
            ParameterRef = parameter;
        }

        internal ScoredProperty(
            ScoredPropertyName name,
            Value value,
            ParameterRef parameter,
            ImmutableNamedElementCollection<ScoredProperty> scoredProperties,
            ImmutableNamedElementCollection<Property> properties)
        {
            Name = name;
            Value = value;
            ParameterRef = parameter;
            _scoredProperties = scoredProperties;
            _properties = properties;
        }

        #endregion

        public ScoredPropertyName Name
        {
            get;
        }

        #region Value or parameter refernce

        public Value Value
        {
            get;
        }

        public ParameterRef ParameterRef
        {
            get;
        }

        public ValueOrParameterRef ValueOrParameterRef
        {
            get
            {
                if (Value != null) return new ValueOrParameterRef(Value);
                if (ParameterRef != null) return new ValueOrParameterRef(ParameterRef);
                else return null;
            }
        }

        #endregion

        #region Nested scored properties

        private readonly ImmutableNamedElementCollection<ScoredProperty> _scoredProperties;
        public IReadOnlyNamedElementCollection<ScoredProperty> ScoredProperties
        {
            get { return _scoredProperties; }
        }

        public ValueOrParameterRef this[ScoredPropertyName name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));
                return _scoredProperties[name].ValueOrParameterRef;
            }
        }

        public ValueOrParameterRef Get(ScoredPropertyName name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            return _scoredProperties.Get(name)?.ValueOrParameterRef;
        }

        #endregion

        private readonly ImmutableNamedElementCollection<Property> _properties;
        public IReadOnlyNamedElementCollection<Property> Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Adds the specified element to the <see cref="ScoredProperty"/>.
        /// </summary>
        /// <returns>A new ScoredProperty with the element added.</returns>
        public ScoredProperty Add(ScoredProperty element)
        {
            return new ScoredProperty(
                Name, Value, ParameterRef,
                _scoredProperties.Add(element), _properties);
        }

        /// <summary>
        /// Adds the specified element to the <see cref="ScoredProperty"/>.
        /// </summary>
        /// <returns>A new ScoredProperty with the element added.</returns>
        public ScoredProperty Add(Property property)
        {
            return new ScoredProperty(
                Name, Value, ParameterRef,
                _scoredProperties, _properties.Add(property));
        }
        
        public override bool Equals(object obj)
        {
            return Equals(obj as ScoredProperty);
        }

        public bool Equals(ScoredProperty rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                Value?.GetHashCode() ?? 0 ^
                ParameterRef?.GetHashCode() ?? 0 ^
                _scoredProperties.GetHashCode() ^
                _properties.GetHashCode();
        }

        public static bool operator ==(ScoredProperty v1, ScoredProperty v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1.Value == v2.Value &&
                v1.ParameterRef == v2.ParameterRef &&
                v1._scoredProperties == v2._scoredProperties &&
                v1._properties == v2._properties;
        }

        public static bool operator !=(ScoredProperty v1, ScoredProperty v2)
        {
            return !(v1 == v2);
        }
    }

    public sealed class ScoredPropertyChild
    {
        private Element _holder;

        private ScoredPropertyChild(Element holder) { _holder = holder; }

        internal void Apply(
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty)
        {
            _holder.Apply(
                onProperty: onProperty,
                onScoredProperty: onScoredProperty);
        }

        public static implicit operator ScoredPropertyChild(Property element)
        {
            return new ScoredPropertyChild(element);
        }

        public static implicit operator ScoredPropertyChild(ScoredProperty element)
        {
            return new ScoredPropertyChild(element);
        }
    }

    public class ValueOrParameterRef
    {
        private object _content;

        public ValueOrParameterRef(Value value)
        {
            _content = value;
        }

        public ValueOrParameterRef(ParameterRef paramterRef)
        {
            _content = paramterRef;
        }

        public XName Type
        {
            get
            {
                var type = _content.GetType();
                if (type == typeof(Value))
                {
                    return Psf.Feature;
                }
                else if (type == typeof(ParameterRef))
                {
                    return Psf.ParameterRef;
                }
                else
                {
                    throw new InvalidOperationException("Unexpected type.");
                }
            }
        }

        public Value AsValue()
        {
            return _content as Value;
        }

        public ParameterRef AsParamterRef()
        {
            return _content as ParameterRef; ;
        }
    }
}