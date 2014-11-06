PoliceUK.NET [![Build Status](https://travis-ci.org/FlyingTopHat/PoliceUK.NET.svg?branch=master)](https://travis-ci.org/FlyingTopHat/PoliceUK.NET)
============
An API wrapper for the [Police UK][police-api] API.

Installation
------------
Download the [latest release][latest-release] and add a reference to `PoliceUK.dll` in your project.

Example Usage
-------------

### Get a summary for all forces

```csharp
var policeClient = new PoliceUkClient();
IEnumerable<ForceSummary> forces = policeClient.Forces();
```

Prerequisites
-------------
.NET 3.5+ or Mono.

License
-------
PoliceUK.NET is released under the [MIT License][license-file].

[police-api]:http://police.uk/
[latest-release]:https://github.com/FlyingTopHat/PoliceUK.NET/releases
[license-file]:LICENSE.txt