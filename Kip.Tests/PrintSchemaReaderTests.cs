using System;
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
            var pc = PrintCapabilitiesWith(@"
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

            var d = f.Properties[Psk.DisplayName];
            Assert.Equal("Collate Copies", d?.Value);

            var o = f.Options().Select(x => x.Properties[Psk.DisplayName]?.Value);
            Assert.Contains(o, x => x == "Yes");
            Assert.Contains(o, x => x == "No");
        }

        [Fact]
        public void ReadEmptyOption()
        {
            var pc = PrintCapabilitiesWith(@"
                <psf:Feature name='psk:JobCollateAllDocuments'>
                  <psf:Option name='psk:Collated' constrained='psk:None'/>
                  <psf:Option name='psk:Uncollated' constrained='psk:None'/>
                </psf:Feature>");

            var actual = Capabilities.Parse(pc);
            Assert.True(0 < actual.Features().Count());

            var f = actual.Feature(Psk.JobCollateAllDocuments);
            Assert.Equal(2, f?.Options()?.Count());
        }

        [Fact]
        public void ReadValueWithUnspecifedType()
        {
            var pc = PrintCapabilitiesWith(@"
                <psf:Feature name='psk:JobCollateAllDocuments'>
                  <psf:Property name='psk:DisplayName'>
                    <psf:Value>Copies Collate</psf:Value>
                  </psf:Property>
                  <psf:Option name='psk:Collated'></psf:Option>
                  <psf:Option name='psk:Uncollated'></psf:Option>
                </psf:Feature>");

            var actual = Capabilities.Parse(pc);
            var value = actual.Feature(Psk.JobCollateAllDocuments)?.Properties[Psk.DisplayName]?.Value;
            Assert.NotNull(value);
            Assert.Equal(Xsd.String, value?.ValueType);
            Assert.Equal("Copies Collate", value?.AsString());
        }

        [Fact]
        public void ReadEmptyValue()
        {
            var pc = PrintCapabilitiesWith(@"
                <psf:Property name='exp:SomeProperty'>
                  <psf:Value/>
                </psf:Property>");

            var actual = Capabilities.Parse(pc);
            var value = actual.Property(Exp.SomeProperty)?.Value;
            Assert.NotNull(value);
            Assert.Equal(Value.Empty, value);
        }

        [Fact]
        public void ReadUnassingnedValue()
        {
            var pc = PrintCapabilitiesWith(@"
                <psf:Property name='exp:SomeProperty'>
                  <psf:Value></psf:Value>
                </psf:Property>");

            var actual = Capabilities.Parse(pc);
            var value = actual.Property(Exp.SomeProperty)?.Value;
            Assert.NotNull(value);
            Assert.Equal(Value.Empty, value);
        }

        [Fact]
        public void ThrowsExceptionWhenScoredPropertyContainsBothValueAndParameterRef()
        {
            var pc = PrintCapabilitiesWith(@"
                <psf:Feature name='exp:SomeFeature'>
                  <psf:Option name='exp:SomeOption'>
                    <psf:ScoredProperty name='exp:SomeScoredProperty'>
                      <psf:Value xsi:type='xsd:string'>some value</psf:Value>
                      <psf:ParameterRef name='exp:SomeParameter'></psf:ParameterRef>
                    </psf:ScoredProperty>
                  </psf:Option>                  
                </psf:Feature>
                <psf:ParameterDef name='exp:SomeParameter'></psf:ParameterDef>");

            Assert.ThrowsAny<InvalidChildElementException>(() =>
            {
                var actual = Capabilities.Parse(pc);
            });
        }
    }
}
