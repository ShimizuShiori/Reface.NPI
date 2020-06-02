using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class DeleteInfo : ICommandInfo, ICommandInfoHasCondition
    {

        public CommandInfoTypes Type => CommandInfoTypes.Delete;

        public IConditionInfo Condition { get; set; }

    }
}
