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
  /// Subclass this class, forward the constructor argument and call base 
  /// in the <see cref="Apply"/> method with a possible modified uri. See example.
  /// </summary>
  /// <example>
  ///   <code>
  ///     class MyFilter : UriFiler
  ///     {
  ///       public MyFilter(IUriFilter nextFilter = null) : base(nextFilter)
  ///       {
  ///       }
  /// 
  ///       public Uri Apply(Uri uri, NancyContext context)
  ///       {
  ///         Uri resultUri = // Do your work here.
  ///         return base.Apply(resultUri, context);
  ///       }
  ///     }
  ///   </code>
  /// </example>
  public class UriFilter : IUriFilter
  {
    private readonly IUriFilter nextFilter;

    public UriFilter(IUriFilter nextFilter = null)
    {
      this.nextFilter = nextFilter;
    }

    /// <summary>
    /// Implements the decorator behaviour by calling the nextFilter if has a not null value.
    /// If nextFilter is null, then this method is the identity.
    /// </summary>
    public virtual Uri Apply(Uri uri, NancyContext context)
    {
      if (this.nextFilter != null)
      {
        return this.nextFilter.Apply(uri, context);
      }
      else
      {
        return uri;
      }
    }
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
