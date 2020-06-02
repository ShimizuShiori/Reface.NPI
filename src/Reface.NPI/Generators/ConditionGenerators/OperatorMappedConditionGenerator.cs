using Reface.NPI.Generators.OperatorMappings;
using System.Data.SqlClient;
using System.Text;

namespace Reface.NPI.Generators.ConditionGenerators
{
    /// <summary>
    /// 在 OperatorMapper 中映射的条件生成器。
    /// 它会生成 [字段名] [操作符] [字段] 的形式
    /// </summary>
    public class OperatorMappedConditionGenerator : IConditionGenerator
    {
        private static readonly IOperatorMapper operatorMapper;

        static OperatorMappedConditionGenerator()
        {
            operatorMapper = NpiServicesCollection.GetService<IOperatorMapper>();
        }

        public bool Generate(ConditionGeneratorContext context)
        {
            string opr = context.Operator;
            opr = operatorMapper.GetOperatorByText(opr);
            if (string.IsNullOrEmpty(opr)) return false;

            context.SqlBuilder.Append($"[{context.FieldName}] {opr} {context.GetParameterCommand(context.ParameterName)}");
            context.AddParameter(context.ParameterName);
            return true;
        }
    }
}
