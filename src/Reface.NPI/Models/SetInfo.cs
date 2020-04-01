namespace Reface.NPI.Models
{
    public class SetInfo
    {
        public string Field { get; set; }
        public string Parameter { get; set; }

        public override string ToString()
        {
            return $"[{Field}]=[{Parameter}]";
        }

    }
}
