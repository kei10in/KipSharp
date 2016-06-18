using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class ParameterDefTests
    {
        private List<Tuple<ParameterDef, ParameterDef>> _equalsPair = new List<Tuple<ParameterDef, ParameterDef>>()
        {
            Tuple.Create<ParameterDef, ParameterDef>(null, null),
            Tuple.Create(new ParameterDef("a"), new ParameterDef("a")),
            Tuple.Create(new ParameterDef("a", new Property(Exp.SomeProperty)), new ParameterDef("a", new Property(Exp.SomeProperty))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<ParameterDef, ParameterDef>> _notEqualsPair = new List<Tuple<ParameterDef, ParameterDef>>()
        {
            Tuple.Create<ParameterDef, ParameterDef>(null, new ParameterDef("a")),
            Tuple.Create<ParameterDef, ParameterDef>(new ParameterDef("a"), null),
            Tuple.Create(new ParameterDef("a"), new ParameterDef("b")),
            Tuple.Create(new ParameterDef("a"), new ParameterDef("a", new Property(Exp.SomeProperty))),
            Tuple.Create(new ParameterDef("a", new Property(Exp.SomeProperty)), new ParameterDef("a", new Property(Exp.OtherProperty))),
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
