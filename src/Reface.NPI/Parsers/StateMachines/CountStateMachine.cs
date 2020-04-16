using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers.StateMachines
{
    public class CountStateMachine : DefaultParseStateMachine<CountToken, CountParseStates, CountParseActions>
    {
        public CountStateMachine() : base(Constant.MACHINE_NAME_COUNT)
        {
        }
    }
}
