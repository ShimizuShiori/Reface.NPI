using System.Text;

namespace Reface.NPI.Generators.ConditionGenerators
{
    /// <summary>
    /// 对 Between 生成条件语句
    /// </summary>
    public class BetweenConditionGenerator : IConditionGenerator
    {

        public bool Generate(ConditionGeneratorContext context)
        {
            if (context.Operator != "Between") return false;


            string paraNameBegin = string.Format("{0}{1}", context.ParameterName, Constant.PARAMETER_SUFFIX_BETWEEN_BEGIN);
            string paraNameEnd = string.Format("{0}{1}", context.ParameterName, Constant.PARAMETER_SUFFIX_BETWEEN_END);
            context.SqlBuilder.Append($"[{context.FieldName}] BETWEEN {context.GetParameterCommand(paraNameBegin)} AND {context.GetParameterCommand(paraNameEnd)}");

            context.AddParameter(paraNameBegin);
            context.AddParameter(paraNameEnd);
            return true;
        }
    }
}
