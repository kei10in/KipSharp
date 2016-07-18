using System;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Represents a Value element defined in the Print Schema Specification.
    /// </summary>
    public sealed class Value : IEquatable<Value>
    {
        /// <summary>
        /// It represents empty value. This field is read only.
        /// </summary>
        public static readonly Value Empty = new Value();

        private readonly object _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Value"/> class.
        /// </summary>
        /// <param name="value">The integer value of the instance.</param>
        public Value(int? value)
        {
            _value = value.GetValueOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Value"/> class.
        /// </summary>
        /// <param name="value">The float value of the instance.</param>
        public Value(float? value)
        {
            _value = value.GetValueOrDefault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Value"/> class.
        /// </summary>
        /// <param name="value">The string value of the instance.</param>
        public Value(string value)
        {
            if (string.IsNullOrEmpty(value))
                _value = string.Empty;
            else
                _value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Value"/> class.
        /// </summary>
        /// <param name="value">The QName value of the instance.</param>
        public Value(XName value)
        {
            _value = value;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="Value"/> class from
        /// being created.
        /// </summary>
        private Value()
        {
            _value = null;
        }

        /// <summary>
        /// Gets the value type of this value as XName.
        /// </summary>
        public XName ValueType
        {
            get
            {
                var type = _value.GetType();
                if (type == typeof(int))
                    return Xsd.Integer;
                else if (type == typeof(float))
                    return Xsd.Decimal;
                else if (type == typeof(XName))
                    return Xsd.QName;
                else
                    return Xsd.String;
            }
        }

        /// <summary>
        /// Extract as integer value.
        /// </summary>
        /// <returns>Integer value if the value type is integer, otherwise null.</returns>
        public int? AsInt()
        {
            return _value as int?;
        }

        /// <summary>
        /// Extract as float value.
        /// </summary>
        /// <returns>Float value if the value type is float, otherwise null.</returns>
        public float? AsFloat()
        {
            return _value as float?;
        }

        /// <summary>
        /// Extract as QName value.
        /// </summary>
        /// <returns>QName value if the value type is QName, otherwise null.</returns>
        public XName AsXName()
        {
            return _value as XName;
        }

        /// <summary>
        /// Extract as string value.
        /// </summary>
        /// <returns>String value if the value type is string, otherwise null.</returns>
        public string AsString()
        {
            return _value as string;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Value);
        }

        public bool Equals(Value value)
        {
            return this == value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public static bool operator ==(Value v1, Value v2)
        {
            if (ReferenceEquals(v1, v2))
                return true;
            if ((object)v1 == null || (object)v2 == null)
                return false;

            if (v1.ValueType != v2.ValueType)
                return false;

            var type = v1.ValueType;

            if (type == Xsd.Integer)
                return v1.AsInt() == v2.AsInt();
            else if (type == Xsd.Decimal)
                return v1.AsFloat() == v2.AsFloat();
            else if (type == Xsd.QName)
                return v1.AsXName() == v2.AsXName();
            else
                return v1.AsString() == v2.AsString();
        }

        public static bool operator !=(Value v1, Value v2)
        {
            return !(v1 == v2);
        }

        public static implicit operator Value(int value)
        {
            return new Value(value);
        }

        public static implicit operator Value(float value)
        {
            return new Value(value);
        }

        public static implicit operator Value(XName value)
        {
            return new Value(value);
        }

        public static implicit operator Value(string value)
        {
            return new Value(value);
        }
    }
}