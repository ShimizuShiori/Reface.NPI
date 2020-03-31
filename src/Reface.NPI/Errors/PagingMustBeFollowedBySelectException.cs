using System;

namespace Reface.NPI.Errors
{
    /// <summary>
    /// Paging 关键字后必须是 Select 语句
    /// </summary>
    public class PagingMustBeFollowedBySelectException : NPIException
    {
    }
}
