using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class PropertyTests
    {
        [Fact]
        public void AddNestedProperty()
        {
            var nested = new Property(Exp.NestedProperty);
            var property = new Property(Exp.SomeProperty, nested);

            var found = property.Properties[Exp.NestedProperty];

            Assert.Equal(found, nested);
        }

        private List<Tuple<Property, Property>> _equalsPair = new List<Tuple<Property, Property>>()
        {
            Tuple.Create<Property, Property>(null, null),
            Tuple.Create(
                new Property(Exp.SomeProperty),
                new Property(Exp.SomeProperty)
                ),

            Tuple.Create(
                new Property(Exp.SomeProperty, 1),
                new Property(Exp.SomeProperty, 1)
                ),

            Tuple.Create(
                new Property(Exp.SomeProperty, new Property(Exp.NestedProperty)),
                new Property(Exp.SomeProperty, new Property(Exp.NestedProperty))
                ),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Property, Property>> _notEqualsPair = new List<Tuple<Property, Property>>()
        {
            Tuple.Create<Property, Property>(null, new Property(Exp.SomeProperty)),
            Tuple.Create<Property, Property>(new Property(Exp.SomeProperty), null),
            Tuple.Create(new Property(Exp.SomeProperty), new Property(Exp.OtherProperty)),
            Tuple.Create(new Property(Exp.SomeProperty), new Property(Exp.SomeProperty, 1)),
            Tuple.Create(new Property(Exp.SomeProperty, 1), new Property(Exp.SomeProperty, 2)),
            Tuple.Create(new Property(Exp.SomeProperty), new Property(Exp.SomeProperty, new Property(Exp.OtherProperty))),
            Tuple.Create(new Property(Exp.SomeProperty, new Property(Exp.SomeProperty1)), new Property(Exp.SomeProperty, new Property(Exp.SomeProperty2))),
        };

        [Fact]
        public void EqualsOpeartorAndFalse()
        {
            foreach (var pair in _notEqualsPair)
            {
                Assert.NotEqual(pair.Item1, pair.Item2);
            }
        }
    }
}
