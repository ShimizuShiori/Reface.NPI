using Reface.StateMachine;

namespace Reface.NPI.Parsers.StateMachines
{
    interface IStateMachineBuilderFactory
    {
        IStateMachineBuilder<TState, TAction> Create<TState, TAction>(string stateMachineName)
            where TState : struct
            where TAction : struct;
    }
}
