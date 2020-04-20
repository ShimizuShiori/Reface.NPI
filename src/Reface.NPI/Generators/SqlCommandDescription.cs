using System.Collections.Generic;

namespace Reface.NPI.Generators
{
    public class SqlCommandDescription : ICopy
    {
        public SqlCommandExecuteModes Mode { get; set; }

        public string SqlCommand { get; set; }
        public Dictionary<string, SqlParameterInfo> Parameters { get; private set; }

        public void AddParameter(SqlParameterInfo info)
        {
            this.Parameters[info.Name] = info;
        }

        public SqlCommandDescription()
        {
            this.Parameters = new Dictionary<string, SqlParameterInfo>();
        }

        public override string ToString()
        {
            return $"Sql : {SqlCommand} \nParameterValues : \n{Parameters.Join("\n", x => $"\t{x.Value.ToString()}")}";
        }

        public object Copy()
        {
            return new SqlCommandDescription()
            {
                SqlCommand = this.SqlCommand,
                Mode = this.Mode,
                Parameters = new Dictionary<string, SqlParameterInfo>(this.Parameters)
            };
        }
    }
}
