using System.Collections.Generic;

namespace Reface.NPI.Generators
{
    public class SqlCommandDescription
    {
        public string SqlCommand { get; set; }
        public Dictionary<string, object> Parameters { get; private set; }

        public SqlCommandDescription()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        public override string ToString()
        {
            return $"Sql : {SqlCommand} \nParameters : {Parameters.Join(",", x => $"{x.Key} = {x.Value.ToString()}")}";
        }
    }
}
