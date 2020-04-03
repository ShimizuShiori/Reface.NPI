using Reface.StateMachine;
using Reface.StateMachine.CsvBuilder;

namespace Reface.NPI.Parsers.StateMachines
{
    public class DefaultStateMachineBuilderFactory : IStateMachineBuilderFactory
    {
        private readonly ICache cache;
        private readonly IResourceProvider resourceProvider;
        private readonly IResourceNameProvider resourceNameProvider;

        public DefaultStateMachineBuilderFactory()
        {
            this.cache = NpiServicesCollection.GetService<ICache>();
            this.resourceProvider = NpiServicesCollection.GetService<IResourceProvider>();
            this.resourceNameProvider = NpiServicesCollection.GetService<IResourceNameProvider>();
        }

        public IStateMachineBuilder<TState, TAction> Create<TState, TAction>(string stateMachineName)
            where TState : struct
            where TAction : struct
        {
            DebugLogger.Debug($"创建状态机 : {stateMachineName}");
            return this.cache.GetOrCreate<IStateMachineBuilder<TState, TAction>>($"STATE_MACHINE_BUILDER_{stateMachineName}", key =>
            {
                string name = this.resourceNameProvider.GetStateMachineCsv(stateMachineName);
                using (var stream = this.resourceProvider.Provide(name))
                {
                    return CsvStateMachineBuilder<TState, TAction>.FromStream(stream);
                }
            });
        }
    }
}
