using Kip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
