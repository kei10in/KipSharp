using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    internal abstract class NamedElementContainer<T>
        : IEnumerable<T>
        , IReadOnlyCollection<T>
        where T : class
    {
        private List<T> _elements = new List<T>();

        public NamedElementContainer() { }

        public NamedElementContainer(IEnumerable<T> collection)
        {
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
            if (_elements.Any(x => NameOf(x) == NameOf(element)))
            {
                throw new DuplicateNameException(
                    $"{NameOf(element)} is already exists. The attribute \"name\" must be unique.");
            }
            _elements.Add(element);
        }

        public T Get(XName name)
        {
            return _elements.FirstOrDefault(x => NameOf(x) == name);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }

        protected abstract XName NameOf(T element);
    }

    internal sealed class FeatureContainer
        : NamedElementContainer<Feature>
    {
        protected override XName NameOf(Feature element)
        {
            return element.Name;
        }
    }

    internal sealed class ParameterDefContainer
        : NamedElementContainer<ParameterDef>
    {
        protected override XName NameOf(ParameterDef element)
        {
            return element.Name;
        }
    }

    internal sealed class ParameterInitContainer
        : NamedElementContainer<ParameterInit>
    {
        protected override XName NameOf(ParameterInit element)
        {
            return element.Name;
        }
    }

    internal sealed class PropertyContainer
        : NamedElementContainer<Property>
    {
        internal PropertyContainer() : base() { }

        internal PropertyContainer(IEnumerable<Property> collection)
            : base(collection)
        { }

        protected override XName NameOf(Property element)
        {
            return element.Name;
        }
    }

    internal sealed class ScoredPropertyContainer
        : NamedElementContainer<ScoredProperty>
    {
        internal ScoredPropertyContainer() : base() { }

        internal ScoredPropertyContainer(IEnumerable<ScoredProperty> collection)
            : base(collection)
        { }

        protected override XName NameOf(ScoredProperty element)
        {
            return element.Name;
        }
    }
}
