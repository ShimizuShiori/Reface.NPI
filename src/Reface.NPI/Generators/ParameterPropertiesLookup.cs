using Reface.NPI.Attributes;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators
{
    public class ParameterPropertiesLookup : IParameterLookup
    {
        public void Lookup(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            var parameterInfos = methodInfo.GetParameters();
            foreach (var pi in parameterInfos[0].ParameterType.GetProperties())
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
                    matchedParameters.First().Value = pi.GetValue(values[0], null);
                }
            }
        }

        public bool Match(SqlCommandDescription description, MethodInfo methodInfo)
        {
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            if (description.Parameters.Count() > 1)
                return parameterInfos.Length == 1;

            if (description.Parameters.Count() == 1)
                return !parameterInfos[0].ParameterType.IsValueType;

            return false;
        }
    }
}
