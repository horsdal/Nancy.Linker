namespace Nancy.Linker.Tests
{
  using Testing;
  using Xunit;

  public class NancyContextExtensions_Should
  {
    private Browser app;
    public NancyContextExtensions_Should()
    {
      this.app = new Browser(new DefaultNancyBootstrapper());
    }

    [Fact]
    public void build_absolute_uri_through_extension_method()
    {
      var actual = this.app.Get("/bar").Context.BuildAbsoluteUri("foo");

      Assert.Equal("http://localhost/foo", actual.ToString());
    }

    [Fact]
    public void build_relative_uri_through_extension_method()
    {
      var actual = this.app.Get("/bar").Context.BuildRelativeUri("foo");

      Assert.Equal("/foo", actual.ToString());
    }
  }
}
