using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo[] infos = daoType.GetMethods();
            foreach (var info in infos)
            {
                System.ComponentModel.DescriptionAttribute desc = info.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                var d = g.Generate(info, null);
                Console.WriteLine(d);
                Assert.AreEqual(desc.Description, d.SqlCommand.Trim(), info.Name);
            }
        }

        [TestMethod]
        public void GenCmd_SelectById()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.SelectById));
            var desc = g.Generate(methodInfo, new object[] { 1 });
            Assert.AreEqual(1, desc.Parameters["Id"].Value);
        }

        [TestMethod]
        public void GenCmd_Insert()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.Insert));
            User user = new User()
            {
                CreateTime = DateTime.Now,
                Id = 1,
                Name = "Test",
                Password = "TestPwd"
            };
            var desc = g.Generate(methodInfo, new object[] { user });
            Console.WriteLine(desc);
        }

        [TestMethod]
        public void GenCmd_DelByStr()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.DeleteByName));
            var desc = g.Generate(methodInfo, new object[] { "Shiori" });
            Assert.AreEqual("Shiori", desc.Parameters["Name"].Value);
        }
    }
}
