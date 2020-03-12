using Reface.NPI.Generators.OperatorMappings.Models;
using System.Collections.Generic;
using System.IO;

namespace Reface.NPI.Generators.OperatorMappings
{
    public abstract class OperatorMapperBase : IOperatorMapper
    {
        protected abstract string OperatorMappingName { get; }

        private Dictionary<string, string> textToOperatorMap;

        public string GetOperatorByText(string text)
        {
            if (textToOperatorMap == null)
            {
                textToOperatorMap = new Dictionary<string, string>();
                string xml = File.ReadAllText(PathProvider.GetOperatorMappingXml(this.OperatorMappingName));
                Mappings mappings = xml.ToObjectAsXml<Mappings>();
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
    }
}
