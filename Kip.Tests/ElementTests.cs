using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class ElementTests
    {
        static readonly XName someName1 = "SomeName1";

        [Fact]
        public void ThrowsExceptionWhenAddFeatureWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc.Add(new Feature(someName1));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc.Add(new Feature(someName1));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddParameterWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc.Add(new ParameterDef(someName1));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc.Add(new ParameterDef(someName1));
            });
        }
    }
}
