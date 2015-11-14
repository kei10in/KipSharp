using System.Linq;
using Xunit;
using static Kip.Tests.Utility;

namespace Kip.Tests
{
    public class PrintSchemaReaderTests
    {
        [Fact]
        public void ReadSimpleFeatureOptionStructure()
        {
            var pc = CreatePrintCapabilities(@"
                <psf:Feature name='psk:JobCollateAllDocuments'>
                  <psf:Property name='psf:SelectionType'>
                    <psf:Value xsi:type='xsd:QName'>psk:PickOne</psf:Value>
                  </psf:Property>
                  <psf:Property name='psk:DisplayName'>
                    <psf:Value xsi:type='xsd:string'>Collate Copies</psf:Value>
                  </psf:Property>
                  <psf:Option name='psk:Collated' constrained='psk:None'>
                    <psf:Property name='psk:DisplayName'>
                      <psf:Value xsi:type='xsd:string'>Yes</psf:Value>
                    </psf:Property>
                  </psf:Option>
                  <psf:Option name='psk:Uncollated' constrained='psk:None'>
                    <psf:Property name='psk:DisplayName'>
                      <psf:Value xsi:type='xsd:string'>No</psf:Value>
                    </psf:Property>
                  </psf:Option>
                </psf:Feature>");

            var actual = Capabilities.Parse(pc);

            Assert.NotNull(actual);
            Assert.True(0 < actual.Features().Count());

            var f = actual.Feature(Psk.JobCollateAllDocuments);
            Assert.True(0 < f?.Options()?.Count());

            var d = f.Property(Psk.DisplayName);
            Assert.Equal("Collate Copies", d?.Value);

            var o = f.Options().Select(x => x.Property(Psk.DisplayName)?.Value);
            Assert.Contains(o, x => x == "Yes");
            Assert.Contains(o, x => x == "No");
        }

        [Fact]
        public void ReadValueWithUnspecifedType()
        {
            var pc = CreatePrintCapabilities(@"
                <psf:Feature name='psk:JobCollateAllDocuments'>
                  <psf:Property name='psk:DisplayName'>
                    <psf:Value>Copies Collate</psf:Value>
                  </psf:Property>
                  <psf:Option name='psk:Collated'></psf:Option>
                  <psf:Option name='psk:Uncollated'></psf:Option>
                </psf:Feature>");

            var actual = Capabilities.Parse(pc);
            var value = actual.Feature(Psk.JobCollateAllDocuments)?.Property(Psk.DisplayName)?.Value;
            Assert.NotNull(value);
            Assert.Equal(Xsd.String, value?.ValueType);
            Assert.Equal("Copies Collate", value?.AsString());
        }
    }
}
