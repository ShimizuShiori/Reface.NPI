using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reface.NPI;
using Reface.NPI.Generators;
using Reface.NPI.Generators.Sqlite;
using Reface.NPITests.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reface.NPITests.Parsers.Sqlite
{
    [TestClass]
    public class SqliteGeneratorTests
    {
        public interface IUserDao : INpiDao<User>
        {
            [ESql("SELECT * FROM [User] WHERE Id = @Id")]
            void SelectById(int id);
        }

        private ISqlCommandGenerator GetSqlCommandGenerator()
        {
            return new DefaultSqliteCommandGenerator();
        }

        private MethodInfo GetMethod(string name)
        {
            return typeof(IUserDao).GetMethod(name);
        }

        private SqlCommandDescription GetDescription(string name)
        {
            var generator = this.GetSqlCommandGenerator();
            return generator.Generate(GetMethod(name), new object[] { });
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var d = GetDescription(nameof(IUserDao.SelectById));
        }
    }
}
