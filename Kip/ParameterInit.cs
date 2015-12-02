﻿using System.Xml.Linq;

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