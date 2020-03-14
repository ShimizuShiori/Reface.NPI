using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Attributes;
using Reface.NPI.Generators.SqlServer;
using System;
using System.Reflection;

namespace Reface.NPITests.Generators.SqlServer
{
    [TestClass]
    public class SqlCommandGeneratorTests
    {

        [TestMethod]
        public void GenerateCommand()
        {
            var g = new SqlCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo[] infos = daoType.GetMethods();
            foreach (var info in infos)
            {
                System.ComponentModel.DescriptionAttribute desc = info.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                var d = g.Generate(info);
                Console.WriteLine(d);
                Assert.AreEqual(desc.Description, d.SqlCommand.Trim(), info.Name);
            }
        }
    }
}
