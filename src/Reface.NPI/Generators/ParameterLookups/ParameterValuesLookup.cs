using Reface.NPI.Attributes;
using System;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators.ParameterLookups
{
    public class ParameterValuesLookup : IParameterLookup
    {
        private bool IsBaseType(Type type)
        {
            return type == typeof(string)
                || type == typeof(int)
                || type == typeof(long)
                || type == typeof(float)
                || type == typeof(double)
                || type == typeof(short)
                || type == typeof(byte)
                || type == typeof(DateTime)
                || type == typeof(bool)
                || type == typeof(Guid)
                ;
        }

        private bool IsArray(Type type)
        {
            return typeof(Array).IsAssignableFrom(type);
        }
        public void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            int i = 0;
            foreach (var pi in parameterInfos)
            {
                if (!(IsArray(pi.ParameterType) || IsBaseType(pi.ParameterType)))
                    continue;
                bool isSet = false,
                isCondition = false;
                string pName = "";

                pName = pi.Name;

                ParameterAttribute pa = pi.GetCustomAttribute<ParameterAttribute>();
                if (pa != null) pName = pa.Name;

                ForSetAttribute fsa = pi.GetCustomAttribute<ForSetAttribute>();
                if (fsa != null) isSet = true;

                ForConditionAttribute fca = pi.GetCustomAttribute<ForConditionAttribute>();
                if (fca != null) isCondition = true;

                var matchedParameters = description.Parameters.Values
                    .Where(x => x.Name.ToLower() == pName.ToLower());
                if (isSet)
                    matchedParameters = matchedParameters.Where(x => x.Use == ParameterUses.ForSet);
                if (isCondition)
                    matchedParameters = matchedParameters.Where(x => x.Use == ParameterUses.ForCondition);

                if (matchedParameters.Any())
                {
                    matchedParameters.First().Value = values[i];
                }

                i++;
            }
        }
    }
}
