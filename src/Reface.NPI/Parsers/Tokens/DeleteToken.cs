using Reface.NPI.Parsers.Actions;

namespace Reface.NPI.Parsers.Tokens
{
    public class DeleteToken : IToken<DeleteParseActions>
    {
        public const string TEXT_BY = "By";
        public const string TEXT_AND = "And";
        public const string TEXT_OR = "Or";

        public string Text { get; private set; }

        public DeleteParseActions Action { get; private set; }

        private DeleteToken(string text, DeleteParseActions action)
        {
            Text = text;
            Action = action;
        }

        public static DeleteToken Create(string text)
        {
            switch (text)
            {
                case TEXT_BY:return new DeleteToken(text, DeleteParseActions.By);
                case TEXT_AND:return new DeleteToken(text, DeleteParseActions.And);
                case TEXT_OR:return new DeleteToken(text, DeleteParseActions.Or);
                default: return new DeleteToken(text, DeleteParseActions.Word);
            }
        }
    }
}
