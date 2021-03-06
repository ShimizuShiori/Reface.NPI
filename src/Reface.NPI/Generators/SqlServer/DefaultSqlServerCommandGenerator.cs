﻿using Reface.NPI.Generators.ConditionGenerators;
using Reface.NPI.Generators.OperatorMappings;
using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reface.NPI.Generators.SqlServer
{
    /// <summary>
    /// 面向 SqlServer 的 <see cref="ISqlCommandGenerator"/> 。
    /// </summary>
    public class DefaultSqlServerCommandGenerator : SqlCommandGeneratorBase, ISqlServerCommandGenerator
    {
        private readonly IFieldNameProvider fieldNameProvider;
        private readonly IConditionGenerateHandler conditionGenerateHandler;

        public DefaultSqlServerCommandGenerator()
        {
            this.fieldNameProvider = NpiServicesCollection.GetService<IFieldNameProvider>();
            this.conditionGenerateHandler = NpiServicesCollection.GetService<IConditionGenerateHandler>();
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
            shellBuilder.Append($") t ) t WHERE t.__RN__ > @{Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER} AND t.__RN__ <= @{Constant.PARAMETER_NAME_END_ROW_NUMBER}");
            result.AddParameter(new SqlParameterInfo() { Name = Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER });
            result.AddParameter(new SqlParameterInfo() { Name = Constant.PARAMETER_NAME_END_ROW_NUMBER });
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
            string setCommand;
            if (updateInfo.SetFields.Any())
            {
                setCommand = updateInfo.SetFields.Join(",", x =>
                {
                    result.AddParameter(new SqlParameterInfo(x.Parameter));
                    return $"[{x.Field}] = @{x.Parameter}";
                });
            }
            else
            {
                HashSet<string> conditionField = new HashSet<string>(updateInfo.Conditions.Select(x => x.Field));
                HashSet<string> lowerCaseWithoutFields = new HashSet<string>(updateInfo.WithoutFields.Select(x => x.ToLower()));
                setCommand = GetColumnNames(context)
                    .Where(x => !conditionField.Contains(x))
                    .Where(x => !lowerCaseWithoutFields.Contains(x.ToLower()))
                    .Join(",", x =>
                    {
                        result.AddParameter(new SqlParameterInfo(x));
                        return $"[{x}] = @{x}";
                    });

            }

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

            IEnumerable<string> columnNames = GetColumnNames(context)
                .Where(x => !lowerCaseWithoutFields.Contains(x.ToLower()));
            string fields = columnNames.Join(",", x => $"[{x}]");
            string values = columnNames.Join(",", x => $"@{x}");
            foreach (var columnName in columnNames)
            {
                description.AddParameter(new SqlParameterInfo(columnName));
            }
            description.SqlCommand = $"INSERT INTO [{context.TableName}]({fields})VALUES({values});SELECT ISNULL(SCOPE_IDENTITY(),0) AS [Id]";
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
                if (condition.IsNot)
                    sqlBuilder.Append(" NOT(");
                ConditionGeneratorContext conditionContext = new ConditionGeneratorContext(this,
                    result,
                    sqlBuilder,
                    condition);
                this.conditionGenerateHandler.Handle(conditionContext);
                if (condition.IsNot)
                    sqlBuilder.Append(")");
                if (condition.JoinerToNext != ConditionJoiners.Null)
                    sqlBuilder.Append($" {condition.JoinerToNext}");
            }
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

        private IEnumerable<string> GetColumnNames(SqlCommandGenerateContext context)
        {
            return context.EntityType.GetProperties()
                .Select(x => fieldNameProvider.Provide(x));
        }

        public override string GenerateParameterName(string name)
        {
            return $"@{name}";
        }

        protected override SqlCommandDescription GenerateCount(SqlCommandGenerateContext context)
        {
            string tableName = context.TableName;
            CountInfo deleteInfo = (CountInfo)context.CommandInfo;
            StringBuilder sqlBuilder = new StringBuilder();
            SqlCommandDescription description = new SqlCommandDescription();
            GenerateContext generateContext = new GenerateContext(description, sqlBuilder);

            sqlBuilder.Append($"SELECT COUNT(*) AS [{Constant.RESULT_FIELD_NAME_COUNT}] FROM [{tableName}]");
            GenerateByConditions(ref generateContext, deleteInfo.ConditionInfos);

            description.SqlCommand = sqlBuilder.ToString();
            return description;
        }
    }
}
