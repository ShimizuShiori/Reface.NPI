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
        NextConditionWithF,
        NextCondition,
        OrderBy,
        OrderBySkipOutput,
        OrderBySkipCondition,
        OrderByField,
        AscOrDesc,
        NextOrderBy,
        EndFromOutput,
        EndFromCondition,
        EndFromConditionOperator,
        EndFromOrderByField,
        EndFromAscOrDesc,
        EndFromStart

    }
}
