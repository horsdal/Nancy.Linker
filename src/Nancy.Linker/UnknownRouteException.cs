namespace Nancy.Linker
{
  using Nancy.Routing;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public class UnknownRouteException : Exception
  {
    private const string message = @"Could not find a route named {0}
The following routes are available:
{1}";

    public UnknownRouteException(
      string missingRoute, 
      IEnumerable<RouteDescription> routes)
      : base(string.Format(message, missingRoute, RouteList(routes)))
    {}

    public UnknownRouteException(string message, Exception innerException)
      : base(message, innerException)
    {}

    private static string RouteList(IEnumerable<RouteDescription> routes)
    {
      var lines = routes.Select(rd => $"\t{rd.Name}: {rd.Description}");
      return string.Join("\n", lines);
    }
  }
}
