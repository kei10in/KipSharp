using System;
using System.Diagnostics;
using System.Linq;

namespace Kip
{
    internal sealed class ImmutableNamedElementCollectionDebugView<T>
        where T : class
    {
        private ImmutableNamedElementCollection<T> nec;

        public ImmutableNamedElementCollectionDebugView(ImmutableNamedElementCollection<T> namedElementCollection)
        {
            if (namedElementCollection == null)
            {
                throw new ArgumentNullException(nameof(namedElementCollection));
            }

            nec = namedElementCollection;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items
        {
            get
            {
                return nec.ToArray();
            }
        }
    }
}
