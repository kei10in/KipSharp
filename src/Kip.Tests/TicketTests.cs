﻿using System;
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
            Tuple.Create(new Ticket(new Feature(Exp.SomeFeature)), new Ticket(new Feature(Exp.SomeFeature))),
            Tuple.Create(new Ticket(new Property(Exp.SomeProperty)), new Ticket(new Property(Exp.SomeProperty))),
            Tuple.Create(new Ticket(new ParameterInit(Exp.SomeParameter, 1)), new Ticket(new ParameterInit(Exp.SomeParameter, 1))),
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
            Tuple.Create(new Ticket(), new Ticket(new Feature(Exp.SomeFeature))),
            Tuple.Create(new Ticket(new Feature(Exp.SomeFeature)), new Ticket(new Feature(Exp.OtherFeature))),
            Tuple.Create(new Ticket(), new Ticket(new Property(Exp.SomeProperty))),
            Tuple.Create(new Ticket(new Property(Exp.SomeProperty)), new Ticket(new Property(Exp.OtherProperty))),
            Tuple.Create(new Ticket(), new Ticket(new ParameterInit(Exp.SomeParameter, 1))),
            Tuple.Create(new Ticket(new ParameterInit(Exp.SomeParameter, 1)), new Ticket(new ParameterInit(Exp.OtherParameter, 2))),
        };

        [Fact]
        public void EqualsOpeartorAndFalse()
        {
            foreach (var pair in _notEqualsPair)
            {
                Assert.NotEqual(pair.Item1, pair.Item2);
            }
        }

        private readonly Ticket EmptyTicket = new Ticket();
        private readonly Option ISOA4 = new Option(Psk.ISOA4);
        private readonly Option NorthAmericaLetter = new Option(Psk.NorthAmericaLetter);

        [Fact]
        public void SetOption()
        {
            var pt = EmptyTicket.Set(Psk.PageMediaSize, ISOA4);
            var op = pt.Get(Psk.PageMediaSize);
            Assert.NotNull(op);
            Assert.Equal(1, op.Count);
            Assert.Equal(Psk.ISOA4, op[0].Name);
        }

        [Fact]
        public void OverwriteOptionOfFeatureAlreadyExisted()
        {
            var pt = EmptyTicket.Set(Psk.PageMediaSize, ISOA4);

            var overwitten = pt.Set(Psk.PageMediaSize, NorthAmericaLetter);
            var op = overwitten.Get(Psk.PageMediaSize);
            Assert.NotNull(op);
            Assert.Equal(1, op.Count);
            Assert.Equal(Psk.NorthAmericaLetter, op[0].Name);
        }

        [Fact]
        public void SetOptionToNestedFeature()
        {
            var pd = new Option(Psk.LeftBottom);
            var pt = EmptyTicket.Set(Psk.JobNUpAllDocumentsContiguously, Psk.PresentationDirection, pd);
            var op = pt.Get(Psk.JobNUpAllDocumentsContiguously, Psk.PresentationDirection);
            Assert.NotNull(op);
            Assert.Equal(1, op.Count);
            Assert.Equal(Psk.LeftBottom, op[0].Name);
        }

        [Fact]
        public void SetValueToParameter()
        {
            var pt = EmptyTicket.Set(Psk.JobCopiesAllDocuments, 2);
            var copies = pt.Get(Psk.JobCopiesAllDocuments);
            Assert.NotNull(copies);
            Assert.Equal(copies, 2);
        }

        [Fact]
        public void OverwriteParameterAlreadyExisted()
        {
            var pt = EmptyTicket.Set(Psk.JobCopiesAllDocuments, 2);

            var overwitten = pt.Set(Psk.JobCopiesAllDocuments, 3);
            var copies = overwitten.Get(Psk.JobCopiesAllDocuments);
            Assert.NotNull(copies);
            Assert.Equal(copies, 3);
        }

        [Fact]
        public void SetValueToProperty()
        {
            var pt = EmptyTicket.Set(Psk.JobID, "Some job name");
            var prop = pt.Get(Psk.JobID);
            Assert.NotNull(prop);
            Assert.Equal(prop, "Some job name");
        }

        [Fact]
        public void OverwritePropertyAlreadyExisted()
        {
            var pt = EmptyTicket.Set(Psk.JobID, "Some job name");

            var overwitten = pt.Set(Psk.JobCopiesAllDocuments, "New job name");
            var prop = overwitten.Get(Psk.JobCopiesAllDocuments);
            Assert.NotNull(prop);
            Assert.Equal(prop, "New job name");
        }

        [Fact]
        public void RemoveInvalidPropertyFromOption()
        {
            var op = new Option(Psk.ISOA4, Psk.PrintTicketSettings,
                new Property(Psk.DisplayName, "A4"),
                new Property(Psk.DisplayUI, "Show"),
                new Property(Psf.IdentityOption, "True"));
            var pt = EmptyTicket.Set(Psk.PageMediaSize, op);
            var actual = pt.Get(Psk.PageMediaSize)[0];

            Assert.NotNull(actual);
            Assert.Null(actual.Constrained);
            Assert.Null(actual.Get(Psk.DisplayName));
            Assert.Null(actual.Get(Psk.DisplayUI));
            Assert.Null(actual.Get(Psf.IdentityOption));
        }
    }
}
