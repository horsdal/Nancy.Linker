namespace Nancy.Linker.Tests
{
  using System;
  using Testing;
  using Xunit;

  public class PassthroughUriFilter_Should
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
    public void throw_if_constructed_with_null_argument()
    {
      Assert.Throws<ArgumentNullException>(() => new PassthroughUriFilter(null));
    }

    [Fact]
    public void throw_if_apply_is_passed_null_as_uri()
    {
      PassthroughUriFilter filter = new PassthroughUriFilter(new string[] { });

      Assert.Throws<ArgumentNullException>(() => filter.Apply(null, app.Get("").Context));
    }

    [Fact]
    public void throw_if_apply_is_passed_null_as_context()
    {
      PassthroughUriFilter filter = new PassthroughUriFilter(new string[] { });

      Assert.Throws<ArgumentNullException>(() => filter.Apply(new Uri("http://www.nancyfx.org"), null));
    }

    [Fact]
    public void passes_the_query_through()
    {
      PassthroughUriFilter filter = new PassthroughUriFilter(new string[] { "foo" });

      Browser appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => to.Query("foo", "bar"));

      Uri result = filter.Apply(new Uri("http://www.nancyfx.org"), appWithQueryString.Get("").Context);

      Assert.Equal("?foo=bar", result.Query);
    }

    [Fact]
    public void passes_multiple_queries_through()
    {
      PassthroughUriFilter filter = new PassthroughUriFilter(new string[] { "foo" }, new PassthroughUriFilter(new string[] { "blip" }));

      Browser appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => { to.Query("foo", "bar"); to.Query("blib", "blop"); });

      Uri result = filter.Apply(new Uri("http://www.nancyfx.org"), appWithQueryString.Get("").Context);

      Assert.Equal("?foo=bar", result.Query);
    }

    [Fact]
    public void passes_multiple_queries_through_using_multiple_filters()
    {
      PassthroughUriFilter filter = new PassthroughUriFilter(new string[] { "foo" }, new PassthroughUriFilter(new string[] { "blib" }));

      Browser appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => { to.Query("foo", "bar"); to.Query("blib", "blob"); });

      Uri result = filter.Apply(new Uri("http://www.nancyfx.org"), appWithQueryString.Get("").Context);

      Assert.Equal("?foo=bar&blib=blob", result.Query);
    }

    [Fact]
    public void does_not_passes_the_query_through()
    {
      PassthroughUriFilter filter = new PassthroughUriFilter(new string[] { "does_not_exist" });

      Browser appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => to.Query("foo", "bar"));

      Uri result = filter.Apply(new Uri("http://www.nancyfx.org"), appWithQueryString.Get("").Context);

      Assert.Equal("", result.Query);
    }
  }
}
