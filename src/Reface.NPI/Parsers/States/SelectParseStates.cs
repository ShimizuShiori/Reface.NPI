namespace Reface.NPI.Parsers.States
{
    public enum SelectParseStates
    {
        Start,
        OutputField,
        NextOutputField,
        Condition,
        ConditionField,
        ConditionOperator,
        NextCondition,
        OrderBy,
        OrderByField,
        AscOrDesc,
        End,
        ConditionParameter
    }
}
