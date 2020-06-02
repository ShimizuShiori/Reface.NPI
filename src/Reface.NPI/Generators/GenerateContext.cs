using System.Text;

namespace Reface.NPI.Generators
{
    class GenerateContext
    {
        public SqlCommandDescription SqlCommandDescription { get; set; }

        public StringBuilder StringBuilder { get; set; }

        public bool HasWhereKeyword { get; set; } = false;
        public GenerateContext(SqlCommandDescription sqlCommandDescription, StringBuilder stringBuilder)
        {
            SqlCommandDescription = sqlCommandDescription;
            StringBuilder = stringBuilder;
        }
    }
}
