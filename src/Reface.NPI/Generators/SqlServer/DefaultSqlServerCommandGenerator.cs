using Reface.NPI.Generators.OperatorMappings;
using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reface.NPI.Generators.SqlServer
{
    public class DefaultSqlServerCommandGenerator : SqlCommandGeneratorBase, ISqlServerCommandGenerator
    {
        private readonly IOperatorMapper operatorMapper = new SqlServerOperatorMapper();
        private readonly IFieldNameProvider fieldNameProvider;

        public DefaultSqlServerCommandGenerator()
        {
            this.fieldNameProvider = NpiServicesCollection.GetService<IFieldNameProvider>();
        }

        protected override SqlCommandDescription GenerateSelect(SqlCommandGenerateContext context)
        {
            SelectInfo selectInfo = (SelectInfo)context.CommandInfo;
            string tableName = context.TableName;

            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription result = new SqlCommandDescription();
            var generateContext = new GenerateContext(result, sqlBuilder);

            sqlBuilder.Append("SELECT ");

            if (!selectInfo.Fields.Any())
                sqlBuilder.Append("*");
            else
                sqlBuilder.Append(selectInfo.Fields.Join(",", x => $"[{x}]"));

            sqlBuilder.Append($" FROM [{tableName}]");

            GenerateByConditions(ref generateContext, selectInfo.Conditions);

            string orderBy;
            if (!selectInfo.Paging)
            {
                GenerateSqlByOrders(ref generateContext, selectInfo.Orders);
                result.SqlCommand = sqlBuilder.ToString();
                return result;
            }

            if (!selectInfo.Orders.Any())
                throw new NotImplementedException("分页查询必须具有排序字段");

            StringBuilder shellBuilder = new StringBuilder();
            shellBuilder.Append("SELECT * FROM ( SELECT *,ROW_NUMBER() OVER (");
            generateContext.StringBuilder = shellBuilder;
            GenerateSqlByOrders(ref generateContext, selectInfo.Orders);
            shellBuilder.Append(" ) AS __RN__ FROM ( ");
            shellBuilder.Append(sqlBuilder.ToString());
            shellBuilder.Append(") t ) t WHERE t.__RN__ > @BEGINRN AND t.__RN__ <= @ENDRN");
            result.AddParameter(new SqlParameterInfo() { Name = Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER, Use = ParameterUses.ForCondition });
            result.AddParameter(new SqlParameterInfo() { Name = Constant.PARAMETER_NAME_END_ROW_NUMBER, Use = ParameterUses.ForCondition });
            result.SqlCommand = shellBuilder.ToString();
            return result;
        }

        protected override SqlCommandDescription GenerateUpdate(SqlCommandGenerateContext context)
        {
            string tableName = context.TableName;
            UpdateInfo updateInfo = (UpdateInfo)context.CommandInfo;

            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription result = new SqlCommandDescription();
            GenerateContext generateContext = new GenerateContext(result, sqlBuilder);

            sqlBuilder.Append($"UPDATE [{tableName}] SET ");

            string setCommand = updateInfo.SetFields.Join(",", x =>
            {
                result.AddParameter(new SqlParameterInfo(x.Parameter, ParameterUses.ForSet));
                return $"[{x.Field}] = @{x.Parameter}";
            });

            sqlBuilder.Append(setCommand);

            GenerateByConditions(ref generateContext, updateInfo.Conditions);

            result.SqlCommand = sqlBuilder.ToString();
            return result;
        }

        protected override SqlCommandDescription GenerateDelete(SqlCommandGenerateContext context)
        {
            string tableName = context.TableName;
            DeleteInfo deleteInfo = (DeleteInfo)context.CommandInfo;
            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription description = new SqlCommandDescription();
            GenerateContext generateContext = new GenerateContext(description, sqlBuilder);

            sqlBuilder.Append($"DELETE FROM [{tableName}]");
            GenerateByConditions(ref generateContext, deleteInfo.ConditionInfos);

            description.SqlCommand = sqlBuilder.ToString();
            return description;
        }

        protected override SqlCommandDescription GenerateInsert(SqlCommandGenerateContext context)
        {
            SqlCommandDescription description = new SqlCommandDescription();

            InsertInfo info = (InsertInfo)context.CommandInfo;

            HashSet<string> lowerCaseWithoutFields = new HashSet<string>
                (
                    info.WithoutFields.Select(x => x.ToLower())
                );

            IEnumerable<string> columnNames = context.EntityType.GetProperties()
                .Select(x => fieldNameProvider.Provide(x))
                .Where(x => !lowerCaseWithoutFields.Contains(x.ToLower()));
            string fields = columnNames.Join(",", x => $"[{x}]");
            string values = columnNames.Join(",", x => $"@{x}");
            foreach (var columnName in columnNames)
            {
                description.AddParameter(new SqlParameterInfo(columnName, ParameterUses.ForInsert));
            }
            description.SqlCommand = $"INSERT INTO [{context.TableName}]({fields})VALUES({values})";
            return description;
        }

        private void GenerateByConditions(ref GenerateContext context, IEnumerable<ConditionInfo> conditions)
        {
            if (conditions == null) return;
            if (!conditions.Any()) return;
            var sqlBuilder = context.StringBuilder;
            var result = context.SqlCommandDescription;
            sqlBuilder.Append(" WHERE");
            foreach (var condition in conditions)
            {
                GenerateSqlByCondition(ref sqlBuilder, condition);
                result.AddParameter(new SqlParameterInfo(condition.Parameter, ParameterUses.ForCondition));
            }
        }

        private void GenerateSqlByCondition(ref StringBuilder sqlBuilder, ConditionInfo condition)
        {
            sqlBuilder.Append($" [{condition.Field}]");
            sqlBuilder.Append($" {operatorMapper.GetOperatorByText(condition.Operators)} @{condition.Parameter}");
            if (condition.JoinerToNext != ConditionJoiners.Null)
                sqlBuilder.Append($" {condition.JoinerToNext.ToString()}");
        }

        private void GenerateSqlByOrders(ref GenerateContext context, IEnumerable<OrderInfo> orders)
        {
            if (orders == null) return;
            if (orders.Count() == 0) return;
            var sqlBuilder = context.StringBuilder;
            var description = context.SqlCommandDescription;
            sqlBuilder.Append(" ORDER BY ");
            sqlBuilder.Append(orders.Join(",", x =>
            {
                return $"[{x.Field}] {x.Type.ToString()}";
            }));
        }
    }
}
