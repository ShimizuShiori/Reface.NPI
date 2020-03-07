namespace Reface.NPI.Parsers
{
    public class SelectToken
    {
        private const string TEXT_AND = "And";
        private const string TEXT_OR = "Or";
        private const string TEXT_BY = "By";
        private const string TEXT_ORDER_BY = "Orderby";
        private const string TEXT_ASC = "Asc";
        private const string TEXT_DESC = "Desc";
        private const string TEXT_IS = "Is";
        private const string TEXT_LIKE = "Like";
        private const string TEXT_GREATER_THAN = "Greaterthan";
        private const string TEXT_LESS_THAN = "Lessthan";
        private const string TEXT_ALL = "All";

        public string Text { get; private set; }
        public SelectParseActions Action { get; private set; }

        private SelectToken(string text, SelectParseActions action)
        {
            Text = text;
            Action = action;
        }

        public static SelectToken Create(string text)
        {
            switch (text)
            {
                case TEXT_AND:
                    return new SelectToken(text, SelectParseActions.And);
                case TEXT_OR:
                    return new SelectToken(text, SelectParseActions.Or);
                case TEXT_BY:
                    return new SelectToken(text, SelectParseActions.By);
                case TEXT_ORDER_BY:
                    return new SelectToken(text, SelectParseActions.Orderby);
                case TEXT_ASC:
                    return new SelectToken(text, SelectParseActions.Asc);
                case TEXT_DESC:
                    return new SelectToken(text, SelectParseActions.Desc);
                case TEXT_IS:
                case TEXT_LIKE:
                case TEXT_GREATER_THAN:
                case TEXT_LESS_THAN:
                    return new SelectToken(text, SelectParseActions.Operator);
                case TEXT_ALL:
                    return new SelectToken(text, SelectParseActions.All);
                default:
                    return new SelectToken(text, SelectParseActions.Field);
            }
        }
    }
}
