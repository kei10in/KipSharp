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
            Tuple.Create(new Feature(Exp.SomeFeature), new Feature(Exp.SomeFeature)),
            Tuple.Create(new Feature(Exp.SomeFeature, new Option("b")), new Feature(Exp.SomeFeature, new Option("b"))),
            Tuple.Create(new Feature(Exp.SomeFeature, new Property(Exp.SomeProperty)), new Feature(Exp.SomeFeature, new Property(Exp.SomeProperty))),
            Tuple.Create(new Feature(Exp.SomeFeature, new Feature(Exp.OtherFeature)), new Feature(Exp.SomeFeature, new Feature(Exp.OtherFeature))),
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
            Tuple.Create<Feature, Feature>(null, new Feature(Exp.SomeFeature)),
            Tuple.Create<Feature, Feature>(new Feature(Exp.SomeFeature), null),

            Tuple.Create(
                new Feature(Exp.SomeFeature),
                new Feature(Exp.OtherFeature)
                ),

            Tuple.Create(
                new Feature(Exp.SomeFeature),
                new Feature(Exp.SomeFeature, new Option("b"))
                ),

            Tuple.Create(
                new Feature(Exp.SomeFeature, new Option("b")),
                new Feature(Exp.SomeFeature, new Option("c"))
                ),

            Tuple.Create(
                new Feature(Exp.SomeFeature),
                new Feature(Exp.SomeFeature, new Property(Exp.SomeProperty))
                ),

            Tuple.Create(
                new Feature(Exp.SomeFeature, new Property(Exp.SomeProperty)),
                new Feature(Exp.SomeFeature, new Property(Exp.OtherProperty))
                ),

            Tuple.Create(
                new Feature(Exp.SomeFeature),
                new Feature(Exp.SomeFeature, new Feature(Exp.NestedFeature))
                ),

            Tuple.Create(
                new Feature(Exp.SomeFeature, new Feature(Exp.SomeFeature1)),
                new Feature(Exp.SomeFeature, new Feature(Exp.SomeFeature2))
                ),
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
