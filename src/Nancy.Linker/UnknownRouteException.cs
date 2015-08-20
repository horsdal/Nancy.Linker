using Nancy.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nancy.Linker
{
    public class UnknownRouteException : Exception
    {
        private const string message = @"Could not find a route named {0}

The following routes are available:
{1}";
        
        public UnknownRouteException(string missingRoute, IEnumerable<RouteDescription> routes) : base(string.Format(message, missingRoute, RouteList(routes)))
        {
        }
        
        private static string RouteList(IEnumerable<RouteDescription> routes)
        {
            var lines = routes.Select(rd => string.Format("\t{0}: {1}", rd.Name, rd.Description));
            return string.Join("\n", lines);
        }
    }
}
