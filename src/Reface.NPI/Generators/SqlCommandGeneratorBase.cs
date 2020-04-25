using Reface.NPI.Attributes;
using Reface.NPI.Configs;
using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    /// <summary>
    /// <see cref="ISqlCommandGenerator"/> 的基本实现。
    /// </summary>
    public abstract class SqlCommandGeneratorBase : ISqlCommandGenerator
    {
        private readonly IParameterLookupFactory parameterLookupFactory;
        private readonly ICache cache;
        private readonly ISqlParameterFinder sqlParameterFinder;

        public SqlCommandGeneratorBase()
        {
            this.parameterLookupFactory = NpiServicesCollection.GetService<IParameterLookupFactory>();
            this.cache = NpiServicesCollection.GetService<ICache>();
            this.sqlParameterFinder = NpiServicesCollection.GetService<ISqlParameterFinder>();
        }

        public SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments)
        {
            var context = SqlCommandGenerateContext.Create()
                .SetMethod(methodInfo)
                .Build();

            string cacheKey = $"SqlCOmmandDescription_{context.ToString()}";
            SqlCommandDescription description = cache.GetOrCreate<SqlCommandDescription>(cacheKey, key =>
            {
                if (context.HasAnyQueryAttribute)
                    return GetDescriptionByQuery(context);
                else
                    return GetSqlCommandDescriptionWithourParameterFilled(context);
            });

            if (arguments != null && arguments.Length != 0)
            {
                ParameterLookupContext lookupContet
                    = new ParameterLookupContext
                    (
                        this,
                        description,
                        methodInfo,
                        arguments
                    );
                this.parameterLookupFactory.Lookup(lookupContet);
            }
            return description;
        }

        /// <summary>
        /// 获取未填充数据的数据库执行信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private SqlCommandDescription GetSqlCommandDescriptionWithourParameterFilled(SqlCommandGenerateContext context)
        {
            SqlCommandDescription description;
            switch (context.CommandInfo.Type)
            {
                case CommandInfoTypes.Delete:
                    description = GenerateDelete(context);
                    description.Mode = SqlCommandExecuteModes.Execute;
                    break;
                case CommandInfoTypes.Update:
                    description = GenerateUpdate(context);
                    description.Mode = SqlCommandExecuteModes.Execute;
                    break;
                case CommandInfoTypes.Select:
                    description = GenerateSelect(context);
                    description.Mode = SqlCommandExecuteModes.Query;
                    break;
                case CommandInfoTypes.Insert:
                    description = GenerateInsert(context);
                    description.Mode = SqlCommandExecuteModes.Query;
                    break;
                case CommandInfoTypes.Count:
                    description = GenerateCount(context);
                    description.Mode = SqlCommandExecuteModes.Query;
                    break;
                default: throw new NotImplementedException($"不能处理的命令类型 : {context.CommandInfo.Type.ToString()}");
            }
            return description;
        }

        /// <summary>
        /// 根据 <see cref="QueryAttribute"/>  获取数据库执行信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private SqlCommandDescription GetDescriptionByQuery(SqlCommandGenerateContext context)
        {
            string querySelector = NpiConfig.QuerySelector;
            IEnumerable<SqlAttribute> queryAttributes = context.QueryAttributes.Where(x => x.Selector == querySelector);
            if (queryAttributes.Count() > 1)
                throw new ApplicationException("匹配到多个 QueryAttribute");

            SqlAttribute queryAttribute = queryAttributes.FirstOrDefault();

            SqlCommandDescription description = new SqlCommandDescription()
            {
                SqlCommand = queryAttribute.Sql,
                Mode = queryAttribute.Mode
            };

            foreach (var ps in this.sqlParameterFinder.Find(queryAttribute.Sql))
                description.AddParameter(new SqlParameterInfo(ps));

            return description;
        }

        #region 抽象方法

        /// <summary>
        /// 生成查询语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract SqlCommandDescription GenerateSelect(SqlCommandGenerateContext context);

        /// <summary>
        /// 生成更新语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract SqlCommandDescription GenerateUpdate(SqlCommandGenerateContext context);

        /// <summary>
        /// 生成删除语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract SqlCommandDescription GenerateDelete(SqlCommandGenerateContext context);
        
        /// <summary>
        /// 生成新增语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract SqlCommandDescription GenerateInsert(SqlCommandGenerateContext context);

        /// <summary>
        /// 生成查询行数的语句
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected abstract SqlCommandDescription GenerateCount(SqlCommandGenerateContext context);

        /// <summary>
        /// 生成参数语句
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract string GenerateParameterName(string name);

        #endregion


    }
}
