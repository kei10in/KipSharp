using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class OptionTests
    {
        private List<Tuple<Option, Option>> _equalsPair = new List<Tuple<Option, Option>>()
        {
            Tuple.Create<Option, Option>(null, null),
            Tuple.Create(new Option(), new Option()),
            Tuple.Create(new Option("a"), new Option("a")),
            Tuple.Create(new Option("a", new ScoredProperty(Exp.SomeScoredProperty)), new Option("a", new ScoredProperty(Exp.SomeScoredProperty))),
            Tuple.Create(new Option("a", new Property(Exp.SomeProperty)), new Option("a", new Property(Exp.SomeProperty))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Option, Option>> _notEqualsPair = new List<Tuple<Option, Option>>()
        {
            Tuple.Create<Option, Option>(null, new Option("a")),
            Tuple.Create<Option, Option>(new Option("a"), null),
            Tuple.Create(new Option("a"), new Option("b")),
            Tuple.Create(new Option("a"), new Option("a", new ScoredProperty(Exp.SomeScoredProperty))),
            Tuple.Create(new Option("a", new ScoredProperty(Exp.SomeScoredProperty)), new Option("a", new ScoredProperty(Exp.OtherScoredProperty))),
            Tuple.Create(new Option("a"), new Option("a", new Property(Exp.SomeProperty))),
            Tuple.Create(new Option("a", new Property(Exp.SomeProperty)), new Option("a", new Property(Exp.OtherProperty))),
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
