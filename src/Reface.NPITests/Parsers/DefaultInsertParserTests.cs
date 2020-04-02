using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using Reface.NPI.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultInsertParserTests
    {
        private readonly ICommandParser parser;

        public DefaultInsertParserTests()
        {
            this.parser = NpiServicesCollection.GetService<ICommandParser>();
        }

        private string GetMethodName()
        {
            StackFrame sf = new StackFrame(2);
            return sf.GetMethod().Name;
        }

        private InsertInfo GenInsertInfo()
        {
            var methodName = GetMethodName();
            return this.parser.Parse(methodName) as InsertInfo;
        }

        [TestMethod]
        public void Insert()
        {
            var info = this.GenInsertInfo();
            Assert.AreEqual(0, info.WithoutFields.Count);
            Assert.IsFalse(info.SelectNewRow);
        }

        [TestMethod]
        public void InsertWithoutId()
        {
            var info = this.GenInsertInfo();
            Assert.AreEqual(1, info.WithoutFields.Count);
            Assert.IsTrue(info.WithoutFields.Contains("Id"));
            Assert.IsFalse(info.SelectNewRow);
        }

        [TestMethod]
        public void InsertWithoutIdName()
        {
            var info = this.GenInsertInfo();
            Assert.AreEqual(2, info.WithoutFields.Count);
            Assert.IsTrue(info.WithoutFields.Contains("Id"));
            Assert.IsTrue(info.WithoutFields.Contains("Name"));
            Assert.IsFalse(info.SelectNewRow);
        }

        [TestMethod]
        public void InsertWithoutIdNameAndSelect()
        {
            var info = this.GenInsertInfo();
            Assert.AreEqual(2, info.WithoutFields.Count);
            Assert.IsTrue(info.WithoutFields.Contains("Id"));
            Assert.IsTrue(info.WithoutFields.Contains("Name"));
            Assert.IsTrue(info.SelectNewRow);
        }
    }
}