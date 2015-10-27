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
    public class PrintSchemaReader
    {
        private XmlReader _reader;

        public PrintSchemaReader(XmlReader reader)
        {
            _reader = reader;
        }

        public PrintSchemaReader(string xml) : this(XmlReader.Create(new StringReader(xml)))
        {
        }

        public object Read()
        {
            _reader.Read();

            var name = _reader.XName();

            if (name == psf.Value)
            {
                return ReadValue();
            }

            return null;
        }

        private PrintSchemaValue ReadValue()
        {
            XName type = null;

            if (!_reader.MoveToFirstAttribute()) return null;
            do
            {
                var attrName = _reader.XName();
                if (attrName == xsi.Type)
                {
                    type = _reader.ValueAsXName();
                }
            } while (_reader.MoveToNextAttribute());

            // move to text node
            _reader.Read();

            if (type == xsd.Integer)
            {
                return new PrintSchemaValue(_reader.Value.AsInt32());
            }
            else if (type == xsd.Decimal)
            {
                return new PrintSchemaValue(_reader.Value.AsFloat());
            }
            else if (type == xsd.QName)
            {
                return new PrintSchemaValue(_reader.ValueAsXName());
            }
            else
            {
                return new PrintSchemaValue(_reader.Value);
            }
        }
    }
}
