using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kip
{
    internal class Element
    {
        private ElementHolder _holder;

        private Element(ElementHolder holder)
        {
            _holder = holder;
        }

        internal void Apply(
            Action<Feature> onFeature = null,
            Action<Option> onOption = null,
            Action<ParameterDef> onParameterDef = null,
            Action<ParameterInit> onParameterInit = null,
            Action<ParameterRef> onParameterRef = null,
            Action<Property> onProperty = null,
            Action<ScoredProperty> onScoredProperty = null,
            Action<Value> onValue = null)
        {
            _holder.Apply(
                onFeature: onFeature,
                onOption: onOption,
                onParameterDef: onParameterDef,
                onParameterInit: onParameterInit,
                onParameterRef: onParameterRef,
                onProperty: onProperty,
                onScoredProperty: onScoredProperty,
                onValue: onValue);
        }

        private interface ElementHolder
        {
            void Apply(
                Action<Feature> onFeature = null,
                Action<Option> onOption = null,
                Action<ParameterDef> onParameterDef = null,
                Action<ParameterInit> onParameterInit = null,
                Action<ParameterRef> onParameterRef = null,
                Action<Property> onProperty = null,
                Action<ScoredProperty> onScoredProperty = null,
                Action<Value> onValue = null);
        }

        private class FeatureHolder : ElementHolder
        {
            public Feature Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onFeature?.Invoke(Element);
            }
        }

        private class OptionHolder : ElementHolder
        {
            public Option Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onOption?.Invoke(Element);
            }
        }

        private class ParameterDefHolder : ElementHolder
        {
            public ParameterDef Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onParameterDef?.Invoke(Element);
            }
        }

        private class ParameterInitHolder : ElementHolder
        {
            public ParameterInit Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onParameterInit?.Invoke(Element);
            }
        }

        private class ParameterRefHolder : ElementHolder
        {
            public ParameterRef Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onParameterRef?.Invoke(Element);
            }
        }

        private class PropertyHolder : ElementHolder
        {
            public Property Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onProperty?.Invoke(Element);
            }
        }

        private class ScoredPropertyHolder : ElementHolder
        {
            public ScoredProperty Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onScoredProperty?.Invoke(Element);
            }
        }

        private class ValueHolder : ElementHolder
        {
            public Value Element { get; set; }

            void ElementHolder.Apply(
                Action<Feature> onFeature,
                Action<Option> onOption,
                Action<ParameterDef> onParameterDef,
                Action<ParameterInit> onParameterInit,
                Action<ParameterRef> onParameterRef,
                Action<Property> onProperty,
                Action<ScoredProperty> onScoredProperty,
                Action<Value> onValue)
            {
                onValue?.Invoke(Element);
            }
        }

        public static implicit operator Element(Feature element)
        {
            return new Element(new FeatureHolder() { Element = element });
        }

        public static implicit operator Element(Option element)
        {
            return new Element(new OptionHolder() { Element = element });
        }

        public static implicit operator Element(ParameterDef element)
        {
            return new Element(new ParameterDefHolder() { Element = element });
        }

        public static implicit operator Element(ParameterInit element)
        {
            return new Element(new ParameterInitHolder() { Element = element });
        }

        public static implicit operator Element(ParameterRef element)
        {
            return new Element(new ParameterRefHolder() { Element = element });
        }

        public static implicit operator Element(Property element)
        {
            return new Element(new PropertyHolder() { Element = element });
        }

        public static implicit operator Element(ScoredProperty element)
        {
            return new Element(new ScoredPropertyHolder() { Element = element });
        }

        public static implicit operator Element(Value element)
        {
            return new Element(new ValueHolder() { Element = element });
        }
    }
}
