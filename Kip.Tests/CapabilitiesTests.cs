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
    }
}
