using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kip.Helper
{
    /// <summary>
    /// Wrapper class that representing a child elements of the PrintTicket document.
    /// </summary>
    public sealed class TicketChild
    {
        private Element _holder;

        private TicketChild(Element holder)
        {
            _holder = holder;
        }

        internal void Apply(
            Action<Feature> onFeature,
            Action<ParameterInit> onParameterInit,
            Action<Property> onProperty)
        {
            _holder.Apply(
                onFeature: onFeature,
                onParameterInit: onParameterInit,
                onProperty: onProperty);
        }

        public static implicit operator TicketChild(Feature element)
        {
            return new TicketChild(element);
        }

        public static implicit operator TicketChild(ParameterInit element)
        {
            return new TicketChild(element);
        }

        public static implicit operator TicketChild(Property element)
        {
            return new TicketChild(element);
        }
    }
}
