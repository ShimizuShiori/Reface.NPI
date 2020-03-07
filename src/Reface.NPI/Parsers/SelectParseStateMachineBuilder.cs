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
            builder
                .From(SelectParseStates.Start)
                    .When(SelectParseActions.Field).To(SelectParseStates.OutputField)
                    .When(SelectParseActions.By).To(SelectParseStates.Condition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
                    .When(SelectParseActions.All).To(SelectParseStates.SkipOutputAndCondition)
                .From(SelectParseStates.OutputField)
                    .When(SelectParseActions.Field).To(SelectParseStates.OutputField)
                    .When(SelectParseActions.And).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.By).To(SelectParseStates.Condition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
                .From(SelectParseStates.NextOutputField)
                    .When(SelectParseActions.Field).To(SelectParseStates.OutputField)
                .From(SelectParseStates.Condition)
                    .When(SelectParseActions.Field).To(SelectParseStates.ConditionField)
                .From(SelectParseStates.ConditionField)
                    .When(SelectParseActions.And).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Or).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
                    .When(SelectParseActions.Operator).To(SelectParseStates.ConditionOperator)
                    .When(SelectParseActions.End).To(SelectParseStates.End)
                .From(SelectParseStates.ConditionOperator)
                    .When(SelectParseActions.And).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Or).To(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
                    .When(SelectParseActions.End).To(SelectParseStates.End)
                .From(SelectParseStates.NextCondition)
                    .When(SelectParseActions.Field).To(SelectParseStates.ConditionField)
                .From(SelectParseStates.OrderBy)
                    .When(SelectParseActions.Field).To(SelectParseStates.OrderByField)
                .From(SelectParseStates.OrderByField)
                    .When(SelectParseActions.And).To(SelectParseStates.NextOrderBy)
                    .When(SelectParseActions.Asc).To(SelectParseStates.OrderByAsc)
                    .When(SelectParseActions.Desc).To(SelectParseStates.OrderByDesc)
                    .When(SelectParseActions.End).To(SelectParseStates.End)
                .From(SelectParseStates.OrderByAsc)
                    .When(SelectParseActions.And).To(SelectParseStates.NextOrderBy)
                    .When(SelectParseActions.End).To(SelectParseStates.End)
                .From(SelectParseStates.NextOrderBy)
                    .When(SelectParseActions.Field).To(SelectParseStates.OrderByField)
                .From(SelectParseStates.SkipOutputAndCondition)
                    .When(SelectParseActions.Orderby).To(SelectParseStates.OrderBy)
            ;
        }

        public static IStateMachineBuilder<SelectParseStates, SelectParseActions> Default { get { return builder; } }
    }
}
