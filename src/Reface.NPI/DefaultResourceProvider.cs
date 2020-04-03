using System.IO;
using System.Reflection;

namespace Reface.NPI
{
    public class DefaultResourceProvider : IResourceProvider
    {
        private static Assembly assembly;

        static DefaultResourceProvider()
        {
            assembly = typeof(DefaultResourceNameProvider).Assembly;
        }

        public Stream Provide(string resourceName)
        {
            return assembly.GetManifestResourceStream(resourceName);
        }
    }
}
