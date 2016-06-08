using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class TicketTests
    {
        private List<Tuple<Ticket, Ticket>> _equalsPair = new List<Tuple<Ticket, Ticket>>()
        {
            Tuple.Create<Ticket, Ticket>(null, null),
            Tuple.Create(new Ticket(), new Ticket()),
            Tuple.Create(new Ticket(new Feature("a")), new Ticket(new Feature("a"))),
            Tuple.Create(new Ticket(new Property("a")), new Ticket(new Property("a"))),
            Tuple.Create(new Ticket(new ParameterInit("a", 1)), new Ticket(new ParameterInit("a", 1))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Ticket, Ticket>> _notEqualsPair = new List<Tuple<Ticket, Ticket>>()
        {
            Tuple.Create<Ticket, Ticket>(null, new Ticket()),
            Tuple.Create<Ticket, Ticket>(new Ticket(), null),
            Tuple.Create(new Ticket(), new Ticket(new Feature("a"))),
            Tuple.Create(new Ticket(new Feature("a")), new Ticket(new Feature("b"))),
            Tuple.Create(new Ticket(), new Ticket(new Property("a"))),
            Tuple.Create(new Ticket(new Property("a")), new Ticket(new Property("b"))),
            Tuple.Create(new Ticket(), new Ticket(new ParameterInit("a", 1))),
            Tuple.Create(new Ticket(new ParameterInit("a", 1)), new Ticket(new ParameterInit("b", 2))),
        };

        [Fact]
        public void EqualsOpeartorAndFalse()
        {
            foreach (var pair in _notEqualsPair)
            {
                Assert.NotEqual(pair.Item1, pair.Item2);
            }
        }

        readonly Ticket EmptyTicket = new Ticket();
        readonly Option ISOA4 = new Option(Psk.ISOA4);
        readonly Option NorthAmericaLetter = new Option(Psk.NorthAmericaLetter);

        [Fact]
        public void SetOption()
        {
            var pt = EmptyTicket.Set(Psk.PageMediaSize, ISOA4);
            var op = pt[Psk.PageMediaSize];
            Assert.NotNull(op);
            Assert.Equal(1, op.Count);
            Assert.Equal(Psk.ISOA4, op[0].Name);
        }

        [Fact]
        public void OverwriteOptionOfFeatureAlreadyExisted()
        {
            var pt = EmptyTicket.Set(Psk.PageMediaSize, ISOA4);

            var overwitten = pt.Set(Psk.PageMediaSize, NorthAmericaLetter);
            var op = overwitten[Psk.PageMediaSize];
            Assert.NotNull(op);
            Assert.Equal(1, op.Count);
            Assert.Equal(Psk.NorthAmericaLetter, op[0].Name);
        }

        [Fact]
        public void SetValueToParameter()
        {
            var pt = EmptyTicket.Set(Psk.JobCopiesAllDocuments, 2);
            var copies = pt[Psk.JobCopiesAllDocuments];
            Assert.NotNull(copies);
            Assert.Equal(copies, 2);
        }

        [Fact]
        public void OverwriteParameterAlreadyExisted()
        {
            var pt = EmptyTicket.Set(Psk.JobCopiesAllDocuments, 2);

            var overwitten = pt.Set(Psk.JobCopiesAllDocuments, 3);
            var copies = overwitten[Psk.JobCopiesAllDocuments];
            Assert.NotNull(copies);
            Assert.Equal(copies, 3);
        }

        [Fact]
        public void SetValueToProperty()
        {
            var pt = EmptyTicket.Set(Psk.JobID, "Some job name");
            var prop = pt[Psk.JobID];
            Assert.NotNull(prop);
            Assert.Equal(prop, "Some job name");
        }

        [Fact]
        public void OverwritePropertyAlreadyExisted()
        {
            var pt = EmptyTicket.Set(Psk.JobID, "Some job name");

            var overwitten = pt.Set(Psk.JobCopiesAllDocuments, "New job name");
            var prop = overwitten[Psk.JobCopiesAllDocuments];
            Assert.NotNull(prop);
            Assert.Equal(prop, "New job name");
        }
    }
}
