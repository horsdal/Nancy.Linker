namespace Nancy.Linker
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;

  public class EnsureHttpsFilter : UriFilter
  {
    public EnsureHttpsFilter(IUriFilter nextFilter = null) : 
      base(nextFilter)
    {
    }

    protected override Uri OnApply(Uri uri, NancyContext context)
    {
      if (string.Compare(uri.Scheme, "https", true) == 0) return uri;

      UriBuilder builder = new UriBuilder(uri);
      builder.Scheme = "https";
      return builder.Uri;
    }
  }
}
