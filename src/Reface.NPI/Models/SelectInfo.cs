using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Models
{
    public class SelectInfo
    {
        public List<string> Fields { get; private set; } = new List<string>();
        public List<ConditionInfo> Conditions { get; private set; } = new List<ConditionInfo>();

        public List<OrderInfo> Orders { get; private set; } = new List<OrderInfo>();
    }
}
