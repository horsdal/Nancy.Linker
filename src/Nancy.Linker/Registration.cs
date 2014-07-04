namespace Nancy.Linker
{
  using System.Collections.Generic;
  using System.Linq;
  using Bootstrapper;

  public class Registration : IRegistrations
  {
    public IEnumerable<TypeRegistration> TypeRegistrations
    {
      get
      {
        return Enumerable.Repeat(new TypeRegistration(typeof(IResourceLinker), typeof(ResourceLinker)), 1);
      }
    }
    public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations { get; private set; }
    public IEnumerable<InstanceRegistration> InstanceRegistrations { get; private set; }
  }
}
