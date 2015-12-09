using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class PrintSchemaCapabilitiesTests
    {
        [Fact]
        public void FindPropertyByNameTest()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Property(Psk.DisplayName, "value"));

            var p = pc.Properties[Psk.DisplayName];

            Assert.Equal("value", p.Value);
        }

        [Fact]
        public void RoundTripSafetyForProperties()
        {
            var pc = new Capabilities();
            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer);
            pc.Save(writer);

            using (var textReader = new StringReader(buffer.ToString()))
            {
                var actual = Capabilities.Load(textReader);

                Assert.NotNull(actual);
                Assert.Empty(pc.Properties);
            }
        }

        [Fact]
        public void WriteValueInProperty()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Property(Psk.JobName, "some job name"));

            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer);
            pc.Save(writer);

            var doc = XDocument.Parse(buffer.ToString());

            Assert.Equal(Psf.PrintCapabilities, doc.Root.Name);

            var prop = doc.Root.Element(Psf.Property);
            Assert.NotNull(prop);
            Assert.Equal("psk:JobName", prop.Attribute("name").Value);

            var value = prop.Element(Psf.Value);
            Assert.Equal("xsd:string", value.Attribute(Xsi.Type).Value);
            Assert.Equal("some job name", value.Value);
        }
    }
}
