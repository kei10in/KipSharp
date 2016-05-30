using System;
using System.Xml.Linq;

namespace Kip
{
    public sealed class NamespaceDeclaration : IEquatable<NamespaceDeclaration>
    {
        public string Prefix { get; }
        public XNamespace Uri { get; }

        public NamespaceDeclaration(string prefix, XNamespace uri)
        {
            Prefix = prefix;
            Uri = uri;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as NamespaceDeclaration);
        }

        public bool Equals(NamespaceDeclaration rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Prefix.GetHashCode() ^ Uri.GetHashCode();
        }

        public static bool operator ==(NamespaceDeclaration v1, NamespaceDeclaration v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Prefix == v2.Prefix && v1.Uri == v2.Uri;
        }

        public static bool operator !=(NamespaceDeclaration v1, NamespaceDeclaration v2)
        {
            return !(v1 == v2);
        }
    }
}
