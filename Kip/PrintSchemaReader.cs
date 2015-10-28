using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using psf = Kip.PrintSchemaFramework;
using xsi = Kip.XmlSchemaInstance;
using xsd = Kip.XmlSchema;
using System.IO;

namespace Kip
{
    public static class PrintSchemaReader
    {
        public static Capabilities Read(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    break;
                }
            }

            var tagName = reader.XName();
            if (tagName != psf.PrintCapabilities)
                return null;

            var pc = new Capabilities();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.XName() == psf.Property)
                    {
                        var prop = ReadProperty(reader);
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
            }

            return pc;
        }

        public static IEnumerable<PrintSchemaElement> ReadChildren(XmlReader reader)
        {
            List<PrintSchemaElement> result = new List<PrintSchemaElement>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    result.Add(ReadElement(reader));
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    return result;
                }
            }

            return null;
        }

        public static PrintSchemaElement ReadElement(XmlReader reader)
        {
            XName tagName = reader.XName();
            if (tagName == psf.Feature) return ReadFeature(reader);
            else if (tagName == psf.Option) return ReadOption(reader);
            else if (tagName == psf.ParameterDef)
            {
                reader.Skip();
            }
            else if (tagName == psf.ParameterInit)
            {
                reader.Skip();
            }
            else if (tagName == psf.ParameterRef)
            {
                reader.Skip();
            }
            else if (tagName == psf.Property) return ReadProperty(reader);
            else if (tagName == psf.Value) return ReadValue(reader);
            else reader.Skip();
            return null;
        }

        public static PrintSchemaElement ReadFeature(XmlReader reader)
        {
            reader.Skip();
            return null;
        }

        public static PrintSchemaElement ReadOption(XmlReader reader)
        {
            reader.Skip();
            return null;
        }

        public static PrintSchemaElement ReadProperty(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
            {
                reader.Skip();
                return null;
            }

            var name = reader.ValueAsXName();
            var property = new PrintSchemaProperty(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(property);
            }

            return property;
        }

        private static PrintSchemaElement ReadValue(XmlReader reader)
        {
            XName type = null;

            if (!reader.MoveToFirstAttribute()) return null;
            do
            {
                var attrName = reader.XName();
                if (attrName == xsi.Type)
                {
                    type = reader.ValueAsXName();
                }
            } while (reader.MoveToNextAttribute());

            // move to text node
            reader.Read();

            PrintSchemaValue value = null;

            if (type == xsd.Integer)
            {
                value = new PrintSchemaValue(reader.Value.AsInt32());
            }
            else if (type == xsd.Decimal)
            {
                value = new PrintSchemaValue(reader.Value.AsFloat());
            }
            else if (type == xsd.QName)
            {
                value = new PrintSchemaValue(reader.ValueAsXName());
            }
            else
            {
                value = new PrintSchemaValue(reader.Value);
            }

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    reader.Skip();
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    break;
                }
            }

            return value;
        }
    }

    public class InvalidChildElementException : Exception
    {
        public InvalidChildElementException()
        {
        }

        public InvalidChildElementException(string message)
            : base(message)
        {
        }

        public InvalidChildElementException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public interface PrintSchemaElement
    {
        void Add(Feature feature);

        void Add(Option feature);

        void Add(ParameterDef parameterDef);

        void Add(ParameterInit parameterInit);

        void Add(ParameterRef parameterRef);

        void Add(Property property);

        void Add(ScoredProperty scoredProperty);

        void Add(Value value);

        void AddTo(PrintSchemaElement element);
    }

    public abstract class DefaultPrintSchemaElement : PrintSchemaElement
    {
        public abstract string TagName
        {
            get;
        }

        public virtual void Add(Feature feature)
        {
            throw new InvalidChildElementException($"{TagName} can't contain Feature");
        }

        public virtual void Add(ParameterDef parameterDef)
        {
            throw new InvalidChildElementException($"{TagName} can't contain ParameterDef");
        }

        public virtual void Add(ParameterRef parameterRef)
        {
            throw new InvalidChildElementException($"{TagName} can't contain ParameterRef");
        }

        public virtual void Add(ScoredProperty scoredProperty)
        {
            throw new InvalidChildElementException($"{TagName} can't contain ScoredProperty");
        }

        public virtual void Add(Value value)
        {
            throw new InvalidChildElementException($"{TagName} can't contain Value");
        }

        public virtual void Add(Property property)
        {
            throw new InvalidChildElementException($"{TagName} can't contain Property");
        }

        public virtual void Add(ParameterInit parameterInit)
        {
            throw new InvalidChildElementException($"{TagName} can't contain ParameterInit");
        }

        public virtual void Add(Option feature)
        {
            throw new InvalidChildElementException($"{TagName} can't contain Option");
        }

        public virtual void AddTo(PrintSchemaElement element)
        {
            throw new NotImplementedException();
        }
    }

    public class PrintSchemaCapabilities : DefaultPrintSchemaElement
    {
        public Capabilities Result
        {
            get;
        }

        public override string TagName
        {
            get
            {
                return psf.PrintCapabilities.LocalName;
            }
        }

        public override void Add(Feature feature)
        {
            throw new NotImplementedException();
        }

        public override void Add(ParameterDef parameterDef)
        {
            throw new NotImplementedException();
        }

        public override void Add(Property property)
        {
            Result.Add(property);
        }
    }

    public class PrintSchemaProperty : DefaultPrintSchemaElement
    {
        private Property _property;

        public PrintSchemaProperty(XName name)
        {
            _property = new Property(name);
        }

        public override string TagName
        {
            get
            {
                return psf.Property.LocalName;
            }
        }

        public override void Add(Property property)
        {
            throw new NotImplementedException();
        }

        public override void Add(Value value)
        {
            _property.Value = value;
        }
    }

    public class PrintSchemaValue : DefaultPrintSchemaElement
    {
        private Value _value;

        public PrintSchemaValue(Value value)
        {
            _value = value;
        }

        public override string TagName
        {
            get
            {
                return psf.Value.LocalName;
            }
        }
    }
}
