using System;
using System.Linq;

namespace Reface.NPI.Generators.ParameterLookups
{
    public class BetweenParameterLookup : IParameterLookup
    {
        public void Lookup(ParameterLookupContext context)
        {
            var methodParameters = context.MethodInfo.GetParameters();
            Type betweenType = typeof(BetweenParameter);

            for (int i = 0; i < methodParameters.Length; i++)
            {
                var methodParameter = methodParameters[i];
                if (methodParameter.ParameterType != betweenType) continue;

                BetweenParameter value = context.Values[i] as BetweenParameter;
                if (value == null) return;

                string lowerCaseParaNameBegin = string.Format("{0}{1}", methodParameter.Name, Constant.PARAMETER_SUFFIX_BETWEEN_BEGIN).ToLower();
                string lowerCaseParaNameEnd = string.Format("{0}{1}", methodParameter.Name,
                    Constant.PARAMETER_SUFFIX_BETWEEN_END).ToLower();

                context.Description.Parameters
                    .Where(x => x.Key.ToLower() == lowerCaseParaNameBegin)
                    .First().Value.Value = value.Begin;
                context.Description.Parameters
                    .Where(x => x.Key.ToLower() == lowerCaseParaNameEnd)
                    .First().Value.Value = value.End;
            }
        }
    }
}
