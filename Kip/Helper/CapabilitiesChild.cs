using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kip.Helper
{
    /// <summary>
    /// Wrapper class that representing a child elements of the PrintCapabilities document.
    /// </summary>
    public sealed class CapabilitiesChild
    {
        private Element _holder;

        private CapabilitiesChild(Element holder)
        {
            _holder = holder;
        }

        internal void Apply(
            Action<Feature> onFeature,
            Action<ParameterDef> onParameterDef,
            Action<Property> onProperty)
        {
            _holder.Apply(
                onFeature: onFeature,
                onParameterDef: onParameterDef,
                onProperty: onProperty);
        }

        public static implicit operator CapabilitiesChild(Feature element)
        {
            return new CapabilitiesChild(element);
        }

        public static implicit operator CapabilitiesChild(ParameterDef element)
        {
            return new CapabilitiesChild(element);
        }

        public static implicit operator CapabilitiesChild(Property element)
        {
            return new CapabilitiesChild(element);
        }
    }
}
