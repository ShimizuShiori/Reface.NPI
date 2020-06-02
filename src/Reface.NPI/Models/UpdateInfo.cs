using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class UpdateInfo : ICommandInfo, ICommandInfoHasCondition
    {
        public List<SetInfo> SetFields { get; private set; }

        public HashSet<string> WithoutFields { get; private set; }

        public CommandInfoTypes Type => CommandInfoTypes.Update;

        public IConditionInfo Condition { get; set; }

        public UpdateInfo()
        {
            this.SetFields = new List<SetInfo>();
            this.WithoutFields = new HashSet<string>();
        }
    }
}
