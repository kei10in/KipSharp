using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class CapabilitiesTests
    {
        [Fact]
        public void GetFeatureOptionsReturnsOptions()
        {
            var pc = new Capabilities(
                new Feature(Exp.SomeFeature,
                    new Option(Exp.SomeOption1),
                    new Option(Exp.SomeOption2)));
            var options = pc.GetFeatureOptions(Exp.SomeFeature);

            var expected = new List<XName>() { Exp.SomeOption1, Exp.SomeOption2 };
            Assert.Equal(expected, options.Select(x => x.Name));
        }

        [Fact]
        public void GetFeatureOptionsReturnsEmptyIfFeatureNotFound()
        {
            var pc = new Capabilities(
                new Feature(Exp.SomeFeature,
                    new Option(Exp.SomeOption1),
                    new Option(Exp.SomeOption2)));
            var options = pc.GetFeatureOptions(Exp.OtherFeature);

            Assert.Empty(options);
        }

        [Fact]
        public void GetNestedFeatureOptionsReturnsOptions()
        {
            var pc = new Capabilities(
                new Feature(Exp.SomeFeature,
                    new Feature(Exp.NestedFeature,
                        new Option(Exp.SomeOption1),
                        new Option(Exp.SomeOption2))));
            var options = pc.GetFeatureOptions(Exp.SomeFeature, Exp.NestedFeature);

            var expected = new List<XName>() { Exp.SomeOption1, Exp.SomeOption2 };
            Assert.Equal(expected, options.Select(x => x.Name));
        }

        [Fact]
        public void GetFeatureOptionsReturnsEmptyIfNestedFeatureNotFound()
        {
            var pc = new Capabilities(
                new Feature(Exp.SomeFeature,
                    new Feature(Exp.NestedFeature,
                        new Option(Exp.SomeOption1),
                        new Option(Exp.SomeOption2))));
            var options = pc.GetFeatureOptions(Exp.SomeFeature, Exp.OtherFeature);

            Assert.Empty(options);
        }

        private List<Tuple<Capabilities, Capabilities>> _equalsPair = new List<Tuple<Capabilities, Capabilities>>()
        {
            Tuple.Create<Capabilities, Capabilities>(null, null),
            Tuple.Create(new Capabilities(), new Capabilities()),
            Tuple.Create(new Capabilities(new Feature("a")), new Capabilities(new Feature("a"))),
            Tuple.Create(new Capabilities(new Property("a")), new Capabilities(new Property("a"))),
            Tuple.Create(new Capabilities(new ParameterDef("a")), new Capabilities(new ParameterDef("a"))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Capabilities, Capabilities>> _notEqualsPair = new List<Tuple<Capabilities, Capabilities>>()
        {
            Tuple.Create<Capabilities, Capabilities>(null, new Capabilities()),
            Tuple.Create<Capabilities, Capabilities>(new Capabilities(), null),
            Tuple.Create(new Capabilities(), new Capabilities(new Feature("a"))),
            Tuple.Create(new Capabilities(new Feature("a")), new Capabilities(new Feature("b"))),
            Tuple.Create(new Capabilities(), new Capabilities(new Property("a"))),
            Tuple.Create(new Capabilities(new Property("a")), new Capabilities(new Property("b"))),
            Tuple.Create(new Capabilities(), new Capabilities(new ParameterDef("a"))),
            Tuple.Create(new Capabilities(new ParameterDef("a")), new Capabilities(new ParameterDef("b"))),
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
