using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class BasicPrintCapabilitiesReadingTests
    {
        Capabilities _actual;
        static readonly XNamespace PrivateNamespace = "http://schemas.microsoft.com/windows/printing/oemdriverpt/Printer";

        public BasicPrintCapabilitiesReadingTests()
        {
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            var names = assembly.GetManifestResourceNames();
            using (var stream = assembly.GetManifestResourceStream("Kip.Tests.Data.BasicPrintCapabilities.xml"))
            {
                _actual = Capabilities.Load(stream);
            }
        }

        [Fact]
        public void ReadFirstElement()
        {
            // Structure is following:
            // <psf:ParameterDef name="ns0000:PageDevmodeSnapshot">
            //   <psf:Property name="psf:DataType">
            //     <psf:Value xsi:type="xsd:QName">xsd:string</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psf:UnitType">
            //     <psf:Value xsi:type="xsd:string">base64</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psf:DefaultValue">
            //     <psf:Value xsi:type="xsd:string">SABlAGwAbABvACEAAAA=</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psf:Mandatory">
            //     <psf:Value xsi:type="xsd:QName">psk:Optional</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psf:MinLength">
            //     <psf:Value xsi:type="xsd:integer">0</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psf:MaxLength">
            //     <psf:Value xsi:type="xsd:integer">174760</psf:Value>
            //   </psf:Property>
            // </psf:ParameterDef>

            var parameter = _actual.Parameter(PrivateNamespace + "PageDevmodeSnapshot");
            Assert.NotNull(parameter);
            Assert.Equal(6, parameter.Properties().Count());
        }

        [Fact]
        public void ReadLastElement()
        {
            // <psf:Property name="psk:PageImageableSize">
            //   <psf:Property name="psk:ImageableSizeWidth">
            //     <psf:Value xsi:type="xsd:integer">215900</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psk:ImageableSizeHeight">
            //     <psf:Value xsi:type="xsd:integer">279400</psf:Value>
            //   </psf:Property>
            //   <psf:Property name="psk:ImageableArea">
            //     <psf:Property name="psk:OriginWidth">
            //       <psf:Value xsi:type="xsd:integer">6350</psf:Value>
            //     </psf:Property>
            //     <psf:Property name="psk:OriginHeight">
            //       <psf:Value xsi:type="xsd:integer">1693</psf:Value>
            //     </psf:Property>
            //     <psf:Property name="psk:ExtentWidth">
            //       <psf:Value xsi:type="xsd:integer">203200</psf:Value>
            //     </psf:Property>
            //     <psf:Property name="psk:ExtentHeight">
            //       <psf:Value xsi:type="xsd:integer">264837</psf:Value>
            //     </psf:Property>
            //   </psf:Property>
            // </psf:Property>

            var property = _actual.Property(Psk.PageImageableSize);
            Assert.NotNull(property);
            Assert.Equal(3, property.SubProperties().Count());
        }

        [Fact]
        public void ReadAllElements()
        {
            Assert.Equal(11, _actual.Features().Count());
            Assert.Equal(4, _actual.Parameters().Count());
            Assert.Equal(1, _actual.Properties().Count());
        }

        [Fact]
        public void RoundtripSafeForReadAndWrite()
        {
            var buffer1 = new StringBuilder();
            var writer1 = XmlWriter.Create(buffer1);
            PrintSchemaWriter.Write(writer1, _actual);

            var writeResult1 = buffer1.ToString();

            var pc = Capabilities.Parse(writeResult1);

            var buffer2 = new StringBuilder();
            var writer2 = XmlWriter.Create(buffer2);
            PrintSchemaWriter.Write(writer2, pc);

            var writeResult2 = buffer2.ToString();

            Assert.Equal(writeResult1, writeResult2);
        }
    }
}
