using Reface.NPI.Attributes;

namespace Reface.NPI.Configs
{
    /// <summary>
    /// NPI 全局配置类
    /// </summary>
    public static class NpiConfig
    {
        /// <summary>
        /// <see cref="SqlAttribute.Selector"/> 与该值相同的特征会被使用。
        /// </summary>
        public static string QuerySelector { get; set; } = "";
    }
}
