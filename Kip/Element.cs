using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kip
{
    internal interface ElementHolder
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

    internal class FeatureHolder : ElementHolder
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

    internal class OptionHolder : ElementHolder
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

    internal class ParameterDefHolder : ElementHolder
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

    internal class ParameterInitHolder : ElementHolder
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

    internal class ParameterRefHolder : ElementHolder
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

    internal class PropertyHolder : ElementHolder
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

    internal class ScoredPropertyHolder : ElementHolder
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

    internal class ValueHolder : ElementHolder
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
}
