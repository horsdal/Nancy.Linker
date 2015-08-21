namespace Nancy.Linker
{
  using System;

  /// <summary>
  /// Consider subclass <see cref="UriFilter"/> for getting decorator behaviour
  /// and being able to chain filters easy throw the contructor.
  /// </summary>
  public interface IUriFilter
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
  /// Base class with decorator pattern behaviour.
  /// </summary>
  /// <example>
  ///   <code>
  ///     class MyFilter : UriFiler
  ///     {
  ///       public MyFilter(IUriFilter nextFilter = null) : base(nextFilter)
  ///       {
  ///       }
  /// 
  ///       protected override OnUri Apply(Uri uri, NancyContext context)
  ///       {
  ///         Uri resultUri = // Do your work here.
  ///         return resultUri;
  ///       }
  ///     }
  ///   </code>
  /// </example>
  public abstract class UriFilter : IUriFilter
  {
    private readonly IUriFilter nextFilter;

    /// <summary>
    /// </summary>
    /// <param name="nextFilter">The next filter or null.</param>
    public UriFilter(IUriFilter nextFilter)
    {
      this.nextFilter = nextFilter;
    }

    /// <summary>
    /// Implements the decorator behaviour by calling the nextFilter if has a not null value.
    /// If nextFilter is null, then this method is the identity.
    /// </summary>
    public Uri Apply(Uri uri, NancyContext context)
    {
      var resultUri = OnApply(uri, context);

      if (this.nextFilter != null)
        return this.nextFilter.Apply(resultUri, context);
      else
        return resultUri;
    }

    protected abstract Uri OnApply(Uri uri, NancyContext context);
  }

  /// <summary>
  /// Default query filter that does not append anything.
  /// </summary>
  public class IdentityUriFilter : IUriFilter
  {
    public Uri Apply(Uri uri, NancyContext context)
    {
      return uri;
    }
  }
}
