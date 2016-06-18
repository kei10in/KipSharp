using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class ElementTests
    {
        static readonly XName someName2 = "SomeName2";

        [Fact]
        public void ThrowsExceptionWhenAddFeatureWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Feature(Exp.SomeFeature));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new Feature(Exp.SomeFeature));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddParameterWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new ParameterDef(Exp.SomeFeature));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new ParameterDef(Exp.SomeFeature));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddPropertyWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Property(Exp.SomeFeature));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new Property(Exp.SomeFeature));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddPropertyWithExistingNameToProperty()
        {
            Assert.Throws<DuplicateNameException>(() =>
            {
                var parent = new Property(Exp.SomeFeature,
                    new Property(someName2),
                    new Property(someName2));
            });
        }
    }
}
