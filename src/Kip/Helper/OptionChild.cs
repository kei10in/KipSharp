using System;

namespace Kip.Helper
{
    public sealed class OptionChild
    {
        private Element _holder;

        private OptionChild(Element holder)
        {
            _holder = holder;
        }

        internal void Apply(
            Action<Property> onProperty,
            Action<ScoredProperty> onScoredProperty)
        {
            _holder.Apply(
                onProperty: onProperty,
                onScoredProperty: onScoredProperty);
        }

        public static implicit operator OptionChild(Property element)
        {
            return new OptionChild(element);
        }

        public static implicit operator OptionChild(ScoredProperty element)
        {
            return new OptionChild(element);
        }
    }
}
