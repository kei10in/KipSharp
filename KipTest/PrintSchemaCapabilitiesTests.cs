using Kip;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Xunit;
using psk = Kip.PrintSchemaKeywords;

namespace KipTest
{
    public class PrintSchemaCapabilitiesTests
    {
        [Fact]
        public void FindPropertyByNameTest()
        {
            var pc = new PrintSchemaCapabilities();
            pc.Add(new PrintSchemaProperty(psk.DisplayName, "value"));

            var p = pc.Property(psk.DisplayName);

            Assert.Equal("value", p.Value);
        }

        [Fact]
        public void RoundTripSafetyForProperties()
        {
            var pc = new PrintSchemaCapabilities();
            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer);
            pc.WriteTo(writer);

            var reader = XmlReader.Create(new StringReader(buffer.ToString()));
            var actual = PrintSchemaCapabilities.ReadFrom(reader);

            Assert.NotNull(actual);
            Assert.Empty(pc.Properties());
        }
    }
}
