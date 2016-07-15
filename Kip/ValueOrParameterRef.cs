using System;
using System.Xml.Linq;

namespace Kip
{
    public class ValueOrParameterRef
    {
        private object _content;

        public ValueOrParameterRef(Value value)
        {
            _content = value;
        }

        public ValueOrParameterRef(ParameterRef paramterRef)
        {
            _content = paramterRef;
        }

        public XName Type
        {
            get
            {
                var type = _content.GetType();
                if (type == typeof(Value))
                {
                    return Psf.Feature;
                }
                else if (type == typeof(ParameterRef))
                {
                    return Psf.ParameterRef;
                }
                else
                {
                    throw new InvalidOperationException("Unexpected type.");
                }
            }
        }

        public Value AsValue()
        {
            return _content as Value;
        }

        public ParameterRef AsParamterRef()
        {
            return _content as ParameterRef;
        }
    }
}
