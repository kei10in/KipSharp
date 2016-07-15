using System;
using System.Diagnostics;
using System.Xml.Linq;

namespace Kip
{
    [DebuggerDisplay("Name = {LocalName}")]
    public class PrintSchemaName : IEquatable<PrintSchemaName>
    {
        private XName _name;

        public PrintSchemaName(XName name)
        {
            _name = name;
        }

        public string LocalName
        {
            get { return _name.LocalName; }
        }

        public XNamespace Namespace
        {
            get { return _name.Namespace; }
        }

        public string NamespaceName
        {
            get { return _name.NamespaceName; }
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PrintSchemaName);
        }

        public bool Equals(PrintSchemaName rhs)
        {
            return this == rhs;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        public static bool operator ==(PrintSchemaName v1, PrintSchemaName v2)
        {
            if (ReferenceEquals(v1, v2)) return true;
            if ((object)v1 == null || (object)v2 == null) return false;

            return v1._name == v2._name;
        }

        public static bool operator !=(PrintSchemaName v1, PrintSchemaName v2)
        {
            return !(v1 == v2);
        }

        public static implicit operator XName(PrintSchemaName name)
        {
            return name._name;
        }
    }

    public class FeatureName : PrintSchemaName, IEquatable<FeatureName>
    {
        public FeatureName(XName name) : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as FeatureName);
        }

        public bool Equals(FeatureName rhs)
        {
            if ((object)rhs == null) return false;

            return base.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(FeatureName lhs, FeatureName rhs)
        {
            if ((object)lhs == null)
            {
                return (object)rhs == null;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(FeatureName v1, FeatureName v2)
        {
            return !(v1 == v2);
        }
    }

    public class ParameterName : PrintSchemaName, IEquatable<ParameterName>
    {
        public ParameterName(XName name) : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ParameterName);
        }

        public bool Equals(ParameterName rhs)
        {
            if ((object)rhs == null) return false;

            return base.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ParameterName lhs, ParameterName rhs)
        {
            if ((object)lhs == null)
            {
                return (object)rhs == null;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ParameterName v1, ParameterName v2)
        {
            return !(v1 == v2);
        }
    }

    public class ScoredPropertyName : PrintSchemaName, IEquatable<ScoredPropertyName>
    {
        public ScoredPropertyName(XName name) : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ScoredPropertyName);
        }

        public bool Equals(ScoredPropertyName rhs)
        {
            if ((object)rhs == null) return false;

            return base.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(ScoredPropertyName lhs, ScoredPropertyName rhs)
        {
            if ((object)lhs == null)
            {
                return (object)rhs == null;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(ScoredPropertyName v1, ScoredPropertyName v2)
        {
            return !(v1 == v2);
        }
    }

    public class PropertyName : PrintSchemaName, IEquatable<PropertyName>
    {
        public PropertyName(XName name) : base(name)
        {
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as PropertyName);
        }

        public bool Equals(PropertyName rhs)
        {
            if ((object)rhs == null) return false;

            return base.Equals(rhs);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(PropertyName v1, PropertyName v2)
        {
            if ((object)v1 == null)
            {
                return (object)v2 == null;
            }

            return v1.Equals(v2);
        }

        public static bool operator !=(PropertyName v1, PropertyName v2)
        {
            return !(v1 == v2);
        }
    }
}
