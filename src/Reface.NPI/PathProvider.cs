namespace Reface.NPI
{
    class PathProvider
    {
        private const string ResoucesPath = "./Resources";
        public const string MACHINE_NAME_SELECT = "Select";
        public const string MACHINE_NAME_DELETE = "Delete";
        public const string MACHINE_NAME_UPDATE = "Update";

        public static string GetResourcePath(string path)
        {
            return $"{PathProvider.ResoucesPath}/{path}";
        }

        public static string GetStateMachine(string mathineName)
        {
            return PathProvider.GetResourcePath($"/StateMachines/{mathineName}.csv");
        }
    }
}
