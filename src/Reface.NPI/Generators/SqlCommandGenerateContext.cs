using Reface.NPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Generators
{
    public class SqlCommandGenerateContext
    {
        public ICommandInfo CommandInfo { get; private set; }

        public Type IDaoType { get; private set; }
        public Type EntityType { get; private set; }
    }
}
