namespace Reface.NPI.Generators
{
    public class SqlParameterInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public SqlParameterInfo() : this("")
        {

        }

        public SqlParameterInfo(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"{Name} : {Value}";
        }
    }
}
