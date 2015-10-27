using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kip
{
    public static class XmlUtils
    {
        public static XName ToXName(this string self, XElement context)
        {
            char[] separater = { ':' };
            var separeted = self.Split(separater, 2);
            if (separeted.Length != 2)
            {
                return self;
            }

            var prefix = separeted[0];
            var name = separeted[1];

            var ns = context.GetNamespaceOfPrefix(prefix);
            return ns + name;
        }

        public static string ToQName(this XName self, XElement context)
        {
            return context.GetPrefixOfNamespace(self.Namespace) + ":" + self.LocalName;
        }
    }
}
