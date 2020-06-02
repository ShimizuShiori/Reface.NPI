using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class SelectInfo : ICommandInfo, ICommandInfoHasCondition
    {
        public bool Paging { get; set; } = false;
        public List<string> Fields { get; private set; } = new List<string>();
        public IConditionInfo Condition { get; set; }

        public List<OrderInfo> Orders { get; private set; } = new List<OrderInfo>();

        public CommandInfoTypes Type => CommandInfoTypes.Select;

    }
}
