using Kip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;
using psk = Kip.PrintSchemaKeywords;
using xsd = Kip.XmlSchema;

namespace Kip.Tests
{
    public class PrintSchemaReaderTests
    {

        [Fact]
        public void ReadSimpleFeatureOptionStructure()
        {
            Assembly assembly = this.GetType().GetTypeInfo().Assembly;
            var names = assembly.GetManifestResourceNames();
            var stream = assembly.GetManifestResourceStream("KipTest.Data.SimpleFeatureOptionStructure.xml");
            
            var reader = XmlReader.Create(stream);
            var actual = PrintSchemaReader.Read(reader);

            Assert.NotNull(actual);
            Assert.True(0 < actual.Features().Count());

            var f = actual.Feature(psk.JobCollateAllDocuments);
            Assert.True(0 < f?.Options()?.Count());

            var d = f.Property(psk.DisplayName);
            Assert.Equal("Collate Copies", d?.Value);

            var o = f.Options().Select(x => x.Property(psk.DisplayName)?.Value);
            Assert.Contains(o, x => x == "Yes");
            Assert.Contains(o, x => x == "No");
        }
    }
}
