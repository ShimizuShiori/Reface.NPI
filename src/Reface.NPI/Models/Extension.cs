using System.Collections.Generic;

namespace Reface.NPI.Models
{
    public static class Extension
    {
        public static HashSet<string> GetAllFieldNames(this IConditionInfo condition)
        {
            HashSet<string> set = new HashSet<string>();
            if (condition == null)
                return set;
            if (condition is FieldConditionInfo fc)
            {
                set.Add(fc.Field);
                return set;
            }

            if (condition is GroupConditionInfo gc)
            {
                foreach (var field in gc.LeftCondition.GetAllFieldNames())
                    set.Add(field);
                foreach (var field in gc.RightCondition.GetAllFieldNames())
                    set.Add(field);
                return set;
            }

            return set;
        }

        public static GroupConditionInfo AsGroupCondition(this IConditionInfo condition)
        {
            return condition.As<GroupConditionInfo>();
        }

        public static FieldConditionInfo AsFieldCondition(this IConditionInfo condition)
        {
            return condition.As<FieldConditionInfo>();
        }
    }
}
