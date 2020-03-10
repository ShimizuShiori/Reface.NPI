namespace Reface.NPI.Models
{
    public class OrderInfo
    {
        public string Field { get; private set; }
        public OrderTypes Type { get; private set; }

        public OrderInfo(string field, OrderTypes type)
        {
            Field = field;
            Type = type;
        }

        public OrderInfo(string field) : this(field, OrderTypes.Asc)
        {

        }

        public override string ToString()
        {
            return $"[{Field}] [{Type.ToString()}]";
        }
    }
}
