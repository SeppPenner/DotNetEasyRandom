DotNetEasyRandom
====================================

DotNetEasyRandom is an assembly/ library to generate random numbers with the internal Random library from .Net more easily.

[![Build status](https://ci.appveyor.com/api/projects/status/rq1fsgtfg4n9lfnx?svg=true)](https://ci.appveyor.com/project/SeppPenner/dotneteasyrandom)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/DotNetEasyRandom.svg)](https://github.com/SeppPenner/DotNetEasyRandom/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/DotNetEasyRandom.svg)](https://github.com/SeppPenner/DotNetEasyRandom/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/DotNetEasyRandom.svg)](https://github.com/SeppPenner/DotNetEasyRandom/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/DotNetEasyRandom/master/License.txt)
[![Nuget](https://img.shields.io/badge/DotNetEasyRandom-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.DotNetEasyRandom.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/DotNetEasyRandom/badge.svg)](https://snyk.io/test/github/SeppPenner/DotNetEasyRandom)
[![Gitter](https://img.shields.io/matrix/DotNetEasyRandom_community%3Agitter.im?server_fqdn=matrix.org)](https://matrix.to/#/#DotNetEasyRandom_community:gitter.im)
[![Blogger](https://img.shields.io/badge/Follow_me_on-blogger-orange)](https://franzhuber23.blogspot.de/)
[![Patreon](https://img.shields.io/badge/Patreon-F96854?logo=patreon&logoColor=white)](https://patreon.com/SeppPennerOpenSourceDevelopment)
[![PayPal](https://img.shields.io/badge/PayPal-00457C?logo=paypal&logoColor=white)](https://paypal.me/th070795)

## Available for
* Net 8.0
* Net 9.0

## Net Core and Net Framework latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet

## Basic usage
```csharp
public class ExampleUsage
{
    private readonly IEasyRandom _random = new EasyRandom(); //Normal construtor

    private readonly IEasyRandom _random2 = new EasyRandom(12); //Construtor with seed

    public void Test()
    {
        _random.Next(); //Function from the "normal" Random class
        _random.Next(12); //Function from the "normal" Random class
        _random.Next(1, 5, IncludeType.None); //Gives back 2, 3 or 4
        _random.Next(1, 5, IncludeType.StartOnly); //Gives back 1, 2, 3 or 4
        _random.Next(1, 5, IncludeType.EndOnly); //Gives back 2, 3, 4 or 5
        _random.Next(1, 5, IncludeType.Both); //Gives back 1, 2, 3, 4 or 5
        _random.NextBytes(new byte[] {1, 2, 3, 4}); //Function from the "normal" Random class
        _random.NextDouble(); //Function from the "normal" Random class
        _random.NextDoubleExcludedBoth(1.0, 2.0);
        //Gives back a double value between 1.0 and 2.0 EXCLUDING the borders
        //(e.g. from 1.0000000000000000000000000000000001 to 1.9999999999999999999999999999999999
    }
}
```

## Install

```bash
dotnet add package HaemmerElectronics.SeppPenner.DotNetEasyRandom
```

Change history
--------------

See the [Changelog](https://github.com/SeppPenner/DotNetEasyRandom/blob/master/Changelog.md).