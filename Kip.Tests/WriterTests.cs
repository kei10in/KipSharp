using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Kip.Tests
{
    public class WriterTests
    {
        [Fact]
        public void DeclarePrivateNamaespaceToPrintCapabilities()
        {
            var pc = new Capabilities(
                new Property(Exp.SomeProperty, Exp.SomeValue));

            var writer = new StringWriter();
            pc.Save(writer);

            var doc = XDocument.Parse(writer.ToString());
            var element = doc.Root;
            var decls = element.Attributes().Where(x => x.IsNamespaceDeclaration).ToList();

            Assert.Contains(decls, CompareWith(Psf.Declaration));
            Assert.Contains(decls, CompareWith(Psk.Declaration));
            Assert.Contains(decls, CompareWith(Xsd.Declaration));
            Assert.Contains(decls, CompareWith(Xsi.Declaration));
            Assert.Contains(decls, x => x.Value == Exp.Namespace.NamespaceName);
        }

        [Fact]
        public void DeclarePrivateNamaespaceToPrintTicket()
        {
            var pt = new Ticket(
                new Property(Exp.SomeProperty, Exp.SomeValue));

            var writer = new StringWriter();
            pt.Save(writer);

            var doc = XDocument.Parse(writer.ToString());
            var element = doc.Root;
            var decls = element.Attributes().Where(x => x.IsNamespaceDeclaration).ToList();

            Assert.Contains(decls, CompareWith(Psf.Declaration));
            Assert.Contains(decls, CompareWith(Psk.Declaration));
            Assert.Contains(decls, CompareWith(Xsd.Declaration));
            Assert.Contains(decls, CompareWith(Xsi.Declaration));
            Assert.Contains(decls, x => x.Value == Exp.Namespace.NamespaceName);
        }

        private Predicate<XAttribute> CompareWith(NamespaceDeclaration decl)
        {
            return (XAttribute x) =>
            {
                return x.IsNamespaceDeclaration
                    && x.Name.LocalName == decl.Prefix
                    && x.Value == decl.Uri.NamespaceName;
            };
        }
    }
}
