﻿using System.Xml.Linq;
using psf = Kip.PrintSchemaFramework;
using xsi = Kip.XmlSchemaInstance;
using xsd = Kip.XmlSchema;

namespace Kip
{
    public class PrintSchemaCapabilities
    {
    }

    public class PrintSchemaFeature
    {
    }

    public class PrintSchemaOption
    {
    }

    public class PrintSchemaProperty
    {
    }

    public class PrintSchemaScoredProperty
    {
        
    }

    public class PrintSchemaValue
    {
        object _value;

        public PrintSchemaValue(int value)
        {
            _value = value;
        }

        public PrintSchemaValue(float value)
        {
            _value = value;
        }

        public PrintSchemaValue(string value)
        {
            _value = value;
        }

        public PrintSchemaValue(XName value)
        {
            _value = value;
        }

        public XElement AsXElement()
        {
            var type = GetValueType();
            
            var element = new XElement(
                psf.Value,
                new XAttribute(XNamespace.Xmlns + "psf", psf.Url),
                new XAttribute(XNamespace.Xmlns + "xsi", xsi.Url),
                new XAttribute(XNamespace.Xmlns + "xsd", xsd.Url));
            element.Add(new XAttribute(xsi.Type, type.ToQName(element)));

            if (type == xsd.QName)
            {
                var value = _value as XName;
                element.Add(value.ToQName(element));
            }
            else
            {
                element.Add(_value);
            }

            return element;
        }

        private XName GetValueType()
        {
            var type = _value.GetType();
            if (type == typeof(int))
            {
                return xsd.Integer;
            }
            else if (type == typeof(float))
            {
                return xsd.Decimal;
            }
            else if (type == typeof(XName))
            {
                return xsd.QName;
            }
            else
            {
                return xsd.String;
            }
        }
    }
}