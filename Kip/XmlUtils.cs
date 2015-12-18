using System;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    internal static class XmlUtils
    {
        internal static XName ToXName(this string self, XmlReader reader)
        {
            var i = self.IndexOf(':');
            if (i == 0 || i == self.Length - 1)
            {
                throw new XmlException($"\"{self}\" is not QName.");
            }

            var prefix = string.Empty;
            var localPart = string.Empty;

            if (i < 0)
            {
                localPart = self;
            }
            else
            {
                prefix = self.Substring(0, i);
                localPart = self.Substring(i + 1);
            }

            XNamespace ns = reader.LookupNamespace(prefix);
            if (ns == null)
            {
                throw new XmlException($"The prefix \"{prefix}\" is not declared.");
            }

            return ns + localPart;
        }

        internal static string ToQName(this XName self, XmlWriter writer)
        {
            var prefix = writer.LookupPrefix(self.NamespaceName);
            if (prefix == null)
            {
                throw new InternalException($"Namespace declaration not found: {self.Namespace}");
            }
            else if (prefix == string.Empty)
            {
                return self.LocalName;
            }
            else
            {
                return prefix + ":" + self.LocalName;
            }
        }

        internal static XName XName(this XmlReader self)
        {
            XNamespace ns = self.NamespaceURI;
            return ns + self.LocalName;
        }

        internal static XName ValueAsXName(this XmlReader self)
        {
            return self.Value.ToXName(self);
        }
    }
}
