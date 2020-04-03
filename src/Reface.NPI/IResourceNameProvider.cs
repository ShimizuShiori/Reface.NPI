namespace Reface.NPI
{
    public interface IResourceNameProvider
    {
        string SelectStateMachineCsv { get; }
        string InsertStateMachineCsv { get; }
        string UpdateStateMachineCsv { get; }
        string DeleteStateMachineCsv { get; }
    }
}
