namespace Election.Interfaces
{
    public interface IVote
    {
        IVoter Voter { get; }
        public ICandidate Candidate { get; }
    }

    public interface IRankedVote : IVote
    {
        int Rank { get; }
    }
}
