PoliceUK.NET [![Build Status](https://travis-ci.org/FlyingTopHat/PoliceUK.NET.svg?branch=master)](https://travis-ci.org/FlyingTopHat/PoliceUK.NET)
============

An API wrapper for the [Police UK][police-api] API that provides easy access to crime data provided by [data.police.uk][police-data].


Example Usages
--------------

**Get a summary for all forces**

```csharp
var policeClient = new PoliceUkClient();
IEnumerable<ForceSummary> forces = policeClient.Forces();
```

**Get street level crimes**

```csharp
var policeClient = new PoliceUkClient();

var chawton = new Geoposition(51.133112, -0.989054);
StreetLevelCrimeResults results = policeClient.StreetLevelCrimes(chawton);

foreach (Crime crime in results.Crimes)
{
    Console.WriteLine(crime.Category);
}
```


Installation
------------

### NuGet

Run the following command in the Package Manager Console 

```
PM> Install-Package PoliceUK 
```

### Manually

Download the [latest release][latest-release] and add a reference to `PoliceUK.dll` in your project.


Prerequisites
-------------
.NET 3.5+ or Mono.


License
-------
PoliceUK.NET is released under the [MIT License][license-file].

The data provided by [data.police.uk][police-data] is covered by the [Open Government Licence v3.0][police-data-licence].


[police-api]:http://police.uk/
[police-data]:https://data.police.uk/
[police-data-licence]:https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/
[latest-release]:https://github.com/FlyingTopHat/PoliceUK.NET/releases
[license-file]:LICENSE.txt
