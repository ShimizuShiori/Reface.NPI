using Reface.NPI.Parsers.Actions;

namespace Reface.NPI.Parsers.Tokens
{
    public class SelectToken : IToken<SelectParseActions>
    {
        public const string TEXT_AND = "And";
        public const string TEXT_OR = "Or";
        public const string TEXT_BY = "By";
        public const string TEXT_ORDER_BY = "Orderby";
        public const string TEXT_ASC = "Asc";
        public const string TEXT_DESC = "Desc";
        public const string TEXT_NOT = "Not";

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
                    return new SelectToken(text, SelectParseActions.OrderBy);
                case TEXT_ASC:
                    return new SelectToken(text, SelectParseActions.AscOrDesc);
                case TEXT_DESC:
                    return new SelectToken(text, SelectParseActions.AscOrDesc);
                case TEXT_NOT:
                    return new SelectToken(text, SelectParseActions.Not);
                default:
                    return new SelectToken(text, SelectParseActions.Word);
            }
        }

        public static SelectToken CreateEndToken()
        {
            return new SelectToken("", SelectParseActions.End);
        }

        public override string ToString()
        {
            return $"{this.Action} : {this.Text}";
        }
    }
}
