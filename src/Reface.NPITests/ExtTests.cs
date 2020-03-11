using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}