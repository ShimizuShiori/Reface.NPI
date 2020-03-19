using System;
using System.Reflection;

namespace Reface.NPI.Generators
{
    /// <summary>
    /// 实体类型供应商
    /// </summary>
    public interface IEntityTypeProvider
    {
        /// <summary>
        /// 通过给定的方法名称，确定所要操作的实体类型
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        Type Provide(MethodInfo methodInfo);
    }
}
