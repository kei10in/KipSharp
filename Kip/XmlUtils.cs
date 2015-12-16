using System;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    internal static class XmlUtils
    {
        internal static XName ToXName(this string self, XmlReader reader)
        {
            char[] separater = { ':' };
            var separeted = self.Split(separater, 2);
            if (separeted.Length != 2)
            {
                return self;
            }

            var prefix = separeted[0];
            var name = separeted[1];

            XNamespace ns = reader.LookupNamespace(prefix);
            return ns + name;
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
