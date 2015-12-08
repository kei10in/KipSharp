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
    }
}
