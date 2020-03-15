using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Generators;
using Reface.NPI.Generators.SqlServer;
using Reface.NPITests;
using System;
using System.Reflection;

namespace Reface.NPI.Tests
{
    [TestClass()]
    public class DefaultParameterFillerTests
    {
        [TestMethod()]
        public void FillTest()
        {
            Type type = typeof(IUserDao);
            MethodInfo mi = type.GetMethod("SelectById");
            SqlCommandGenerator g = new SqlCommandGenerator();
            SqlCommandDescription d = g.Generate(mi, new object[] { 1 });

            Console.WriteLine(d);
        }
    }
}