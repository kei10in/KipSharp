using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    internal sealed class ImmutableNamedElementCollection<T>
        : IEnumerable<T>, IReadOnlyCollection<T>
        where T : class
    {
        private readonly ImmutableDictionary<XName, T> _elements;
        private Func<T, XName> _nameOf;

        internal ImmutableNamedElementCollection(Func<T, XName> nameOf)
            : this(ImmutableDictionary.Create<XName, T>(), nameOf)
        { }

        internal ImmutableNamedElementCollection(IDictionary<XName, T> elements, Func<T, XName> nameOf)
        {
            _elements = elements.ToImmutableDictionary();
            _nameOf = nameOf;
        }

        public int Count
        {
            get
            {
                return _elements.Count;
            }
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

            return new ImmutableNamedElementCollection<T>(newElement, _nameOf);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.Values.GetEnumerator();
        }
    }

    internal static class ImmutableNamedElementCollection
    {
        public static ImmutableNamedElementCollection<Feature> CreateFeatureCollection()
        {
            return new ImmutableNamedElementCollection<Feature>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ParameterDef> CreateParameterDefCollection()
        {
            return new ImmutableNamedElementCollection<ParameterDef>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ParameterInit> CreateParameterInitCollection()
        {
            return new ImmutableNamedElementCollection<ParameterInit>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<Property> CreatePropertyCollection()
        {
            return new ImmutableNamedElementCollection<Property>(x => x.Name);
        }

        public static ImmutableNamedElementCollection<ScoredProperty> CreateScoredPropertyCollection()
        {
            return new ImmutableNamedElementCollection<ScoredProperty>(x => x.Name);
        }
    }
}
