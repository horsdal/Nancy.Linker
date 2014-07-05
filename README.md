# Nancy.Linker

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
    var absoluteLink = linker..BuildAbsoluteUri(this.Context, "aNamedRoute", parameters: new {id = 123})
    var relativeLink = linker..BuildAbsoluteUri(this.Context, "aNamedRoute", parameters: new {id = 123})
  }
}
```

## More information

http://www.horsdal-consult.dk/search/label/Nancy.Linker

