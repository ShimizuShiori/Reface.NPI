namespace Reface.NPI
{
    public class BetweenParameter
    {
        public object Begin { get; set; }
        public object End { get; set; }

        public BetweenParameter(object begin, object end)
        {
            Begin = begin;
            End = end;
        }

        public BetweenParameter()
        {

        }
    }
}
