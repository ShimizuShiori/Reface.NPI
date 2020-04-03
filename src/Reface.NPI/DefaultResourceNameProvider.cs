namespace Reface.NPI
{
    public class DefaultResourceNameProvider : IResourceNameProvider
    {
        private const string formatter = "Reface.NPI.Resources.StateMachines.{0}.csv";

        public string GetStateMachineCsv(string stateMachineName)
        {
            return string.Format(DefaultResourceNameProvider.formatter, stateMachineName);
        }
    }
}
