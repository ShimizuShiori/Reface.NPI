using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class SelectInfo : ICommandInfo
    {
        public bool Paging { get; set; } = false;
        public List<string> Fields { get; private set; } = new List<string>();
        public List<ConditionInfo> Conditions { get; private set; } = new List<ConditionInfo>();

        public List<OrderInfo> Orders { get; private set; } = new List<OrderInfo>();

        public CommandInfoTypes Type => CommandInfoTypes.Select;

        public override string ToString()
        {
            return $"Outputs : {Fields.Join(",", x => $"[{x}]")} " +
                $"Conditions : {Conditions.Join(",", x => x.ToString())} " +
                $"Orders : {Orders.Join(",", x => x.ToString())}";
        }
    }
}
