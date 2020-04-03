using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers.StateMachines
{
    public class SelectStateMachine : DefaultParseStateMachine<SelectToken, SelectParseStates, SelectParseActions>
    {
        public SelectStateMachine() : base(Constant.MACHINE_NAME_SELECT)
        {
        }
    }
}
