namespace Election.Interfaces
{
    public interface IPerson
    {
        int Id { get; }
        string Name { get; }
    }
    public interface IVoter : IPerson { }
    public interface ICandidate : IPerson { }
}
