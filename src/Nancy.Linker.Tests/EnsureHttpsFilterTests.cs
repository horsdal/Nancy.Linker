namespace Nancy.Linker.Tests
{
  using System;
  using Testing;
  using Xunit;

  public class EnsureHttpsFilterr_Should
  {
    private readonly Browser app = new Browser(with => with.Module<TestModule>(), defaults: to => to.HostName("nancyfx.org"));

    public class TestModule : NancyModule
    {
      public static IResourceLinker linker;

      public TestModule(IResourceLinker linker)
      {
        TestModule.linker = linker;
        this.Get[""] = _ => 200;
      }
    }

    [Fact]
    public void changes_scheme_from_http_to_https()
    {
      EnsureHttpsFilter filter = new EnsureHttpsFilter();

      Uri result = filter.Apply(new Uri("http://www.nancyfx.org"), null);
      Assert.Equal("https", result.Scheme);
    }

    [Fact]
    public void handles_mixed_scheme_casing()
    {
      EnsureHttpsFilter filter = new EnsureHttpsFilter();

      Uri result = filter.Apply(new Uri("HttPS://www.nancyfx.org"), null);
      Assert.Equal("https", result.Scheme);
    }
  }
}
