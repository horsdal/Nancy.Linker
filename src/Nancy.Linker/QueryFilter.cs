namespace Nancy.Linker
{
  using System;

  public interface IQueryFilter
  {
    /// <summary>
    /// Applies the filter to the given <paramref name="uri"/> by appending
    /// the allowed query string parameters from the given <paramref name="context"/>.
    /// </summary>
    /// <param name="uri">The uri to append query string parameters to.</param>
    /// <param name="context">The context to get the query string paramters values from.</param>
    /// <returns>New uri with appended query string parameters.</returns>
    Uri Apply(Uri uri, NancyContext context);
  }

  /// <summary>
  /// Default query filter that does not append anything.
  /// </summary>
  public class QueryFilter : IQueryFilter
  {
    public Uri Apply(Uri uri, NancyContext context)
    {
      return uri;
    }
  }
}
