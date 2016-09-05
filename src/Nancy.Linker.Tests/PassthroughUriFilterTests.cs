using System.Threading.Tasks;

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
        this.Get("", _ => 200);
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
      var filter = new PassthroughUriFilter(new string[] { });

      Assert.Throws<ArgumentNullException>(() => filter.Apply(null, app.Get("").Result.Context));
    }

    [Fact]
    public void throw_if_apply_is_passed_null_as_context()
    {
      var filter = new PassthroughUriFilter(new string[] { });

      Assert.Throws<ArgumentNullException>(() => filter.Apply(new Uri("http://www.nancyfx.org"), null));
    }

    [Fact]
    public async Task passes_the_query_through()
    {
      var filter = new PassthroughUriFilter(new string[] { "foo" });

      var appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => to.Query("foo", "bar"));

      var response = await appWithQueryString.Get("");
      var result = filter.Apply(new Uri("http://www.nancyfx.org"), response.Context);

      Assert.Equal("?foo=bar", result.Query);
    }

    [Fact]
    public async Task passes_multiple_queries_through()
    {
      var filter = new PassthroughUriFilter(new string[] { "foo" }, new PassthroughUriFilter(new string[] { "blip" }));

      var appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => { to.Query("foo", "bar"); to.Query("blib", "blop"); });

      var response = await appWithQueryString.Get("");
      var result = filter.Apply(new Uri("http://www.nancyfx.org"), response.Context);

      Assert.Equal("?foo=bar", result.Query);
    }

    [Fact]
    public async Task passes_multiple_queries_through_using_multiple_filters()
    {
      var filter = new PassthroughUriFilter(new string[] { "foo" }, new PassthroughUriFilter(new string[] { "blib" }));

      var appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => { to.Query("foo", "bar"); to.Query("blib", "blob"); });

      var response = await appWithQueryString.Get("");
      var result = filter.Apply(new Uri("http://www.nancyfx.org"), response.Context);

      Assert.Equal("?foo=bar&blib=blob", result.Query);
    }

    [Fact]
    public async Task does_not_passes_the_query_through()
    {
      var filter = new PassthroughUriFilter(new string[] { "does_not_exist" });

      var appWithQueryString = new Browser(with => with.Module<TestModule>(), defaults: to => to.Query("foo", "bar"));

      var response = await appWithQueryString.Get("");
      var result = filter.Apply(new Uri("http://www.nancyfx.org"), response.Context);

      Assert.Equal("", result.Query);
    }
  }
}
