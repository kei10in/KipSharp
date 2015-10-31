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

            var pc = new PrintSchemaCapabilities();

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(pc);
            }

            return pc.Result;
        }

        public static IEnumerable<PrintSchemaChildElement> ReadChildren(XmlReader reader)
        {
            reader.MoveToElement();  // reader mights points Attribute
            if (reader.IsEmptyElement)
            {
                // exactly no children
                return Enumerable.Empty<PrintSchemaChildElement>();
            }

            List<PrintSchemaChildElement> result = new List<PrintSchemaChildElement>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    var e = ReadElement(reader);
                    if (e != null)
                        result.Add(e);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    return result;
                }
            }

            return Enumerable.Empty<PrintSchemaChildElement>();
        }

        public static PrintSchemaChildElement ReadElement(XmlReader reader)
        {
            XName tagName = reader.XName();
            if (tagName == psf.Feature) return ReadFeature(reader);
            else if (tagName == psf.Option) return ReadOption(reader);
            else if (tagName == psf.ParameterDef) return ReadParameterDef(reader);
            else if (tagName == psf.ParameterInit) return ReadParameterInit(reader);
            else if (tagName == psf.ParameterRef) return ReadParameterRef(reader);
            else if (tagName == psf.Property) return ReadProperty(reader);
            else if (tagName == psf.ScoredProperty) return ReadScoredProperty(reader);
            else if (tagName == psf.Value) return ReadValue(reader);
            else reader.Skip();
            return null;
        }

        public static PrintSchemaChildElement ReadFeature(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("Feature element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaFeature(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        public static PrintSchemaChildElement ReadOption(XmlReader reader)
        {
            XName name = null;
            if (reader.MoveToAttribute("name"))
                name = reader.ValueAsXName();

            XName constrained = null;
            if (reader.MoveToAttribute("constrained"))
                constrained = reader.ValueAsXName();

            var element = new PrintSchemaOption(name, constrained);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        public static PrintSchemaChildElement ReadParameterDef(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ParameterDef element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaParameterDef(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        public static PrintSchemaChildElement ReadParameterInit(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ParameterInit element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaParameterInit(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        public static PrintSchemaChildElement ReadParameterRef(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ParameterRef element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaParameterRef(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        public static PrintSchemaChildElement ReadProperty(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("Property element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaProperty(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        public static PrintSchemaChildElement ReadScoredProperty(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ScoredProperty element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaScoredProperty(name);

            foreach (var child in ReadChildren(reader))
            {
                child.AddTo(element);
            }

            return element;
        }

        private static PrintSchemaChildElement ReadValue(XmlReader reader)
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

    public interface PrintSchemaChildElement
    {
        void AddTo(PrintSchemaElement element);
    }


    public interface PrintSchemaElement
    {
        void Add(Feature feature);

        void Add(Option option);

        void Add(ParameterDef parameterDef);

        void Add(ParameterInit parameterInit);

        void Add(ParameterRef parameterRef);

        void Add(Property property);

        void Add(ScoredProperty scoredProperty);

        void Add(Value value);
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

        public virtual void Add(Option option)
        {
            throw new InvalidChildElementException($"{TagName} can't contain Option");
        }
    }

    public class PrintSchemaCapabilities : DefaultPrintSchemaElement
    {
        public PrintSchemaCapabilities()
        {
            Result = new Capabilities();
        }

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
            Result.Add(feature);
        }

        public override void Add(ParameterDef parameterDef)
        {
            Result.Add(parameterDef);
        }

        public override void Add(Property property)
        {
            Result.Add(property);
        }
    }

    public class PrintSchemaFeature : DefaultPrintSchemaElement, PrintSchemaChildElement
    {
        private Feature _feature;

        public PrintSchemaFeature(XName name)
        {
            _feature = new Feature(name);
        }

        public override string TagName
        {
            get
            {
                return psf.Feature.LocalName;
            }
        }

        public override void Add(Feature feature)
        {
            _feature.Add(feature);
        }

        public override void Add(Option option)
        {
            _feature.Add(option);
        }

        public override void Add(Property property)
        {
            _feature.Add(property);
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_feature);
        }
    }

    public class PrintSchemaOption : DefaultPrintSchemaElement, PrintSchemaChildElement
    {
        private Option _option;

        public PrintSchemaOption(XName name, XName constrained)
        {
            _option = new Option(name, constrained);
        }

        public override string TagName
        {
            get
            {
                return psf.Option.LocalName;
            }
        }

        public override void Add(Property property)
        {
            _option.Add(property);
        }

        public override void Add(ScoredProperty scoredProperty)
        {
            _option.Add(scoredProperty);
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_option);
        }
    }

    public class PrintSchemaParameterDef : DefaultPrintSchemaElement, PrintSchemaChildElement
    {
        private ParameterDef _parameterDef;

        public PrintSchemaParameterDef(XName name)
        {
            _parameterDef = new ParameterDef(name);
        }

        public override string TagName
        {
            get
            {
                return psf.ParameterDef.LocalName;
            }
        }

        public override void Add(Property property)
        {
            _parameterDef.Add(property);
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_parameterDef);
        }
    }

    public class PrintSchemaParameterInit : DefaultPrintSchemaElement, PrintSchemaChildElement
    {
        private ParameterInit _parameterInit;

        public PrintSchemaParameterInit(XName name)
        {
            _parameterInit = new ParameterInit(name);
        }

        public override string TagName
        {
            get
            {
                return psf.ParameterDef.LocalName;
            }
        }

        public override void Add(Value value)
        {
            _parameterInit.Value = value;
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_parameterInit);
        }
    }

    public class PrintSchemaParameterRef : DefaultPrintSchemaElement, PrintSchemaChildElement
    {
        private ParameterRef _parameterRef;

        public PrintSchemaParameterRef(XName name)
        {
            _parameterRef = new ParameterRef(name);
        }

        public override string TagName
        {
            get
            {
                return psf.ParameterDef.LocalName;
            }
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_parameterRef);
        }
    }

    public class PrintSchemaProperty : DefaultPrintSchemaElement, PrintSchemaChildElement
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
            _property.Add(property);
        }

        public override void Add(Value value)
        {
            _property.Value = value;
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_property);
        }
    }

    public class PrintSchemaScoredProperty : DefaultPrintSchemaElement, PrintSchemaChildElement
    {
        private ScoredProperty _scoredProperty;

        public PrintSchemaScoredProperty(XName name)
        {
            _scoredProperty = new ScoredProperty(name);
        }

        public override string TagName
        {
            get
            {
                return psf.Property.LocalName;
            }
        }

        public override void Add(ParameterRef parameterRef)
        {
            _scoredProperty.ParameterRef = parameterRef;
        }

        public override void Add(Property property)
        {
            _scoredProperty.Add(property);
        }

        public override void Add(ScoredProperty scoredProperty)
        {
            _scoredProperty.Add(scoredProperty);
        }

        public override void Add(Value value)
        {
            _scoredProperty.Value = value;
        }

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_scoredProperty);
        }
    }

    public class PrintSchemaValue : DefaultPrintSchemaElement, PrintSchemaChildElement
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

        public void AddTo(PrintSchemaElement element)
        {
            element.Add(_value);
        }
    }
}
