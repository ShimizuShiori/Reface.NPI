namespace Reface.NPI.Generators
{
    public class ParameterInfo
    {
        public string Name { get; set; }
        public ParameterUses Use { get; set; } = ParameterUses.ForCondition;
        public object Value { get; set; }

        public ParameterInfo() : this("", ParameterUses.ForCondition)
        {

        }

        public ParameterInfo(string name, ParameterUses use)
        {
            this.Name = name;
            this.Use = use;
        }

        public override string ToString()
        {
            return $"{Name} [{Use.ToString()}] : {Value}";
        }
    }
}
