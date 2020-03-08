using Reface.StateMachine;
using Reface.StateMachine.CsvBuilder;

namespace Reface.NPI.Parsers
{
    public static class SelectParseStateMachineBuilder
    {
        private readonly static IStateMachineBuilder<SelectParseStates, SelectParseActions> builder;
        static SelectParseStateMachineBuilder()
        {
            builder = CsvStateMachineBuilder<SelectParseStates, SelectParseActions>.FromFile("./Resouce/NPI.csv");
        }

        public static IStateMachineBuilder<SelectParseStates, SelectParseActions> Default { get { return builder; } }
    }
}
