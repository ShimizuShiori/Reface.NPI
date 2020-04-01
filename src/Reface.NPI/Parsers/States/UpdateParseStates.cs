namespace Reface.NPI.Parsers.States
{
    public enum UpdateParseStates
    {
        Start,
        SetField,
        NextSetField,
        Condition,
        ConditionField,
        ConditionOperator,
        NextCondition,
        End,
        ConditionParameter,
        SetParameter,
        SetEquals

    }
}
