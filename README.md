DotNetEasyRandom
====================================

DotNetEasyRandom is an assembly/ library to generate random numbers with the internal Random library from .Net more easily.
The assembly was written and tested in .Net 5.0.

[![Build status](https://ci.appveyor.com/api/projects/status/rq1fsgtfg4n9lfnx?svg=true)](https://ci.appveyor.com/project/SeppPenner/dotneteasyrandom)
[![GitHub issues](https://img.shields.io/github/issues/SeppPenner/DotNetEasyRandom.svg)](https://github.com/SeppPenner/DotNetEasyRandom/issues)
[![GitHub forks](https://img.shields.io/github/forks/SeppPenner/DotNetEasyRandom.svg)](https://github.com/SeppPenner/DotNetEasyRandom/network)
[![GitHub stars](https://img.shields.io/github/stars/SeppPenner/DotNetEasyRandom.svg)](https://github.com/SeppPenner/DotNetEasyRandom/stargazers)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://raw.githubusercontent.com/SeppPenner/DotNetEasyRandom/master/License.txt)
[![Nuget](https://img.shields.io/badge/DotNetEasyRandom-Nuget-brightgreen.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/HaemmerElectronics.SeppPenner.DotNetEasyRandom.svg)](https://www.nuget.org/packages/HaemmerElectronics.SeppPenner.DotNetEasyRandom/)
[![Known Vulnerabilities](https://snyk.io/test/github/SeppPenner/DotNetEasyRandom/badge.svg)](https://snyk.io/test/github/SeppPenner/DotNetEasyRandom)
[![Gitter](https://badges.gitter.im/DotNetEasyRandom/community.svg)](https://gitter.im/DotNetEasyRandom/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge)

## Available for
* NetFramework 4.6
* NetFramework 4.6.2
* NetFramework 4.7
* NetFramework 4.7.2
* NetFramework 4.8
* NetStandard 2.0
* NetStandard 2.1
* NetCore 3.1
* Net 5.0

## Net Core and Net Framework latest and LTS versions
* https://dotnet.microsoft.com/download/dotnet-framework
* https://dotnet.microsoft.com/download/dotnet-core
* https://dotnet.microsoft.com/download/dotnet/5.0

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