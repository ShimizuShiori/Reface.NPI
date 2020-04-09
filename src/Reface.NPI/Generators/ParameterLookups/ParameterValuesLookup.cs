using Reface.NPI.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators.ParameterLookups
{
    [Obsolete("DefaultParameterLookup 中实现了此功能", true)]
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
                bool isArray = IsArray(pi.ParameterType);
                if (!(isArray || IsBaseType(pi.ParameterType)))
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
                    var matchedParameter = matchedParameters.First();
                    if (isArray)
                    {
                        Array array = (Array)values[i];
                        int length = array.Length;
                        List<string> newParameterNameList = new List<string>();
                        for (int index = 0; index < length; index++)
                        {
                            string newParameterName = $"{matchedParameter.Name}{index}";
                            newParameterNameList.Add(newParameterName);
                            description.AddParameter(new SqlParameterInfo()
                            {
                                Name = newParameterName,
                                Use = matchedParameter.Use,
                                Value = array.GetValue(index)
                            });
                        }
                        description.Parameters.Remove(matchedParameter.Name);
                        string newParameterNameListStr = newParameterNameList.Join(",", x => x);
                        description.SqlCommand = description.SqlCommand.Replace($"@{matchedParameter.Name}", $"({newParameterNameListStr})");

                    }
                    else
                        matchedParameter.Value = values[i];
                }

                i++;
            }
        }
    }
}
