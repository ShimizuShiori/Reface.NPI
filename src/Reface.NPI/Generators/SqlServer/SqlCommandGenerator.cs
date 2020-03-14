using Reface.NPI.Models;
using System;
using System.Linq;
using System.Text;

namespace Reface.NPI.Generators.SqlServer
{
    public class SqlCommandGenerator : SqlCommandGeneratorBase
    {
        private readonly SqlServerOperatorMapper operatorMapper = new SqlServerOperatorMapper();

        protected override SqlCommandDescription Generate(SelectInfo selectInfo, string tableName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription result = new SqlCommandDescription();

            sqlBuilder.Append("SELECT ");

            if (!selectInfo.Fields.Any())
                sqlBuilder.Append("*");
            else
                sqlBuilder.Append(selectInfo.Fields.Join(",", x => $"[{x}]"));

            sqlBuilder.Append($" FROM [{tableName}]");

            if (selectInfo.Conditions.Any())
            {
                sqlBuilder.Append(" WHERE");
                foreach (var condition in selectInfo.Conditions)
                {
                    sqlBuilder.Append($" [{condition.Field}]");
                    sqlBuilder.Append($" {operatorMapper.GetOperatorByText(condition.Operators)} @{condition.Field}");
                    result.AddParameter(new SqlParameterInfo(condition.Field, ParameterUses.ForCondition));
                    if (condition.JoinerToNext != ConditionJoiners.Null)
                        sqlBuilder.Append($" {condition.JoinerToNext.ToString()}");
                }
            }
            if (selectInfo.Orders.Any())
            {
                sqlBuilder.Append(" ORDER BY ");
                string orderBy = selectInfo.Orders.Join(",", x =>
                {
                    return $"[{x.Field}] {x.Type.ToString()}";
                });
                sqlBuilder.Append(orderBy);
            }
            result.SqlCommand = sqlBuilder.ToString();
            return result;
        }

        protected override SqlCommandDescription Generate(UpdateInfo updateInfo, string tableName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription result = new SqlCommandDescription();

            sqlBuilder.Append($"UPDATE [{tableName}] SET ");

            string setCommand = updateInfo.SetFields.Join(",", x =>
            {
                result.AddParameter(new SqlParameterInfo(x, ParameterUses.ForSet));
                return $"[{x}] = @{x}";
            });

            sqlBuilder.Append(setCommand);
            sqlBuilder.Append(" WHERE");

            foreach (var condition in updateInfo.Conditions)
            {
                sqlBuilder.Append($" [{condition.Field}] {operatorMapper.GetOperatorByText(condition.Operators)} @{condition.Field}");
                if (condition.JoinerToNext != ConditionJoiners.Null)
                    sqlBuilder.Append($" {condition.JoinerToNext.ToString()}");
                result.AddParameter(new SqlParameterInfo(condition.Field, ParameterUses.ForCondition));
            }
            result.SqlCommand = sqlBuilder.ToString();
            return result;
        }

        protected override SqlCommandDescription Generate(DeleteInfo deleteInfo, string tableName)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription description = new SqlCommandDescription();
            sqlBuilder.Append($"DELETE FROM [{tableName}]");
            if (deleteInfo.ConditionInfos.Any())
            {
                sqlBuilder.Append(" WHERE");
                foreach (var condition in deleteInfo.ConditionInfos)
                {
                    sqlBuilder.Append($" [{condition.Field}] {operatorMapper.GetOperatorByText(condition.Operators)} @{condition.Field}");
                    if (condition.JoinerToNext != ConditionJoiners.Null)
                        sqlBuilder.Append($" {condition.JoinerToNext.ToString()}");
                    description.AddParameter(new SqlParameterInfo(condition.Field, ParameterUses.ForCondition));
                }
            }
            description.SqlCommand = sqlBuilder.ToString();
            return description;
        }
    }
}
