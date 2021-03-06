﻿using Reface.NPI.Parsers.Actions;

namespace Reface.NPI.Parsers.Tokens
{
    public class UpdateToken : IToken<UpdateParseActions>
    {
        private const string TEXT_AND = "And";
        private const string TEXT_OR = "Or";
        private const string TEXT_BY = "By";
        private const string TEXT_EQUALS = "Equals";
        public const string TEXT_WITHOUT = "Without";
        public const string TEXT_NOT = "Not";

        public string Text { get; private set; }

        public UpdateParseActions Action { get; private set; }

        private UpdateToken(string text, UpdateParseActions action)
        {
            Text = text;
            Action = action;
        }

        public static UpdateToken Create(string text)
        {
            switch (text)
            {
                case TEXT_AND: return new UpdateToken(text, UpdateParseActions.And);
                case TEXT_OR: return new UpdateToken(text, UpdateParseActions.Or);
                case TEXT_BY: return new UpdateToken(text, UpdateParseActions.By);
                case TEXT_EQUALS: return new UpdateToken(text, UpdateParseActions.Equals);
                case TEXT_WITHOUT: return new UpdateToken(text, UpdateParseActions.Without);
                case TEXT_NOT: return new UpdateToken(text, UpdateParseActions.Not);
                default: return new UpdateToken(text, UpdateParseActions.Word);
            }
        }
    }
}
