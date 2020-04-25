using Reface.NPI.Generators.OperatorMappings.Models;
using System.Collections.Generic;

namespace Reface.NPI.Generators.OperatorMappings
{
    public class DefaultOperatorMapper : IOperatorMapper
    {

        private Dictionary<string, string> textToOperatorMap;
        private static readonly ICache cache;

        static DefaultOperatorMapper()
        {
            cache = NpiServicesCollection.GetService<ICache>();
        }

        protected IResourceProvider ResourceProvider { get; private set; }

        public DefaultOperatorMapper()
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
            string result;
            if (this.textToOperatorMap.TryGetValue(text, out result))
                return result;

            return null;
        }

        private Mappings GetMappings()
        {
            return cache.GetOrCreate<Mappings>($"{this.GetType().FullName}.{nameof(GetMappings)}", key =>
            {
                string name = "Reface.NPI.Resources.OperatorMappings.xml";
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
