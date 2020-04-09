using Reface.NPI.Errors;
using Reface.NPI.Parsers;
using System.Linq;
using System.Reflection;

namespace Reface.NPI.Generators.ParameterLookups
{
    public class PagingParameterLookup : IParameterLookup
    {
        public void Lookup(ParameterLookupContext context)
        {
            var description = context.Description;
            var values = context.Values;
            if (!description.Parameters.ContainsKey(Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER))
                return;
            if (!description.Parameters.ContainsKey(Constant.PARAMETER_NAME_END_ROW_NUMBER))
                return;

            Paging paging = values.OfType<Paging>().FirstOrDefault();
            if (paging == null)
                throw new PagingParameterMustBeGivenExpcetion();
            description.Parameters[Constant.PARAMETER_NAME_BEGIN_ROW_NUMBER].Value = paging.PageIndex * paging.PageSize;
            description.Parameters[Constant.PARAMETER_NAME_END_ROW_NUMBER].Value = (paging.PageIndex + 1) * paging.PageSize;
        }
    }
}
