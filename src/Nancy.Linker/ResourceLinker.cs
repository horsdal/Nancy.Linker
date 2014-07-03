namespace Nancy.Linker
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using Nancy;
  using Nancy.Routing;

  public class ResourceLinker
  {
    private readonly IRouteCacheProvider routesProvider;
    private List<RouteDescription> allRoutes = null; 
    private List<RouteDescription> AllRoutes
    {
      get
      {
        if (this.allRoutes == null)
          this.allRoutes = this.routesProvider.GetCache().SelectMany(pair => pair.Value.Select(tuple => tuple.Item2)).ToList();
        return this.allRoutes;
      }
    }

    public ResourceLinker(IRouteCacheProvider routesProvider)
    {
      this.routesProvider = routesProvider;
    }

    public Uri BuildAbsoluteUri(NancyContext context, string routeName, dynamic parameters = null)
    {
      var baseUri = new Uri(context.Request.Url.SiteBase.TrimEnd('/'));
      var pathTemplate = this.AllRoutes.Single(r => r.Name == routeName).Path;
      var uriTemplate = new UriTemplate(pathTemplate, true);
      return uriTemplate.BindByName(baseUri, ToDictionary(parameters ?? new {}));
    }

    private static IDictionary<string, string> ToDictionary(object anonymousInstance)
    {
      var dictionary = anonymousInstance as IDictionary<string, string>;
      if (dictionary != null) return dictionary;

      return TypeDescriptor.GetProperties(anonymousInstance)
        .OfType<PropertyDescriptor>()
        .ToDictionary(p => p.Name, p => p.GetValue(anonymousInstance).ToString());
    }
  }
}