using System.Text;

namespace Reface.NPI.Generators.ConditionGenerators
{
    /// <summary>
    /// 条件语句生成器
    /// </summary>
    public interface IConditionGenerator
    {
        /// <summary>
        /// 生成语句，当对条件已生成则返回 true, 否则返回 false
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool Generate(ConditionGeneratorContext context);
    }
}
