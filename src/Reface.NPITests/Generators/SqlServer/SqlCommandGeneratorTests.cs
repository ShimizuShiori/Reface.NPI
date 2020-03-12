using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Generators.SqlServer;
using System;
using System.Reflection;

namespace Reface.NPITests.Generators.SqlServer
{
    [TestClass]
    public class SqlCommandGeneratorTests
    {
        interface IUserDao
        {
            [Description("SELECT * WHERE [Id] = @Id")]
            void SelectById(int id);

            [Description("SELECT * WHERE [Id] = @Id")]
            void GetById(int id);

            [Description("SELECT [Name] WHERE [Id] = @Id")]
            void GetNameById(int id);

            [Description("SELECT [Id],[Name] WHERE [Birthday] > @Birthday")]
            void GetIdAndNameByBirthdayGreaterthan(int id);

            [Description("SELECT [Id],[Name] WHERE [Birthday] > @Birthday ORDER BY [Sn] Asc")]
            void GetIdAndNameByBirthdayGreaterthanOrderbySn(int id);

            [Description("SELECT [Id],[Name] WHERE [Birthday] > @Birthday ORDER BY [Sn] Asc,[Type] Desc")]
            void GetIdAndNameByBirthdayGreaterthanOrderbySnTypeDesc(int id);
        }


        [TestMethod]
        public void GenSelectCommand()
        {
            var g = new SqlCommandGenerator();
            Type daoType = typeof(IUserDao);
            MethodInfo[] infos = daoType.GetMethods();
            foreach (var info in infos)
            {
                DescriptionAttribute desc = info.GetCustomAttribute<DescriptionAttribute>();
                var d = g.Generate(info);
                Console.WriteLine(d);
                Assert.AreEqual(desc.Value, d.SqlCommand.Trim(), info.Name);
            }
        }
    }
}
