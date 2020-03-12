using Reface.NPI.Models;
using System;
using System.Linq;
using System.Text;

namespace Reface.NPI.Generators.SqlServer
{
    public class SqlCommandGenerator : SqlCommandGeneratorBase
    {
        private readonly SqlServerOperatorMapper operatorMapper = new SqlServerOperatorMapper();

        protected override SqlCommandDescription Generate(SelectInfo selectInfo)
        {
            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription result = new SqlCommandDescription();

            sqlBuilder.Append("SELECT ");

            if (!selectInfo.Fields.Any())
                sqlBuilder.Append("*");
            else
                sqlBuilder.Append(selectInfo.Fields.Join(",", x => $"[{x}]"));

            if (selectInfo.Conditions.Any())
            {
                sqlBuilder.Append(" WHERE");
                foreach (var condition in selectInfo.Conditions)
                {
                    sqlBuilder.Append($" [{condition.Field}]");
                    sqlBuilder.Append($" {operatorMapper.GetOperatorByText(condition.Operators)} @{condition.Field}");
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

        protected override SqlCommandDescription Generate(UpdateInfo updateInfo)
        {
            throw new NotImplementedException();
        }

        protected override SqlCommandDescription Generate(DeleteInfo deleteInfo)
        {
            throw new NotImplementedException();
        }

        protected override SqlCommandDescription Generate(InsertInfo insertInfo)
        {
            throw new NotImplementedException();
        }
    }
}
