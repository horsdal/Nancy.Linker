namespace Nancy.Linker
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Linq;
  using Extensions;
  using Nancy;
  using Nancy.Routing;

  public class ResourceLinker
  {
    private readonly IRouteCacheProvider routesProvider;
    private readonly IRouteSegmentExtractor segmentExtractor;
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

    public ResourceLinker(IRouteCacheProvider routesProvider, IRouteSegmentExtractor extractor)
    {
      this.routesProvider = routesProvider;
      this.segmentExtractor = extractor;
    }

    public Uri BuildAbsoluteUri(NancyContext context, string routeName, dynamic parameters = null)
    {
      var parameterDictionary = ToDictionary(parameters as object ?? new { });
      var pathTemplate = this.AllRoutes.Single(r => r.Name == routeName).Path;
      var realizedPath = 
        this.segmentExtractor.Extract(pathTemplate)
        .Aggregate((accumulatedtPath, segment) => GetSegmentValue(segment, parameterDictionary, accumulatedtPath));
      return new Uri(GetBaseUri(context), realizedPath);
    }

    private static string GetSegmentValue(string segment, IDictionary<string, string> parameterDictionary, string current)
    {
      var res = TryGetParameterValue(segment, parameterDictionary);
      if (res == null)
        throw new ArgumentException(string.Format("Value for path segment {0} missing", segment), "parameters");
      return string.Concat(current, "/", res);
    }

    private static string TryGetParameterValue(string segment, IDictionary<string, string> parameterDictionary)
    {
      if (segment.IsParameterized())
        return GetParameterizedSegmentValue(segment, parameterDictionary);
      else if (IsContrainedParameter(segment))
         return GetConstrainedParamterValue(segment, parameterDictionary);
      else
        return segment;
    }

    private static string GetConstrainedParamterValue(string segment, IDictionary<string, string> parameterDictionary)
    {
      string res = null;
      parameterDictionary.TryGetValue(segment.Substring(1, segment.IndexOf(':') - 1).Trim(), out res);
      return res;
    }

    private static bool IsContrainedParameter(string segment)
    {
      return segment.Contains(':');
    }

    private static string GetParameterizedSegmentValue(string segment, IDictionary<string, string> parameterDictionary)
    {
      string res = null;
      var segmentInfo = segment.GetParameterDetails().Single();
      if (!segmentInfo.IsOptional || string.IsNullOrEmpty(segmentInfo.DefaultValue))
        parameterDictionary.TryGetValue(segmentInfo.Name, out res);
      else
        res = parameterDictionary.ContainsKey(segmentInfo.Name)
          ? parameterDictionary[segmentInfo.Name]
          : segmentInfo.DefaultValue;
      return res;
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