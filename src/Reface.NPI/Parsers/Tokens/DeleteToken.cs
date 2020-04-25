using Reface.NPI.Parsers.Actions;

namespace Reface.NPI.Parsers.Tokens
{
    public class CountToken : IToken<CountParseActions>
    {
        public const string TEXT_BY = "By";
        public const string TEXT_AND = "And";
        public const string TEXT_OR = "Or";
        public const string TEXT_NOT = "Not";
        public string Text { get; private set; }

        public CountParseActions Action { get; private set; }

        private CountToken(string text, CountParseActions action)
        {
            Text = text;
            Action = action;
        }

        public static CountToken Create(string text)
        {
            switch (text)
            {
                case TEXT_BY: return new CountToken(text, CountParseActions.By);
                case TEXT_AND: return new CountToken(text, CountParseActions.And);
                case TEXT_OR: return new CountToken(text, CountParseActions.Or);
                case TEXT_NOT: return new CountToken(text, CountParseActions.Not);
                default: return new CountToken(text, CountParseActions.Word);
            }
        }
    }
}
