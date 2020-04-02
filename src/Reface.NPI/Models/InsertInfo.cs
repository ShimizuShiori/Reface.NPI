using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class InsertInfo : ICommandInfo
    {
        public CommandInfoTypes Type => CommandInfoTypes.Insert;

        public HashSet<string> WithoutFields { get; private set; }

        public bool SelectNewRow { get; set; }

        public InsertInfo()
        {
            this.WithoutFields = new HashSet<string>();
            this.SelectNewRow = false;
        }
    }
}
