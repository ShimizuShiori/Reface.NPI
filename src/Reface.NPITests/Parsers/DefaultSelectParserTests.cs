using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultSelectParserTests
    {
        [TestMethod()]
        public void SplitCommandToTokensTest3Words()
        {
            string command = "NameById";
            DefaultSelectParser parser = new DefaultSelectParser();
            List<SelectToken> tokens = parser.SplitCommandToTokens(command);
            Assert.AreEqual(4, tokens.Count);
            Assert.AreEqual("Name", tokens[0].Text);
            Assert.AreEqual(SelectParseActions.Field, tokens[0].Action);
            Assert.AreEqual("By", tokens[1].Text);
            Assert.AreEqual(SelectParseActions.By, tokens[1].Action);
            Assert.AreEqual("Id", tokens[2].Text);
            Assert.AreEqual(SelectParseActions.Field, tokens[2].Action);
        }


        [TestMethod()]
        public void SplitCommandToTokensTestSingleWord()
        {
            string command = "Name";
            DefaultSelectParser parser = new DefaultSelectParser();
            List<SelectToken> tokens = parser.SplitCommandToTokens(command);
            Assert.AreEqual(2, tokens.Count);
            Assert.AreEqual("Name", tokens[0].Text);
            Assert.AreEqual(SelectParseActions.Field, tokens[0].Action);
        }

        [TestMethod()]
        public void ParseTest_NameById()
        {
            string command = "NameById";
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse(command);
            Assert.AreEqual(1, info.Fields.Count, "count of field should be 1");
            Assert.AreEqual("Name", info.Fields[0]);
            Assert.AreEqual(1, info.Conditions.Count, "count of condition should be 1");
            Assert.AreEqual("Id", info.Conditions[0].Field);
            Assert.AreEqual("", info.Conditions[0].Operators);
            Assert.AreEqual(0, info.Orders.Count);
        }


        [TestMethod()]
        public void ParseTest_ByIdAndName()
        {
            string command = "ByIdAndName";
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse(command);
            Assert.AreEqual(0, info.Fields.Count, "count of field should be 0");
            Assert.AreEqual(2, info.Conditions.Count, "count of condition should be 2");
            Assert.AreEqual("Id", info.Conditions[0].Field);
            Assert.AreEqual("", info.Conditions[0].Operators);
            Assert.AreEqual("Name", info.Conditions[1].Field);
            Assert.AreEqual("", info.Conditions[1].Operators);
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
            Assert.AreEqual(conditionCount, info.Conditions.Count, "count of condition");
            Assert.AreEqual(orderByCount, info.Orders.Count, "count of orderby");
        }

        [TestMethod]
        public void ParseCommand_NameAndIconByRegstertimeAndStateOrderbyRegtertimeDescId()
        {
            DefaultSelectParser parser = new DefaultSelectParser();
            SelectInfo info = parser.Parse("NameAndIconByRegstertimeAndStateOrderbyRegtertimeDescId");
            Assert.AreEqual("Name", info.Fields[0]);
            Assert.AreEqual("Icon", info.Fields[1]);

            Assert.AreEqual("Regstertime", info.Conditions[0].Field);
            Assert.AreEqual("", info.Conditions[0].Operators);
            Assert.AreEqual(ConditionJoiners.And, info.Conditions[0].JoinerToNext);

            Assert.AreEqual("State", info.Conditions[1].Field);
            Assert.AreEqual("", info.Conditions[1].Operators);
            Assert.AreEqual(ConditionJoiners.Null, info.Conditions[1].JoinerToNext);

            Assert.AreEqual("Regtertime", info.Orders[0].Field);
            Assert.AreEqual(OrderTypes.Desc, info.Orders[0].Type);

            Assert.AreEqual("Id", info.Orders[1].Field);
            Assert.AreEqual(OrderTypes.Asc, info.Orders[1].Type);
        }
    }
}