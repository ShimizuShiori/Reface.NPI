using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Generators.OperatorMappings.Models;
using System;
using System.Collections.Generic;

namespace Reface.NPI.Tests
{
    [TestClass()]
    public class ExtTests
    {
        [DataRow("HelloWorld", "Hello,World")]
        [DataRow("ALittlePig", "A,Little,Pig")]
        [DataRow("ABC", "A,B,C")]
        [TestMethod()]
        public void SplitToWordsTest(string text, string words)
        {
            string[] exceptWords = words.Split(new char[] { ',' });
            List<string> realWords = text.SplitToWords();
            Assert.AreEqual(exceptWords.Length, realWords.Count);
            for (int i = 0; i < exceptWords.Length; i++)
            {
                Assert.AreEqual(exceptWords[i], realWords[i]);
            }
        }

        [TestMethod]
        public void ToXmlTest()
        {
            Mapping operatorMapping = new Mapping()
            {
                Operator = "="
            };
            operatorMapping.Texts.Add("");
            operatorMapping.Texts.Add("Is");
            operatorMapping.Texts.Add("Equal");
            operatorMapping.Texts.Add("Equals");

            Mappings operatorMappings = new Mappings();
            operatorMappings.Add(operatorMapping);
            string xml = operatorMappings.ToXml();
            Console.WriteLine(xml);

            Mappings operatorMappings2 = xml.ToObjectAsXml<Mappings>();
            Assert.AreEqual(1, operatorMappings2.Count);
            var mapping = operatorMappings2[0];
            Assert.AreEqual("=", mapping.Operator);
            Assert.AreEqual("", mapping.Texts[0]);
            Assert.AreEqual("Is", mapping.Texts[1]);
            Assert.AreEqual("Equal", mapping.Texts[2]);
            Assert.AreEqual("Equals", mapping.Texts[3]);
        }
    }
}