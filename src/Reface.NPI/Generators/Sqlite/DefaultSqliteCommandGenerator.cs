using Reface.NPI.Generators.SqlServer;
using Reface.NPI.Models;
using System.Text;

namespace Reface.NPI.Generators.Sqlite
{
    /// <summary>
    /// Sqlite 的语句生成
    /// </summary>
    public class DefaultSqliteCommandGenerator : DefaultSqlServerCommandGenerator
    {
        protected override SqlCommandDescription GenerateSelect(SqlCommandGenerateContext context)
        {
            SelectInfo selectInfo = (SelectInfo)context.CommandInfo;

            if (!selectInfo.Paging)
                return base.GenerateSelect(context);

            selectInfo.Paging = false;
            SqlCommandDescription sqlCommandDescription = base.GenerateSelect(context);
            StringBuilder sqlBuilder = new StringBuilder(sqlCommandDescription.SqlCommand);
            sqlBuilder.AppendFormat(" LIMIT {0}-{1} OFFSET {1}", Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER,
                Constant.PARAMETER_NAME_END_ROW_NUMBER);
            sqlCommandDescription.SqlCommand = sqlBuilder.ToString();
            sqlCommandDescription.AddParameter(new SqlParameterInfo(Constant.PARAMETER_NAME_END_ROW_NUMBER));
            sqlCommandDescription.AddParameter(new SqlParameterInfo(Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER));
            return sqlCommandDescription;
        }
    }
}
