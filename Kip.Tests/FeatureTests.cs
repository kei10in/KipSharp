using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class FeatureTests
    {
        private List<Tuple<Feature, Feature>> _equalsPair = new List<Tuple<Feature, Feature>>()
        {
            Tuple.Create<Feature, Feature>(null, null),
            Tuple.Create(new Feature("a"), new Feature("a")),
            Tuple.Create(new Feature("a", new Option("b")), new Feature("a", new Option("b"))),
            Tuple.Create(new Feature("a", new Property("b")), new Feature("a", new Property("b"))),
            Tuple.Create(new Feature("a", new Feature("b")), new Feature("a", new Feature("b"))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Feature, Feature>> _notEqualsPair = new List<Tuple<Feature, Feature>>()
        {
            Tuple.Create<Feature, Feature>(null, new Feature("a")),
            Tuple.Create<Feature, Feature>(new Feature("a"), null),
            Tuple.Create(new Feature("a"), new Feature("b")),
            Tuple.Create(new Feature("a"), new Feature("a", new Option("b"))),
            Tuple.Create(new Feature("a", new Option("b")), new Feature("a", new Option("c"))),
            Tuple.Create(new Feature("a"), new Feature("a", new Property("b"))),
            Tuple.Create(new Feature("a", new Property("b")), new Feature("a", new Property("c"))),
            Tuple.Create(new Feature("a"), new Feature("a", new Feature("b"))),
            Tuple.Create(new Feature("a", new Feature("b")), new Feature("a", new Feature("c"))),
        };

        [Fact]
        public void EqualsOpeartorAndFalse()
        {
            foreach (var pair in _notEqualsPair)
            {
                Assert.NotEqual(pair.Item1, pair.Item2);
            }
        }

        [Fact]
        public void SetPropertyValue()
        {
            var ft = new Feature(Exp.SomeFeature);
            ft = ft.Set(Exp.SomeProperty, 10);

            Assert.Equal(10, ft.Get(Exp.SomeProperty));
        }

        [Fact]
        public void SetPropertyValueToTheNestedFeature()
        {
            var ft = new Feature(Exp.SomeFeature);
            ft = ft.Set(Exp.NestedFeature, Exp.SomeProperty, 10);

            Assert.Equal(10, ft.Get(Exp.NestedFeature, Exp.SomeProperty));
        }
    }
}
