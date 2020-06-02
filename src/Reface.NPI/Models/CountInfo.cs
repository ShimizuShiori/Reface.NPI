namespace Reface.NPI.Models
{
    public class CountInfo : ICommandInfo, ICommandInfoHasCondition
    {
        public CommandInfoTypes Type => CommandInfoTypes.Count;

        public IConditionInfo Condition { get; set; }

    }
}
