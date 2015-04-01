namespace Nancy.Linker
{
  using System.Collections.Generic;
  using System.Linq;
  using Nancy.Bootstrapper;

  public class Registration : IRegistrations
  {
    public IEnumerable<TypeRegistration> TypeRegistrations { get; } = Enumerable.Repeat(new TypeRegistration(typeof(IResourceLinker), typeof(ResourceLinker)), 1);
    public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations { get; } = null;
    public IEnumerable<InstanceRegistration> InstanceRegistrations { get; } = null;
  }
}
