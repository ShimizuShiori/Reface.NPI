using System;
using System.Reflection;

namespace Reface.NPI.Generators
{
    /// <summary>
    /// 默认的实体类型供应商，以 INpiDao 作为实现方式
    /// </summary>
    public class DefaultEntityTypeProvider : IEntityTypeProvider
    {
        private readonly ICache cache;

        public DefaultEntityTypeProvider()
        {
            this.cache = NpiServicesCollection.GetService<ICache>();
        }

        public Type Provide(MethodInfo methodInfo)
        {
            return this.cache.GetOrCreate<Type>($"EntityType_{methodInfo.DeclaringType.FullName}.{methodInfo.Name}", key =>
            {
                Type idaoType = methodInfo.DeclaringType;
                Type baseType = idaoType.GetInterface(Constant.TYPE_INPIDAO.FullName);
                if (baseType == null)
                    throw new ApplicationException("未从 INpiDao 继承"); //todo : 细化异常
                return baseType.GetGenericArguments()[0];
            });
        }
    }
}
