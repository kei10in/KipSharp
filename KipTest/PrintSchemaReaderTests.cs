using Kip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;
using xsd = Kip.XmlSchema;

namespace KipTest
{
    public class PrintSchemaReaderTests
    {
        [Fact]
        public void ReadIntValue()
        {
            var v = new PrintSchemaValue(1);
            var reader = new PrintSchemaReader(v.AsXElement().ToString());
            var actual = reader.Read() as PrintSchemaValue;

            Assert.NotNull(actual);
            Assert.Equal(1, actual.AsInt());
        }

        [Fact]
        public void ReadFloatValue()
        {
            var v = new PrintSchemaValue(3.14f);
            var reader = new PrintSchemaReader(v.AsXElement().ToString());
            var actual = reader.Read() as PrintSchemaValue;

            Assert.NotNull(actual);
            Assert.Equal(3.14f, actual.AsFloat());
        }

        [Fact]
        public void ReadQNameValue()
        {
            var v = new PrintSchemaValue(xsd.QName);
            var reader = new PrintSchemaReader(v.AsXElement().ToString());
            var actual = reader.Read() as PrintSchemaValue;

            Assert.NotNull(actual);
            Assert.Equal(xsd.QName, actual.AsXName());
        }

        [Fact]
        public void ReadStringValue()
        {
            var v = new PrintSchemaValue("value");
            var reader = new PrintSchemaReader(v.AsXElement().ToString());
            var actual = reader.Read() as PrintSchemaValue;

            Assert.NotNull(actual);
            Assert.Equal("value", actual.AsString());
        }
    }
}
