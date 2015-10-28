using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kip
{
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

    public class ReadPrintSchemaDocumentException : Exception
    {
        public ReadPrintSchemaDocumentException()
        {
        }

        public ReadPrintSchemaDocumentException(string message)
            : base(message)
        {
        }

        public ReadPrintSchemaDocumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
