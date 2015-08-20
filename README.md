# Nancy.Linker [![NuGet Status](http://img.shields.io/nuget/v/Nancy.Linker.svg?style=flat)](https://www.nuget.org/packages/Nancy.Linker/) [![License](https://img.shields.io/github/license/horsdal/nancy.linker.svg)](./LICENSE)

Simple URI builder for named Nancy routes with optional pass through of query parameters.

## Installation

```PowerShell
PM> Install-Package Nancy.Linker
```

## General Usage

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

### Query Parameter Passthrough Usage

To enable the query parameters pass through filter, bootstrap the PassthroughQueryFilter class.
In the code below, the parameter 'foo' will be passed to the build URI from the ResourceLinker.
```C#
public class Bootstrapper : DefaultNancyBootstrapper
{
  protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
  {
    // Other bootstrapper code here  
    container.Register<IQueryFilter>(new PassthroughQueryFilter(new[] { "foo" }));
  }
}
```

## More information

http://www.horsdal-consult.dk/search/label/Nancy.Linker

