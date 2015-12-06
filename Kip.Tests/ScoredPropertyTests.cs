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
            Tuple.Create(new ScoredProperty("a"), new ScoredProperty("a")),
            Tuple.Create(new ScoredProperty("a", 1), new ScoredProperty("a", 1)),
            Tuple.Create(new ScoredProperty("a", new ParameterRef("b")), new ScoredProperty("a", new ParameterRef("b"))),
            Tuple.Create(new ScoredProperty("a", new ScoredProperty("b")), new ScoredProperty("a", new ScoredProperty("b"))),
            Tuple.Create(new ScoredProperty("a", new Property("b")), new ScoredProperty("a", new Property("b"))),
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
            Tuple.Create<ScoredProperty, ScoredProperty>(null, new ScoredProperty("a")),
            Tuple.Create<ScoredProperty, ScoredProperty>(new ScoredProperty("a"), null),
            Tuple.Create(new ScoredProperty("a"), new ScoredProperty("b")),
            Tuple.Create(new ScoredProperty("a"), new ScoredProperty("a", 1)),
            Tuple.Create(new ScoredProperty("a", 1), new ScoredProperty("a", 2)),
            Tuple.Create(new ScoredProperty("a"), new ScoredProperty("a", new ParameterRef("b"))),
            Tuple.Create(new ScoredProperty("a", new ParameterRef("b")), new ScoredProperty("a", new ParameterRef("c"))),
            Tuple.Create(new ScoredProperty("a", 1), new ScoredProperty("a", new ParameterRef("b"))),
            Tuple.Create(new ScoredProperty("a"), new ScoredProperty("a", new ScoredProperty("b"))),
            Tuple.Create(new ScoredProperty("a", new ScoredProperty("b")), new ScoredProperty("a", new ScoredProperty("c"))),
            Tuple.Create(new ScoredProperty("a"), new ScoredProperty("a", new Property("b"))),
            Tuple.Create(new ScoredProperty("a", new Property("b")), new ScoredProperty("a", new Property("c"))),
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
