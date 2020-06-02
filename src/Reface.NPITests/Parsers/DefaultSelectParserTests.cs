using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using System.Configuration;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultSelectParserTests
    {

        [TestMethod()]
        public void ParseTest_NameById()
        {
            string command = "NameById";
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse(command);
            Assert.AreEqual(1, info.Fields.Count, "count of field should be 1");
            Assert.AreEqual("Name", info.Fields[0]);
            Assert.IsInstanceOfType(info.Condition, typeof(FieldConditionInfo));
            FieldConditionInfo fc = (FieldConditionInfo)info.Condition;
            Assert.AreEqual("Id", fc.Field);
            Assert.AreEqual("", fc.Operators);
            Assert.AreEqual(0, info.Orders.Count);
        }


        [TestMethod()]
        public void ParseTest_ByIdAndName()
        {
            string command = "ByIdAndName";
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse(command);
            Assert.AreEqual(0, info.Fields.Count, "count of field should be 0");

            var gc = info.Condition.AsGroupCondition();
            var c1 = info.Condition.AsGroupCondition().LeftCondition.AsFieldCondition();
            var c2 = info.Condition.AsGroupCondition().RightCondition.AsFieldCondition();

            Assert.AreEqual(ConditionJoiners.And, gc.Joiner);

            Assert.AreEqual("Id", c1.Field);
            Assert.AreEqual("", c1.Operators);
            Assert.AreEqual("Name", c2.Field);
            Assert.AreEqual("", c2.Operators);
            Assert.AreEqual(0, info.Orders.Count);
        }

        [TestMethod]
        [DataRow("", 0, 0, 0)]
        [DataRow("IdAndName", 2, 0, 0)]
        [DataRow("Id", 1, 0, 0)]
        [DataRow("ById", 0, 1, 0)]
        [DataRow("NameById", 1, 1, 0)]
        [DataRow("IdAndNameById", 2, 1, 0)]
        [DataRow("IdAndNameByIdIsAndNameIs", 2, 2, 0)]
        [DataRow("OrderbyId", 0, 0, 1)]
        [DataRow("OrderbyIdName", 0, 0, 2)]
        [DataRow("NameAndIconByRegstertimeAndStateOrderbyRegtertimeDescId", 2, 2, 2)]
        public void ParseTestsAncCheckCount(string command, int outputCount, int conditionCount, int orderByCount)
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse(command);
            Assert.AreEqual(outputCount, info.Fields.Count, "count of output");
            Assert.AreEqual(orderByCount, info.Orders.Count, "count of orderby");
        }

        [TestMethod]
        public void ParseCommand_NameAndIconByRegstertimeAndStateOrderbyRegtertimeDescId()
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse("NameAndIconByRegstertimeAndStateOrderbyRegtertimeDescId");
            Assert.AreEqual("Name", info.Fields[0]);
            Assert.AreEqual("Icon", info.Fields[1]);

            Assert.IsInstanceOfType(info.Condition, typeof(GroupConditionInfo));
            GroupConditionInfo gc = (GroupConditionInfo)info.Condition;
            Assert.AreEqual(ConditionJoiners.And, gc.Joiner);

            Assert.IsInstanceOfType(gc.LeftCondition, typeof(FieldConditionInfo));
            FieldConditionInfo fc = (FieldConditionInfo)gc.LeftCondition;
            Assert.AreEqual("Regstertime", fc.Field);
            Assert.AreEqual("", fc.Operators);

            Assert.IsInstanceOfType(gc.RightCondition, typeof(FieldConditionInfo));
            fc = (FieldConditionInfo)gc.RightCondition;
            Assert.AreEqual("State", fc.Field);
            Assert.AreEqual("", fc.Operators);

            Assert.AreEqual("Regtertime", info.Orders[0].Field);
            Assert.AreEqual(OrderTypes.Desc, info.Orders[0].Type);

            Assert.AreEqual("Id", info.Orders[1].Field);
            Assert.AreEqual(OrderTypes.Asc, info.Orders[1].Type);
        }


        [TestMethod]
        public void ParseCommand_NameByIdIsMyid()
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse("NameByIdIsMyid");
            Assert.AreEqual("Name", info.Fields[0]);

            Assert.IsInstanceOfType(info.Condition, typeof(FieldConditionInfo));
            FieldConditionInfo fc = (FieldConditionInfo)info.Condition;

            Assert.AreEqual("Id", fc.Field);
            Assert.AreEqual("Is", fc.Operators);
            Assert.AreEqual("Myid", fc.Parameter);
        }

        [TestMethod]
        public void ParseCommand_NameByIdIsMyidAndAgeGtMyage()
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse("NameByIdIsMyidAndAgeGtMyage");
            Assert.AreEqual("Name", info.Fields[0]);

            GroupConditionInfo gc = (GroupConditionInfo)info.Condition;
            Assert.AreEqual(ConditionJoiners.And, gc.Joiner);

            FieldConditionInfo fc = (FieldConditionInfo)gc.LeftCondition;
            Assert.AreEqual("Id", fc.Field);
            Assert.AreEqual("Is", fc.Operators);
            Assert.AreEqual("Myid", fc.Parameter);

            fc = (FieldConditionInfo)gc.RightCondition;

            Assert.AreEqual("Age", fc.Field);
            Assert.AreEqual("Gt", fc.Operators);
            Assert.AreEqual("Myage", fc.Parameter);
        }

        [TestMethod]
        public void ParseCommand_NameByIdIs()
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse("NameByIdIs");
            Assert.AreEqual("Name", info.Fields[0]);

            FieldConditionInfo fc = (FieldConditionInfo)info.Condition;

            Assert.AreEqual("Id", fc.Field);
            Assert.AreEqual("Is", fc.Operators);
            Assert.AreEqual("Id", fc.Parameter);
        }

        [TestMethod]
        public void ParseCommand_NameByIdIsMyidAndAgeGtMyageOrderbyId()
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse("NameByIdIsMyidAndAgeGtMyageOrderbyId");
            Assert.AreEqual("Name", info.Fields[0]);

            GroupConditionInfo gc = (GroupConditionInfo)info.Condition;
            Assert.AreEqual(ConditionJoiners.And, gc.Joiner);

            FieldConditionInfo fc = (FieldConditionInfo)gc.LeftCondition;
            Assert.AreEqual("Id", fc.Field);
            Assert.AreEqual("Is", fc.Operators);
            Assert.AreEqual("Myid", fc.Parameter);

            fc = (FieldConditionInfo)gc.RightCondition;
            Assert.AreEqual("Age", fc.Field);
            Assert.AreEqual("Gt", fc.Operators);
            Assert.AreEqual("Myage", fc.Parameter);

            Assert.AreEqual("Id", info.Orders[0].Field);
            Assert.AreEqual(OrderTypes.Asc, info.Orders[0].Type);
        }

        [TestMethod]
        public void ParseCommand_ByNotId()
        {
            string command = "ByNotId";
            SelectInfo info = GetInfoByCommand(command);

            FieldConditionInfo fc = (FieldConditionInfo)info.Condition;
            Assert.IsTrue(fc.IsNot);
        }

        private SelectInfo GetInfoByCommand(string command)
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            return parser.Parse(command);
        }
    }
}