using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var reader = new StringReader(writer.ToString());
            var actual = Capabilities.Load(reader);

            Assert.Equal(pc, actual);
        }

        [Fact]
        public void DeclarePrivateNamaespaceToPrintTicket()
        {
            var pt = new Ticket(
                new Property(Exp.SomeProperty, Exp.SomeValue));

            var writer = new StringWriter();
            pt.Save(writer);

            var reader = new StringReader(writer.ToString());
            var actual = Ticket.Load(reader);

            Assert.Equal(pt, actual);
        }
    }
}
