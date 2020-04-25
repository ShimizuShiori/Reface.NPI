using Reface.NPI.Models;
using System.Text;

namespace Reface.NPI.Generators.ConditionGenerators
{
    /// <summary>
    /// 生成条件语句的上下文
    /// </summary>
    public class ConditionGeneratorContext
    {
        private readonly ISqlCommandGenerator sqlCommandGenerator;
        private readonly SqlCommandDescription sqlCommandDescription;
        private readonly StringBuilder sqlBuilder;
        private readonly ConditionInfo conditionInfo;

        public ConditionGeneratorContext(ISqlCommandGenerator sqlCommandGenerator, SqlCommandDescription sqlCommandDescription, StringBuilder sqlBuilder, ConditionInfo conditionInfo)
        {
            this.sqlCommandGenerator = sqlCommandGenerator;
            this.sqlCommandDescription = sqlCommandDescription;
            this.sqlBuilder = sqlBuilder;
            this.conditionInfo = conditionInfo;
        }

        public StringBuilder SqlBuilder { get { return this.sqlBuilder; } }

        public string FieldName { get { return conditionInfo.Field; } }

        public string Operator { get { return conditionInfo.Operators; } }

        public string ParameterName { get { return conditionInfo.Parameter; } }

        public void AddParameter(string name)
        {
            this.sqlCommandDescription.AddParameter(new SqlParameterInfo(name));
        }

        public string GetParameterCommand(string name)
        {
            return this.sqlCommandGenerator.GenerateParameterName(name);
        }
    }
}
