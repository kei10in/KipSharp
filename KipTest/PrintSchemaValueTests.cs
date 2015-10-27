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
