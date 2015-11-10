using System.Xml.Linq;
using Xunit;

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
        public void IntegetToString()
        {
            Value value = 1;
            Assert.Equal("1", value.ToString());
        }

        [Fact]
        public void DecimalToString()
        {
            Value value = 3.14f;
            Assert.Equal("3.14", value.ToString());
        }

        [Fact]
        public void StringToString()
        {
            Value value = "abc";
            Assert.Equal("abc", value.ToString());
        }

        [Fact]
        public void QNameValueToString()
        {
            Value v = Xsd.Integer;
            Assert.Equal(Xsd.Integer.ToString(), v.ToString());
        }
    }
}
