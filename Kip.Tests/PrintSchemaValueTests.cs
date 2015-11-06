using System;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using psf = Kip.Psf;
using xsi = Kip.Xsi;
using xsd = Kip.Xsd;
using Kip;

namespace Kip.Tests
{

    public class PrintSchemaValueTests
    {
        [Fact]
        public void ComparableWithIntValue()
        {
            var v = new Value(1);
            Assert.Equal(1, v);
        }

        [Fact]
        public void ComparableWithFloatValue()
        {
            var v = new Value(3.14f);
            Assert.Equal(3.14f, v);
        }

        [Fact]
        public void ComparableWithXNameValue()
        {
            var v = new Value(Psf.Feature);
            Assert.Equal(Psf.Feature, v);
        }

        [Fact]
        public void ComparableWithStringValue()
        {
            var v = new Value("value");
            Assert.Equal("value", v);
        }

        [Fact]
        public void CompareWithNullOfPrintSchemaValue()
        {
            Value nullValue = null;
            var v = new Value(1);
            Assert.False(v == nullValue);
        }

        [Fact]
        public void CompareWithNull()
        {
            var v = new Value(1);
            Assert.False(v.Equals(null));
        }

        [Fact]
        public void CompareNullValueWithNull()
        {
            Value v = null;
            Assert.True(v == null);
        }

        [Fact]
        public void ConvertIntValueToXElement()
        {
            var v = new Value(1);
            XElement element = v.AsXElement();
            var type = element.Attribute(Xsi.Type);

            Assert.NotNull(type);
            Assert.Equal(Xsd.Integer, type.Value.ToXName(element));
            Assert.Equal("1", element.Value);
        }

        [Fact]
        public void ConvertDecimalValueToXElement()
        {
            var v = new Value(3.14f);
            XElement element = v.AsXElement();
            var type = element.Attribute(Xsi.Type);

            Assert.NotNull(type);
            Assert.Equal(Xsd.Decimal, type.Value.ToXName(element));
            Assert.Equal("3.14", element.Value);
        }
    }
}
