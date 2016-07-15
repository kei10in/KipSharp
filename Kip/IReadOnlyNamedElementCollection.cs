using System.Collections.Generic;
using System.Xml.Linq;

namespace Kip
{
    public interface IReadOnlyNamedElementCollection<T>
        : IReadOnlyCollection<T>
        where T : class
    {
        T this[XName name] { get; }

        T Get(XName name);

        bool Contains(XName name);
    }
}
