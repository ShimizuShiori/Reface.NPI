namespace Reface.NPI.Errors
{
    public class NotSupportActionException : NPIException
    {
        public string Action { get; private set; }

        public NotSupportActionException(string action)
        {
            Action = action;
        }
    }
}
