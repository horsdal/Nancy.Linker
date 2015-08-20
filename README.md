# Nancy.Linker [![NuGet Status](http://img.shields.io/nuget/v/Nancy.Linker.svg?style=flat)](https://www.nuget.org/packages/Nancy.Linker/)

Simple URI builder for named Nancy routes with optional pass through of query parameters.
If query parameters pass through is enabled, then allowed paramters is passed from the reqeust context to the builded URI.

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

### Query parameters usage

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

