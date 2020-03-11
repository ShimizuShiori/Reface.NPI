﻿namespace Reface.NPI
{
    class PathProvider
    {
        private const string ResoucesPath = "./Resources";
        private const string MACHINE_NAME_SELECT = "Select";

        public static string GetResourcePath(string path)
        {
            return $"{PathProvider.ResoucesPath}/{path}";
        }

        public static string GetStateMachine(string mathineName)
        {
            return PathProvider.GetResourcePath($"/StateMachines/{mathineName}.csv");
        }

        public static string SelectStateMachine
        {
            get
            {
                return PathProvider.GetStateMachine(PathProvider.MACHINE_NAME_SELECT);
            }
        }
    }
}
