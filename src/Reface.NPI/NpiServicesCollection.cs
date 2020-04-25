using Reface.NPI.Generators;
using Reface.NPI.Generators.ConditionGenerators;
using Reface.NPI.Generators.OperatorMappings;
using Reface.NPI.Generators.ParameterLookups;
using Reface.NPI.Generators.SqlServer;
using Reface.NPI.Parsers;
using Reface.NPI.Parsers.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI
{
    /// <summary>
    /// 与 NPI 有关的所有服务的集合。
    /// 开发者可以通过这个类型追加或替换已有的服务来扩展或重写各种逻辑。
    /// </summary>
    public class NpiServicesCollection
    {
        private readonly static Dictionary<Type, List<Func<Type, object>>> factories = new Dictionary<Type, List<Func<Type, object>>>();

        private readonly static Dictionary<Type, Object> singletonPool = new Dictionary<Type, object>();

        static NpiServicesCollection()
        {
            RegisterService<ISelectParser>(t => new DefaultSelectParser());
            RegisterService<IUpdateParser>(t => new DefaultUpdateParser());
            RegisterService<IDeleteParser>(t => new DefaultDeleteParser());
            RegisterService<IInsertParser>(t => new DefaultInsertParser());
            RegisterService<ICountParser>(t => new DefaultCountParser());
            RegisterService<ICommandParser>(t => new DefaultCommandParser());

            RegisterService<IParameterLookup, PagingParameterLookup>();
            RegisterService<IParameterLookup, DefaultParameterLookup>();
            RegisterService<IParameterLookup, BetweenParameterLookup>();

            RegisterService<IParameterLookupFactory>(t => new DefaultParameterLookupFactory());
            RegisterService<ISqlServerCommandGenerator>(t => new DefaultSqlServerCommandGenerator());
            RegisterService<IEntityTypeProvider>(t => new DefaultEntityTypeProvider());
            RegisterService<ITableNameProvider>(t => new DefaultTableNameProvider());
            RegisterService<IFieldNameProvider>(t => new DefaultFieldNameProvider());
            RegisterService<IStateMachineBuilderFactory>(t => new DefaultStateMachineBuilderFactory());
            RegisterService<ICache>(t => new DefaultCache());
            RegisterService<IResourceNameProvider>(t => new DefaultResourceNameProvider());
            RegisterService<IResourceProvider>(t => new DefaultResourceProvider());
            RegisterService<ISqlParameterFinder>(t => new DefaultSqlParameterFinder());
            RegisterService<IConditionGenerateHandler>(t => new DefaultConditionGenerateHandler());

            RegisterService<IConditionGenerator, OperatorMappedConditionGenerator>();
            RegisterService<IConditionGenerator, BetweenConditionGenerator>();

            RegisterService<IOperatorMapper, DefaultOperatorMapper>();
        }

        /// <summary>
        /// 注册一个新的服务。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        public static void RegisterService<T>(Func<Type, T> factory)
        {
            Type type = typeof(T);
            List<Func<Type, Object>> fs;
            if (!factories.TryGetValue(type, out fs))
            {
                fs = new List<Func<Type, object>>();
                factories[type] = fs;
            }
            fs.Add(x => factory(type));
        }

        public static void RegisterService<TService, TComponent>()
            where TComponent : TService, new()
        {
            RegisterService<TService>(t => new TComponent());
        }

        /// <summary>
        /// 替换服务，会将已有的服务删除，并注册指定的服务。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="factory"></param>
        public static void ReplaceService<T>(Func<Type, T> factory)
        {
            List<Func<Type, object>> fs = new List<Func<Type, object>>();
            fs.Add(x => factory(typeof(T)));
            factories[typeof(T)] = fs;
        }

        /// <summary>
        /// 获取一个服务，当集合内有多个相同的服务，会抛出异常。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            List<Func<Type, Object>> fs;
            if (!factories.TryGetValue(typeof(T), out fs))
                throw new KeyNotFoundException("未注册的组件 : " + typeof(T).FullName);

            if (fs.Count() > 1)
                throw new IndexOutOfRangeException("注册有多个组件，请使用 GetServices");

            return (T)fs.First()(typeof(T));
        }

        /// <summary>
        /// 获取一组服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> GetServices<T>()
        {
            List<Func<Type, Object>> fs;
            if (!factories.TryGetValue(typeof(T), out fs))
                throw new KeyNotFoundException("未注册的组件 : " + typeof(T).FullName);

            return fs.Select(x => x(typeof(T))).Cast<T>();
        }
    }
}
