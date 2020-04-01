using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using System;
using System.Diagnostics;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultDeleteParserTests
    {
        private readonly DefaultDeleteParser defaultDeleteParser = new DefaultDeleteParser();

        [DataRow("ById", 1)]
        [DataRow("ByIdAndName", 2)]
        [DataRow("ByIdIsAndNameLike", 2)]
        [DataRow("ByIdIsOrNameLike", 2)]
        [TestMethod()]
        public void DeleteParseTest(string command, int conditionCount)
        {
            DeleteInfo deleteInfo = defaultDeleteParser.Parse(command);
            Assert.AreEqual(conditionCount, deleteInfo.ConditionInfos.Count);
            Console.WriteLine(deleteInfo);
        }

        public void ParseCommand_ByIdIsAndNameLike()
        {
            DeleteInfo deleteInfo = defaultDeleteParser.Parse("ByIdIsAndNameLike");
        }

        [TestMethod]
        public void ByCdateGtBegindateAndCdateLtEnddate()
        {
            StackFrame sf = new StackFrame();
            var method = sf.GetMethod();
            DeleteInfo info = defaultDeleteParser.Parse(method.Name);

            int i = 0;

            Assert.AreEqual(2, info.ConditionInfos.Count);

            Assert.AreEqual("Cdate", info.ConditionInfos[i].Field);
            Assert.AreEqual("Gt", info.ConditionInfos[i].Operators);
            Assert.AreEqual("Begindate", info.ConditionInfos[i].Parameter);
            Assert.AreEqual(ConditionJoiners.And, info.ConditionInfos[i].JoinerToNext);

            i++;
            Assert.AreEqual("Cdate", info.ConditionInfos[i].Field);
            Assert.AreEqual("Lt", info.ConditionInfos[i].Operators);
            Assert.AreEqual("Enddate", info.ConditionInfos[i].Parameter);
            Assert.AreEqual(ConditionJoiners.Null, info.ConditionInfos[i].JoinerToNext);
        }
    }
}