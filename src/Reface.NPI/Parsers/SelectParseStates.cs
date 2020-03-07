namespace Reface.NPI.Parsers
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
        OrderByAsc,
        OrderByDesc,
        NextOrderBy,
        SkipOutputAndCondition,
        End
    }
}
