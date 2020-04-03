namespace Reface.NPI
{
    public class DefaultResourceNameProvider : IResourceNameProvider
    {
        private const string formatter = "Reface.NPI.Resources.StateMachines.{0}.csv";

        public string SelectStateMachineCsv => string.Format(formatter, "Select");

        public string InsertStateMachineCsv => string.Format(formatter, "Insert");

        public string UpdateStateMachineCsv => string.Format(formatter, "Update");

        public string DeleteStateMachineCsv => string.Format(formatter, "Delete");
    }
}
