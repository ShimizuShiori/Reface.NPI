using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI.Generators;
using System.Linq;

namespace Reface.NPITests
{
    [TestClass]
    public class ParameterFinderTests
    {
        [TestMethod]
        public void Test()
        {
            string command = "SELECT * FROM TABLENAME WHERE ID = @Id AND NAME = @Name";
            var finder = new DefaultSqlParameterFinder();
            var ps = finder.Find(command);
            Assert.AreEqual(2, ps.Count());
            Assert.AreEqual("Id", ps.ElementAt(0));
            Assert.AreEqual("Name", ps.ElementAt(1));

            var ps2 = finder.Find(command);
        }
    }
}
