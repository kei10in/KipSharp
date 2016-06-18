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

            pc.Add(ReadNamespaceDeclarations(reader));

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

            pt.Add(ReadNamespaceDeclarations(reader));

            foreach (var child in ReadChildren(reader))
            {
                pt.Add(child);
            }

            return pt.GetResult();
        }

        public static IEnumerable<NamespaceDeclaration> ReadNamespaceDeclarations(XmlReader reader)
        {
            if (!reader.MoveToFirstAttribute())
            {
                return Enumerable.Empty<NamespaceDeclaration>();
            }

            var result = new List<NamespaceDeclaration>();

            do
            {
                var decl = ReadNamespaceDeclaration(reader);
                if (decl != null)
                {
                    result.Add(decl);
                }
            } while (reader.MoveToNextAttribute());

            return result;
        }

        private static NamespaceDeclaration ReadNamespaceDeclaration(XmlReader reader)
        {
            if (reader.NamespaceURI != XNamespace.Xmlns.NamespaceName)
            {
                return null;
            }
            var prefix = (reader.LocalName == "xmlns") ? string.Empty : reader.LocalName;
            var uri = reader.Value;
            return new NamespaceDeclaration(prefix, uri);
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
            var element = new PrintSchemaFeature(new FeatureName(name));

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
            var element = new PrintSchemaParameterDef(new ParameterName(name));

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
            var element = new PrintSchemaParameterInit(new ParameterName(name));

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
            var element = new PrintSchemaParameterRef(new ParameterName(name));

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
            var element = new PrintSchemaProperty(new PropertyName(name));

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
            var element = new PrintSchemaScoredProperty(new ScoredPropertyName(name));

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
        private readonly ImmutableNamedElementCollection<Feature>.Builder _features
            = ImmutableNamedElementCollection.CreateFeatureCollectionBuilder();
        private readonly ImmutableNamedElementCollection<ParameterDef>.Builder _parameters
            = ImmutableNamedElementCollection.CreateParameterDefCollectionBuilder();
        private readonly ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
        private List<NamespaceDeclaration> _namespaceDeclarations;

        public PrintSchemaCapabilities() { }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x => _features.Add(x),
                onParameterDef: x => _parameters.Add(x),
                onProperty: x => _properties.Add(x));
        }

        public void Add(IEnumerable<NamespaceDeclaration> declarations)
        {
            _namespaceDeclarations = declarations.ToList();
        }

        public Capabilities GetResult()
        {
            var nm = _namespaceDeclarations == null
                ? NamespaceDeclarationCollection.Default
                : new NamespaceDeclarationCollection(_namespaceDeclarations);
            return new Capabilities(
                _features.ToImmutable(),
                _parameters.ToImmutable(),
                _properties.ToImmutable(),
                nm);
        }
    }

    internal class PrintSchemaTicket : PrintSchemaElement
    {
        private readonly ImmutableNamedElementCollection<Feature>.Builder _features
            = ImmutableNamedElementCollection.CreateFeatureCollectionBuilder();
        private readonly ImmutableNamedElementCollection<ParameterInit>.Builder _parameters
            = ImmutableNamedElementCollection.CreateParameterInitCollectionBuilder();
        private readonly ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
        private List<NamespaceDeclaration> _namespaceDeclarations;

        public PrintSchemaTicket() { }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x => _features.Add(x),
                onParameterInit: x => _parameters.Add(x),
                onProperty: x => _properties.Add(x));
        }

        public void Add(IEnumerable<NamespaceDeclaration> declarations)
        {
            _namespaceDeclarations = declarations.ToList();
        }

        public Ticket GetResult()
        {
            var nm = _namespaceDeclarations == null
                ? NamespaceDeclarationCollection.Default
                : new NamespaceDeclarationCollection(_namespaceDeclarations);
            return new Ticket(
                _features.ToImmutable(),
                _parameters.ToImmutable(),
                _properties.ToImmutable(),
                nm);
        }
    }

    internal class PrintSchemaFeature : PrintSchemaElement
    {
        private FeatureName _name;
        private readonly ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
        private readonly ImmutableList<Option>.Builder _options
            = ImmutableList.CreateBuilder<Option>();
        private readonly ImmutableNamedElementCollection<Feature>.Builder _features
            = ImmutableNamedElementCollection.CreateFeatureCollectionBuilder();

        public PrintSchemaFeature(FeatureName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(
                onFeature: x => _features.Add(x),
                onOption: x => _options.Add(x),
                onProperty: x => _properties.Add(x));
        }

        public Element GetResult()
        {
            return new Feature(
                _name,
                _properties.ToImmutable(),
                _options.ToImmutable(),
                _features.ToImmutable());
        }
    }

    internal class PrintSchemaOption : PrintSchemaElement
    {
        private XName _name;
        private XName _constrained;
        private ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
        private ImmutableNamedElementCollection<ScoredProperty>.Builder _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollectionBuilder();

        public PrintSchemaOption(XName name, XName constrained)
        {
            _name = name;
            _constrained = constrained;
        }

        public void Add(Element element)
        {
            element.Apply(
                onProperty: x => _properties.Add(x),
                onScoredProperty: x => _scoredProperties.Add(x));
        }

        public Element GetResult()
        {
            return new Option(
                _name,
                _constrained,
                _properties.ToImmutable(),
                _scoredProperties.ToImmutable());
        }
    }

    internal class PrintSchemaParameterDef : PrintSchemaElement
    {
        private ParameterName _name;
        private ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();

        public PrintSchemaParameterDef(ParameterName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(onProperty: x => _properties.Add(x));

        }

        public Element GetResult()
        {
            return new ParameterDef(_name, _properties.ToImmutable());
        }
    }

    internal class PrintSchemaParameterInit : PrintSchemaElement
    {
        private ParameterName _name;
        private Value _value;

        public PrintSchemaParameterInit(ParameterName name)
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

        public PrintSchemaParameterRef(ParameterName name)
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
        private PropertyName _name;
        private Value _value;
        private ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();

        public PrintSchemaProperty(PropertyName name)
        {
            _name = name;
        }

        public void Add(Element element)
        {
            element.Apply(
                onProperty: x => _properties.Add(x),
                onValue: x => _value = x);
        }

        public Element GetResult()
        {
            var p = new Property(_name, _value, _properties.ToImmutable());
            return p;
        }
    }

    internal class PrintSchemaScoredProperty : PrintSchemaElement
    {
        private ScoredPropertyName _name;
        private Value _value;
        private ParameterRef _parameterRef;
        private ImmutableNamedElementCollection<Property>.Builder _properties
            = ImmutableNamedElementCollection.CreatePropertyCollectionBuilder();
        private ImmutableNamedElementCollection<ScoredProperty>.Builder _scoredProperties
            = ImmutableNamedElementCollection.CreateScoredPropertyCollectionBuilder();

        public PrintSchemaScoredProperty(ScoredPropertyName name)
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
            return new ScoredProperty(
                _name,
                _value,
                _parameterRef,
                _scoredProperties.ToImmutable(),
                _properties.ToImmutable());
        }

        private void ThrowIfValueOrParameterRefExists()
        {
            if (_value == null && _parameterRef == null)
                return;

            throw new InvalidChildElementException("ScoredProperty can contain only one Value or ParameterRef");
        }
    }
}
