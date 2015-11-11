using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Kip
{
    internal static class PrintSchemaReader
    {
        public static Capabilities ReadCapabilities(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    break;
                }
            }

            var tagName = reader.XName();
            if (tagName != Psf.PrintCapabilities)
                return null;

            var pc = new PrintSchemaCapabilities();

            foreach (var child in ReadChildren(reader))
            {
                pc.Add(child);
            }

            return pc.Result;
        }

        public static Ticket ReadTicket(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    break;
                }
            }

            var tagName = reader.XName();
            if (tagName != Psf.PrintTicket)
                return null;

            var pc = new PrintSchemaTicket();

            foreach (var child in ReadChildren(reader))
            {
                pc.Add(child);
            }

            return pc.Result;
        }

        public static IEnumerable<Element> ReadChildren(XmlReader reader)
        {
            reader.MoveToElement();  // reader mights points Attribute
            if (reader.IsEmptyElement)
            {
                // exactly no children
                return Enumerable.Empty<Element>();
            }

            List<Element> result = new List<Element>();

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

            return Enumerable.Empty<Element>();
        }

        public static Element ReadElement(XmlReader reader)
        {
            XName tagName = reader.XName();
            if (tagName == Psf.Feature) return ReadFeature(reader);
            else if (tagName == Psf.Option) return ReadOption(reader);
            else if (tagName == Psf.ParameterDef) return ReadParameterDef(reader);
            else if (tagName == Psf.ParameterInit) return ReadParameterInit(reader);
            else if (tagName == Psf.ParameterRef) return ReadParameterRef(reader);
            else if (tagName == Psf.Property) return ReadProperty(reader);
            else if (tagName == Psf.ScoredProperty) return ReadScoredProperty(reader);
            else if (tagName == Psf.Value) return ReadValue(reader);
            else reader.Skip();
            return null;
        }

        public static Element ReadFeature(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("Feature element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaFeature(name);

            foreach (var child in ReadChildren(reader))
            {
                element.Add(child);
            }

            return element.GetResult();
        }

        public static Element ReadOption(XmlReader reader)
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
                element.Add(child);
            }

            return element.GetResult();
        }

        public static Element ReadParameterDef(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ParameterDef element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaParameterDef(name);

            foreach (var child in ReadChildren(reader))
            {
                element.Add(child);
            }

            return element.GetResult();
        }

        public static Element ReadParameterInit(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ParameterInit element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaParameterInit(name);

            foreach (var child in ReadChildren(reader))
            {
                element.Add(child);
            }

            return element.GetResult();
        }

        public static Element ReadParameterRef(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ParameterRef element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaParameterRef(name);

            foreach (var child in ReadChildren(reader))
            {
                element.Add(child);
            }

            return element.GetResult();
        }

        public static Element ReadProperty(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("Property element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaProperty(name);

            foreach (var child in ReadChildren(reader))
            {
                element.Add(child);
            }

            return element.GetResult();
        }

        public static Element ReadScoredProperty(XmlReader reader)
        {
            if (!reader.MoveToAttribute("name"))
                throw new ReadPrintSchemaDocumentException("ScoredProperty element must contains name attribute");

            var name = reader.ValueAsXName();
            var element = new PrintSchemaScoredProperty(name);

            foreach (var child in ReadChildren(reader))
            {
                element.Add(child);
            }

            return element.GetResult();
        }

        private static Element ReadValue(XmlReader reader)
        {
            XName type = null;

            if (reader.MoveToFirstAttribute())  // Read value type
            {
                do
                {
                    var attrName = reader.XName();
                    if (attrName == Xsi.Type)
                    {
                        type = reader.ValueAsXName();
                    }
                } while (reader.MoveToNextAttribute());

                reader.MoveToElement();
            }

            if (reader.IsEmptyElement)
            {
                return Value.Empty;
            }

            var value = Value.Empty;

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    if (type == Xsd.Integer)
                    {
                        value = reader.Value.AsInt32();
                    }
                    else if (type == Xsd.Decimal)
                    {
                        value = reader.Value.AsFloat();
                    }
                    else if (type == Xsd.QName)
                    {
                        value = reader.ValueAsXName();
                    }
                    else
                    {
                        value = reader.Value;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
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

    internal interface PrintSchemaElement
    {
        void Add(Element element);
    }

    internal class PrintSchemaCapabilities : PrintSchemaElement
    {
        public PrintSchemaCapabilities()
        {
            Result = new Capabilities();
        }

        public Capabilities Result
        {
            get;
        }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x => Result.Add(x),
                onParameterDef: x => Result.Add(x),
                onProperty: x => Result.Add(x));
        }
    }

    internal class PrintSchemaTicket : PrintSchemaElement
    {
        public PrintSchemaTicket()
        {
            Result = new Ticket();
        }

        public Ticket Result
        {
            get;
        }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x => Result.Add(x),
                onParameterInit: x => Result.Add(x),
                onProperty: x => Result.Add(x));
        }
    }

    internal class PrintSchemaFeature : PrintSchemaElement
    {
        private Feature _feature;

        public PrintSchemaFeature(XName name)
        {
            _feature = new Feature(name);
        }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x => _feature.Add(x),
                onOption: x => _feature.Add(x),
                onProperty: x => _feature.Add(x));
        }

        public Element GetResult()
        {
            return _feature;
        }
    }

    internal class PrintSchemaOption : PrintSchemaElement
    {
        private Option _option;

        public PrintSchemaOption(XName name, XName constrained)
        {
            _option = new Option(name, constrained);
        }

        public void Add(Element element)
        {
            element.Apply(
                onScoredProperty: x => _option.Add(x),
                onProperty: x => _option.Add(x));
        }

        public Element GetResult()
        {
            return _option;
        }
    }

    internal class PrintSchemaParameterDef : PrintSchemaElement
    {
        private ParameterDef _parameterDef;

        public PrintSchemaParameterDef(XName name)
        {
            _parameterDef = new ParameterDef(name);
        }

        public void Add(Element element)
        {
            element.Apply(onProperty: x => _parameterDef.Add(x));
        }

        public Element GetResult()
        {
            return _parameterDef;
        }
    }

    internal class PrintSchemaParameterInit : PrintSchemaElement
    {
        private ParameterInit _parameterInit;

        public PrintSchemaParameterInit(XName name)
        {
            _parameterInit = new ParameterInit(name);
        }

        public void Add(Element element)
        {
            element.Apply(onValue: x => _parameterInit.Value = x);
        }

        public Element GetResult()
        {
            return _parameterInit;
        }
    }

    internal class PrintSchemaParameterRef : PrintSchemaElement
    {
        private ParameterRef _parameterRef;

        public PrintSchemaParameterRef(XName name)
        {
            _parameterRef = new ParameterRef(name);
        }

        public void Add(Element element)
        {
            throw new InvalidChildElementException("ParameterRef can't contain any element");
        }

        public Element GetResult()
        {
            return _parameterRef;
        }
    }

    internal class PrintSchemaProperty : PrintSchemaElement
    {
        private XName _name;
        private Value _value;
        private List<Property> _properties;

        public PrintSchemaProperty(XName name)
        {
            _name = name;
            _properties = new List<Property>();
        }

        public void Add(Element element)
        {
            element.Apply(
                onProperty: x => _properties.Add(x),
                onValue: x => _value = x);
        }

        public Element GetResult()
        {
            var p = new Property(_name, _value, _properties.ToArray());
            return p;
        }
    }

    internal class PrintSchemaScoredProperty : PrintSchemaElement
    {
        private XName _name;
        private Value _value;
        private ParameterRef _parameterRef;
        private List<Property> _properties = new List<Property>();
        private List<ScoredProperty> _scoredProperties = new List<ScoredProperty>();

        public PrintSchemaScoredProperty(XName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(
                onScoredProperty: x => _scoredProperties.Add(x),
                onProperty: x => _properties.Add(x),
                onParameterRef: x => _parameterRef = x,
                onValue: x => _value = x);
        }

        public Element GetResult()
        {
            var sp = new ScoredProperty(_name, _value, _parameterRef, _scoredProperties, _properties);
            return sp;
        }
    }
}
