using Reface.NPI.Models;
using System;

namespace Reface.NPI.Generators
{
    public class SqlCommandGenerateContext
    {
        public ICommandInfo CommandInfo { get; private set; }

        public Type IDaoType { get; private set; }

        public Type EntityType { get; private set; }

    }
}
