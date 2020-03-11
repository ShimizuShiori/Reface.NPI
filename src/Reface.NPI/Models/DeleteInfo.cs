using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class DeleteInfo
    {
        public List<ConditionInfo> ConditionInfos { get; private set; }

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
