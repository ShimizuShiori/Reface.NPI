namespace Reface.NPI
{
    class DebugLogger
    {
        public static void Info(string command)
        {
            System.Diagnostics.Debug.WriteLine(command);
        }
    }
}
