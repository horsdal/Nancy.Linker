namespace Nancy.Linker
{
  using System;

  public static class NancyContextExtensions
  {
    internal static IResourceLinker linker;

    public static Uri BuildAbsoluteUri(this NancyContext context, string routeName, dynamic parameters = null)
    {
      return linker.BuildAbsoluteUri(context, routeName);
    }

    public static Uri BuildRelativeUri(this NancyContext context, string routeName, dynamic parameters = null)
    {
      return linker.BuildRelativeUri(context, routeName);
    }
  }

  public class ResourceLinkerStartupResolver : NancyModule
  {
    public ResourceLinkerStartupResolver(IResourceLinker linker)
    {
      NancyContextExtensions.linker = linker;
    }
  }
}
