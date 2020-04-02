using System.Text;

namespace Reface.NPI.Generators.SqlServer
{
    class GenerateContext
    {
        public SqlCommandDescription SqlCommandDescription { get; set; }

        public StringBuilder StringBuilder { get; set; }

        public GenerateContext(SqlCommandDescription sqlCommandDescription, StringBuilder stringBuilder)
        {
            SqlCommandDescription = sqlCommandDescription;
            StringBuilder = stringBuilder;
        }
    }
}
