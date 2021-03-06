﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Xml.Linq;

namespace Kip
{
    public interface IReadOnlyNamespaceDeclarationCollection : IReadOnlyCollection<NamespaceDeclaration>
    {
        XNamespace LookupNamespace(string prefix);
        string LookupPrefix(XNamespace uri);
        bool ContainsPrefix(string prefix);
        bool ContainsNamespace(XNamespace uri);
    }

    public sealed class NamespaceDeclarationCollection : IReadOnlyNamespaceDeclarationCollection
    {
        private ImmutableDictionary<string, XNamespace> _map
            = ImmutableDictionary.Create<string, XNamespace>();

        public static NamespaceDeclarationCollection Empty = new NamespaceDeclarationCollection();
        public static NamespaceDeclarationCollection Default;

        static NamespaceDeclarationCollection()
        {
            var map = ImmutableDictionary.CreateBuilder<string, XNamespace>();
            map.Add(Psf.Prefix, Psf.Namespace);
            map.Add(Xsi.Prefix, Xsi.Namespace);
            map.Add(Xsd.Prefix, Xsd.Namespace);
            map.Add(Psk.Prefix, Psk.Namespace);
            map.Add(Pskv11.Prefix, Pskv11.Namespace);

            Default = new NamespaceDeclarationCollection(map.ToImmutableDictionary());
        }

        private NamespaceDeclarationCollection() { }

        private NamespaceDeclarationCollection(ImmutableDictionary<string, XNamespace> map)
        {
            _map = map;
        }

        internal NamespaceDeclarationCollection(IEnumerable<NamespaceDeclaration> declarations)
        {
            var builder = ImmutableDictionary.CreateBuilder<string, XNamespace>();
            foreach (var decl in declarations)
            {
                builder.Add(decl.Prefix, decl.Uri);
            }
            _map = builder.ToImmutableDictionary();
        }

        public int Count
        {
            get { return _map.Count; }
        }

        public XNamespace LookupNamespace(string prefix)
        {
            return _map.GetValueOrDefault(prefix);
        }

        public string LookupPrefix(XNamespace uri)
        {
            foreach (var item in _map)
            {
                if (uri == item.Value)
                {
                    return item.Key;
                }
            }
            return null;
        }

        public bool ContainsPrefix(string prefix)
        {
            return _map.ContainsValue(prefix);
        }

        public bool ContainsNamespace(XNamespace uri)
        {
            return _map.ContainsValue(uri);
        }

        public NamespaceDeclarationCollection Add(NamespaceDeclaration declaration)
        {
            if (ContainsPrefix(declaration.Prefix))
            {
                throw new ArgumentException($"\"{declaration.Prefix}\" is already declared.", nameof(declaration));
            }

            return Add(declaration.Prefix, declaration.Uri);
        }

        public NamespaceDeclarationCollection Add(string prefix, XNamespace uri)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));
            if (uri == null) throw new ArgumentNullException(nameof(uri));

            if (ContainsPrefix(prefix))
            {
                throw new ArgumentException($"\"{prefix}\" is already declared.", nameof(prefix));
            }

            return new NamespaceDeclarationCollection(_map.Add(prefix, uri));
        }

        public NamespaceDeclarationCollection Remove(string prefix)
        {
            if (prefix == null) throw new ArgumentNullException(nameof(prefix));

            return new NamespaceDeclarationCollection(_map.Remove(prefix));
        }

        public IEnumerator<NamespaceDeclaration> GetEnumerator()
        {
            return _map
                .Select(kv => new NamespaceDeclaration(kv.Key, kv.Value))
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map
                .Select(kv => new NamespaceDeclaration(kv.Key, kv.Value))
                .GetEnumerator();
        }
    }
}
