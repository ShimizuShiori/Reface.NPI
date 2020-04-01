namespace Reface.NPI.Parsers.States
{
    public enum DeleteParseStates
    {
        Start,
        Condition,
        ConditionField,
        ConditionOperator,
        NextCondition,
        End,
        ConditionParameter

    }
}
