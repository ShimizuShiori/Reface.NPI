using System.IO;

namespace Reface.NPI
{
    public interface IResourceProvider
    {
        Stream Provide(string resourceName);
    }
}
