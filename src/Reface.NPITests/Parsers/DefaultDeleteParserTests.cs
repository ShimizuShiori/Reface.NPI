using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using System;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultDeleteParserTests
    {
        [DataRow("ById", 1)]
        [DataRow("ByIdAndName", 2)]
        [DataRow("ByIdIsAndNameLike", 2)]
        [DataRow("ByIdIsOrNameLike", 2)]
        [TestMethod()]
        public void ParseTest(string command, int conditionCount)
        {
            DefaultDeleteParser defaultDeleteParser = new DefaultDeleteParser();
            DeleteInfo deleteInfo = defaultDeleteParser.Parse(command);
            Assert.AreEqual(conditionCount, deleteInfo.ConditionInfos.Count);
            Console.WriteLine(deleteInfo);
        }

        public void ParseCommand_ByIdIsAndNameLike()
        {
            DefaultDeleteParser defaultDeleteParser = new DefaultDeleteParser();
            DeleteInfo deleteInfo = defaultDeleteParser.Parse("ByIdIsAndNameLike");

            //ssert.AreEqual("Id",deleteInfo.)
        }
    }
}