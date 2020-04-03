using Reface.NPI.Generators.OperatorMappings.Models;
using System.Collections.Generic;
using System.IO;

namespace Reface.NPI.Generators.OperatorMappings
{
    public abstract class OperatorMapperBase : IOperatorMapper
    {
        protected abstract string OperatorMappingName { get; }

        private Dictionary<string, string> textToOperatorMap;
        private static readonly ICache cache;

        static OperatorMapperBase()
        {
            cache = NpiServicesCollection.GetService<ICache>();
        }

        protected IResourceProvider ResourceProvider { get; private set; }

        public OperatorMapperBase()
        {
            this.ResourceProvider = NpiServicesCollection.GetService<IResourceProvider>();
        }

        public string GetOperatorByText(string text)
        {
            if (textToOperatorMap == null)
            {
                textToOperatorMap = new Dictionary<string, string>();
                Mappings mappings = this.GetMappings();
                foreach (var mapping in mappings)
                {
                    foreach (var t in mapping.Texts)
                    {
                        this.textToOperatorMap[t] = mapping.Operator;
                    }
                }
            }
            return this.textToOperatorMap[text];
        }

        private Mappings GetMappings()
        {
            return cache.GetOrCreate<Mappings>($"{this.GetType().FullName}.{nameof(GetMappings)}", key =>
            {
                string name = "Reface.NPI.Resources.OperatorMappings.SqlServer.xml";
                using (var stream = this.ResourceProvider.Provide(name))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    string xml = System.Text.Encoding.UTF8.GetString(buffer);
                    return xml.ToObjectAsXml<Mappings>();
                }
            });
        }
    }
}
