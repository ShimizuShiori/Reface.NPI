using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class DeleteInfo : ICommandInfo
    {
        public List<ConditionInfo> ConditionInfos { get; private set; }

        public CommandInfoTypes Type => CommandInfoTypes.Delete;

        public DeleteInfo()
        {
            this.ConditionInfos = new List<ConditionInfo>();
        }

        public override string ToString()
        {
            return $"Condition : {this.ConditionInfos.Join(",", x => x.ToString())}";
        }
    }
}
