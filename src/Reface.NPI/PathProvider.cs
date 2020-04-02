namespace Reface.NPI
{
    class PathProvider
    {
        private const string ResoucesPath = "./Resources";
        private const string OperatorMappingsPath = "./OperatorMappings";

        public const string MACHINE_NAME_SELECT = "Select";
        public const string MACHINE_NAME_DELETE = "Delete";
        public const string MACHINE_NAME_UPDATE = "Update";
        public const string MACHINE_NAME_INSERT = "Insert";

        public static string GetResourcePath(string path)
        {
            return $"{PathProvider.ResoucesPath}/{path}";
        }

        public static string GetStateMachine(string mathineName)
        {
            return PathProvider.GetResourcePath($"/StateMachines/{mathineName}.csv");
        }

        public static string GetOperatorMappingXml(string name)
        {
            return PathProvider.GetResourcePath($"/{OperatorMappingsPath}/{name}.xml");
        }
    }
}
