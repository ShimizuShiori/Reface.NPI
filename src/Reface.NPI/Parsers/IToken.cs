namespace Reface.NPI.Parsers
{
    public interface IToken<TAction>
    {
        string Text { get; }
        TAction Action { get; }
    }
}
