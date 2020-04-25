using System.Collections.Generic;
using System.Text;

namespace Reface.NPI.Generators.ConditionGenerators
{
    public class DefaultConditionGenerateHandler : IConditionGenerateHandler
    {
        private readonly IEnumerable<IConditionGenerator> conditionGenerators;

        public DefaultConditionGenerateHandler()
        {
            this.conditionGenerators = NpiServicesCollection.GetServices<IConditionGenerator>();
        }
        public void Handle(ConditionGeneratorContext context)
        {
            foreach (var gen in this.conditionGenerators)
            {
                if (gen.Generate(context)) return;
            }
        }
    }
}
