﻿using Reface.NPI.Attributes;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class ParameterValuesLookup : IParameterLookup
    {

        public bool Match(SqlCommandDescription description, MethodInfo methodInfo)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (description.Parameters.Count() > 1)
                return description.Parameters.Count() == parameterInfos.Length;

            if (description.Parameters.Count() == 1)
                return parameterInfos[0].ParameterType.IsValueType;

            return false;
        }
        public void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            int i = 0;
            foreach (var pi in parameterInfos)
            {
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