using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI;
using Reface.NPI.Generators.SqlServer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Console.WriteLine(desc);
            Assert.AreEqual(1, desc.Parameters["Id"].Value);
        }

        [TestMethod]
        public void GenCmd_SelectById2()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.SelectById));
            var desc = g.Generate(methodInfo, new object[] { 1 });
            Console.WriteLine(desc);
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
            Console.WriteLine(desc);
            Assert.AreEqual("Shiori", desc.Parameters["Name"].Value);
        }

        [TestMethod]
        public void GenCmd_IdInByArray()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.GetByIdIn));
            var desc = g.Generate(methodInfo, new object[] { new int[] { 1, 2, 3 } });
            Console.WriteLine(desc);
            Assert.AreEqual(1, desc.Parameters["Id0"].Value);
            Assert.AreEqual(2, desc.Parameters["Id1"].Value);
            Assert.AreEqual(3, desc.Parameters["Id2"].Value);

        }
        [TestMethod]
        public void GenCmd_IdInByList()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.GetByIdIn));
            var desc = g.Generate(methodInfo, new object[] { new List<int>() { 1, 2, 3 } });
            Console.WriteLine(desc);
            Assert.AreEqual(1, desc.Parameters["Id0"].Value);
            Assert.AreEqual(2, desc.Parameters["Id1"].Value);
            Assert.AreEqual(3, desc.Parameters["Id2"].Value);

        }

        [TestMethod]
        public void SelectByGid()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.SelectByGid));
            var desc = g.Generate(methodInfo, new object[] { Guid.Empty });
            Console.WriteLine(desc);
            Assert.AreEqual(Guid.Empty, desc.Parameters["Gid"].Value);
        }

        [TestMethod]
        public void PagingSelectOrderbyId()
        {
            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.PagingSelectOrderbyId));
            var desc = g.Generate(methodInfo, new object[] { new Paging() { PageSize = 10, PageIndex = 0 } });
            Console.WriteLine(desc);
            Assert.AreEqual(0, desc.Parameters["BEGINRN"].Value);
            Assert.AreEqual(10, desc.Parameters["ENDRN"].Value);
        }

        [TestMethod]
        public void PagingSelectByGidOrderbyCreatetime()
        {

            var g = new DefaultSqlServerCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo methodInfo = daoType.GetMethod(nameof(IUserDao.PagingSelectByGidOrderbyCreatetime));
            var desc = g.Generate(methodInfo, new object[] { Guid.Empty, new Paging() { PageSize = 10, PageIndex = 0 } });
            Console.WriteLine(desc);
            Assert.AreEqual(Guid.Empty, desc.Parameters["Gid"].Value);
            Assert.AreEqual(0, desc.Parameters["BEGINRN"].Value);
            Assert.AreEqual(10, desc.Parameters["ENDRN"].Value);
        }

        [TestMethod]
        public void UpdatePasswordEqualsNewpasswordByUseridAndPasswordIsOldpassword()
        {
            var g = new DefaultSqlServerCommandGenerator();
            StackFrame sf = new StackFrame();
            Type type = typeof(IUserDao);
            var method = type.GetMethod(sf.GetMethod().Name);
            var d = g.Generate(method, new object[] { "888888", "123", "123456" });
            Console.WriteLine(d);
            Assert.AreEqual("888888", d.Parameters["Newpassword"].Value);
            Assert.AreEqual("123", d.Parameters["Userid"].Value);
            Assert.AreEqual("123456", d.Parameters["Oldpassword"].Value);
        }

        [TestMethod]
        public void UpdateWithoutCreatetimeById()
        {
            var g = new DefaultSqlServerCommandGenerator();
            StackFrame sf = new StackFrame();
            Type type = typeof(IUserDao);
            var method = type.GetMethod(sf.GetMethod().Name);
            var d = g.Generate(method, new object[] { new User()
            {
                Id=1,
                Name="NewName",
                Password="NewPassword"
            } });
            Console.WriteLine(d);
            Assert.AreEqual(3, d.Parameters.Count);
            Assert.AreEqual(1, d.Parameters["Id"].Value);
            Assert.AreEqual("NewName", d.Parameters["Name"].Value);
            Assert.AreEqual("NewPassword", d.Parameters["Password"].Value);
        }

        [TestMethod]
        public void DiyQueryByIdAndName()
        {
            var g = new DefaultSqlServerCommandGenerator();
            var method = typeof(IUserDao).GetMethod(nameof(IUserDao.DiyQueryByIdAndName));
            var d = g.Generate(method, new object[] { 1, "fc" });
            Assert.AreEqual(2, d.Parameters.Count);
            Assert.AreEqual(1, d.Parameters["id"].Value);
            Assert.AreEqual("fc", d.Parameters["name"].Value);
        }
    }
}
