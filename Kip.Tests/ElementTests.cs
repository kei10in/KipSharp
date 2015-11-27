using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class ElementTests
    {
        static readonly XName someName1 = "SomeName1";
        static readonly XName someName2 = "SomeName2";

        [Fact]
        public void ThrowsExceptionWhenAddFeatureWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Feature(someName1));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new Feature(someName1));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddParameterWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new ParameterDef(someName1));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new ParameterDef(someName1));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddPropertyWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Property(someName1));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new Property(someName1));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddPropertyWithExistingNameToProperty()
        {
            Assert.Throws<DuplicateNameException>(() =>
            {
                var parent = new Property(someName1,
                    new Property(someName2),
                    new Property(someName2));
            });
        }
    }
}
