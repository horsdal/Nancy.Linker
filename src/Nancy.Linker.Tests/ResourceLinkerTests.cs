using System.Threading.Tasks;

namespace Nancy.Linker.Tests
{
  using System;
  using Testing;
  using Xunit;

  public class ResourceLinker_Should
  {
    private readonly Browser app = new Browser(with => with.Module<TestModule>(), defaults: to => to.HostName("nancyfx.org"));

    public class TestModule : NancyModule
    {
      public static IResourceLinker linker;

      public TestModule(IResourceLinker linker)
      {
        TestModule.linker = linker;
        this.Get("/foo", _ => 200, _ => true, "foo");
        this.Get("/bar/{id}", _ => 200, _ => true, "bar");
        this.Get("/", _ => 200, _ => true, "no segments");
        this.Get("/intConstraint/{id: int}", _ => 200, _ => true, "constraint");
        this.Get(@"/regex/(?<id>[\d]{ 1,7})", _ => 200, _ => true, "regex");
        this.Get("optional/{id?}", _ => 200, _ => true, "optional");
        this.Get("default/{id?123}", _ => 200, _ => true, "default");
      }
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_no_params()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "foo");

      Assert.Equal("http://nancyfx.org/foo", actual.ToString());
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_params()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "bar", new {id = 123 });

      Assert.Equal("http://nancyfx.org/bar/123", actual.ToString());
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_segments()
    {
      var response = await this.app.Get("/");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "no segments");

      Assert.Equal("http://nancyfx.org/", actual.ToString());
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_constraint()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "constraint", new { id = 123 });

      Assert.Equal("http://nancyfx.org/intConstraint/123", actual.ToString());
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_regex()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "regex", new { id = 123 });

      Assert.Equal("http://nancyfx.org/regex/123", actual.ToString());
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_optional()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "optional", new { id = 123 });

      Assert.Equal("http://nancyfx.org/optional/123", actual.ToString());
    }

    [Fact]
    public async Task generate_absolute_uri_correctly_when_route_has_optional_with_default()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "default", new { id = 123});

      Assert.Equal("http://nancyfx.org/default/123", actual.ToString());
    }

    [Fact]
    public async Task generate_relative_uri_correctly_when_route_has_no_params()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildRelativeUri(response.Context, "foo");

      Assert.Equal("/foo", actual.ToString());
    }

    [Fact]
    public async Task generate_relative_uri_correctly_when_route_has_params()
    {
      var response = await this.app.Get("/foo");
      var actual = TestModule.linker.BuildRelativeUri(response.Context, "bar", new { id = 123 });

      Assert.Equal("/bar/123", actual.ToString());
    }

    [Fact]
    public void throw_if_parameter_from_template_cannot_be_bound()
    {
      Assert.Throws<ArgumentException>(() =>
        TestModule.linker.BuildAbsoluteUri(this.app.Get("/foo").Result.Context, "bar")
      );
    }

    [Fact]
    public void throw_if_route_cannot_be_found()
    {
      var actual = Assert.Throws<UnknownRouteException>(() =>
        TestModule.linker.BuildAbsoluteUri(this.app.Get("/foo").Result.Context, "missing_route")
      );
        Assert.Contains("foo", actual.Message);
        Assert.Contains("bar", actual.Message);
        Assert.Contains("no segments", actual.Message);
        Assert.Contains("constraint", actual.Message);
        Assert.Contains("regex", actual.Message);
        Assert.Contains("optional", actual.Message);
        Assert.Contains("default", actual.Message);
    }

    [Fact]
    public async Task default_to_localhost_when_request_has_no_host()
    {
      var appWithoutHost = new Browser(with => with.Module<TestModule>());

      var response = await appWithoutHost.Get("/foo");
      var actual = TestModule.linker.BuildAbsoluteUri(response.Context, "bar", new { id = 123 });

      Assert.Equal("http://localhost/bar/123", actual.ToString());
    }
  }
}