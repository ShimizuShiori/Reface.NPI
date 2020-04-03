using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPITests
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class ResourceNameProviderTests
    {
        private Stream GetResourceStream(string name)
        {
            Assembly assembly = typeof(IResourceNameProvider).Assembly;
            return assembly.GetManifestResourceStream(name);
        }

        private readonly IResourceNameProvider resourceNameProvider;

        public ResourceNameProviderTests()
        {
            this.resourceNameProvider = NpiServicesCollection.GetService<IResourceNameProvider>();
        }

        [TestMethod]
        [DataRow("Select")]
        public void GetStateMachineCsv(string stateMachineName)
        {
            using (var stream = GetResourceStream(this.resourceNameProvider.GetStateMachineCsv(stateMachineName)))
            {
                Assert.IsNotNull(stream);
            }
        }
    }
}
