using System;
using System.Diagnostics;

namespace Kip
{
    /// <summary>
    /// Represents a <see cref="ParameterInit"/> element defined in the
    /// PrintSchema Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ParameterInit")]
    public sealed class ParameterInit : IEquatable<ParameterInit>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterInit"/> class.
        /// </summary>
        /// <param name="name">Name of the element.</param>
        /// <param name="value">Value of the element.</param>
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
        /// Sets the specified value to the <see cref="ParameterInit"/>.
        /// </summary>
        /// <returns>A new <see cref="ParameterInit"/> with the value set.</returns>
        /// <param name="value">The value to set .</param>
        public ParameterInit Set(Value value)
        {
            return new ParameterInit(Name, value);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ParameterInit);
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