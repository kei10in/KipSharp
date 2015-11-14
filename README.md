# KipSharp

[![Build Status](https://travis-ci.org/kei10in/KipSharp.svg)](https://travis-ci.org/kei10in/KipSharp)

Handle PrintCapabilities and PrintTicket documents on .NET Framework.

## Usage

### Read option in Print Capabilities document

```csharp
using Kip;

var pc = Capabiliites.Load(inputStream);
var options = for op in Feature(Psf.PageMediaSize).Options()
var displayNames = for op in options
                   select op.Property(Psk.DisplayName).Value.AsStirng();
```

### Set option to Print Ticket document

```csharp
using Kip;

var pt = Ticket.Load(inputStream);
pt.SetFeatureOption(Psk.PageMediaSize,
    new Option(Psk.ISOA4,
        new ScoredProperty(Psk.MediaSizeWidth, 210000),
        new ScoredProperty(Psk.MediaSizeHeight, 297000)));
pt.Save(outputStream);
```
