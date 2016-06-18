using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class ScoredPropertyTests
    {
        private List<Tuple<ScoredProperty, ScoredProperty>> _equalsPair = new List<Tuple<ScoredProperty, ScoredProperty>>()
        {
            Tuple.Create<ScoredProperty, ScoredProperty>(null, null),
            Tuple.Create(new ScoredProperty(Exp.SomeScoredProperty), new ScoredProperty(Exp.SomeScoredProperty)),
            Tuple.Create(new ScoredProperty(Exp.SomeScoredProperty, 1), new ScoredProperty(Exp.SomeScoredProperty, 1)),
            Tuple.Create(new ScoredProperty(Exp.SomeScoredProperty, new ParameterRef(Exp.SomeParameter)), new ScoredProperty(Exp.SomeScoredProperty, new ParameterRef(Exp.SomeParameter))),
            Tuple.Create(new ScoredProperty(Exp.SomeScoredProperty, new ScoredProperty(Exp.OtherScoredProperty)), new ScoredProperty(Exp.SomeScoredProperty, new ScoredProperty(Exp.OtherScoredProperty))),
            Tuple.Create(new ScoredProperty(Exp.SomeScoredProperty, new Property(Exp.SomeProperty)), new ScoredProperty(Exp.SomeScoredProperty, new Property(Exp.SomeProperty))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<ScoredProperty, ScoredProperty>> _notEqualsPair = new List<Tuple<ScoredProperty, ScoredProperty>>()
        {
            Tuple.Create<ScoredProperty, ScoredProperty>(
                null, new ScoredProperty(Exp.SomeScoredProperty)),

            Tuple.Create<ScoredProperty, ScoredProperty>(
                new ScoredProperty(Exp.SomeScoredProperty), null),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty),
                new ScoredProperty(Exp.OtherScoredProperty)
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty),
                new ScoredProperty(Exp.SomeScoredProperty, 1)
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty, 1),
                new ScoredProperty(Exp.SomeScoredProperty, 2)
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty),
                new ScoredProperty(Exp.SomeScoredProperty, new ParameterRef(Exp.SomeParameter))
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty, new ParameterRef(Exp.SomeParameter)),
                new ScoredProperty(Exp.SomeScoredProperty, new ParameterRef(Exp.OtherParameter))
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty, 1),
                new ScoredProperty(Exp.SomeScoredProperty, new ParameterRef(Exp.SomeParameter))
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty),
                new ScoredProperty(Exp.SomeScoredProperty, new ScoredProperty(Exp.OtherScoredProperty))
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty, new ScoredProperty(Exp.SomeScoredProperty1)),
                new ScoredProperty(Exp.SomeScoredProperty, new ScoredProperty(Exp.SomeScoredProperty2))
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty),
                new ScoredProperty(Exp.SomeScoredProperty, new Property(Exp.SomeProperty))
                ),

            Tuple.Create(
                new ScoredProperty(Exp.SomeScoredProperty, new Property(Exp.SomeProperty)),
                new ScoredProperty(Exp.SomeScoredProperty, new Property(Exp.OtherProperty))
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
    }
}
