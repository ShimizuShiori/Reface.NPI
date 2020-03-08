namespace Reface.NPI.Parsers
{
    public enum SelectParseStates
    {
        OutputField,
        NextOutputField,
        Condition,
        ConditionField,
        ConditionOperator,
        NextCondition,
        OrderBy,
        OrderByField,
        AscOrDesc

    }
}
