using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers.StateMachines
{
    public class DeleteStateMachine : DefaultParseStateMachine<DeleteToken, DeleteParseStates, DeleteParseActions>
    {
        public DeleteStateMachine() : base(PathProvider.MACHINE_NAME_DELETE)
        {
        }
    }
}
