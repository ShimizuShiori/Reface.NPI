using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class UpdateInfo : ICommandInfo
    {
        public List<SetInfo> SetFields { get; private set; }

        public HashSet<string> WithoutFields { get; private set; }

        public List<ConditionInfo> Conditions { get; private set; }

        public CommandInfoTypes Type => CommandInfoTypes.Update;

        public UpdateInfo()
        {
            this.SetFields = new List<SetInfo>();
            this.Conditions = new List<ConditionInfo>();
            this.WithoutFields = new HashSet<string>();
        }
    }
}
