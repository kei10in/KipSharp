using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Kip.Tests
{
    internal static class Utility
    {
        private static readonly string PrintCapabilitiesHeader =
            @"<?xml version='1.0' encoding='UTF-8'?>
            <psf:PrintCapabilities
                version='1'
                xmlns:psf='http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework'
                xmlns:psk='http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords'
                xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
                xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
        private static readonly string PrintCapabilitiesFooter = "</psf:PrintCapabilities>";

        internal static string PrintCapabilitiesWith(string content)
        {
            return string.Join(
                Environment.NewLine,
                PrintCapabilitiesHeader,
                content,
                PrintCapabilitiesFooter);
        }
    }
}
