using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;
using System;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultCommandParserTests
    {
        [DataRow("SelectIdAndName", false, CommandInfoTypes.Select)]
        [DataRow("GetById", false, CommandInfoTypes.Select)]
        [DataRow("FetchByNameLike", false, CommandInfoTypes.Select)]
        [DataRow("UpdateIdAndNameByGuid", false, CommandInfoTypes.Update)]
        [DataRow("ModifyIdAndNameByGuid", false, CommandInfoTypes.Update)]
        [DataRow("DeleteById", false, CommandInfoTypes.Delete)]
        [DataRow("RemoveByUserid", false, CommandInfoTypes.Delete)]
        [DataRow("PickById", true, CommandInfoTypes.Delete)]
        [TestMethod()]
        public void DetaultCommandParse(string command, bool hasError, CommandInfoTypes commandInfoType)
        {
            ICommandParser parser = new DefaultCommandParser();
            if (hasError)
            {
                Assert.ThrowsException<NotImplementedException>(() => parser.Parse(command));
            }
            else
            {
                ICommandInfo info = parser.Parse(command);
                Assert.AreEqual(commandInfoType, info.Type);
            }
        }
    }
}