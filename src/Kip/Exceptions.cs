﻿using System;
using System.Xml.Linq;

namespace Kip
{
    /// <summary>
    /// Thrown when an attempt is made to add a Print Schema element with the
    /// name that already exists to the parent element.
    /// </summary>
    public class DuplicateNameException : InvalidOperationException
    {
        public DuplicateNameException() { }

        public DuplicateNameException(string message) : base(message) { }

        public DuplicateNameException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidChildElementException : Exception
    {
        public InvalidChildElementException() { }

        public InvalidChildElementException(string message) : base(message) { }

        public InvalidChildElementException(string message, Exception inner) : base(message, inner) { }
    }

    public class ReadPrintSchemaDocumentException : Exception
    {
        public ReadPrintSchemaDocumentException() { }

        public ReadPrintSchemaDocumentException(string message) : base(message) { }

        public ReadPrintSchemaDocumentException(string message, Exception inner) : base(message, inner) { }
    }

    public class InternalException : Exception
    {
        public InternalException() { }

        public InternalException(string message) : base(message) { }

        public InternalException(string message, Exception inner) : base(message, inner) { }
    }
}
