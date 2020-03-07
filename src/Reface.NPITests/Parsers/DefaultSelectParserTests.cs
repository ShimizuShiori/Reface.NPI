using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}