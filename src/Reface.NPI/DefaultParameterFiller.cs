using Reface.NPI.Attributes;
using Reface.NPI.Generators;
using System.Linq;
using System.Reflection;

namespace Reface.NPI
{
    public class DefaultParameterFiller : IParameterFiller
    {
        public void Fill(SqlCommandDescription description, MethodInfo methodInfo, object[] values)
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
