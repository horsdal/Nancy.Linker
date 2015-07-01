﻿namespace Nancy.Linker
{
  using Nancy.Helpers;
  using System;
  using System.Collections.Generic;
  using System.Linq;

  /// <summary>
  /// <see cref="IQueryFilter"/> that appends all given query string parameters by name.
  /// </summary>
  public class PassthroughQueryFilter : IQueryFilter
  {
    IList<string> queryNamesToPassThrough;

    public PassthroughQueryFilter(IEnumerable<string> queryNamesToPassThrough)
    {
      if (queryNamesToPassThrough == null) throw new ArgumentNullException(nameof(queryNamesToPassThrough));

      this.queryNamesToPassThrough = queryNamesToPassThrough.ToList();
    }

    public Uri Apply(Uri uri, NancyContext context)
    {
      if (uri == null) throw new ArgumentNullException(nameof(uri));
      if (context == null) throw new ArgumentNullException(nameof(context));

      var query = HttpUtility.ParseQueryString(uri.Query);

      foreach (string name in queryNamesToPassThrough)
      {
        DynamicDictionaryValue queryValue = context.Request.Query[name];
        if (!queryValue.HasValue) continue;

        query.Add(name, queryValue.Value.ToString());
      }

      UriBuilder builder = new UriBuilder(uri);
      builder.Query = query.ToString();
      return builder.Uri;
    }
  }
}