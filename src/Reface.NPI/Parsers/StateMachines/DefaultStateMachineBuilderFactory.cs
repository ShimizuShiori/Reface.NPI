using Reface.StateMachine;
using Reface.StateMachine.CsvBuilder;

namespace Reface.NPI.Parsers.StateMachines
{
    public class DefaultStateMachineBuilderFactory : IStateMachineBuilderFactory
    {
        private readonly ICache cache;

        public DefaultStateMachineBuilderFactory()
        {
            this.cache = NpiServicesCollection.GetService<ICache>();
        }

        public IStateMachineBuilder<TState, TAction> Create<TState, TAction>(string stateMachineName)
            where TState : struct
            where TAction : struct
        {
            DebugLogger.Debug($"创建状态机 : {stateMachineName}");
            return this.cache.GetOrCreate<IStateMachineBuilder<TState, TAction>>($"STATE_MACHINE_BUILDER_{stateMachineName}", key => 
            {
                return CsvStateMachineBuilder<TState, TAction>.FromFile(PathProvider.GetStateMachine(stateMachineName));
            });
        }
    }
}
