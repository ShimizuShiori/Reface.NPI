using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers.StateMachines
{
    public class UpdateStateMachine : DefaultParseStateMachine<UpdateToken, UpdateParseStates, UpdateParseActions>
    {
        public UpdateStateMachine() : base(Constant.MACHINE_NAME_UPDATE)
        {
        }
    }
}
