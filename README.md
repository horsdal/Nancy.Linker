# Nancy.Linker [![NuGet Status](http://img.shields.io/nuget/v/Nancy.Linker.svg?style=flat)](https://www.nuget.org/packages/Nancy.Linker/) [![License](https://img.shields.io/github/license/horsdal/nancy.linker.svg)](./LICENSE)

Simple URI builder for named Nancy routes

## Installation

```PowerShell
PM> Install-Package Nancy.Linker
```

## Usage

```C#
public class BazModule : NancyModule
{
  public BazModule(IResourceLinker linker)
  {
    var absoluteLink = linker.BuildAbsoluteUri(this.Context, "aNamedRoute", parameters: new {id = 123})
    var relativeLink = linker.BuildRelativeUri(this.Context, "aNamedRoute", parameters: new {id = 123})
  }
}
```

## More information

http://www.horsdal-consult.dk/search/label/Nancy.Linker

