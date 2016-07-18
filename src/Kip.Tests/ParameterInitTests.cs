using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class ParameterInitTests
    {
        private List<Tuple<ParameterInit, ParameterInit>> _equalsPair = new List<Tuple<ParameterInit, ParameterInit>>()
        {
            Tuple.Create<ParameterInit, ParameterInit>(null, null),
            Tuple.Create(new ParameterInit(Exp.SomeParameter, 1), new ParameterInit(Exp.SomeParameter, 1)),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<ParameterInit, ParameterInit>> _notEqualsPair = new List<Tuple<ParameterInit, ParameterInit>>()
        {
            Tuple.Create<ParameterInit, ParameterInit>(null, new ParameterInit(Exp.SomeParameter, 1)),
            Tuple.Create<ParameterInit, ParameterInit>(new ParameterInit(Exp.SomeParameter, 1), null),
            Tuple.Create(new ParameterInit(Exp.SomeParameter, 1), new ParameterInit(Exp.OtherParameter, 1)),
            Tuple.Create(new ParameterInit(Exp.SomeParameter, 1), new ParameterInit(Exp.SomeParameter, 2)),
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
