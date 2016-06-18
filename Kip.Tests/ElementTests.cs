using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class ElementTests
    {
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
            pc = pc.Add(new ParameterDef(Exp.SomeParameter));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new ParameterDef(Exp.SomeParameter));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddPropertyWithExistingNameToCapabilites()
        {
            var pc = new Capabilities();
            pc = pc.Add(new Property(Exp.SomeProperty));

            Assert.Throws<DuplicateNameException>(() =>
            {
                pc = pc.Add(new Property(Exp.SomeProperty));
            });
        }

        [Fact]
        public void ThrowsExceptionWhenAddPropertyWithExistingNameToProperty()
        {
            Assert.Throws<DuplicateNameException>(() =>
            {
                var parent = new Property(Exp.SomeProperty,
                    new Property(Exp.NestedProperty),
                    new Property(Exp.NestedProperty));
            });
        }
    }
}
