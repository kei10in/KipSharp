using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class ParameterRefTests
    {
        private List<Tuple<ParameterRef, ParameterRef>> _equalsPair = new List<Tuple<ParameterRef, ParameterRef>>()
        {
            Tuple.Create<ParameterRef, ParameterRef>(null, null),
            Tuple.Create(new ParameterRef(Exp.SomeParameter), new ParameterRef(Exp.SomeParameter)),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<ParameterRef, ParameterRef>> _notEqualsPair = new List<Tuple<ParameterRef, ParameterRef>>()
        {
            Tuple.Create<ParameterRef, ParameterRef>(null, new ParameterRef(Exp.SomeParameter)),
            Tuple.Create<ParameterRef, ParameterRef>(new ParameterRef(Exp.SomeParameter), null),
            Tuple.Create(new ParameterRef(Exp.SomeParameter), new ParameterRef(Exp.OtherParameter)),
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
