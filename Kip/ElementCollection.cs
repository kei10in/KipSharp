using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    internal class NamedElementCollection<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        where T : class
    {
        private Func<T, XName> _nameOf;
        private List<T> _elements = new List<T>();

        internal NamedElementCollection(Func<T, XName> nameOf)
            : this(new List<T>(), nameOf)
        { }

        internal NamedElementCollection(IEnumerable<T> collection, Func<T, XName> nameOf)
        {
            _nameOf = nameOf;
            foreach (var element in collection)
            {
                Add(element);
            }
        }

        public int Count
        {
            get
            {
                return _elements.Count;
            }
        }

        public void Add(T element)
        {
            if (_elements.Any(x => _nameOf(x) == _nameOf(element)))
            {
                throw new DuplicateNameException(
                    $"{_nameOf(element)} is already exists. The attribute \"name\" must be unique.");
            }
            _elements.Add(element);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }

    internal static class NamedElementCollection
    {
        public static NamedElementCollection<Feature> CreateFeatureCollection()
        {
            return new NamedElementCollection<Feature>(x => x.Name);
        }

        public static NamedElementCollection<Feature> CreateFeatureCollection(IEnumerable<Feature> elements)
        {
            return new NamedElementCollection<Feature>(elements, x => x.Name);
        }

        public static NamedElementCollection<ParameterDef> CreateParameterDefCollection()
        {
            return new NamedElementCollection<ParameterDef>(x => x.Name);
        }

        public static NamedElementCollection<ParameterDef> CreateParameterDefCollection(IEnumerable<ParameterDef> elements)
        {
            return new NamedElementCollection<ParameterDef>(elements, x => x.Name);
        }

        public static NamedElementCollection<ParameterInit> CreateParameterInitCollection()
        {
            return new NamedElementCollection<ParameterInit>(x => x.Name);
        }

        public static NamedElementCollection<ParameterInit> CreateParameterInitCollection(IEnumerable<ParameterInit> elements)
        {
            return new NamedElementCollection<ParameterInit>(elements, x => x.Name);
        }

        public static NamedElementCollection<Property> CreatePropertyCollection()
        {
            return new NamedElementCollection<Property>(x => x.Name);
        }

        public static NamedElementCollection<Property> CreatePropertyCollection(IEnumerable<Property> elements)
        {
            return new NamedElementCollection<Property>(elements, x => x.Name);
        }

        public static NamedElementCollection<ScoredProperty> CreateScoredPropertyCollection()
        {
            return new NamedElementCollection<ScoredProperty>(x => x.Name);
        }

        public static NamedElementCollection<ScoredProperty> CreateScoredPropertyCollection(IEnumerable<ScoredProperty> elements)
        {
            return new NamedElementCollection<ScoredProperty>(elements, x => x.Name);
        }
    }
}
