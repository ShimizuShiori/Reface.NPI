using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultUpdateParserTests
    {
        [DataRow("PasswordById", 1, 1)]
        [DataRow("PasswordAndIconByIdIsAndNameLike", 2, 2)]
        [TestMethod()]
        public void UpdateParseTest(string command, int setCount, int conditionCount)
        {
            IUpdateParser updateParser = new DefaultUpdateParser();
            var info = updateParser.Parse(command);
            Assert.AreEqual(setCount, info.SetFields.Count);
            Assert.AreEqual(conditionCount, info.Conditions.Count);
        }

        [TestMethod]
        public void UpdateParser_PasswordAndIconByIdIsAndNameLike()
        {
            string command = "PasswordAndIconByIdIsAndNameLike";
            IUpdateParser updateParser = new DefaultUpdateParser();
            var info = updateParser.Parse(command);
            Assert.AreEqual("Password", info.SetFields[0]);
            Assert.AreEqual("Icon", info.SetFields[1]);
            Assert.AreEqual("Id", info.Conditions[0].Field);
            Assert.AreEqual("Is", info.Conditions[0].Operators);
            Assert.AreEqual(ConditionJoiners.And, info.Conditions[0].JoinerToNext);
            Assert.AreEqual("Name", info.Conditions[1].Field);
            Assert.AreEqual("Like", info.Conditions[1].Operators);
            Assert.AreEqual(ConditionJoiners.Null, info.Conditions[1].JoinerToNext);
        }
    }
}