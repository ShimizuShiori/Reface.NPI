namespace Reface.NPI.Models
{
    public class GroupConditionInfo : IConditionInfo
    {
        public IConditionInfo LeftCondition { get; set; }
        public ConditionJoiners Joiner { get; set; }
        public IConditionInfo RightCondition { get; set; }

        public override string ToString()
        {
            return $"( {LeftCondition} ) {Joiner} ( {RightCondition} )";
        }
    }
}
