using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a ParameterRef element defined in the Print Schema
    /// Specification.
    /// </summary>
    [DebuggerDisplay("{Name.LocalName}: ParameterRef")]
    public sealed class ParameterRef : IEquatable<ParameterRef>
    {
        /// <summary>
        /// Constructs with the name.
        /// </summary>
        /// <param name="name"></param>
        public ParameterRef(ParameterName name)
        {
            Name = name;
        }

        public ParameterName Name
        {
            get;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ParameterRef);
        }

        public bool Equals(ParameterRef rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(ParameterRef v1, ParameterRef v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1.Name == v2.Name;
        }

        public static bool operator !=(ParameterRef v1, ParameterRef v2)
        {
            return !(v1 == v2);
        }
    }
}