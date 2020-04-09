using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Generators
{
    public class ParameterLookupContext
    {
        public ISqlCommandGenerator SqlCommandGenerator { get; private set; }
        public SqlCommandDescription Description { get; private set; }
        public MethodInfo MethodInfo { get; private set; }
        public object[] Values { get; private set; }

        public ParameterLookupContext(ISqlCommandGenerator sqlCommandGenerator, SqlCommandDescription description, MethodInfo methodInfo, object[] values)
        {
            SqlCommandGenerator = sqlCommandGenerator;
            Description = description;
            MethodInfo = methodInfo;
            Values = values;
        }
    }
}
