namespace Nancy.Linker.Tests
{
  using System;
  using Testing;
  using Xunit;

  public class ResourceLinker_Should
  {
    private Browser app;

    public class TestModule : NancyModule
    {
      public static IResourceLinker linker;

      public TestModule(IResourceLinker linker)
      {
        TestModule.linker = linker;
        Get["foo", "/foo"] = _ => 200;
        Get["bar", "/bar/{id}"] = _ => 200;
        Get["no segments", "/"] = _ => 200;
        Get["constraint", "/intConstraint/{id: int}"] = _ => 200;
        Get["regex", @"/regex/(?<id>[\d]{ 1,7})"] = _ => 200;
        Get["optional", "optional/{id?}"] = _ => 200;
        Get["default", "default/{id?123}"] = _ => 200;
      }
    }

    public ResourceLinker_Should()
    {
      app = new Browser(with => with.Module<TestModule>(), 
      defaults: to => to.HostName("nancyfx.org"));
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_no_params()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "foo");

      Assert.Equal("http://nancyfx.org/foo", uriString.ToString());
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_params()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "bar", new {id = 123 });

      Assert.Equal("http://nancyfx.org/bar/123", uriString.ToString());
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_segments()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/").Context, "no segments");

      Assert.Equal("http://nancyfx.org/", uriString.ToString());
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_constraint()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "constraint", new { id = 123 });

      Assert.Equal("http://nancyfx.org/intConstraint/123", uriString.ToString());
    }

    [Fact(/*Skip = "might not want to support regex routes??"*/)]
    public void generate_absolute_uri_correctly_when_route_has_regex()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "regex", new { id = 123 });

      Assert.Equal("http://nancyfx.org/regex/123", uriString.ToString());
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_optional()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "optional", new { id = 123 });

      Assert.Equal("http://nancyfx.org/optional/123", uriString.ToString());
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_optional_with_default()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "default", new { id = 123});

      Assert.Equal("http://nancyfx.org/default/123", uriString.ToString());
    }

    [Fact]
    public void generate_relative_uri_correctly_when_route_has_no_params()
    {
      var uriString = TestModule.linker.BuildRelativeUri(app.Get("/foo").Context, "foo");

      Assert.Equal("/foo", uriString.ToString());
    }

    [Fact]
    public void generate_relative_uri_correctly_when_route_has_params()
    {
      var uriString = TestModule.linker.BuildRelativeUri(app.Get("/foo").Context, "bar", new { id = 123 });

      Assert.Equal("/bar/123", uriString.ToString());
    }

    [Fact]
    public void throw_if_parameter_from_template_cannot_be_bound()
    {
      Assert.Throws<ArgumentException>(() =>
        TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "bar")
      );
    }

    [Fact]
    public void default_to_localhost_when_request_has_no_host()
    {
      var appWithoutHost = new Browser(with => with.Module<TestModule>());

      var uriString = TestModule.linker.BuildAbsoluteUri(appWithoutHost.Get("/foo").Context, "bar", new { id = 123 });

      Assert.Equal("http://localhost/bar/123", uriString.ToString());
    }
  }
}