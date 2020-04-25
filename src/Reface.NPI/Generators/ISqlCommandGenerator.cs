using System.Reflection;

namespace Reface.NPI.Generators
{
    /// <summary>
    /// SqlCommand 生成器接口。
    /// 抽象类 <see cref="SqlCommandGeneratorBase"/> 实现了主要功能，
    /// 并将面向不同数据库的功能将由 <see cref="SqlCommandGeneratorBase"/> 的子类实现。
    /// </summary>
    public interface ISqlCommandGenerator
    {
        /// <summary>
        /// 生成 SqlCommand
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        SqlCommandDescription Generate(MethodInfo methodInfo, object[] arguments);

        /// <summary>
        /// 生成语句中的参数语句。
        /// 目前该函数唯一的用途是在参数填充阶段，
        /// 会将集合类型的参数转换为多个参数，
        /// 替换原有的参数。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GenerateParameterName(string name);
    }
}
