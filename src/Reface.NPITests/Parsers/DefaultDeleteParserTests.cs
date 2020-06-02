using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Models;

namespace Reface.NPI.Parsers.Tests
{
    [TestClass()]
    public class DefaultDeleteParserTests
    {
        private readonly DefaultDeleteParser defaultDeleteParser = new DefaultDeleteParser();

        public void ParseCommand_ByIdIsAndNameLike()
        {
            DeleteInfo deleteInfo = defaultDeleteParser.Parse("ByIdIsAndNameLike");
        }

    }
}