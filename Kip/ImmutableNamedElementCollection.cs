using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(ImmutableNamedElementCollectionDebugView<>))]
    internal sealed class ImmutableNamedElementCollection<T>
        : IReadOnlyNamedElementCollection<T>
        where T : class
    {
        private Func<T, XName> _nameOf;
        private readonly ImmutableDictionary<XName, T> _elements;

        internal ImmutableNamedElementCollection(Func<T, XName> nameOf)
            : this(nameOf, ImmutableDictionary.Create<XName, T>())
        { }

        private ImmutableNamedElementCollection(Func<T, XName> nameOf, ImmutableDictionary<XName, T> elements)
        {
            _nameOf = nameOf;
            _elements = elements;
        }

        public int Count
        {
            get
            {
                return _elements.Count;
            }
        }
        
        public T this[XName name]
        {
            get
            {
                T result;
                if (_elements.TryGetValue(name, out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool Contains(XName name)
        {
            return _elements.ContainsKey(name);
        }

        public ImmutableNamedElementCollection<T> Add(T element)
        {
            XName name = _nameOf(element);
            if (_elements.ContainsKey(name))
            {
                throw new DuplicateNameException(
                    $"{_nameOf(element)} is already exists. The attribute \"name\" must be unique.");
            }
            var newElement = _elements.Add(name, element);

            return new ImmutableNamedElementCollection<T>(_nameOf, newElement);
        }

        public ImmutableNamedElementCollection<T> Clear()
        {
            return new ImmutableNamedElementCollection<T>(_nameOf);
        }

        public ImmutableNamedElementCollection<T> Remove(XName name)
        {
            return new ImmutableNamedElementCollection<T>(_nameOf, _elements.Remove(name));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        internal sealed class Builder
        {
            private Func<T, XName> _nameOf;
            private readonly ImmutableDictionary<XName, T>.Builder _elements
                = ImmutableDictionary.CreateBuilder<XName, T>();

            internal Builder(Func<T, XName> nameOf)
            {
                _nameOf = nameOf;
            }

            internal void Add(T element)
            {
                XName name = _nameOf(element);
                if (_elements.ContainsKey(name))
                {
                    throw new DuplicateNameException(
                        $"{_nameOf(element)} is already exists. The attribute \"name\" must be unique.");
                }
                _elements.Add(name, element);
            }

            internal ImmutableNamedElementCollection<T> ToImmutable()
            {
                return new ImmutableNamedElementCollection<T>(
                    _nameOf, _elements.ToImmutable());
            }
        }

        public override string ToString()
        {
            return _elements.ToString();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ImmutableNamedElementCollection<T>);
        }

        public bool Equals(ImmutableNamedElementCollection<T> rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return _elements.GetHashCode();
        }

        public static bool operator ==(ImmutableNamedElementCollection<T> v1, ImmutableNamedElementCollection<T> v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.SequenceEqual(v2);
        }

        public static bool operator !=(ImmutableNamedElementCollection<T> v1, ImmutableNamedElementCollection<T> v2)
        {
            return !(v1 == v2);
        }
    }

    internal static class ImmutableNamedElementCollection
    {
        public static ImmutableNamedElementCollection<Feature> CreateFeatureCollection()
        {
            return new ImmutableNamedElementCollection<Feature>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<Feature>.Builder CreateFeatureCollectionBuilder()
        {
            return new ImmutableNamedElementCollection<Feature>.Builder(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ParameterDef> CreateParameterDefCollection()
        {
            return new ImmutableNamedElementCollection<ParameterDef>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ParameterDef>.Builder CreateParameterDefCollectionBuilder()
        {
            return new ImmutableNamedElementCollection<ParameterDef>.Builder(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ParameterInit> CreateParameterInitCollection()
        {
            return new ImmutableNamedElementCollection<ParameterInit>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ParameterInit>.Builder CreateParameterInitCollectionBuilder()
        {
            return new ImmutableNamedElementCollection<ParameterInit>.Builder(x => x.Name);
        }

        public static ImmutableNamedElementCollection<Property> CreatePropertyCollection()
        {
            return new ImmutableNamedElementCollection<Property>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<Property>.Builder CreatePropertyCollectionBuilder()
        {
            return new ImmutableNamedElementCollection<Property>.Builder(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ScoredProperty> CreateScoredPropertyCollection()
        {
            return new ImmutableNamedElementCollection<ScoredProperty>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ScoredProperty>.Builder CreateScoredPropertyCollectionBuilder()
        {
            return new ImmutableNamedElementCollection<ScoredProperty>.Builder(x => x.Name);
        }
    }
}
