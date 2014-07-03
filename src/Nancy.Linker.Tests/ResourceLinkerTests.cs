namespace Nancy.Linker.Tests
{
  using System.Runtime.InteropServices;
  using Testing;
  using Xunit;

  public class ResourceLinkerTests
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

    public ResourceLinkerTests()
    {
      app = new Browser(with => with.Module<TestModule>(), defaults: to => to.HostName("localhost"));
    }

    [Fact]
    public void Link_generated_is_correct_when_base_uri_has_trailing_slash()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "foo",  new {});

      Assert.Equal("http://localhost/foo", uriString.ToString());
    }


    [Fact]
    public void Link_generated_is_correct_with_bound_parameter()
    {
      var uriString = TestModule.linker.BuildAbsoluteUri(app.Get("/foo").Context, "bar", new {id = 123 });

      Assert.Equal("http://localhost/bar/123", uriString.ToString());
    }

    [Fact]
    public void Argument_exception_is_thrown_if_parameter_from_template_cannot_be_bound()
    {
    }
  }
}