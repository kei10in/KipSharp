using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ParameterInit element defined in the Print Schema
    /// Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ParameterInit")]
    public sealed class ParameterInit : IEquatable<ParameterInit>
    {
        /// <summary>
        /// Constructs with the name and the <see cref="Value"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public ParameterInit(ParameterName name, Value value)
        {
            Name = name;
            Value = value;
        }

        public ParameterName Name
        {
            get;
        }

        public Value Value
        {
            get;
        }

        /// <summary>
        /// Sets the specified <see cref="Value"/> to the ParameterInit.
        /// </summary>
        /// <returns>A new ParameterInit with the value set.</returns>
        public ParameterInit Set(Value value)
        {
            return new ParameterInit(Name, value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParameterInit);
        }

        public bool Equals(ParameterInit rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^
                Value?.GetHashCode() ?? 0;
        }

        public static bool operator ==(ParameterInit v1, ParameterInit v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name &&
                v1.Value == v2.Value;
        }

        public static bool operator !=(ParameterInit v1, ParameterInit v2)
        {
            return !(v1 == v2);
        }
    }
}