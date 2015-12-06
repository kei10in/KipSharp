using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class PropertyTests
    {
        private List<Tuple<Property, Property>> _equalsPropertiesPair = new List<Tuple<Property, Property>>()
        {
            Tuple.Create<Property, Property>(null, null),
            Tuple.Create(new Property("a"), new Property("a")),
            Tuple.Create(new Property("a", 1), new Property("a", 1)),
            Tuple.Create(new Property("a", new Property("b")), new Property("a", new Property("b"))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPropertiesPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Property, Property>> _notEqualsPropertiesPair = new List<Tuple<Property, Property>>()
        {
            Tuple.Create<Property, Property>(null, new Property("a")),
            Tuple.Create<Property, Property>(new Property("a"), null),
            Tuple.Create(new Property("a"), new Property("b")),
            Tuple.Create(new Property("a"), new Property("a", 1)),
            Tuple.Create(new Property("a", 1), new Property("a", 2)),
            Tuple.Create(new Property("a"), new Property("a", new Property("b"))),
            Tuple.Create(new Property("a", new Property("b")), new Property("a", new Property("c"))),
        };

        [Fact]
        public void EqualsOpeartorAndFalse()
        {
            foreach (var pair in _notEqualsPropertiesPair)
            {
                Assert.NotEqual(pair.Item1, pair.Item2);
            }
        }
    }
}
