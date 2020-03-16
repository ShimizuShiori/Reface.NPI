using Reface.NPI.Generators;
using Reface.NPI.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reface.NPI
{
    public class NpiServicesCollection
    {
        private readonly static Dictionary<Type, List<Func<Type, Object>>> factories = new Dictionary<Type, List<Func<Type, object>>>();

        static NpiServicesCollection()
        {
            RegisterService<ISelectParser>(t => new DefaultSelectParser());
            RegisterService<IUpdateParser>(t => new DefaultUpdateParser());
            RegisterService<IDeleteParser>(t => new DefaultDeleteParser());
            RegisterService<ICommandParser>(t => new DefaultCommandParser());
            RegisterService<IParameterLookup>(t => new ParameterValuesLookup());
            RegisterService<IParameterLookup>(t => new ParameterPropertiesLookup());
            RegisterService<IParameterLookupFactory>(t => new DefaultParameterLookupFactory());
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
