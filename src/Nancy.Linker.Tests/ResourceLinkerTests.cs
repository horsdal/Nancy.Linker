namespace Nancy.Linker.Tests
{
  using System;
  using System.Runtime.InteropServices;
  using Testing;
  using Xunit;

  public class ResourceLinker_Should
  {
    private Browser app;

    public class TestModule : NancyModule
    {
      public static ResourceLinker linker;

      public TestModule(ResourceLinker linker)
      {
        TestModule.linker = linker;
        Get["foo", "/foo"] = _ => 200;
        Get["bar", "/bar/{id}"] = _ => 200;
      }
    }

    public ResourceLinker_Should()
    {
      app = new Browser(with => with.Module<TestModule>(), defaults: to => to.HostName("localhost"));
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_no_params()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "foo");

      Assert.Equal("http://localhost/foo", uriString.ToString());
    }

    [Fact]
    public void generate_absolute_uri_correctly_when_route_has_params()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "bar", new {id = 123 });

      Assert.Equal("http://localhost/bar/123", uriString.ToString());
    }

    [Fact]
    public void throw_if_parameter_from_template_cannot_be_bound()
    {
      Assert.Throws<ArgumentException>(() =>
        TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "bar")
      );
    }
  }
}