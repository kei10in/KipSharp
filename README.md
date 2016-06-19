# KipSharp

[![Build status](https://ci.appveyor.com/api/projects/status/bpw4bkpty6f9ny85?svg=true)](https://ci.appveyor.com/project/kei10in/kipsharp)

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
pt = pt.Set(Psk.PageMediaSize,
    new Option(Psk.ISOA4,
        new ScoredProperty(Psk.MediaSizeWidth, 210000),
        new ScoredProperty(Psk.MediaSizeHeight, 297000)));
pt.Save(outputStream);
```
