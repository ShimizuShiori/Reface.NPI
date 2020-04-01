using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using System.Diagnostics;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultUpdateParserTests
    {
        private readonly DefaultUpdateParser parser = new DefaultUpdateParser();

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
            Assert.AreEqual("Password", info.SetFields[0].Field);
            Assert.AreEqual("Icon", info.SetFields[1].Field);
            Assert.AreEqual("Id", info.Conditions[0].Field);
            Assert.AreEqual("Is", info.Conditions[0].Operators);
            Assert.AreEqual(ConditionJoiners.And, info.Conditions[0].JoinerToNext);
            Assert.AreEqual("Name", info.Conditions[1].Field);
            Assert.AreEqual("Like", info.Conditions[1].Operators);
            Assert.AreEqual(ConditionJoiners.Null, info.Conditions[1].JoinerToNext);
        }

        [TestMethod]
        public void PasswordEqualsNewpasswordByUseridAndPasswordEqualsOldpassword()
        {
            StackFrame sf = new StackFrame();
            var method = sf.GetMethod();
            var info = parser.Parse(method.Name);

            Assert.AreEqual(1, info.SetFields.Count);

            Assert.AreEqual("Password", info.SetFields[0].Field);
            Assert.AreEqual("Newpassword", info.SetFields[0].Parameter);

            Assert.AreEqual("Userid", info.Conditions[0].Field);
            Assert.AreEqual("", info.Conditions[0].Operators);
            Assert.AreEqual("Userid", info.Conditions[0].Parameter);
            Assert.AreEqual(ConditionJoiners.And, info.Conditions[0].JoinerToNext);

            Assert.AreEqual("Password", info.Conditions[1].Field);
            Assert.AreEqual("Equals", info.Conditions[1].Operators);
            Assert.AreEqual("Oldpassword", info.Conditions[1].Parameter);
            Assert.AreEqual(ConditionJoiners.Null, info.Conditions[1].JoinerToNext);
        }
    }
}