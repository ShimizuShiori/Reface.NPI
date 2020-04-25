using System.Text;

namespace Reface.NPI.Generators.ConditionGenerators
{
    /// <summary>
    /// 条件生成处理器
    /// </summary>
    public interface IConditionGenerateHandler
    {
        void Handle(ConditionGeneratorContext context);
    }
}
