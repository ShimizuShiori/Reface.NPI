using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Generators;
using Reface.NPI.Models;
using System.Diagnostics;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultUpdateParserTests
    {
        private readonly DefaultUpdateParser parser = new DefaultUpdateParser();

        [TestMethod]
        public void UpdateParser_PasswordAndIconByIdIsAndNameLike()
        {
            string command = "PasswordAndIconByIdIsAndNameLike";
            IUpdateParser updateParser = new DefaultUpdateParser();
            var info = updateParser.Parse(command);
            Assert.AreEqual("Password", info.SetFields[0].Field);
            Assert.AreEqual("Icon", info.SetFields[1].Field);

            var c1 = info.Condition.AsGroupCondition().LeftCondition.AsFieldCondition();
            var c2 = info.Condition.AsGroupCondition().RightCondition.AsFieldCondition();

            Assert.AreEqual("Id", c1.Field);
            Assert.AreEqual("Is", c1.Operators);
            
            Assert.AreEqual("Name", c2.Field);
            Assert.AreEqual("Like", c2.Operators);
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

            Assert.AreEqual(ConditionJoiners.And, info.Condition.AsGroupCondition().Joiner);

            FieldConditionInfo c1 = info.Condition.AsGroupCondition().LeftCondition.AsFieldCondition();
            FieldConditionInfo c2 = info.Condition.AsGroupCondition().RightCondition.AsFieldCondition();


            Assert.AreEqual("Userid", c1.Field);
            Assert.AreEqual("", c1.Operators);
            Assert.AreEqual("Userid", c1.Parameter);

            Assert.AreEqual("Password", c2.Field);
            Assert.AreEqual("Equals", c2.Operators);
            Assert.AreEqual("Oldpassword", c2.Parameter);
        }
    }
}