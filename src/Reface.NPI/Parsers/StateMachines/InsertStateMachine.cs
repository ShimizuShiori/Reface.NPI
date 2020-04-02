using Reface.NPI.Parsers.Actions;
using Reface.NPI.Parsers.States;
using Reface.NPI.Parsers.Tokens;

namespace Reface.NPI.Parsers.StateMachines
{
    public class InsertStateMachine : DefaultParseStateMachine<InsertToken, InsertParseStates, InsertParseActions>
    {
        public InsertStateMachine() : base(PathProvider.MACHINE_NAME_INSERT)
        {
        }
    }
}
