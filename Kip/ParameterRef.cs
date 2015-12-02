using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ParameterRef element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterRef
    {
        public ParameterRef(XName name)
        {
            Name = name;
        }

        public XName Name
        {
            get;
        }
    }
}