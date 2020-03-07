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
            Assert.AreEqual(3, tokens.Count);
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
            Assert.AreEqual(1, tokens.Count);
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
            Assert.AreEqual(ConditionOperators.Is, info.Conditions[0].Operators);
            Assert.AreEqual(0, info.Orders.Count);
        }
    }
}