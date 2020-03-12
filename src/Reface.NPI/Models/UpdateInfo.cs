﻿using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public class UpdateInfo
    {
        public List<string> SetFields { get; private set; }
        public List<ConditionInfo> Conditions { get; private set; }
        public UpdateInfo()
        {
            this.SetFields = new List<string>();
            this.Conditions = new List<ConditionInfo>();
        }
    }
}
