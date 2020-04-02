using Reface.NPI.Parsers.Actions;

namespace Reface.NPI.Parsers.Tokens
{
    public class InsertToken : IToken<InsertParseActions>
    {
        public const string TEXT_WITHOUT = "Without";
        public const string TEXT_AND = "And";
        public const string TEXT_SELECT = "Select";

        public string Text { get; private set; }

        public InsertParseActions Action { get; private set; }

        private InsertToken()
        {

        }

        public static InsertToken Create(string text)
        {
            InsertToken token = new InsertToken();
            token.Text = text;
            switch (text)
            {
                case TEXT_AND:
                    token.Action = InsertParseActions.And;
                    break;
                case TEXT_SELECT:
                    token.Action = InsertParseActions.Select;
                    break;
                case TEXT_WITHOUT:
                    token.Action = InsertParseActions.Without;
                    break;
                default:
                    token.Action = InsertParseActions.Word;
                    break;
            }
            return token;
        }
    }
}
