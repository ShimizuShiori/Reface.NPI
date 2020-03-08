using Reface.StateMachine;
using Reface.StateMachine.CodeBuilder;

namespace Reface.NPI.Parsers
{
    public static class SelectParseStateMachineBuilder
    {
        private readonly static CodeStateMachineBuilder<SelectParseStates, SelectParseActions> builder
            = new CodeStateMachineBuilder<SelectParseStates, SelectParseActions>();
        static SelectParseStateMachineBuilder()
        {
            builder.
                From(SelectParseStates.Start)
                    .When(SelectParseActions.Field).To(SelectParseStates.OutputField)
                    .When(SelectParseActions.By).To(SelectParseStates.Condition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBySkipOutput)
                    .When(SelectParseActions.End).To(SelectParseStates.EndFromStart)
                .From(SelectParseStates.OutputField)
                    .When(SelectParseActions.Field).To(SelectParseStates.OutputField)
                    .When(SelectParseActions.And).To(SelectParseStates.NextOutputField)
                    .When(SelectParseActions.By).To(SelectParseStates.Condition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBySkipCondition)
                    .When(SelectParseActions.End).To(SelectParseStates.EndFromOutput)
                .From(SelectParseStates.NextOutputField)
                    .When(SelectParseActions.Field).To(SelectParseStates.OutputField)
                .From(SelectParseStates.Condition)
                    .When(SelectParseActions.Field).To(SelectParseStates.ConditionField)
                .From(SelectParseStates.ConditionField)
                    .When(SelectParseActions.And).To(SelectParseStates.NextConditionWithF)
                    .When(SelectParseActions.Or).To(SelectParseStates.NextConditionWithF)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
                    .When(SelectParseActions.Operator).To(SelectParseStates.ConditionOperator)
                    .When(SelectParseActions.End).To(SelectParseStates.EndFromOrderByField)
                .From(SelectParseStates.ConditionOperator)
                    .When(SelectParseActions.And).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Or).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
                    .When(SelectParseActions.End).To(SelectParseStates.EndFromConditionOperator)
                .From(SelectParseStates.NextConditionWithF)
                    .When(SelectParseActions.Field).To(SelectParseStates.ConditionField)
                .From(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Field).To(SelectParseStates.ConditionField)
                .From(SelectParseStates.OrderBy)
                    .When(SelectParseActions.Field).To(SelectParseStates.OrderByField)
                .From(SelectParseStates.OrderBySkipOutput)
                    .When(SelectParseActions.Field).To(SelectParseStates.OrderByField)
                .From(SelectParseStates.OrderBySkipCondition)
                    .When(SelectParseActions.Field).To(SelectParseStates.OrderByField)
                .From(SelectParseStates.OrderByField)
                    .When(SelectParseActions.And).To(SelectParseStates.NextOrderBy)
                    .When(SelectParseActions.AscOrDesc).To(SelectParseStates.AscOrDesc)
                    .When(SelectParseActions.End).To(SelectParseStates.EndFromOrderByField)
                .From(SelectParseStates.AscOrDesc)
                    .When(SelectParseActions.And).To(SelectParseStates.NextOrderBy)
                    .When(SelectParseActions.End).To(SelectParseStates.EndFromAscOrDesc)
                .From(SelectParseStates.NextOrderBy)
                    .When(SelectParseActions.Field).To(SelectParseStates.OrderByField);
        }

        public static IStateMachineBuilder<SelectParseStates, SelectParseActions> Default { get { return builder; } }
    }
}
