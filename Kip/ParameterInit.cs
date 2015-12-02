using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ParameterInit element defined in the Print Schema
    /// Specificaiton.
    /// </summary>
    public sealed class ParameterInit
    {
        public ParameterInit(XName name, Value value)
        {
            Name = name;
            Value = value;
        }

        public XName Name
        {
            get;
        }

        public Value Value
        {
            get;
        }

        public ParameterInit SetValue(Value value)
        {
            return new ParameterInit(Name, value);
        }
    }
}