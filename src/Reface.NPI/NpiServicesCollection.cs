using Reface.NPI.Generators;
using Reface.NPI.Generators.ParameterLookups;
using Reface.NPI.Generators.SqlServer;
using Reface.NPI.Parsers;
using Reface.NPI.Parsers.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI
{
    public class NpiServicesCollection
    {
        private readonly static Dictionary<Type, List<Func<Type, object>>> factories = new Dictionary<Type, List<Func<Type, object>>>();

        static NpiServicesCollection()
        {
            RegisterService<ISelectParser>(t => new DefaultSelectParser());
            RegisterService<IUpdateParser>(t => new DefaultUpdateParser());
            RegisterService<IDeleteParser>(t => new DefaultDeleteParser());
            RegisterService<IInsertParser>(t => new DefaultInsertParser());
            RegisterService<ICountParser>(t => new DefaultCountParser());
            RegisterService<ICommandParser>(t => new DefaultCommandParser());
            RegisterService<IParameterLookup>(t => new PagingParameterLookup());
            RegisterService<IParameterLookup>(t => new DefaultParameterLookup());
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
        }

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

        public static void ReplaceService<T>(Func<Type, T> factory)
        {
            List<Func<Type, object>> fs = new List<Func<Type, object>>();
            fs.Add(x => factory(typeof(T)));
            factories[typeof(T)] = fs;
        }

        public static T GetService<T>()
        {
            List<Func<Type, Object>> fs;
            if (!factories.TryGetValue(typeof(T), out fs))
                throw new KeyNotFoundException("未注册的组件");

            if (fs.Count() > 1)
                throw new IndexOutOfRangeException("注册有多个组件，请使用 GetServices");

            return (T)fs.First()(typeof(T));
        }

        public static IEnumerable<T> GetServices<T>()
        {
            List<Func<Type, Object>> fs;
            if (!factories.TryGetValue(typeof(T), out fs))
                throw new KeyNotFoundException("未注册的组件");

            return fs.Select(x => x(typeof(T))).Cast<T>();
        }
    }
}
