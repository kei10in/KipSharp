using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kip.Helper
{
    public sealed class FeatureChild
    {
        private Element _holder;

        private FeatureChild(Element holder)
        {
            _holder = holder;
        }

        internal void Apply(
            Action<Property> onProperty,
            Action<Option> onOption,
            Action<Feature> onFeature)
        {
            _holder.Apply(
                onProperty: onProperty,
                onOption: onOption,
                onFeature: onFeature);
        }

        public static implicit operator FeatureChild(Property element)
        {
            return new FeatureChild(element);
        }

        public static implicit operator FeatureChild(Option element)
        {
            return new FeatureChild(element);
        }

        public static implicit operator FeatureChild(Feature element)
        {
            return new FeatureChild(element);
        }
    }
}
