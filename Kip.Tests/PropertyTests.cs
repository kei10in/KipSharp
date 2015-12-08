﻿using System;
using System.Collections.Generic;
using Xunit;

namespace Kip.Tests
{
    public class PropertyTests
    {
        [Fact]
        public void AddNestedProperty()
        {
            var nested = new Property("b");
            var property = new Property("a", nested);

            var found = property.NestedProperty("b");

            Assert.Equal(found, nested);
        }

        private List<Tuple<Property, Property>> _equalsPair = new List<Tuple<Property, Property>>()
        {
            Tuple.Create<Property, Property>(null, null),
            Tuple.Create(new Property("a"), new Property("a")),
            Tuple.Create(new Property("a", 1), new Property("a", 1)),
            Tuple.Create(new Property("a", new Property("b")), new Property("a", new Property("b"))),
        };

        [Fact]
        public void EqualsOpeartorAndTrue()
        {
            foreach (var pair in _equalsPair)
            {
                Assert.Equal(pair.Item1, pair.Item2);
            }
        }

        private List<Tuple<Property, Property>> _notEqualsPair = new List<Tuple<Property, Property>>()
        {
            Tuple.Create<Property, Property>(null, new Property("a")),
            Tuple.Create<Property, Property>(new Property("a"), null),
            Tuple.Create(new Property("a"), new Property("b")),
            Tuple.Create(new Property("a"), new Property("a", 1)),
            Tuple.Create(new Property("a", 1), new Property("a", 2)),
            Tuple.Create(new Property("a"), new Property("a", new Property("b"))),
            Tuple.Create(new Property("a", new Property("b")), new Property("a", new Property("c"))),
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
