using Kip;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;
using psf = Kip.PrintSchemaFramework;
using psk = Kip.PrintSchemaKeywords;
using xsi = Kip.XmlSchemaInstance;

namespace Kip.Tests
{
    public class PrintSchemaCapabilitiesTests
    {
        [Fact]
        public void FindPropertyByNameTest()
        {
            var pc = new Capabilities();
            pc.Add(new Property(psk.DisplayName, "value"));

            var p = pc.Property(psk.DisplayName);

            Assert.Equal("value", p.Value);
        }

        [Fact]
        public void RoundTripSafetyForProperties()
        {
            var pc = new Capabilities();
            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer);
            pc.WriteTo(writer);

            using (var textReader = new StringReader(buffer.ToString()))
            {
                var actual = Capabilities.ReadFrom(textReader);

                Assert.NotNull(actual);
                Assert.Empty(pc.Properties());
            }
        }

        [Fact]
        public void WriteValueInProperty()
        {
            var pc = new Capabilities();
            pc.Add(new Property(psk.JobName, "some job name"));

            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer);
            pc.WriteTo(writer);

            var doc = XDocument.Parse(buffer.ToString());

            Assert.Equal(psf.PrintCapabilities, doc.Root.Name);

            var prop = doc.Root.Element(psf.Property);
            Assert.Equal("psk:JobName", prop.Attribute("name").Value);

            var value = prop.Element(psf.Value);
            Assert.Equal("xsd:string", value.Attribute(xsi.Type).Value);
            Assert.Equal("some job name", value.Value);
        }
    }
}
