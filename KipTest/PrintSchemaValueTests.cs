using System;
using System.Linq;
using System.Xml.Linq;
using Xunit;
using psf = Kip.PrintSchemaFramework;
using xsi = Kip.XmlSchemaInstance;
using xsd = Kip.XmlSchema;
using Kip;

namespace KipTest
{

    public class PrintSchemaValueTests
    {
        [Fact]
        public void ComparableWithIntValue()
        {
            var v = new PrintSchemaValue(1);
            Assert.Equal(1, v);
        }

        [Fact]
        public void ComparableWithFloatValue()
        {
            var v = new PrintSchemaValue(3.14f);
            Assert.Equal(3.14f, v);
        }

        [Fact]
        public void ComparableWithXNameValue()
        {
            var v = new PrintSchemaValue(psf.Feature);
            Assert.Equal(psf.Feature, v);
        }

        [Fact]
        public void ComparableWithStringValue()
        {
            var v = new PrintSchemaValue("value");
            Assert.Equal("value", v);
        }

        [Fact]
        public void CompareWithNullOfPrintSchemaValue()
        {
            PrintSchemaValue nullValue = null;
            var v = new PrintSchemaValue(1);
            Assert.False(v == nullValue);
        }

        [Fact]
        public void CompareWithNull()
        {
            var v = new PrintSchemaValue(1);
            Assert.False(v.Equals(null));
        }

        [Fact]
        public void CompareNullValueWithNull()
        {
            PrintSchemaValue v = null;
            Assert.True(v == null);
        }

        [Fact]
        public void ConvertIntValueToXElement()
        {
            var v = new PrintSchemaValue(1);
            XElement element = v.AsXElement();
            var type = element.Attribute(xsi.Type);

            Assert.NotNull(type);
            Assert.Equal(xsd.Integer, type.Value.ToXName(element));
            Assert.Equal("1", element.Value);
        }

        [Fact]
        public void ConvertDecimalValueToXElement()
        {
            var v = new PrintSchemaValue(3.14f);
            XElement element = v.AsXElement();
            var type = element.Attribute(xsi.Type);

            Assert.NotNull(type);
            Assert.Equal(xsd.Decimal, type.Value.ToXName(element));
            Assert.Equal("3.14", element.Value);
        }
    }
}
