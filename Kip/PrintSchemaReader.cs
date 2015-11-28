using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

            return pc.GetResult();
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

            var pt = new PrintSchemaTicket();

            foreach (var child in ReadChildren(reader))
            {
                pt.Add(child);
            }

            return pt.GetResult();
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
        private ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        private ImmutableNamedElementCollection<ParameterDef> _parameters
            = ImmutableNamedElementCollection.CreateParameterDefCollection();
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public PrintSchemaCapabilities() { }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x =>
                {
                    _features = _features.Add(x);
                },
                onParameterDef: x =>
                {
                    _parameters = _parameters.Add(x);
                },
                onProperty: x =>
                {
                    _properties = _properties.Add(x);
                });
        }

        public Capabilities GetResult()
        {
            return new Capabilities(_features, _parameters, _properties);
        }
    }

    internal class PrintSchemaTicket : PrintSchemaElement
    {
        private ImmutableNamedElementCollection<Feature> _features
            = ImmutableNamedElementCollection.CreateFeatureCollection();
        private ImmutableNamedElementCollection<ParameterInit> _parameters
            = ImmutableNamedElementCollection.CreateParameterInitCollection();
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public PrintSchemaTicket() { }

        public Ticket GetResult()
        {
            return new Ticket(_features, _parameters, _properties);
        }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x =>
                {
                    _features = _features.Add(x);
                },
                onParameterInit: x =>
                {
                    _parameters = _parameters.Add(x);
                },
                onProperty: x =>
                {
                    _properties = _properties.Add(x);
                });
        }
    }

    internal class PrintSchemaFeature : PrintSchemaElement
    {
        private XName _name;
        private ImmutableNamedElementCollection<Property> _properties = ImmutableNamedElementCollection.CreatePropertyCollection();
        private ImmutableList<Option> _options = ImmutableList.Create<Option>();
        private ImmutableNamedElementCollection<Feature> _features = ImmutableNamedElementCollection.CreateFeatureCollection();

        public PrintSchemaFeature(XName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x =>
                {
                    _features = _features.Add(x);
                },
                onOption: x =>
                {
                    _options = _options.Add(x);
                },
                onProperty: x =>
                {
                    _properties = _properties.Add(x);
                });
        }

        public Element GetResult()
        {
            return new Feature(_name, _properties, _options, _features);
        }
    }

    internal class PrintSchemaOption : PrintSchemaElement
    {
        private XName _name;
        private XName _constrained;
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        private ImmutableNamedElementCollection<ScoredProperty> _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollection();

        public PrintSchemaOption(XName name, XName constrained)
        {
            _name = name;
            _constrained = constrained;
        }

        public void Add(Element element)
        {
            element.Apply(
                onProperty: x =>
                {
                    _properties = _properties.Add(x);
                },
                onScoredProperty: x =>
                {
                    _scoredProperties = _scoredProperties.Add(x);
                });
        }

        public Element GetResult()
        {
            return new Option(
                _name,
                _constrained,
                _properties,
                _scoredProperties);
        }
    }

    internal class PrintSchemaParameterDef : PrintSchemaElement
    {
        private XName _name;
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public PrintSchemaParameterDef(XName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(onProperty: x =>
            {
                _properties = _properties.Add(x);
            });
        }

        public Element GetResult()
        {
            return new ParameterDef(_name, _properties);
        }
    }

    internal class PrintSchemaParameterInit : PrintSchemaElement
    {
        private XName _name;
        private Value _value;

        public PrintSchemaParameterInit(XName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(onValue: x => _value = x);
        }

        public Element GetResult()
        {
            return new ParameterInit(_name, _value);
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
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();

        public PrintSchemaProperty(XName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(
                onProperty: x => _properties = _properties.Add(x),
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
        private ImmutableNamedElementCollection<Property> _properties
            = ImmutableNamedElementCollection.CreatePropertyCollection();
        private ImmutableNamedElementCollection<ScoredProperty> _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollection();

        public PrintSchemaScoredProperty(XName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(
                onScoredProperty: x => _scoredProperties.Add(x),
                onProperty: x => _properties.Add(x),
                onParameterRef: x =>
                {
                    ThrowIfValueOrParameterRefExists();
                    _parameterRef = x;
                },
                onValue: x =>
                {
                    ThrowIfValueOrParameterRefExists();
                    _value = x;
                });
        }

        public Element GetResult()
        {
            var sp = new ScoredProperty(_name, _value, _parameterRef, _scoredProperties, _properties);
            return sp;
        }

        private void ThrowIfValueOrParameterRefExists()
        {
            if (_value == null && _parameterRef == null)
                return;

            throw new InvalidChildElementException("ScoredProperty can contain only one Value or ParameterRef");
        }
    }
}
