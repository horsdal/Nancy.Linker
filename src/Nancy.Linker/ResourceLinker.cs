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
      var pathTemplate = this.AllRoutes.Single(r => r.Name == routeName).Path;
      var uriTemplate = new UriTemplate(pathTemplate, true);
      return uriTemplate.BindByName(GetBaseUri(context), ToDictionary(parameters ?? new {}));
    }

    private static Uri GetBaseUri(NancyContext context)
    {
      var baseUriString =
        !string.IsNullOrWhiteSpace(context.Request.Url.HostName)
          ? context.Request.Url.SiteBase.TrimEnd('/')
          : context.Request.Url.Scheme + "://localhost";
      return new Uri(baseUriString);
    }

    public Uri BuildRelativeUri(NancyContext context, string routeName, dynamic parameters = null)
    {
      return new Uri(this.BuildAbsoluteUri(context, routeName, parameters).PathAndQuery, UriKind.Relative);
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