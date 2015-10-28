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

    public static IEnumerable<object> ReadChildren(XmlReader reader)
    {
        List<object> result = new List<object>();

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

    public static object ReadElement(XmlReader reader)
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

    public static Feature ReadFeature(XmlReader reader)
    {
        reader.Skip();
        return null;
    }

    public static Option ReadOption(XmlReader reader)
    {
        reader.Skip();
        return null;
    }

    public static Property ReadProperty(XmlReader reader)
    {
        if (!reader.MoveToAttribute("name"))
        {
            reader.Skip();
            return null;
        }

        var name = reader.ValueAsXName();
        Value value = null;
        var children = ReadChildren(reader);
        foreach (var e in ReadChildren(reader))
        {
            var v = e as Value;
            if (v != null)
            {
                value = v;
            }
        }

        return new Property(name, value);
    }

    private static Value ReadValue(XmlReader reader)
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

        Value value = null;

        if (type == xsd.Integer)
        {
            value = new Value(reader.Value.AsInt32());
        }
        else if (type == xsd.Decimal)
        {
            value = new Value(reader.Value.AsFloat());
        }
        else if (type == xsd.QName)
        {
            value = new Value(reader.ValueAsXName());
        }
        else
        {
            value = new Value(reader.Value);
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
}
